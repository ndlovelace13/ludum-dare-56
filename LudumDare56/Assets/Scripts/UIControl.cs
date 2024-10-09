using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [SerializeField] GameObject MutationsTrigger;
    [SerializeField] GameObject MutationsMenu;
    [SerializeField] GameObject swarmButton;
    [SerializeField] TMP_Text statsField;
    [SerializeField] TMP_Text currentQuota;
    bool mutationsActive = false;

    //swarm handling vars
    bool canSwarm = true;
    float currentTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        GameControl.GameController.GameReset();
    }

    // Update is called once per frame
    void Update()
    {
        currentQuota.text = "Current Quota: " + GameControl.GameController.quota + 
            "\nTime Remaining: " + string.Format("{0:0.00}", GameControl.GameController.timeChunk - GameControl.GameController.globalClock);
        statsField.text = "Current Living Ants: " + GameControl.GameController.activeAnts +
            "\nDead Ants: " + GameControl.GameController.deadAnts +
            "\nFood Consumed: " + GameControl.GameController.foodConsumed +
            "\nAnts Sacrificed: " + GameControl.GameController.antsConsumed +
            "\n\nAvg. Speed: " + GameControl.GameController.GetSpeed() + " in/sec" +
            "\nAvg. Lifespan: " + GameControl.GameController.GetLifespan() + " secs" +
            "\nEfficiency: " + GameControl.GameController.GetEfficiency() + " secs" +
            "\nStrength: " + GameControl.GameController.GetStrength() +
            "\nBirthrate: " + GameControl.GameController.GetSpawn() + " sec cooldown";

        if(Input.GetKeyDown(KeyCode.Escape))
            MutationDeactivate();

        //check for the swarmButton
        if (GameControl.GameController.superLvl > 0 && canSwarm)
        {
            swarmButton.SetActive(true);
            swarmButton.GetComponentInChildren<TMP_Text>().text = "";
        }
            
    }

    public void MutationActivate()
    {
        MutationsMenu.SetActive(true);
        MutationsTrigger.SetActive(false);
        
    }

    public void MutationDeactivate()
    {
        MutationsMenu.SetActive(false);
        MutationsTrigger.SetActive(true);
    }

    public void AntSwarm()
    {
        if (canSwarm)
        {
            //start swarm
            GameControl.GameController.swarmActive = true;
            canSwarm = false;
            //disable button
            swarmButton.GetComponent<Button>().interactable = false;
            StartCoroutine(SwarmCooldown());
        }
    }

    IEnumerator SwarmCooldown()
    {
        currentTime = 0f;
        while (currentTime < GameControl.GameController.superAnts[GameControl.GameController.superLvl])
        {
            swarmButton.GetComponentInChildren<TMP_Text>().text = ((int)(GameControl.GameController.superAnts[GameControl.GameController.superLvl] - currentTime)).ToString();
            if (GameControl.GameController.swarmActive && currentTime > 3f)
                GameControl.GameController.swarmActive = false;
            currentTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        canSwarm = true;
        swarmButton.GetComponentInChildren<TMP_Text>().text = "";
        swarmButton.GetComponent<Button>().interactable = true;
    }
}
