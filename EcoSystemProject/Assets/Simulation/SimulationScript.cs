using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public struct DataRecord
{

    public DataRecord(uint tStep, uint blips, float blipSpeed, float blipVision, uint bBorn, uint bDied
                                , uint preds, float predSpeed, float predVision, uint pBorn, uint pDied)
    {
        timeStep = tStep;

        nrBlips = blips;
        blipAvgSpeed = blipSpeed;
        blipAvgVision = blipVision;
        blipsBorn = bBorn;
        blipsDied = bDied;

        nrPredators = preds;
        predAvgSpeed = predSpeed;
        predAvgVision = predVision;
        predsBorn = pBorn;
        predsDied = pDied;
    }

    //TIME
    public uint timeStep;

    //BLIP STATS
    public uint nrBlips;
    public float blipAvgSpeed;
    public float blipAvgVision;
    public uint blipsBorn;
    public uint blipsDied;


    //PREDATOR STATS
    public uint nrPredators;
    public float predAvgSpeed;
    public float predAvgVision;
    public uint predsBorn;
    public uint predsDied;
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


        //initialize Blip Stating Population
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

            float speed0 = Random.Range(m_BlipMinSpeed, m_BlipMaxSpeed);
            float speed1 = Random.Range(m_BlipMinSpeed, m_BlipMaxSpeed);
            float vision0 = Random.Range(m_BlipMinVisionRange, m_BlipMaxVisionRange);
            float vision1 = Random.Range(m_BlipMinVisionRange, m_BlipMaxVisionRange);



            //set Genetics
            Genetics startGenetics = new Genetics(speed0, speed1, vision0, vision1);



            blipScript.InitializeBlip(startGenetics, Random.Range(0, m_BlipLifeSpan / 2));
        }

        //initialize Predator Stating Population
        for (int i = 0; i < m_InitialNrPredators; i++)
        {
            //random position
            Vector3 predPos = new Vector3(Random.Range(-m_WorldSize.x, m_WorldSize.x), Random.Range(-m_WorldSize.y, m_WorldSize.y), 0f);

            //random rotation
            Quaternion predRot = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.forward);

            //create blip
            GameObject tempPredator = Instantiate(m_PredatorPrefab);
            tempPredator.name = "PREDATOR";

            tempPredator.transform.position = predPos;
            tempPredator.transform.rotation = predRot;
            Predator blipScript = tempPredator.GetComponent<Predator>();
            if (blipScript == null)
            {
                blipScript = tempPredator.AddComponent<Predator>();
            }

            float speed0 = Random.Range(m_PredatorMinSpeed, m_PredatorMaxSpeed);
            float speed1 = Random.Range(m_PredatorMinSpeed, m_PredatorMaxSpeed);
            float vision0 = Random.Range(m_PredatorMinVisionRange, m_PredatorMaxVisionRange);
            float vision1 = Random.Range(m_PredatorMinVisionRange, m_PredatorMaxVisionRange);



            //set Genetics
            Genetics startGenetics = new Genetics(speed0, speed1, vision0, vision1);



            blipScript.InitializePredator(startGenetics, Random.Range(0, m_BlipLifeSpan / 2));
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
            m_Predators = FindObjectsOfType<Predator>();




            for (int i = 0; i < m_AmountOfFood - m_Foods.Length; ++i)
            {
                SpawnFood();
            }

            RecordData();

            m_BlipsBorn = 0;
            m_BlipsDied = 0;
            m_PredsBorn = 0;
            m_PredsDied = 0;

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

    //simulation getters
    public bool UpdateSimulation() => m_UpdateSimulation;
    public Vector3 GetWorldSize() => m_WorldSize;
    public float GetTimeStep() => m_TimeStep;
    //mutation getters
    public float GetMutationChance() => m_MutationChance;
    public float GetMutationAmount() => m_MutationAmount;
    //blip getters
    public int GetBlipMatingTime() => m_BlipMatingTime;
    public int GetBlipLifeSpan() => m_BlipLifeSpan;
    public float GetBlipMaxHunger() => m_BlipMaxHunger;
    public float GetBlipMatingCost() => m_BlipMatingCost;
    public int GetBlipMatingCooldown() => m_BlipMatingCooldown;
    //predator getters
    public int GetPredatorMatingTime() => m_PredatorMatingTime;
    public int GetPredatorLifeSpan() => m_PredatorLifeSpan;
    public float GetPredatorMaxHunger() => m_PredatorMaxHunger;
    public float GetPredatorMatingCost() => m_PredatorMatingCost;
    public int GetPredatorMatingCooldown() => m_PredatorMatingCooldown;
    //food getters
    public float GetBlipFoodEffectiveness() => m_BlipFoodEffectiveness;
    public float GetPredatorFoodEffectiveness() => m_PredatorFoodEffectiveness;

    //record when blips and predators are born & died
    public void BlipBorn()
    {
        m_BlipsBorn++;
    }
    public void BlipDied()
    {
        m_BlipsDied++;
    }
    public void PredatorBorn()
    {
        m_PredsBorn++;
    }
    public void PredatorDied()
    {
        m_PredsDied++;
    }


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

    //get closest Blip for mating
    public Blip GetClosestBlipPartner(Vector3 blipPos)
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

    public Blip GetClosestBlip(Vector3 pos)
    {
        if (m_Blips.Length <= 0)
            return null;

        float distance = float.MaxValue;
        Blip closestBlip = null;
        foreach (Blip blip in m_Blips)
        {
            if (!(pos == blip.gameObject.transform.position) && Vector3.Distance(blip.gameObject.transform.position, pos) < distance)
            {
                distance = Vector3.Distance(blip.transform.position, pos);
                closestBlip = blip;

            }
        }
        return closestBlip;
    }

    //get closest Predator for mating
    public Predator GetClosestPredatorPartner(Vector3 predatorPos)
    {
        if (m_Predators.Length <= 0)
            return null;

        float distance = float.MaxValue;
        Predator closestPartner = null;
        foreach (Predator predator in m_Predators)
        {
            if (!(predatorPos == predator.gameObject.transform.position) && Vector3.Distance(predator.gameObject.transform.position, predatorPos) < distance)
            {
                if (predator.AvailableForMating())
                {
                    distance = Vector3.Distance(predator.transform.position, predatorPos);
                    closestPartner = predator;
                }
            }
        }
        return closestPartner;
    }

    public Predator GetClosestPredator(Vector3 pos)
    {
        if (m_Predators.Length <= 0)
            return null;

        float distance = float.MaxValue;
        Predator closestPredator = null;
        foreach (Predator predator in m_Predators)
        {
            if (!(pos == predator.gameObject.transform.position) && Vector3.Distance(predator.gameObject.transform.position, pos) < distance)
            {
                distance = Vector3.Distance(predator.transform.position, pos);
                closestPredator = predator;
            }
        }
        return closestPredator;
    }



    ///++++++++++++++++++++
    /// SIMULATION SETTINGS
    ///++++++++++++++++++++

    [Header("Simulation Settings")]
    public string m_SimulationName;
    public float m_SetWorldSize = 100f;
    Vector2 m_WorldSize;

    [SerializeField]
    [Range(0.01f, 1f)]
    private float m_TimeStep = 1f;

    private float m_Timer;
    private bool m_UpdateSimulation = false;


    [Space(20)]



    [Header("Mutation Settings", order = 1)]
    [Range(0f, 1f)]
    [SerializeField]
    private float m_MutationChance = 0f;

    [Range(0f, 0.1f)]
    [SerializeField]
    private float m_MutationAmount = 0f;


    [Space(20)]


    //BLIPS 
    [Header("Blip Settings", order = 1)]
    [SerializeField]
    private GameObject m_BlipPrefab = null;
    [SerializeField]
    private int m_InitialNrBlips = 0;

    [Space(10)]
    [SerializeField]
    private int m_BlipLifeSpan = 100;
    [SerializeField]
    private float m_BlipMaxHunger = 100f;


    [Space(10)]
    [SerializeField]
    private int m_BlipMatingTime = 0;
    [Range(0f, 1f)]
    [SerializeField]
    private float m_BlipMatingCost = 0f;
    [SerializeField]
    private int m_BlipMatingCooldown = 0;

    [Header("Blip Starting Genetics", order = 1)]
    [SerializeField]
    private float m_BlipMinSpeed = 0f;
    [SerializeField]
    private float m_BlipMaxSpeed = 0f;

    [Space(10)]
    [SerializeField]
    private float m_BlipMinVisionRange = 0f;
    [SerializeField]
    private float m_BlipMaxVisionRange = 0f;


    [Space(20)]


    //PREDATORS
    [Header("Predator Settings", order = 1)]
    [SerializeField]
    private GameObject m_PredatorPrefab;
    [SerializeField]
    private int m_InitialNrPredators = 0;

    [Space(10)]
    [SerializeField]
    private int m_PredatorLifeSpan = 100;
    [SerializeField]
    private float m_PredatorMaxHunger = 100f;

    [Space(10)]
    [SerializeField]
    private int m_PredatorMatingTime = 0;

    [Range(0f, 1f)]
    [SerializeField]
    private float m_PredatorMatingCost = 0f;
    [SerializeField]
    private int m_PredatorMatingCooldown = 0;

    [Header("Predator Starting Genetics", order = 1)]
    [SerializeField]
    private float m_PredatorMinSpeed = 0f;
    [SerializeField]
    private float m_PredatorMaxSpeed = 0f;

    [Space(10)]
    [SerializeField]
    private float m_PredatorMinVisionRange = 0f;
    [SerializeField]
    private float m_PredatorMaxVisionRange = 0f;


    [Space(20)]

    //FOOD
    [Header("Food Settings", order = 1)]
    [SerializeField]
    private GameObject m_FoodPrefab;
    [SerializeField]
    private int m_AmountOfFood = 0;

    [Range(0f, 1f)]
    [SerializeField]
    private float m_BlipFoodEffectiveness = 1f;
    [Range(0f, 1f)]
    [SerializeField]
    private float m_PredatorFoodEffectiveness = 1f;


    //data
    private Blip[] m_Blips;
    private Food[] m_Foods;
    private Predator[] m_Predators;

    private uint m_BlipsBorn = 0;
    private uint m_BlipsDied = 0;
    private uint m_PredsBorn = 0;
    private uint m_PredsDied = 0;

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
        m_DataRecords.Add(new DataRecord(m_CurrentTimeStep, (uint)m_Blips.Length, AverageBlipSpeed(), AverageBlipVision(), m_BlipsBorn, m_BlipsDied, (uint)m_Predators.Length, AveragePredatorSpeed(), AveragePredatorVision(), m_PredsBorn, m_PredsDied));
    }

    private void WriteDataToFile(string path, string fileName)
    {
        //TODO: ADD MORE STATISTICS TO THE FILE WRITE (PREDATOR POP & GENETICS, DEATHS AND BIRTHS)

        //file content
        string fileContent = "";

        fileContent += "TimeStep\t Blip Population\t Blip Speed\t Blip Vision\t Blips Born\t Blips Died\t Predator Population\t Predator Speed\t Predator Vision\t Predators Born\t Predators Died\n";

        foreach (DataRecord record in m_DataRecords)
        {
            fileContent += record.timeStep.ToString() + "\t"
                + record.nrBlips.ToString() + "\t"
                + record.blipAvgSpeed.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR")) + "\t"
                + record.blipAvgVision.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR")) + "\t"
                + record.blipsBorn.ToString() + "\t"
                + record.blipsDied.ToString() + "\t"
                + record.nrPredators.ToString() + "\t"
                + record.predAvgSpeed.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR")) + "\t"
                + record.predAvgVision.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("fr-FR")) + "\t"
                + record.predsBorn.ToString() + "\t"
                + record.predsDied.ToString() + "\n";
        }


        //filename
        string file = path + fileName + ".txt";

        File.WriteAllText(file, fileContent);
    }


    //blip average speed
    private float AverageBlipSpeed()
    {
        float avgSpeed = 0f;
        foreach (Blip blip in m_Blips)
        {
            avgSpeed += blip.GetGenes().GetMaxSpeed();
        }

        avgSpeed /= m_Blips.Length;
        return avgSpeed;
    }

    //blip average vision range
    private float AverageBlipVision()
    {
        float avgVision = 0f;
        foreach (Blip blip in m_Blips)
        {
            avgVision += blip.GetGenes().GetVisionRange();
        }

        avgVision /= m_Blips.Length;
        return avgVision;
    }

    //predator average speed
    private float AveragePredatorSpeed()
    {
        float avgSpeed = 0f;
        foreach (Predator pred in m_Predators)
        {
            avgSpeed += pred.GetGenes().GetMaxSpeed();
        }

        avgSpeed /= m_Predators.Length;
        return avgSpeed;
    }

    //predator average vision range
    private float AveragePredatorVision()
    {
        float avgVision = 0f;
        foreach (Predator pred in m_Predators)
        {
            avgVision += pred.GetGenes().GetVisionRange();
        }

        avgVision /= m_Predators.Length;
        return avgVision;
    }




}
