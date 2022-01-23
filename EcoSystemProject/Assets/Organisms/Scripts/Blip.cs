using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blip : BaseOrganism
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void InitializeBlip(float maxHunger, int maxAge)
    {
        m_Hunger = 0f;
        m_MaxHunger = maxHunger;

        m_Age = 0;
        m_MaxAge = maxAge;

        m_PreviousPos = gameObject.transform.position;
        m_TargetPos = gameObject.transform.position;
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
        //update stats
        m_Hunger += m_MaxSpeed;
        m_Age++;

        if (m_Hunger >= m_MaxHunger || m_Age >= m_MaxAge)
        {
            Destroy(gameObject);
        }

        //JUST WANDERING AROUND

        Vector2 worldSize = SimulationScript.GetWorldSize();
        //random movement within a cone relative to forward position
        float addedAngle = Random.Range(-60f, 60f);
        float currentAngle = gameObject.transform.rotation.eulerAngles.z;

        Quaternion newRot = Quaternion.AngleAxis(currentAngle + addedAngle, Vector3.forward);
        Vector3 movementDir = newRot * Vector3.right;

        m_TargetPos = gameObject.transform.position + (movementDir * m_MaxSpeed);
        m_PreviousPos = gameObject.transform.position;
        m_PosInterpolationTValue = 0f;



        //KEEP THE BLIPS IN THE PLAY FIELD
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

    }
}
