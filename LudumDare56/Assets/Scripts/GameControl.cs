using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletionObject
{
    public float completionAmt = 0.0f;
    public bool doneFirst;
    public bool doneSecond;
    public bool doneThird;
    public bool doneFourth;
    public bool doneFifth;

    public CompletionObject()
    {
        completionAmt = 0.0f;
        doneFirst = false;
        doneSecond = false;
        doneThird = false;
        doneFourth = false;
        doneFifth = false;
}

    public bool firstChkPnt()
    {
        if (completionAmt >= 0.10f && doneFirst == false)
        {
            doneFirst = true;
            return true;
        }
        return false;
    }
    public bool secondChkPnt()
    {
        if (completionAmt >= 0.25f && doneSecond == false)
        {
            doneSecond = true;
            return true;
        }
        return false;
    }
    public bool thirdChkPnt()
    {
        if (completionAmt >= 0.50f && doneThird == false)
        {
            doneThird = true;
            return true;
        }
        return false;
    }
    public bool fourthChkPnt()
    {
        if (completionAmt >= 0.75f && doneFourth == false)
        {
            doneFourth = true;
            return true;
        }
        return false;
    }
    public bool fifthChkPnt()
    {
        if (completionAmt == 1.00f && doneFifth == false)
        {
            doneFifth = true;
            return true;
        }
        return false;
    }

    public float calcPerc(int x, int y)
    {
        return completionAmt = Mathf.Round(((float) x / y) * 100.0f) * 0.01f;
    }

}


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

    // current values for food maps
    public int totalLeafFood;
    public int totalAppleFood;
    public int totalMeatFood;
    public int totalCarterFood;
    public int totalAtmFood;
    public int totalCarFood;
    public int totalHouseFood;
    public int totalNuclearFood;

    // initial total values for food maps
    public int initialLeafFood;
    public int initialAppleFood;
    public int initialMeatFood;
    public int initialCarterFood;
    public int initialAtmFood;
    public int initialCarFood;
    public int initialHouseFood;
    public int initialNuclearFood;

    // initialize completion objects
    public CompletionObject leafCompletion;
    public CompletionObject appleCompletion;
    public CompletionObject meatCompletion;
    public CompletionObject carterCompletion;
    public CompletionObject atmCompletion;  
    public CompletionObject carCompletion;
    public CompletionObject houseCompletion;
    public CompletionObject nuclearCompletion;


    // super ant storm cooldown
    public float superCooldown = -1;

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
    public int basePoints = 0;
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
    void FixedUpdate()
    {
        if (initialAppleFood != 0)
        {
            // update each completion object's completion value
            leafCompletion.calcPerc(initialLeafFood - totalLeafFood, initialLeafFood);
            appleCompletion.calcPerc(initialAppleFood - totalAppleFood, initialAppleFood);
            meatCompletion.calcPerc(initialMeatFood - totalMeatFood, initialMeatFood);
            carterCompletion.calcPerc(initialCarterFood - totalCarterFood, initialCarterFood);
            atmCompletion.calcPerc(initialAtmFood - totalAtmFood, initialAtmFood);
            carCompletion.calcPerc(initialCarFood - totalCarFood, initialCarFood);
            houseCompletion.calcPerc(initialHouseFood - totalHouseFood, initialHouseFood);
            nuclearCompletion.calcPerc(initialNuclearFood - totalNuclearFood, initialNuclearFood);
            // Debug.Log("apple comlpetion: " + appleCompletion.completionAmt);

            // check each completion object's state
            if (leafCompletion.firstChkPnt() || leafCompletion.secondChkPnt() || leafCompletion.thirdChkPnt() || leafCompletion.fifthChkPnt())
                skillPoints++;
            if (leafCompletion.fourthChkPnt()) basePoints++;

            if (appleCompletion.firstChkPnt() || appleCompletion.secondChkPnt() || appleCompletion.thirdChkPnt() || appleCompletion.fifthChkPnt())
                skillPoints++;
            if (appleCompletion.fourthChkPnt()) basePoints++;

            if (meatCompletion.firstChkPnt() || meatCompletion.secondChkPnt() || meatCompletion.thirdChkPnt() || meatCompletion.fifthChkPnt())
                skillPoints++;
            if (meatCompletion.fourthChkPnt()) basePoints++;

            if (carterCompletion.firstChkPnt() || carterCompletion.secondChkPnt() || carterCompletion.thirdChkPnt() || carterCompletion.fifthChkPnt())
                skillPoints++;
            if (carterCompletion.fourthChkPnt()) basePoints++;

            if (atmCompletion.firstChkPnt() || atmCompletion.secondChkPnt() || atmCompletion.thirdChkPnt() || atmCompletion.fifthChkPnt())
                skillPoints++;
            if (atmCompletion.fourthChkPnt()) basePoints++;

            if (carCompletion.firstChkPnt() || carCompletion.secondChkPnt() || carCompletion.thirdChkPnt() || carCompletion.fifthChkPnt())
                skillPoints++;
            if (carCompletion.fourthChkPnt()) basePoints++;

            if (houseCompletion.firstChkPnt() || houseCompletion.secondChkPnt() || houseCompletion.thirdChkPnt() || houseCompletion.fifthChkPnt())
                skillPoints++;
            if (houseCompletion.fourthChkPnt()) basePoints++;

            if (nuclearCompletion.firstChkPnt() || nuclearCompletion.secondChkPnt() || nuclearCompletion.thirdChkPnt() || nuclearCompletion.fifthChkPnt())
                skillPoints++;
            if (nuclearCompletion.fourthChkPnt()) basePoints++;
        }
    }

    void Awake()
    {
        GameController = this;
        DontDestroyOnLoad(gameObject);
        UpgradeInit();


        // initialize all completion objects
        leafCompletion = new CompletionObject();
        appleCompletion = new CompletionObject();
        meatCompletion = new CompletionObject();
        carterCompletion = new CompletionObject();
        atmCompletion = new CompletionObject();
        carCompletion = new CompletionObject();
        houseCompletion = new CompletionObject();
        nuclearCompletion = new CompletionObject();
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
