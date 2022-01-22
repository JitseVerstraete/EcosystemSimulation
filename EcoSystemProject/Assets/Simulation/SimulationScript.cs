using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //setup the simulation here
        m_Timer = m_TimeStep;
        m_WorldSize.x = m_SetWorldSize * Camera.main.aspect;
        m_WorldSize.y = m_SetWorldSize;

    
    }

    // Update is called once per frame
    void Update()
    {
        m_Timer -= Time.deltaTime;
        if (m_Timer <= 0f)
        {
            m_Timer = m_TimeStep;
            //update world

        }

    }
    
    static public Vector3 GetWorldSize()
    {
        return m_WorldSize;
    }

    //update timer
    [SerializeField]
    private float m_TimeStep = 3f;
    private float m_Timer;


    //world border
    [SerializeField]
    private float m_SetWorldSize = 100f;
    static Vector2 m_WorldSize;

}
