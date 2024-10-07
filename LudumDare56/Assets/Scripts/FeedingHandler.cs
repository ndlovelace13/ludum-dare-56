using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FeedingHandler : MonoBehaviour
{
    public List<GameObject> foodSources;

    public Queue<Ant> targetingAnts;
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
        foodSources = new List<GameObject>();
        targetingAnts = new Queue<Ant>();
        GameObject[] sources = GameObject.FindGameObjectsWithTag("foodSource");
        for (int i = 0; i < sources.Length; i++)
        {
            foodSources.Add(sources[i]);
        }
        foodSources = sources.OrderBy(x => x.transform.position.x).ToList();

        StartCoroutine(NewTarget());
    }

    IEnumerator NewTarget()
    {
        while (true)
        {
            if (targetingAnts.Count > 0)
            {
                List<GameObject> antTargets = new List<GameObject>();
                List<GameObject> foodMapTargets = new List<GameObject>();
                GameObject target;
                // segregate types of food sources
                foreach(GameObject source in foodSources)
                {
                    if (source.tag != "ant")
                        foodMapTargets.Add(source);
                    else
                        antTargets.Add(source);
                }

                List<GameObject> sortedFoodTargets = foodMapTargets.OrderBy(x => x.transform.position.x).ToList();

                Ant currentAnt = targetingAnts.Dequeue();
                // check if we need to determine if ants need to be targeted
                if (antTargets.Count > 0)
                {
                    Debug.Log("allowed to target ants");
                    // determine if we pick up ants or not
                    int targetChoice = Random.Range(0, 2);
                    if (targetChoice == 1)
                    {
                        Debug.Log("Targeting an ant");
                        int randomAnt = Random.Range(0, antTargets.Count);
                        target = antTargets[randomAnt];
                        foodSources.Remove(target);
                        currentAnt.AssignTarget(target);
                    }
                    // eating foodObjects
                    else
                    {
                        int randomFoodInd;
                        // need to generate the correct indices based on where we are currently
                        // if we are not at end base, have previous and future bases
                        if (GameControl.GameController.baseIndex != 7)
                        {

                            int randomVal = Random.Range(0, 100);
                            Debug.Log("Random Value to Beat: " + randomVal);
                            Debug.Log("Harvest Val: " + (int)GameControl.GameController.harvest * 100);
                            Debug.Log("Explore Val: " + (int)GameControl.GameController.explore * 100);
                            // if less than the random val we generated, then we will harvest
                            if ((int)(GameControl.GameController.harvest * 100f) - randomVal > 0)
                            {
                                randomFoodInd = Random.Range(0, GameControl.GameController.baseIndex + 1);
                                // if no food to be found, don't target
                                while (GameControl.GameController.FetchFoodMapState(randomFoodInd) == false)
                                {
                                    randomFoodInd = Random.Range(0, sortedFoodTargets.Count);
                                    Debug.Log("No Food to be found at foodmap");
                                    yield return new WaitForEndOfFrame();
                                };
                                Debug.Log("Harvesting: " + randomFoodInd);
                                target = sortedFoodTargets[randomFoodInd];
                                currentAnt.AssignTarget(target);
                            }
                            // more than the random value we generated, so we will explore
                            else
                            {
                                target = sortedFoodTargets[GameControl.GameController.baseIndex + 1];
                                currentAnt.AssignTarget(target);
                            }
                        }
                        // we are at the end base, so we only have previous bases, therefore harvest and explore have no use here
                        else
                        {
                            randomFoodInd = Random.Range(0, GameControl.GameController.baseIndex + 1);
                            // if no food to be found, don't target
                            while (GameControl.GameController.FetchFoodMapState(randomFoodInd) == false)
                            {
                                randomFoodInd = Random.Range(0, sortedFoodTargets.Count);
                                Debug.Log("No Food to be found at foodmap");
                                yield return new WaitForEndOfFrame();
                            }
                            target = sortedFoodTargets[randomFoodInd];
                            currentAnt.AssignTarget(target);
                        }

                    }
                }
                else
                {
                    int randomFoodInd;
                    // need to generate the correct indices based on where we are currently
                    // if we are not at end base, have previous and future bases
                    if (GameControl.GameController.baseIndex != 7)
                    {

                        int randomVal = Random.Range(0, 100);
                        Debug.Log("Random Value to Beat: " + randomVal);
                        Debug.Log("Harvest Val: " + (int)(GameControl.GameController.harvest * 100));
                        Debug.Log("Explore Val: " + (int)(GameControl.GameController.explore * 100));
                        // if less than the random val we generated, then we will harvest
                        if ((int)(GameControl.GameController.harvest * 100f) - randomVal > 0)
                        {
                            randomFoodInd = Random.Range(0, GameControl.GameController.baseIndex + 1);
                            // if no food to be found, don't target
                            while (GameControl.GameController.FetchFoodMapState(randomFoodInd) == false)
                            {
                                randomFoodInd = Random.Range(0, sortedFoodTargets.Count);
                                Debug.Log("No Food to be found at foodmap");
                                yield return new WaitForEndOfFrame();
                            }
                            Debug.Log("Harvesting: " + randomFoodInd);
                            target = sortedFoodTargets[randomFoodInd];
                            currentAnt.AssignTarget(target);
                        }
                        // more than the random value we generated, so we will explore
                        else
                        {
                            target = sortedFoodTargets[GameControl.GameController.baseIndex + 1];
                            Debug.Log("Exploring: " + (GameControl.GameController.baseIndex + 1));
                            currentAnt.AssignTarget(target);
                        }
                    }
                    // we are at the end base, so we only have previous bases, therefore harvest and explore have no use here
                    else
                    {
                        randomFoodInd = Random.Range(0, GameControl.GameController.baseIndex + 1);
                        // if no food to be found, don't target
                        while (GameControl.GameController.FetchFoodMapState(randomFoodInd) == false)
                        {
                            randomFoodInd = Random.Range(0, sortedFoodTargets.Count);
                            Debug.Log("No Food to be found at foodmap");
                            yield return new WaitForEndOfFrame();
                        }
                        target = sortedFoodTargets[randomFoodInd];
                        currentAnt.AssignTarget(target);
                    }
                }

            }
            yield return new WaitForFixedUpdate();
        }

    }

    
}
