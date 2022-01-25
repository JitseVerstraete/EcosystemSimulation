using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Blip : BaseOrganism
{
    Blip m_Partner = null;
    int m_MatingCounter = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void InitializeBlip(Genetics genetics)
    {
        m_Hunger = 0.5f;
        m_Age = 0;

        m_TargetPos = gameObject.transform.position;
        m_PreviousPos = gameObject.transform.position;

        m_Genes = genetics;

        m_MatingCooldown = 0;

        CircleCollider2D col = GetComponentInChildren<CircleCollider2D>();
        col.radius = m_Genes.GetVisionRange();
    }

    public bool AvailableForMating()
    {
        return m_State == OrganismState.LookingForMate && m_Partner == null;
    }

    public void SetPartner(Blip partner)
    {
        m_Partner = partner;
    }

    public void DoneMating()
    {
        m_CurrentReproductiveUrge = 0f;
        m_Partner = null;
        m_State = OrganismState.LookingForFood;
        m_MatingCounter = 0;
        m_Hunger += SimulationScript.Instance.GetMatingCost();
        m_MatingCooldown = SimulationScript.Instance.GetMatingCooldown();
    }


    private void Update()
    {
        if (SimulationScript.Instance.UpdateSimulation())
        {
            UpdateTimeStep();
        }


        //interpolate position between previous position and target position
        if (SimulationScript.Instance.GetTimeStep() == 0)
            m_PosInterpolationTValue = 1f;

        m_PosInterpolationTValue += (Time.deltaTime / SimulationScript.Instance.GetTimeStep()) * 1.3333f;
        gameObject.transform.position = Vector3.Lerp(m_PreviousPos, m_TargetPos, m_PosInterpolationTValue);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            /*
            //Blip is full again :))
            m_Hunger -= SimulationScript.Instance.GetFoodEffectiveness();
            m_Hunger = Mathf.Clamp(m_Hunger, 0f, 1.01f);

            //Delete food
            Destroy(collision.gameObject);

             */



        }
    }


    // Update is called once per frame
    protected override void UpdateTimeStep()
    {

        //increment hunger
        m_Hunger += (CalulateHunger() / SimulationScript.Instance.GetBlipMaxHunger());
        m_Age++;
        m_CurrentReproductiveUrge += 1f / SimulationScript.Instance.GetBlipLifeSpan();
        m_MatingCooldown--;



        if (m_Hunger >= 1f)
        {
            print("died of hunger");
            Destroy(gameObject);
        }
        if (m_Age >= SimulationScript.Instance.GetBlipLifeSpan())
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

            if (m_MatingCounter >= SimulationScript.Instance.GetBlipMatingTime())
            {
                //create a new blip
                GameObject go = Instantiate(gameObject);
                go.name = "BLIP";
                go.transform.position = Vector3.Lerp(gameObject.transform.position, m_Partner.gameObject.transform.position, 0.5f);
                Blip b = go.GetComponent<Blip>();

                b.InitializeBlip(Genetics.Inherit(m_Genes, m_Partner.m_Genes));

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


        switch (m_State)
        {
            //MOVEMENT WHEN LOOKING FOR FOOD
            case OrganismState.LookingForFood:
                Food closestFood = SimulationScript.Instance.GetClosestFood(gameObject.transform.position);
                float FoodDistance;

                if (closestFood == null)
                    FoodDistance = float.MaxValue;
                else
                    FoodDistance = Vector3.Distance(closestFood.transform.position, gameObject.transform.position);

                if (FoodDistance < m_Genes.GetVisionRange())
                {
                    //go to food
                    movementDir = closestFood.transform.position - gameObject.transform.position;
                    if (movementDir.magnitude > m_Genes.GetMaxSpeed())
                    {
                        movementDir = movementDir.normalized * m_Genes.GetMaxSpeed();
                    }

                    //if close enough to food, destroy it

                    //Blip is full again :))
                    if (FoodDistance < 1f && !closestFood.IsEaten())
                    {
                        m_Hunger -= SimulationScript.Instance.GetFoodEffectiveness();
                        m_Hunger = Mathf.Clamp(m_Hunger, 0f, 1.01f);

                        //Set food as eaten and destroy the food after
                        closestFood.EatFood();
                        Destroy(closestFood.gameObject);
                    }

                }
                else
                {
                    //no food in sight
                    movementDir = Wander(60f);
                    movementDir = movementDir.normalized * m_Genes.GetMaxSpeed();
                }
                break;

            //MOVEMENT WHEN LOOKING FOR MATE
            case OrganismState.LookingForMate:
                Blip closestBlip = SimulationScript.Instance.GetClosestPartner(gameObject.transform.position);
                float BlipDistance;

                if (closestBlip == null)
                    BlipDistance = float.MaxValue;
                else
                    BlipDistance = Vector3.Distance(closestBlip.gameObject.transform.position, gameObject.transform.position);

                if (BlipDistance < m_Genes.GetVisionRange())
                {
                    //go to partner
                    movementDir = closestBlip.gameObject.transform.position - gameObject.transform.position;
                    if (movementDir.magnitude > m_Genes.GetMaxSpeed())
                    {
                        movementDir = movementDir.normalized * m_Genes.GetMaxSpeed();
                    }
                    else
                    {
                        movementDir /= 2;
                    }

                    //if the blip reached the potential mate, set them as eachothers partner
                    if (BlipDistance < 1f)
                    {
                        if (AvailableForMating() && closestBlip.AvailableForMating())
                        {
                            //set each other as partners
                            SetPartner(closestBlip);
                            closestBlip.SetPartner(this);
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



    private Vector2 Wander(float angle)
    {
        //no food in sight
        float addedAngle = Random.Range(-angle, angle);
        float currentAngle = gameObject.transform.rotation.eulerAngles.z;

        Quaternion newRot = Quaternion.AngleAxis(currentAngle + addedAngle, Vector3.forward);
        Vector3 movementDir = newRot * Vector3.right;
        return movementDir;
    }

    private float CalulateHunger()
    {
        const float movementSpeedWeight = 0.7f;
        const float visionRangeWeight = 0.3f;
        //todo: calculate energy loss (( maxspeed^1.2 + (visionrange/2)^1.2 ) / 2)
        float movementSpeedCost = movementSpeedWeight * Mathf.Pow(m_Genes.GetMaxSpeed(), 1.1f);
        float visionRangeCost = visionRangeWeight * Mathf.Pow(m_Genes.GetVisionRange() / 2, 1.1f);

        return movementSpeedCost + visionRangeCost;
    }
}
