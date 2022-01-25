using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OrganismState
{ 
    LookingForFood = 0,
    LookingForMate = 1
}

public class Genetics
{

    public Genetics()
    {
        m_MaxSpeedGene[0] = 0f;
        m_MaxSpeedGene[1] = 0f;

        m_VisionRangeGene[0] = 0f;
        m_VisionRangeGene[1] = 0f;
    }

    public Genetics(float speed0, float speed1, float vision0, float vision1)
    {
        m_MaxSpeedGene[0] = speed1;
        m_MaxSpeedGene[1] = speed1;
       
        m_VisionRangeGene[0] = vision0;
        m_VisionRangeGene[1] = vision1;
    }

    public float GetMaxSpeed()
    {
        return (m_MaxSpeedGene[0] + m_MaxSpeedGene[1]) / 2f;
    }

    public float GetVisionRange()
    {
        return (m_VisionRangeGene[0] + m_VisionRangeGene[1]) / 2f;
    }

    static public Genetics Inherit(Genetics parent1, Genetics parent2)
    {
        Genetics childGenetics = new Genetics();

        //Get one one version of every gene from every parent 
        childGenetics.m_MaxSpeedGene[0] = parent1.m_MaxSpeedGene[Random.Range(0, 2)];
        childGenetics.m_MaxSpeedGene[1] = parent2.m_MaxSpeedGene[Random.Range(0, 2)];

        childGenetics.m_VisionRangeGene[0] = parent1.m_VisionRangeGene[Random.Range(0, 2)];
        childGenetics.m_VisionRangeGene[1] = parent2.m_VisionRangeGene[Random.Range(0, 2)];

        //TODO: ADD SMALL MUTATION CHANCE
        if(Random.Range(0f, 1f) < SimulationScript.GetMutationChance())
        {
            
        }

        if(Random.Range(0f, 1f) < SimulationScript.GetMutationChance())
        {

        }


        return childGenetics;
    }


    //GENES
    //speed
    private float[] m_MaxSpeedGene = new float[2];
    //vision
    private float[] m_VisionRangeGene = new float[2];
}


public class BaseOrganism : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool IsDead() => m_Dead;
  

    //make a custom Update that Updates the organism for one time step
    protected virtual void UpdateTimeStep()
    { }

    //+++++++++
    //STATS
    //+++++++++

    //DEAD?
    protected bool m_Dead = false;

    //HUNGER
    protected float m_Hunger; //value between 0 and 1, increased with caluculatedHunger / maxhunger every timestep
    

    //REPRODUCTIVE URGE
    protected float m_CurrentReproductiveUrge; //value between 0 and 1 that is increase with 1/maxAge every timestep.
    protected int m_MatingCooldown;

    //LIFETIME
    protected int m_Age; //increases by 1 every timestep


    //movement animation things
    protected Vector3 m_TargetPos;
    protected Vector3 m_PreviousPos;
    protected float m_PosInterpolationTValue = 0f;


    protected Genetics m_Genes;
    protected OrganismState m_State;

}
