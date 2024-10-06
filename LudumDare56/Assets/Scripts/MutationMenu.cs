using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class MutationMenu : MonoBehaviour
{
    GameObject[] slices;
    [SerializeField] TMP_Text pointText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pointText.text = "Skill Points Available: " + GameControl.GameController.skillPoints;
        
    }
    /*
     * public int skillPoints = 0;
    public int activeAnts = 0;
    public int deadAnts = 0;
    public int foodConsumed = 0;
    public int antsConsumed = 0;
     */

    private void OnEnable()
    {
        slices = GameObject.FindGameObjectsWithTag("slice");
        Debug.Log(slices.Length);
        SliceUpdate();
    }

    private void SliceUpdate()
    {
        
        for (int i = 0; i < slices.Length; i++)
        {
            slices[i].GetComponent<SliceDetect>().Activate(i);
        }

    }

    
}
