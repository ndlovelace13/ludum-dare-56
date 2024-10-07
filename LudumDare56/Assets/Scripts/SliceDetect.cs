using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System.Linq;

public class SliceDetect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    int powerIndex;
    int currentPower;
    bool sliceHover = false;
    GameObject nextUpgrade;
    [SerializeField] GameObject[] powerObjs;
    AudioSource audioSource;
    bool maxed = false;

    bool up = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnable()
    {
        if (nextUpgrade != null)
            Activate(powerIndex);
    }

    public void Activate(int givenIndex)
    {
        StopAllCoroutines();
        powerIndex = givenIndex;
        currentPower = GameControl.GameController.antLevels[powerIndex];
        SwitchSprite();
        for (int i = 0; i < currentPower; i++)
        {
            powerObjs[i].SetActive(true);
            powerObjs[i].GetComponent<Image>().color = Color.white;
        }
        if (currentPower < powerObjs.Length)
            nextUpgrade = powerObjs[currentPower];
        else
            maxed = true;
        StartCoroutine(UpgradeFlash());
    }

    private void SwitchSprite()
    {
        for (int i = 0; i < powerObjs.Length; i++)
        {
            powerObjs[i].SetActive(true);
            if (powerIndex < 5)
                powerObjs[i].GetComponent<Image>().sprite = powerObjs[i].GetComponent<Wedge>().basicSprite;
            else
                powerObjs[i].GetComponent<Image>().sprite = powerObjs[i].GetComponent<Wedge>().alternateSprite;
            powerObjs[i].SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        sliceHover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("exit");
        sliceHover = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameControl.GameController.skillPoints > 0 && currentPower < powerObjs.Length)
        {
            audioSource = GetComponentInParent<AudioSource>();
            audioSource.Play();
            GameControl.GameController.antLevels[powerIndex]++;
            GameControl.GameController.skillPoints--;
            GameControl.GameController.UpgradeUpdate();
            Activate(powerIndex);
        }
    }

    IEnumerator UpgradeFlash()
    {
        Debug.Log("Upgrade Flash for " + nextUpgrade.name + " starting now");
        while (gameObject.activeSelf && !maxed)
        {
            if (GameControl.GameController.skillPoints > 0 && sliceHover)
            {
                nextUpgrade.SetActive(true);
                float currentAlpha = nextUpgrade.GetComponent<Image>().color.a;
                Color newColor;
                if (currentAlpha <= 0f)
                    up = true;
                else if (currentAlpha >= 1f)
                    up = false;
                if (!up)
                {
                    newColor = new Color(1f, 1f, 1f, currentAlpha - 0.02f);
                }
                else
                {
                    newColor = new Color(1f, 1f, 1f, currentAlpha + 0.02f);
                }
                nextUpgrade.GetComponent<Image>().color = newColor;
            }
            else
                nextUpgrade.SetActive(false);
            yield return new WaitForFixedUpdate();
        }
    }
}
