using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl GameController;
    public FoodIndex[,] leafMap;
    public FoodIndex[,] appleMap;
    public FoodIndex[,] meatMap;
    public FoodIndex[,] carterMap;
    public FoodIndex[,] atmMap;
    public FoodIndex[,] carMap;
    public FoodIndex[,] houseMap;
    public FoodIndex[,] nuclearMap;

    //ant stats
    public int speedLvl = 0;
    public int lifespanLvl = 0;
    public int sizeLvl = 0;
    public int strengthLvl = 0;
    public int spawnLvl = 0;

    public float[] antSpeed = { 2.5f, 4f, 6f, 10f };
    public float[] antLifespan = { 30f, 45f, 60f, 90f };
    public float[] antSize = { 1f, 2f, 4f, 8f };
    public float[] antStrength = { 1f, 2f, 4f, 8f };
    public float[] antSpawn = { 4f, 3f, 2f, 1f };

    public List<float[]> antUpgrades;
    public List<int> antLevels;

    //specializations - needs fine tuning
    public float pestResist = 1f;
    public float antAggression = 1f;
    public float radResist = 1f;
    public float exoSkel = 1f;

    //stats
    public int skillPoints = 0;
    public int activeAnts = 0;
    public int deadAnts = 0;
    public int foodConsumed = 0;
    public int antsConsumed = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        GameController = this;
        DontDestroyOnLoad(gameObject);
        UpgradeInit();
    }

    public void UpgradeInit()
    {
        antLevels = new List<int>
        {
            speedLvl,
            lifespanLvl,
            sizeLvl,
            strengthLvl,
            spawnLvl
        };

        antUpgrades = new List<float[]>
        {
            antSpeed,
            antLifespan,
            antSize,
            antStrength,
            antSpawn
        };
    }

    public void UpgradeUpdate()
    {
        speedLvl = antLevels[0];
        lifespanLvl = antLevels[1];
        sizeLvl = antLevels[2];
        strengthLvl = antLevels[3];
        spawnLvl = antLevels[4];
    }

    public float GetSpeed()
    {
        return antSpeed[speedLvl];
    }
    public float GetLifespan()
    {
        return antLifespan[lifespanLvl];
    }
    public float GetSize()
    {
        return antSize[sizeLvl];
    }
    public float GetStrength()
    {
        return antStrength[strengthLvl];
    }
    public float GetSpawn()
    {
        return antSpawn[spawnLvl];
    }
}
