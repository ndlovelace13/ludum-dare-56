using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [SerializeField] GameObject MutationsTrigger;
    [SerializeField] GameObject MutationsMenu;
    [SerializeField] TMP_Text statsField;
    bool mutationsActive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        statsField.text = "Current Living Ants: " + GameControl.GameController.activeAnts +
            "\nDead Ants: " + GameControl.GameController.deadAnts +
            "\nFood Consumed: " + GameControl.GameController.foodConsumed +
            "\nAnts Sacrificed: " + GameControl.GameController.antsConsumed +
            "\n\nAvg. Speed: " + GameControl.GameController.GetSpeed() + " in/sec" +
            "\nAvg. Lifespan: " + GameControl.GameController.GetLifespan() + " secs" +
            "\nSize: " + GameControl.GameController.GetSize() +
            "\nStrength: " + GameControl.GameController.GetStrength() +
            "\nBirthrate: " + GameControl.GameController.GetSpawn() + " sec cooldown";
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
}
