using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BaseOrganism : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool IsDead()
    {
        return m_Dead;
    }

    //make a custom Update that Updates the organism for one time step
    protected virtual void UpdateTimeStep()
    {}

    //STATS
    //is it dead?
    protected bool m_Dead = false;
    //hunger
    protected float m_Hunger;
    protected float m_MaxHunger;
    //lifetime
    protected int m_Age;
    protected int m_MaxAge;
    //reproductive urge


    protected Vector3 m_TargetPos;
    protected Vector3 m_PreviousPos;
    protected float m_PosInterpolationTValue = 0f;


    //todo: make a class for genetic information
    protected float m_MaxSpeed = 3f;
    protected float m_VisionRange = 5f;

}
