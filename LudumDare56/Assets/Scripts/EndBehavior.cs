using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        if (GameControl.GameController.gameWin == false)
        {
            message.text = "YOUR COLONY WAS ERADICATED";
        }
        else
        {
            message.text = "YOUR COLONY ENDED THE WORLD!";
        }   

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayAgain()
    {
        GameControl.GameController.gameWin = false;
        GameControl.GameController.gameOver = false;
        SceneManager.LoadScene("Gameplay");
    }
}
