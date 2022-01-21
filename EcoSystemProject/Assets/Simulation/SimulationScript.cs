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


    [SerializeField]
    private float m_TimeStep = 3f;

    private float m_Timer;
}
