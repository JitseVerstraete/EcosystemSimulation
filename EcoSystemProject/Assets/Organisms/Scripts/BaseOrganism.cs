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
    public virtual void UpdateTimeStep()
    {

    }

    //STATS
    //hunger
    protected float m_Hunger;
    //lifetime
    protected float m_LifeTime;
    //reproductive urge


    //todo: make a class for genetic information
    protected float m_MaxSpeed = 3f;
    protected float m_VisionRange = 5f;

}
