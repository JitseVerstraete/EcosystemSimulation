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

        m_BlipMatingTime = m_SetBlipMatingTime;
        m_BlipLifeSpan = m_SetBlipLifespan;
        m_BlipMaxHunger = m_SetBlipMaxHunger;

        m_FoodEffectiveness = m_SetFoodEffectiveness;

        m_MatingCost = m_SetMatingCost;
        m_MatingCooldown = m_SetMatingCooldown;

        m_MutationAmount = m_SetMutationAmount;
        m_MutationChance = m_SetMutationChance;

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
            float vision0 = Random.Range(m_MinVisionRange, m_MaxVisionRange);
            float vision1 = Random.Range(m_MinVisionRange, m_MaxVisionRange);



            //set Genetics
            Genetics startGenetics = new Genetics(speed0, speed1, vision0, vision1);



            blipScript.InitializeBlip(startGenetics);
        }

        //initialize food
        for (int i = 0; i < m_AmountOfFood; i++)
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


            GameObject[] blips = GameObject.FindGameObjectsWithTag("Food");
            for (int i = 0; i < m_AmountOfFood - blips.Length; ++i)
            {
                SpawnFood();
            }
            m_QueuedFoods = 0;
            print("sup");
        }
        else
        {
            m_UpdateSimulation = false;
        }

    }

    static public bool UpdateSimulation() => m_UpdateSimulation;
    static public Vector3 GetWorldSize() => m_WorldSize;
    static public float GetTimeStep() => m_StaticTimeStep;
    static public void QueueNewFoodSpawn()
    {
        ++m_QueuedFoods;
    }
    static public int GetBlipMatingTime() => m_BlipMatingTime;
    static public int GetBlipLifeSpan() => m_BlipLifeSpan;
    static public float GetBlipMaxHunger() => m_BlipMaxHunger;
    static public float GetFoodEffectiveness() => m_FoodEffectiveness;
    static public float GetMatingCost() => m_MatingCost;
    static public int GetMatingCooldown() => m_MatingCooldown;
    static public float GetMutationChance() => m_MutationChance;
    static public float GetMutationAmount() => m_MutationAmount;

    static public Vector3 GetClosestFood(Vector3 blipPos)
    {
        GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");
        if (foods.Length <= 0)
            return new Vector2(float.MaxValue, float.MaxValue);


        float distance = float.MaxValue;
        Vector3 closestFoodPos = new Vector2(float.MaxValue, float.MaxValue);
        foreach (GameObject food in foods)
        {
            if (Vector3.Distance(food.transform.position, blipPos) < distance)
            {
                distance = Vector3.Distance(food.transform.position, blipPos);
                closestFoodPos = food.transform.position;
            }
        }


        return closestFoodPos;
    }

    static public Blip GetClosestPartner(Vector3 blipPos)
    {
        GameObject[] blips = GameObject.FindGameObjectsWithTag("Blip");
        if (blips.Length <= 0)
            return null;

        float distance = float.MaxValue;
        Blip closestPartner = null;
        foreach (GameObject blip in blips)
        {

            if (!(blipPos == blip.transform.position) && Vector3.Distance(blip.transform.position, blipPos) < distance)
            {
                Blip blipComp = blip.GetComponent<Blip>();
                if (blipComp.AvailableForMating())
                {
                    distance = Vector3.Distance(blip.transform.position, blipPos);
                    closestPartner = blipComp;
                }
            }
        }

        return closestPartner;
    }

    ///++++++++++++++++++++
    /// SIMULATION SETTINGS
    ///++++++++++++++++++++

    [Header("Simulation Settings")]
    public float m_TimeStep = 3f;
    public float m_SetWorldSize = 100f;

    private float m_Timer;
    static private bool m_UpdateSimulation = false;
    static private float m_StaticTimeStep;
    static Vector2 m_WorldSize;

    [Header("Mutation Settings")]
    [Range(0f,1f)]
    public float m_SetMutationChance;
    static private float m_MutationChance;
    [Range(0f, 0.1f)]
    public float m_SetMutationAmount;
    static private float m_MutationAmount;


    //BLIPS 
    [Header("Blip Settings")]
    public GameObject m_BlipPrefab;
    public int m_InitialNrBlips = 0;
    [Space(10)]
    public int m_SetBlipLifespan = 10;
    public float m_SetBlipMaxHunger = 100f;
    static int m_BlipLifeSpan;
    static float m_BlipMaxHunger;
    [Space(10)]
    public float m_MinSpeed;
    public float m_MaxSpeed;
    [Space(10)]
    public int m_SetBlipMatingTime;
    static int m_BlipMatingTime;
    [Range(0f, 1f)]
    public float m_SetMatingCost;
    static private float m_MatingCost;
    public int m_SetMatingCooldown;
    static private int m_MatingCooldown;

    [Space(10)]
    public float m_MinVisionRange;
    public float m_MaxVisionRange;


    //FOOD
    [Header("Food Settings")]
    public GameObject m_FoodPrefab;
    public int m_AmountOfFood = 0;
    [Range(0f, 1f)]
    public float m_SetFoodEffectiveness;
    static private float m_FoodEffectiveness;
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
