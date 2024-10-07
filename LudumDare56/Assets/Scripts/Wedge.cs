using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wedge : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Sprite basicSprite;
    [SerializeField] public Sprite alternateSprite;
    [SerializeField] public MutationMenu menu;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (menu.basic)
            GetComponent<Image>().sprite = basicSprite;
        else
            GetComponent<Image>().sprite = alternateSprite;
        */
    }

}
