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

            float speed0 = Random.Range(m_MinSpeed, m_MaxSpeed);
            float speed1 = Random.Range(m_MinSpeed, m_MaxSpeed);
            float reproUrge0 = Random.Range(m_MinReproductiveUrge, m_MaxReproductiveUrge);
            float reproUrge1 = Random.Range(m_MinReproductiveUrge, m_MaxReproductiveUrge);
            float vision0 = Random.Range(m_MinVisionRange, m_MaxVisionRange);
            float vision1 = Random.Range(m_MinVisionRange, m_MaxVisionRange);



            //set Genetics
            Genetics startGenetics = new Genetics(speed0, speed1, reproUrge0, reproUrge1, vision0, vision1);



            blipScript.InitializeBlip(m_BlipMaxHunger, m_BlipLifespan, startGenetics);
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

    static public void QueueNewFoodSpawn()
    {
        ++m_QueuedFoods;
    }

    static public Vector3 GetClosestFood(Vector3 blipPos)
    {
        GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");
        if(foods.Length <= 0)
            return new Vector2(float.MaxValue, float.MaxValue);
        

        float distance = float.MaxValue;
        Vector3 closestFoodPos = new Vector2(float.MaxValue, float.MaxValue);
        foreach (GameObject food in foods)
        {
            if(Vector3.Distance(food.transform.position, blipPos) < distance)
            {
                distance = Vector3.Distance(food.transform.position, blipPos);
                closestFoodPos = food.transform.position;
            }
        }


        return closestFoodPos;
    }



    ///++++++++++++++++++++
    /// SIMULATION SETTINGS
    ///++++++++++++++++++++

    [Header("SIMULATION SETTINGS")]
    public float m_TimeStep = 3f;
    public float m_SetWorldSize = 100f;

    private float m_Timer;
    static private bool m_UpdateSimulation = false;
    static private float m_StaticTimeStep;
    static Vector2 m_WorldSize;


    //BLIPS 
    [Header("Blip Settings")]
    public GameObject m_BlipPrefab;
    public int m_InitialNrBlips = 0;
    [Space(10)]
    public int m_BlipLifespan = 10;
    public float m_BlipMaxHunger = 100f;
    [Space(10)]
    public float m_MinSpeed;
    public float m_MaxSpeed;
    [Space(10)]
    public float m_MinReproductiveUrge;
    public float m_MaxReproductiveUrge;
    [Space(10)]
    public float m_MinVisionRange;
    public float m_MaxVisionRange;





    //FOOD
    [Header("Food Settings")]
    public GameObject m_FoodPrefab ;
    public int m_AmountOfFood = 0;

    static private int m_QueuedFoods;




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
