using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BaseOrganism : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //make a custom Update that Updates the organism for one time step
    protected virtual void UpdateTimeStep()
    {

    }

    //todo: make a class for genetic information
    protected float m_MaxSpeed;
    protected float m_VisionRange;

}
