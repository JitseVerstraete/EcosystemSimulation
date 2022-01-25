using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct DataRecord
{

    public DataRecord(uint tStep, uint blips)
    {
        timeStep = tStep;
        nrBlips = blips;
    }


    public uint timeStep;    
    public uint nrBlips;

}

public class SimulationScript : MonoBehaviour
{

    private static SimulationScript m_Instance;
    static public SimulationScript Instance { get { return m_Instance; } }

    //singleton stuff
    private void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_Instance = this;
        }
    }


    void Start()
    {
        //setup the simulation here
        m_Timer = m_TimeStep;
        m_WorldSize.x = m_SetWorldSize * Camera.main.aspect;
        m_WorldSize.y = m_SetWorldSize;


        //initialize Stating Population
        for (int i = 0; i < m_InitialNrBlips; i++)
        {
            //random position
            Vector3 blipPos = new Vector3(Random.Range(-m_WorldSize.x, m_WorldSize.x), Random.Range(-m_WorldSize.y, m_WorldSize.y), 0f);

            //random rotation
            Quaternion blipRot = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);

            //create blip
            GameObject tempBlip = Instantiate(m_BlipPrefab);
            tempBlip.name = "BLIP";

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



            blipScript.InitializeBlip(startGenetics, Random.Range(0, m_BlipLifeSpan / 2));
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

            //record current foods and blips
            m_Blips = FindObjectsOfType<Blip>();
            m_Foods = FindObjectsOfType<Food>();


            GameObject[] foods = GameObject.FindGameObjectsWithTag("Food");
            for (int i = 0; i < m_AmountOfFood - foods.Length; ++i)
            {
                SpawnFood();
            }

            RecordData();

            m_CurrentTimeStep++;
        }
        else
        {
            m_UpdateSimulation = false;
        }

    }

    private void OnDestroy()
    {
        //write data to file
        WriteDataToFile(m_FilePath, m_SimulationName);
    }

    public bool UpdateSimulation() => m_UpdateSimulation;
    public Vector3 GetWorldSize() => m_WorldSize;
    public float GetTimeStep() => m_TimeStep;
    public int GetBlipMatingTime() => m_BlipMatingTime;
    public int GetBlipLifeSpan() => m_BlipLifeSpan;
    public float GetBlipMaxHunger() => m_BlipMaxHunger;
    public float GetFoodEffectiveness() => m_FoodEffectiveness;
    public float GetMatingCost() => m_MatingCost;
    public int GetMatingCooldown() => m_MatingCooldown;
    public float GetMutationChance() => m_MutationChance;
    public float GetMutationAmount() => m_MutationAmount;

    public Food GetClosestFood(Vector3 blipPos)
    {
        if (m_Foods.Length <= 0)
            return null;

        float distance = float.MaxValue;
        Food closestFood = null;
        foreach (Food food in m_Foods)
        {
            if (food != null && Vector3.Distance(food.transform.position, blipPos) < distance)
            {
                distance = Vector3.Distance(food.transform.position, blipPos);
                closestFood = food;
            }
        }

        return closestFood;
    }

    public Blip GetClosestPartner(Vector3 blipPos)
    {
        if (m_Blips.Length <= 0)
            return null;

        float distance = float.MaxValue;
        Blip closestPartner = null;
        foreach (Blip blip in m_Blips)
        {

            if (!(blipPos == blip.gameObject.transform.position) && Vector3.Distance(blip.gameObject.transform.position, blipPos) < distance)
            {
                if (blip.AvailableForMating())
                {
                    distance = Vector3.Distance(blip.transform.position, blipPos);
                    closestPartner = blip;
                }
            }
        }
        return closestPartner;
    }

    ///++++++++++++++++++++
    /// SIMULATION SETTINGS
    ///++++++++++++++++++++

    [Header("Simulation Settings")]
    public string m_SimulationName;
    public float m_SetWorldSize = 100f;
    Vector2 m_WorldSize;

    [SerializeField]
    private float m_TimeStep = 1f;

    private float m_Timer;
    private bool m_UpdateSimulation = false;


    [Header("Mutation Settings")]

    [Range(0f, 1f)]
    [SerializeField]
    private float m_MutationChance = 0f;

    [Range(0f, 0.1f)]
    [SerializeField]
    private float m_MutationAmount = 0f;


    //BLIPS 
    [Header("Blip Settings")]
    public GameObject m_BlipPrefab;
    public int m_InitialNrBlips = 0;

    [Space(10)]
    [SerializeField]
    private int m_BlipLifeSpan = 100;
    [SerializeField]
    private float m_BlipMaxHunger = 100f;

    [Space(10)]
    public float m_MinSpeed;
    public float m_MaxSpeed;

    [Space(10)]
    public float m_MinVisionRange;
    public float m_MaxVisionRange;

    [Space(10)]
    [SerializeField]
    private int m_BlipMatingTime = 0;

    [Range(0f, 1f)]
    [SerializeField]
    private float m_MatingCost = 0f;
    [SerializeField]
    private int m_MatingCooldown = 0;


    //FOOD
    [Header("Food Settings")]
    public GameObject m_FoodPrefab;
    public int m_AmountOfFood = 0;

    [Range(0f, 1f)]
    [SerializeField]
    private float m_FoodEffectiveness = 1f;


    //data
    private Blip[] m_Blips;
    private Food[] m_Foods;
    uint m_CurrentTimeStep;


    //DATA RECORDING
    const string m_FilePath = "../SimulationData/DataFiles/";
    List<DataRecord> m_DataRecords = new List<DataRecord>();
    



    private void SpawnFood()
    {
        Vector3 foodPos = new Vector3(Random.Range(-m_WorldSize.x, m_WorldSize.x), Random.Range(-m_WorldSize.y, m_WorldSize.y), 0f);

        GameObject tempFood = Instantiate(m_FoodPrefab);
        tempFood.transform.position = foodPos;
    }


    private void RecordData()
    {
        m_DataRecords.Add(new DataRecord(m_CurrentTimeStep, (uint)m_Blips.Length));
    }

    private void WriteDataToFile(string path, string fileName)
    {
     

        //file content
        string fileContent = "";

        fileContent += "TimeStep, Blip Population\n";

        foreach(DataRecord record in m_DataRecords)
        {
            fileContent += record.timeStep.ToString() + "," + record.nrBlips.ToString() + "\n"; ;
        }


        //filename
        string file = path + fileName + ".txt";

        File.WriteAllText(file, fileContent);
    }
}
