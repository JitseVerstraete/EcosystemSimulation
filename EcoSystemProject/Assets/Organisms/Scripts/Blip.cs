using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blip : BaseOrganism
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public override void UpdateTimeStep()
    {
        Vector2 worldSize = SimulationScript.GetWorldSize();



        //JUST WANDERING AROUND

        //random movement within a cone relative to forward position
        float addedAngle = Random.Range(-60f, 60f);
        float currentAngle = gameObject.transform.rotation.eulerAngles.z;

        Quaternion newRot = Quaternion.AngleAxis(currentAngle + addedAngle, Vector3.forward);
        Vector3 movementDir = newRot * Vector3.right;

        gameObject.transform.position += movementDir * m_MaxSpeed;



        //KEEP THE BLIPS IN THE PLAY FIELD
        //x
        if(gameObject.transform.position.x < -worldSize.x)
        {
            gameObject.transform.position -= new Vector3((gameObject.transform.position.x + worldSize.x) * 2, 0f, 0f);
            movementDir.x = -movementDir.x;
        }
        else if (gameObject.transform.position.x > worldSize.x)
        {
            gameObject.transform.position -= new Vector3((gameObject.transform.position.x - worldSize.x) * 2, 0f, 0f);
            movementDir.x = -movementDir.x;
        }

        //y
        if (gameObject.transform.position.y < -worldSize.y)
        {
            gameObject.transform.position -= new Vector3(0f, (gameObject.transform.position.y + worldSize.y) * 2, 0f);
            movementDir.y = -movementDir.y;
        }
        else if(gameObject.transform.position.y > worldSize.y)
        {
            gameObject.transform.position -= new Vector3(0f, (gameObject.transform.position.y - worldSize.y) * 2, 0f);
            movementDir.y = -movementDir.y;
        }

        //SET ROTATION TO FORWARD
        float defAngle = Mathf.Atan2(movementDir.y, movementDir.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.AngleAxis(defAngle, Vector3.forward);

    }
}
