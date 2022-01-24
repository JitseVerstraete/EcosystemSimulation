using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blip : BaseOrganism
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void InitializeBlip(float maxHunger, int maxAge, Genetics genetics)
    {
        m_Hunger = 0;
        m_MaxHunger = maxHunger;

        m_Age = 0;
        m_MaxAge = maxAge;

        m_TargetPos = gameObject.transform.position;
        m_PreviousPos = gameObject.transform.position;

        m_Genes = genetics;


        CircleCollider2D col = GetComponentInChildren<CircleCollider2D>();
        col.radius = m_Genes.GetVisionRange();
    }


    private void Update()
    {
        if (SimulationScript.UpdateSimulation())
        {
            UpdateTimeStep();
        }


        //interpolate position between previous position and target position
        if (SimulationScript.GetTimeStep() == 0)
            m_PosInterpolationTValue = 1f;

        m_PosInterpolationTValue += (Time.deltaTime / SimulationScript.GetTimeStep()) * 1.3333f;
        gameObject.transform.position = Vector3.Lerp(m_PreviousPos, m_TargetPos, m_PosInterpolationTValue);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            //Blip is full again :))
            m_Hunger = 0;

            //Delete food
            Destroy(collision.gameObject);
            SimulationScript.QueueNewFoodSpawn();

        }
    }

    // Update is called once per frame
    protected override void UpdateTimeStep()
    {
        //update state
        m_State = OrganismState.LookingForFood;

        //increment hunger
        m_Hunger += (CalulateHunger() / m_MaxHunger);
        m_Age++;
        m_CurrentReproductiveUrge += 1f / m_MaxAge;

        if (m_Hunger >= 1f)
        {
            print("died of hunger");
            Destroy(gameObject);
        }
        if (m_Age >= m_MaxAge)
        {
            print("died of old age");
            Destroy(gameObject);
        }

        if (m_CurrentReproductiveUrge > m_Hunger)
            m_State = OrganismState.LookingForMate;
        else
            m_State = OrganismState.LookingForFood;




        //Handle Movement
        Vector3 movementDir = new Vector3();


        switch (m_State)
        {
            case OrganismState.LookingForFood:
                if (Vector3.Distance(SimulationScript.GetClosestFood(gameObject.transform.position), gameObject.transform.position) < m_Genes.GetVisionRange())
                {
                    //go to food
                    movementDir = SimulationScript.GetClosestFood(gameObject.transform.position) - gameObject.transform.position;
                    if (movementDir.magnitude > m_Genes.GetMaxSpeed())
                    {
                        movementDir = movementDir.normalized * m_Genes.GetMaxSpeed();
                    }

                }
                else
                {
                    //no food in sight
                    movementDir = Wander(60f);
                    movementDir = movementDir.normalized * m_Genes.GetMaxSpeed();
                }
                break;

            case OrganismState.LookingForMate:
                print("looking for sexy time");
                break;

            default:
                break;
        }


       


        m_TargetPos = gameObject.transform.position + movementDir;
        m_PreviousPos = gameObject.transform.position;


        //KEEP THE BLIPS IN THE PLAY FIELD
        Vector2 worldSize = SimulationScript.GetWorldSize();
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
        //todo: calculate energy loss (( maxspeed^1.2 + (visionrange/2)^1.2 ) / 2)
        return m_Genes.GetMaxSpeed();
    }
}
