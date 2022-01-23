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
        m_StaticTimeStep = m_TimeStep;

        //initialize Stating Population
        for (int i = 0; i < m_InitialNrBlips; i++)
        {
            //random position
            Vector3 blipPos = new Vector3(Random.Range(-m_WorldSize.x, m_WorldSize.x), Random.Range(-m_WorldSize.y, m_WorldSize.y), 0f);

            //random rotation
            Quaternion blipRot = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);

            //create blip
            GameObject tempBlip = Instantiate(m_BlipPrefab);

            tempBlip.transform.position = blipPos;
            tempBlip.transform.rotation = blipRot;
            Blip blipScript = tempBlip.GetComponent<Blip>();
            if (blipScript == null)
            {
                blipScript = tempBlip.AddComponent<Blip>();
            }

            //set starting parameters
            blipScript.InitializeBlip(m_BlipHunger, m_BlipLifespan);
        }

        //initialize food
        for(int i = 0; i < m_AmountOfFood; i++)
        {
            SpawnFood();
        }


    }

    // Update is called once per frame
    void Update()
    {
        m_Timer -= Time.deltaTime;
        if (m_Timer <= 0f)
        {
            m_Timer = m_TimeStep;
            m_UpdateSimulation = true; //organisms use this bool to update or not
            for(int i = 0; i < m_QueuedFoods; ++i)
            {
                SpawnFood();
            }
            m_QueuedFoods = 0;
        }
        else
        {
            m_UpdateSimulation = false;
        }

    }

    static public bool UpdateSimulation()
    {
        return m_UpdateSimulation;
    }

    static public Vector3 GetWorldSize()
    {
        return m_WorldSize;   
    }

    static public float GetTimeStep()
    {
        return m_StaticTimeStep;
    }

    //update timer
    [SerializeField]
    [Header("Time Settings")]
    private float m_TimeStep = 3f;
    private float m_Timer;
    static private bool m_UpdateSimulation = false;
    static private float m_StaticTimeStep;

    [Header("World Size")]
    [SerializeField]
    private float m_SetWorldSize = 100f;
    static Vector2 m_WorldSize;

    //BLIPS 
    [Header("Blip Settings")]
    public GameObject m_BlipPrefab;
    public int m_InitialNrBlips = 0;
    public int m_BlipLifespan = 10;
    public float m_BlipHunger = 100f;

    //FOOD
    [Header("Food Settings")]
    public GameObject m_FoodPrefab ;
    public int m_AmountOfFood = 0;

    static private int m_QueuedFoods;


    static public void QueueNewFoodSpawn()
    {
        ++m_QueuedFoods;
    }

    private void SpawnFood()
    {
        Vector3 foodPos = new Vector3(Random.Range(-m_WorldSize.x, m_WorldSize.x), Random.Range(-m_WorldSize.y, m_WorldSize.y), 0f);

        GameObject tempFood = Instantiate(m_FoodPrefab);
        tempFood.transform.position = foodPos;
    }


    private void RecordData()
    {
        
    }
}
