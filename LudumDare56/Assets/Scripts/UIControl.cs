using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [SerializeField] GameObject MutationsTrigger;
    [SerializeField] GameObject MutationsMenu;
    bool mutationsActive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MutationActivate()
    {
        if (!mutationsActive)
        {
            MutationsMenu.SetActive(true);
            MutationsTrigger.GetComponentInChildren<TMP_Text>().text = "Return";
        }
        else
        {
            MutationsMenu.SetActive(false);
            MutationsTrigger.GetComponentInChildren<TMP_Text>().text = "Mutations";
        }
        mutationsActive = !mutationsActive;
        
    }

    public void MutationDeactivate()
    {
        
    }
}
