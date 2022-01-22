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
        //random movement
        float angle = Random.Range(0f, 360f);
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        Vector3 MovementDir = rot * Vector3.right;

        gameObject.transform.position += MovementDir * m_MaxSpeed;

    }
}
