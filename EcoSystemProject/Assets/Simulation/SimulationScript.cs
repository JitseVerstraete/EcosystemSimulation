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

        //initialize Stating Population
        m_Blips = new List<Blip>();
        for(int i = 0; i < m_InitialNrBlips; i++)
        {
            GameObject temp =  Instantiate(m_BlipPrefab);
            Blip blipScript =  temp.GetComponent<Blip>();
            if(blipScript == null)
            {
                blipScript = temp.AddComponent<Blip>();
            }

            m_Blips.Add(blipScript);
        }

    }

    // Update is called once per frame
    void Update()
    {
        m_Timer -= Time.deltaTime;
        if (m_Timer <= 0f)
        {
            m_Timer = m_TimeStep;
            //update All organisms
            UpdateBlips();

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

    //Blips
    List<Blip> m_Blips;
    public int m_InitialNrBlips  = 0;
    public GameObject m_BlipPrefab;



    //world border
    [SerializeField]
    private float m_SetWorldSize = 100f;
    static Vector2 m_WorldSize;




    //member functions
    private void  UpdateBlips()
    {
        foreach(Blip b in m_Blips)
        {
            b.UpdateTimeStep();
        }
    }
}
