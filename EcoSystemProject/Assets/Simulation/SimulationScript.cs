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
        m_WorldSize = m_SetWorldSize;
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

        //draw world border
        Vector3 center = Vector3.zero;
        Vector3 bottomLeft = new Vector3(center.x - m_WorldSize, center.y - m_WorldSize, 0f);
        Vector3 topLeft = new Vector3(center.x - m_WorldSize, center.y + m_WorldSize, 0f);
        Vector3 topRight = new Vector3(center.x + m_WorldSize, center.y + m_WorldSize, 0f);
        Vector3 bottomRight = new Vector3(center.x + m_WorldSize, center.y - m_WorldSize, 0f);

        Debug.DrawLine(bottomLeft, topLeft, Color.red, 10f, false);
        Debug.DrawLine(topLeft, topRight, Color.red, 10f, false);
        Debug.DrawLine(topRight, bottomRight, Color.red, 10f, false);
        Debug.DrawLine(bottomRight, bottomLeft, Color.red, 10f, false);

        

    }
    
    static public float GetWorldSize()
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
    static float m_WorldSize;

}
