using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

// encapsulation class for food completion behavior
public class CompletionObject
{
    public float completionAmt = 0.0f;
    public string name;
    public bool doneFirst;
    public bool doneSecond;
    public bool doneThird;
    public bool doneFourth;
    public bool doneFifth;

    public CompletionObject(string name)
    {
        completionAmt = 0.0f;
        this.name = name;
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
            GameControl.GameController.writeText(name + " 10% consumed, skill point granted");
            doneFirst = true;
            return true;
        }
        return false;
    }
    public bool secondChkPnt()
    {
        if (completionAmt >= 0.25f && doneSecond == false)
        {
            GameControl.GameController.writeText(name + " 25% consumed, skill point granted");
            doneSecond = true;
            return true;
        }
        return false;
    }
    public bool thirdChkPnt()
    {
        if (completionAmt >= 0.50f && doneThird == false)
        {
            GameControl.GameController.writeText(name + " 50% consumed, skill point granted");
            doneThird = true;
            return true;
        }
        return false;
    }
    public bool fourthChkPnt()
    {
        if (completionAmt >= 0.75f && doneFourth == false)
        {
            GameControl.GameController.writeText(name + " 75% consumed, base point granted");
            doneFourth = true;
            return true;
        }
        return false;
    }
    public bool fifthChkPnt()
    {
        if (completionAmt == 1.00f && doneFifth == false)
        {
            GameControl.GameController.writeText(name + " 100% consumed, skill point granted. Your Ants grow hungry");
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
    #region
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
    public int exoLvl = 0;
    public int luckLvl = 0;
    public int pestLvl = 0;
    public int superLvl = 0;
    public int radLvl = 0;

    //all skill levels here
    public float[] antSpeed = { 2.5f, 4f, 6f, 10f };
    public float[] antLifespan = { 30f, 45f, 60f, 90f };
    public float[] antSize = { 1f, 2f, 4f, 8f };
    public float[] antStrength = { 1f, 2f, 4f, 8f };
    public float[] antSpawn = { 4f, 3f, 2f, 1f };
    public float[] exoSkeleton = { 0, 0.1f, 0.2f, 0.3f};
    public float[] luckyAnts = { 0f, 0.01f, 0.02f, 0.05f};
    public float[] pestResist = { 0f, 0.1f, 0.2f, 0.3f };
    public float[] superAnts = { -1f, 150f, 120f, 90f};
    public float[] radResist = { 0f, 0.1f, 0.2f, 0.3f };

    public List<float[]> antUpgrades;
    public List<int> antLevels;

    //stats
    public int basePoints = 2;
    public int skillPoints = 0;
    public int activeAnts = 0;
    public int deadAnts = 0;
    public int foodConsumed = 0;
    public int antsConsumed = 0;
    public int baseIndex = 0;

    // ant behavior values
    public float harvest = 1.0f;
    public float explore = 0.0f;

    // quota to reach
    private int quota = 20;
    public int timeChunk = 20;
    public float globalClock = 1.0f;

    // list for all spawners
    public GameObject[] spawnerList;

    // text field
    public TMP_Text textfield;

    // event probability
    public float eventProb = 0.01f;
    public int radEventWeight = 1;
    public int chemEventWeight = 1;
    public int attkEventWeight = 1;

    // event flag
    public bool triggered = false;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // just a check to ensure we only calculate this when there is actual data loaded
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

        // -----------------------------------------------------
        // Time handler for quota system + EventTrigger     TO DO: IMPLEMENT SCENE CHANGES FOR LOSING QUOTA HERE
        // -----------------------------------------------------
        globalClock += Time.fixedDeltaTime;
        //Debug.Log((int)globalClock);
        //Debug.Log(timeChunk);
        if ((int)globalClock == timeChunk && triggered == false)
        {
            Debug.Log("timechunk is working");
            // failed to meet quota
            if (foodConsumed + antsConsumed < quota)
            { Debug.Log("YOU LOST!!!!"); }
            // met quota
            else
            {
                // set new quota
                quota = quota + ((baseIndex + 1) * quota);
                // set new time limit
                timeChunk += 20;
            }

            // run code for checking if event will occur
            if (triggerEvent())
            {
                triggered = true;
            }
        }
        else if ((int)globalClock != timeChunk)
            triggered = false;

        // --------------------------------------------------
        // Win state Handler TO DO: IMPLEMENT SCENE CHANGES FOR WINNING GAME HERE
        //---------------------------------------------------
        if (checkWinState())
            Debug.Log("YOU WON");

    }

    void Awake()
    {
        GameController = this;
        DontDestroyOnLoad(gameObject);
        UpgradeInit();


        // initialize all completion objects
        leafCompletion = new CompletionObject("Leaf");
        appleCompletion = new CompletionObject("Apple");
        meatCompletion = new CompletionObject("Meat");
        carterCompletion = new CompletionObject("Carter Statue");
        atmCompletion = new CompletionObject("ATM");
        carCompletion = new CompletionObject("PT Cruiser");
        houseCompletion = new CompletionObject("A Family Guy's House");
        nuclearCompletion = new CompletionObject("Nuclear Power Plant");

        spawnerList = GameObject.FindGameObjectsWithTag("spawners");
        spawnerList = spawnerList.OrderBy(x => x.transform.position.x).ToArray<GameObject>();
        // fill spawner list via tag
        //foreach (GameObject spawner in GameObject.FindGameObjectsWithTag("spawners").OrderBy(x => x.transform.position.x));

        for (int i = 0; i <  spawnerList.Length; i++)
        {
            spawnerList[i].SetActive(false);
        }
        spawnerList[0].SetActive(true);
    }

    public void UpgradeInit()
    {
        antLevels = new List<int>
        {
            speedLvl,
            lifespanLvl,
            sizeLvl,
            strengthLvl,
            spawnLvl,
            exoLvl,
            luckLvl,
            pestLvl,
            superLvl,
            radLvl
        };

        antUpgrades = new List<float[]>
        {
            antSpeed,
            antLifespan,
            antSize,
            antStrength,
            antSpawn,
            exoSkeleton,
            luckyAnts,
            pestResist,
            superAnts,
            radResist
        };
    }

    public void UpgradeUpdate()
    {
        speedLvl = antLevels[0];
        lifespanLvl = antLevels[1];
        sizeLvl = antLevels[2];
        strengthLvl = antLevels[3];
        spawnLvl = antLevels[4];
        exoLvl = antLevels[5];
        luckLvl = antLevels[6];
        pestLvl = antLevels[7];
        superLvl = antLevels[8];
        radLvl = antLevels[9];
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

    // returns true if there are FoodIndexes in given indexed foodMap, returns false if foodMap is empty
    public bool FetchFoodMapState(int index)
    {
        switch (index)
        {
            case 0:
                if (totalLeafFood > 0) return true; break;
            case 1:
                if (totalAppleFood > 0) return true; break;
            case 2:
                if (totalMeatFood > 0) return true; break;
            case 3:
                if (totalCarterFood > 0) return true; break;
            case 4:
                if (totalAtmFood > 0) return true; break;
            case 5:
                if (totalCarFood > 0) return true; break;
            case 6:
                if (totalHouseFood > 0) return true; break;
            case 7:
                if (totalNuclearFood > 0) return true; break;
        }
        
        return false;
    }

    // function to check win status (all food objects completely consumed
    private bool checkWinState()
    {
        if ((totalLeafFood == 0) && (totalAppleFood == 0) && (totalMeatFood == 0) && (totalCarterFood == 0) && (totalAtmFood == 0) && (totalCarFood == 0) && (totalHouseFood == 0) && (totalNuclearFood == 0))
            return true;

        return false;
    }

    // function for handling slider UI
    public void SliderChanged(float actionVal)
    {
        harvest = 1.0f - actionVal;
        explore = actionVal;
    }

    // function for handling upgradeBase button
    public void UpgradeBase()
    {
        // if player has any basePoints to spend
        if (basePoints > 0)
        {
            // ensure there is a next base in the list
            if (baseIndex <= spawnerList.Length - 1)
            {
                // turn off spawner, turn on new spawner
                spawnerList[baseIndex].gameObject.SetActive(false);
                baseIndex++;
                spawnerList[baseIndex].gameObject.SetActive(true);

                // remove basePoints
                basePoints--;
            }
            else
            {
                writeText("No More Bases to Buy!");
            }
        }
        else { writeText("Not Enough Base Points!"); }
    }

    // function for updating textfield
    public void writeText(string text)
    {
        textfield.text += text + "\n";
    }

    // returns true if given probability parameters
    public bool triggerEvent()
    {
        // base chance of triggering event check
        int randomInt = Random.Range(0, 100);

        // event occurs
        if ((eventProb * 100f) - randomInt > 0)
        {
            generateInconvenience();

            // reduce probability by a little to prevent event overflow
            eventProb -= (Mathf.Abs(eventProb - 1)) / 2;
            return true;
        }
        // event did not occur
        else
        {
            Debug.Log("didn't generate inconvenience");
            eventProb += 0.01f;
            return false;
        }
    }

    // 
    public void generateInconvenience()
    {
        Debug.Log("Generating inconveince");
        // figure out the type of event
        // sum all weights
        int weightSum = (radLvl + 1) * 10 + (pestLvl + 1) * 10 + (exoLvl + 1) * 10;

        int randomNum = Random.Range(0, weightSum);

        // grab all ants rn
        List<GameObject> currentAnts = GameObject.FindGameObjectsWithTag("ant").ToList();
        // check brackets

        // radiation event happened
        if (randomNum >= 0 && randomNum <= radEventWeight)
        {
            // all ants lifespan reduced by 1/2
            foreach (GameObject ant in currentAnts)
            {
                ant.GetComponent<Ant>().deathTimer = ant.GetComponent<Ant>().deathTimer / (2 * (1+radLvl));
            }

            // randomly choose which blurb of text to write:
            int randText = Random.Range(0, 2);
            if (randText == 0)  writeText("Your ants brought back a toxic water supply and all your ants lifespans are cut in half!");
            if (randText == 1)  writeText("Your ants broke into a nuclear waste dumpsite. Your ants have reduced life expectancy");
            if (randText == 2) writeText("Your ants brought radioactive debris to the nest. Your ants' days are numbered");

        }
        // chemical event happened
        else if (randomNum > radEventWeight && randomNum <= (radEventWeight+chemEventWeight))
        {
            for (int i = 0; i < currentAnts.Count; i += pestLvl + 1)
            {
                currentAnts[i].gameObject.GetComponent<Ant>().deathTimer = 0;
            }

            // randomly choose which blurb of text to write:
            int randText = Random.Range(0, 2);
            if (randText == 0) writeText("Humans discovered your trails and sprayed PAID all over");
            if (randText == 1) writeText("You drew too much attention to yourself and the neighborhood called pest exterminators");
            if (randText == 2) writeText("Anti-ant propaganda has spread throughout the neighborhood. Homeowners are taking up arms");
        }
        // attack event happened
        else
        {
            for (int i = 0; i < currentAnts.Count; i +=exoLvl + 1)
            {
                currentAnts[i].gameObject.GetComponent<Ant>().deathTimer = 0;
            }

            // randomly choose which blurb of text to write
            int randText = Random.Range(0, 2);
            if (randText == 0) writeText("Your colony was attacked by a massive ugly lizard and suffered losses");
            if (randText == 1) writeText("Cordiceps infected your colony, causing an internal mutiny and mandatory culling");
            if (randText == 2) writeText("Rival ants ambushed your colony! Losses were kept to a minimum but still hurt");
        }
    }
}
