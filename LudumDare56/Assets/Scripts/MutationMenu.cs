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
    [SerializeField] TMP_Text titleText;
    [SerializeField] Button alternateTrigger;
    [SerializeField] Image background;

    [SerializeField] Sprite basicSkills;
    [SerializeField] Sprite secondarySkills;

    public bool basic = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (basic)
            titleText.text = "Basic Mutations";
        else
            titleText.text = "Specialized Mutations";

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
            if (basic)
                slices[i].GetComponent<SliceDetect>().Activate(i);
            else
                slices[i].GetComponent<SliceDetect>().Activate(i + 5);
        }

    }
    
    public void Alternate()
    {
        basic = !basic;
        if (basic)
            background.sprite = basicSkills;
        else
            background.sprite = secondarySkills;

        SliceUpdate();
    }
    

    
}
