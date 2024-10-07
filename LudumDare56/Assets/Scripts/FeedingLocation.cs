using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FeedingLocation : MonoBehaviour
{
    //must have an associated list from gameController to dole out food
    GameObject foodPool;
    public FoodIndex chosenFoodIndex;

    public Queue<Ant> harvestQueue;
    public int reqStr = 0;
    public int totalStr = 0;
    public int reqAnt = 0;
    public int mapIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        harvestQueue = new Queue<Ant>();
        foodPool = GameObject.FindWithTag("foodPool");
        StartCoroutine(DistributeFood());
    }

    IEnumerator DistributeFood()
    {
        int indivFood = 0;

        EatFood foodObject = GetComponentInParent<EatFood>();
        AudioSource audioSource = GetComponent<AudioSource>();

        while (true)
        {
            if (harvestQueue.Count > 0)
            {
                if (foodObject.totalFood <= 0)
                {
                    // case where ant gets to food source and there is no food to be found
                    Debug.Log("Ant got to food source but food source was empty");
                    // dequeue ant and give dummy food object
                    Ant currentAnt = harvestQueue.Dequeue();
                    GameObject currentFood = foodPool.GetComponent<ObjectPool>().GetPooledObject();

                    //set the currentFood color to reflect the eaten color
                    currentFood.GetComponent<SpriteRenderer>().color = Color.clear;

                    // assign food value 
                    currentFood.GetComponent<Food>().foodamt = 0;
                    currentFood.SetActive(true);
                    currentAnt.AssignFood(currentFood);
                }
                else
                {
                    // find a foodIndex
                    if (chosenFoodIndex == null)
                    {
                        chosenFoodIndex = foodObject.chooseFoodIndex();
                    }
                    reqStr = chosenFoodIndex.getStrength();
                    Debug.Log("reqStr is: " + reqStr);
                    totalStr = 0;
                    reqAnt = 0;
                    // sum all strength of ants in queue currently
                    foreach (Ant ant in harvestQueue)
                    {
                        totalStr += (int)ant.antStrength;
                        reqAnt++;
                        // if we have more than or equal to total strength, break out since we have enough strength atp
                        if (totalStr >= reqStr)
                        {
                            Debug.Log("totalStr was larger");
                            break;
                        }
                        // if we didn't have enough ants, nothing will happen and we jump back to continue to wait for enough ants to be in queue
                    }
                    //Debug.Log("At the end of ant queue, total strength: " + totalStr);

                    if (totalStr >= reqStr)
                    {
                        //Debug.Log("totalStr was larger");


                        // atp we know we can dequeue ants and have enough strength
                        for (int i = 0; i < reqAnt; i++)
                        {
                            // math for figuring out how much food an ant brings back


                            //will need to assign an index and adjust the global lists here
                            Ant currentAnt = harvestQueue.Dequeue();

                            if (reqStr > 0)
                            {
                                if (currentAnt.antStrength < reqStr)
                                {
                                    reqStr -= currentAnt.antStrength;
                                    indivFood = currentAnt.antStrength;
                                }
                                else if (currentAnt.antStrength == reqStr)
                                {
                                    indivFood = currentAnt.antStrength;
                                    reqStr = 0;
                                }
                                // antstrength > reqstr
                                else
                                {
                                    indivFood = reqStr;
                                    reqStr = 0;
                                }
                            }
                            // grab object from pooled objects
                            GameObject currentFood = foodPool.GetComponent<ObjectPool>().GetPooledObject();

                            //set the currentFood color to reflect the eaten color
                            currentFood.GetComponent<SpriteRenderer>().color = chosenFoodIndex.getBackground();

                            // assign food value 
                            // tie in the luck based thing here
                            currentFood.GetComponent<Food>().foodamt = indivFood;

                            currentFood.SetActive(true);
                            currentAnt.AssignFood(currentFood);
                        }
                        foodObject.eatFoodIndex(chosenFoodIndex);
                        if (!audioSource.isPlaying) { audioSource.Play(); }
                        else { audioSource.Play(); }

                        chosenFoodIndex = null;
                    }
                    else
                    {
                        Debug.Log("TotalStr was not larger");
                    }
                    
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }


}
