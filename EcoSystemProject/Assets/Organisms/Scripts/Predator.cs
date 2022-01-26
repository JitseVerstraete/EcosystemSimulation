using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : BaseOrganism
{
    Predator m_Partner = null;

    private void OnDisable()
    {
        this.enabled = true;
    }

    public void InitializePredator(Genetics genetics, int startAge = 0)
    {
        m_Hunger = 0.5f;
        m_Age = startAge;

        m_TargetPos = gameObject.transform.position;
        m_PreviousPos = gameObject.transform.position;

        m_Genes = genetics;

        m_MatingCooldown = 0;
    }
    public bool AvailableForMating()
    {
        return m_State == OrganismState.LookingForMate && m_Partner == null;
    }

    public void SetPartner(Predator partner)
    {
        m_Partner = partner;
    }

    public void DoneMating()
    {
        m_CurrentReproductiveUrge = 0f;
        m_Partner = null;
        m_State = OrganismState.LookingForFood;
        m_MatingCounter = 0;
        m_Hunger += SimulationScript.Instance.GetPredatorMatingCost();
        m_MatingCooldown = SimulationScript.Instance.GetPredatorMatingCooldown();
    }

    private void Update()
    {
        if(SimulationScript.Instance.UpdateSimulation())
        {
            UpdateTimeStep();
        }

        //interpolate position between previous position and target position
        if (SimulationScript.Instance.GetTimeStep() == 0)
            m_PosInterpolationTValue = 1f;

        m_PosInterpolationTValue += (Time.deltaTime / SimulationScript.Instance.GetTimeStep()) * 1.3333f;
        gameObject.transform.position = Vector3.Lerp(m_PreviousPos, m_TargetPos, m_PosInterpolationTValue);
    }

    protected override void UpdateTimeStep()
    {
        //update hunger, age, reproductive urge and mating cooldown
        m_Hunger += (CalulateHunger() / SimulationScript.Instance.GetPredatorMaxHunger());
        m_Age++;
        m_CurrentReproductiveUrge += 1f / SimulationScript.Instance.GetPredatorLifeSpan();
        m_MatingCooldown--;

        if (m_Hunger >= 1f)
        {
            print("died of hunger");
            Destroy(gameObject);
        }
        if (m_Age >= SimulationScript.Instance.GetPredatorLifeSpan())
        {
            print("died of old age");
            Destroy(gameObject);
        }

        //update if blip found a partner
        if (m_Partner != null)
        {
            m_PreviousPos = gameObject.transform.position;
            m_TargetPos = gameObject.transform.position;
            m_MatingCounter++;

            if (m_MatingCounter >= SimulationScript.Instance.GetPredatorMatingTime())
            {
                //create a new predator
                GameObject go = Instantiate(gameObject);
                go.name = "PREDATOR";
                go.transform.position = Vector3.Lerp(gameObject.transform.position, m_Partner.gameObject.transform.position, 0.5f);
                Predator b = go.GetComponent<Predator>();

                b.InitializePredator(Genetics.Inherit(m_Genes, m_Partner.m_Genes));

                m_Partner.DoneMating();
                DoneMating();


            }
            return;
        }

        //update state
        if (m_CurrentReproductiveUrge > m_Hunger && m_MatingCooldown <= 0)
            m_State = OrganismState.LookingForMate;
        else
            m_State = OrganismState.LookingForFood;


        //Handle Movement
        Vector3 movementDir = new Vector3();

        //do movement switch
        switch (m_State)
        {
            //MOVEMENT WHEN LOOKING FOR FOOD
            case OrganismState.LookingForFood:
                Blip closestBlipFood = SimulationScript.Instance.GetClosestBlip(gameObject.transform.position);
                float FoodDistance;

                if (closestBlipFood == null)
                    FoodDistance = float.MaxValue;
                else
                    FoodDistance = Vector3.Distance(closestBlipFood.transform.position, gameObject.transform.position);

                if (FoodDistance < m_Genes.GetVisionRange())
                {
                    //go to food
                    movementDir = closestBlipFood.transform.position - gameObject.transform.position;
                    if (movementDir.magnitude > m_Genes.GetMaxSpeed())
                    {
                        movementDir = movementDir.normalized * m_Genes.GetMaxSpeed();
                    }

                    //if close enough to food, destroy it

                    //Blip is full again :))
                    if (FoodDistance < 1f && !closestBlipFood.IsEaten())
                    {
                        m_Hunger -= SimulationScript.Instance.GetPredatorFoodEffectiveness();
                        m_Hunger = Mathf.Clamp(m_Hunger, 0f, 1.01f);

                        //Set food as eaten and destroy the food after
                        closestBlipFood.EatBlip();
                        Destroy(closestBlipFood.gameObject);
                    }

                }
                else
                {
                    //no blips in sight
                    movementDir = Wander(60f);
                    movementDir = movementDir.normalized * m_Genes.GetMaxSpeed();
                }
                break;

            //MOVEMENT WHEN LOOKING FOR MATE
            case OrganismState.LookingForMate:
                Predator closestPredator = SimulationScript.Instance.GetClosestPredatorPartner(gameObject.transform.position);
                float predatorDistance;

                if (closestPredator == null)
                    predatorDistance = float.MaxValue;
                else
                    predatorDistance = Vector3.Distance(closestPredator.gameObject.transform.position, gameObject.transform.position);

                if (predatorDistance < m_Genes.GetVisionRange())
                {
                    //go to partner
                    movementDir = closestPredator.gameObject.transform.position - gameObject.transform.position;
                    if (movementDir.magnitude > m_Genes.GetMaxSpeed())
                    {
                        movementDir = movementDir.normalized * m_Genes.GetMaxSpeed();
                    }
                    else
                    {
                        movementDir /= 2;
                    }

                    //if the blip reached the potential mate, set them as eachothers partner
                    if (predatorDistance < 1f)
                    {
                        if (AvailableForMating() && closestPredator.AvailableForMating())
                        {
                            //set each other as partners
                            SetPartner(closestPredator);
                            closestPredator.SetPartner(this);
                        }
                    }

                }
                else
                {
                    //no partner in sight
                    movementDir = Wander(60f);
                    movementDir = movementDir.normalized * m_Genes.GetMaxSpeed();

                }
                break;

            default:
                break;
        }



        m_TargetPos = gameObject.transform.position + movementDir;
        m_PreviousPos = gameObject.transform.position;


        //KEEP THE BLIPS IN THE PLAY FIELD
        Vector2 worldSize = SimulationScript.Instance.GetWorldSize();
        //x
        if (m_TargetPos.x < -worldSize.x)
        {
            m_TargetPos -= new Vector3((m_TargetPos.x + worldSize.x) * 2, 0f, 0f);
            movementDir.x = -movementDir.x;
        }
        else if (m_TargetPos.x > worldSize.x)
        {
            m_TargetPos -= new Vector3((m_TargetPos.x - worldSize.x) * 2, 0f, 0f);
            movementDir.x = -movementDir.x;
        }
        //y
        if (m_TargetPos.y < -worldSize.y)
        {
            m_TargetPos -= new Vector3(0f, (m_TargetPos.y + worldSize.y) * 2, 0f);
            movementDir.y = -movementDir.y;
        }
        else if (m_TargetPos.y > worldSize.y)
        {
            m_TargetPos -= new Vector3(0f, (m_TargetPos.y - worldSize.y) * 2, 0f);
            movementDir.y = -movementDir.y;
        }

        //SET ROTATION TO FORWARD
        float defAngle = Mathf.Atan2(movementDir.y, movementDir.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.AngleAxis(defAngle, Vector3.forward);




        m_PosInterpolationTValue = 0f;

    }





}