using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndBehavior : MonoBehaviour
{
    [SerializeField] TMP_Text message;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameControl.GameController)
        {
            message.text = "YOUR COLONY HAD WORLD ENDING CONSEQUENCES!";
        }
        else
        {
            message.text = "YOUR COLONY WAS ELIMINATED!";
        }
    }

    public void ReturnHome()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
