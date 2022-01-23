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
            //random position
            Vector3 blipPos = new Vector3(Random.Range(-m_WorldSize.x, m_WorldSize.x), Random.Range(-m_WorldSize.y, m_WorldSize.y), 0f);
            //random rotation
            Quaternion blipRot = Quaternion.AngleAxis( Random.Range(0f, 360f), Vector3.forward);
            GameObject tempBlip =  Instantiate(m_BlipPrefab);
            tempBlip.transform.position = blipPos;
            tempBlip.transform.rotation = blipRot;
            Blip blipScript = tempBlip.GetComponent<Blip>();
            if(blipScript == null)
            {
                blipScript = tempBlip.AddComponent<Blip>();
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

    private void RecordData()
    {

    }
}
