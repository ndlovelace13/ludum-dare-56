using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum antState
{
    Birthed,
    Traveling,
    Feeding,
    Returning,
    Delivering,
    Death
}

public class Ant : MonoBehaviour
{
    bool isActive = false;
    bool isAlive = true;

    //target acquisition
    [SerializeField] public GameObject target;
    public GameObject home;
    GameObject testTarget;
    [SerializeField] GameObject targetControl;

    //Index obj here

    //timing stuff
    float feedingTime = 1f;
    float deliveringTime = 1f;
    float antSpeed = 2.5f;
    float deathTimer = 20f;
    public int antStrength = 1;
    float antSize = 1f;

    //state machine
    antState currentState;
    bool readyToFeed = false;
    bool readyToTarget = false;

    //food
    GameObject food;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        
    }

    public void Activate()
    {
        isActive = true;
        targetControl = GameObject.FindWithTag("targetControl");
        currentState = antState.Birthed;
        GameControl.GameController.activeAnts++;
        AntIndividualization();
        StartCoroutine(StateHandling());
    }

    private void OnDisable()
    {
        isActive = false;
    }

    private void AntIndividualization()
    {
        //Load in current vals from GameControl
        antSpeed = GameControl.GameController.GetSpeed();
        deathTimer = GameControl.GameController.GetLifespan();
        antStrength = (int) GameControl.GameController.GetStrength();
        antSize = GameControl.GameController.GetSize();

        //do some random number calc to make sure ants feel individual
        antSpeed = Random.Range(antSpeed - 0.5f, antSpeed + 0.5f);
        deathTimer = Random.Range(deathTimer - 10f, deathTimer + 10f);
    }

    IEnumerator StateHandling()
    {
        float currentTime = 0f;
        float deathTime = 0f;
        while (isActive)
        {
            switch (currentState)
            {
                case antState.Birthed:
                    //add to the targeting queue
                    //if target is acquired then go to travel mode
                    if (!readyToTarget)
                    {
                        readyToTarget = true;
                        targetControl.GetComponent<FeedingHandler>().targetingAnts.Enqueue(this);
                    }
                    if (target != null && target != home)
                    {
                        readyToTarget = false;
                        currentState = antState.Traveling;
                    }
                        
                    Debug.Log("Now Traveling");
                    break;
                case antState.Traveling:
                    //go to the target location
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, antSpeed * Time.fixedDeltaTime);
                    if (Vector3.Distance(transform.position, target.transform.position) < 0.01f)
                    {
                        Debug.Log("Now feeding");
                        currentState = antState.Feeding;
                    }
                    break;
                case antState.Feeding: 
                    //wait for feedCooldown to be done
                    if (currentTime < feedingTime && !readyToFeed)
                    {
                        currentTime += Time.deltaTime;
                    }
                    else if (!readyToFeed)
                    {
                        currentTime = 0f;
                        if (target.tag == "foodSource")
                            target.GetComponent<FeedingLocation>().harvestQueue.Enqueue(this);
                        else
                            AssignFood(target);
                        readyToFeed = true;
                    }
                    else
                    {
                        //check for food extraction
                        if (food != null)
                        {
                            readyToFeed = false;
                            currentState = antState.Returning;
                            Debug.Log("Now Returning");
                        }
                    }
                    break;
                case antState.Returning:
                    //go to the home location
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, antSpeed * Time.fixedDeltaTime);
                    if (Vector3.Distance(transform.position, target.transform.position) < 0.01f)
                    {
                        Debug.Log("Delivering");
                        currentState = antState.Delivering;
                    }
                    break;
                case antState.Delivering:
                    //wait for deliverCooldown to be done
                    if (currentTime < deliveringTime && !readyToTarget)
                    {
                        currentTime += Time.deltaTime;
                    }
                    else if (!readyToTarget)
                    {
                        currentTime = 0f;
                        //add to the targeting queue
                        targetControl.GetComponent<FeedingHandler>().targetingAnts.Enqueue(this);
                        readyToTarget = true;
                        //increment the total, feed the queen
                        DeliverFood();
                    }
                    else
                    {
                        //replace this with whatever is assigned from the targetQueue
                        if (target != home)
                        {
                            readyToTarget = false;
                            currentState = antState.Traveling;
                            Debug.Log("now traveling");
                        }
                    }
                    break;
                case antState.Death:
                    StartCoroutine(DeathHandler());
                    yield break;
            }
            deathTime += Time.fixedDeltaTime;
            if (deathTime >= deathTimer)
            {
                currentState = antState.Death;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator DeathHandler()
    {
        //add the currentLocation to the Feeding Locations
        targetControl.GetComponent<FeedingHandler>().foodSources.Add(gameObject);
        if (target != null)
        {
            if (target.tag == "ant")
                targetControl.GetComponent<FeedingHandler>().foodSources.Add(target);
        }
        target = null;
        readyToTarget = false;
        readyToFeed = false;
        GameControl.GameController.activeAnts--;
        GameControl.GameController.deadAnts++;
        Debug.Log("Le ant is dead");
        yield return null;
    }

    public void AssignFood(GameObject foodInput)
    {
        foodInput.transform.SetParent(transform, false);
        foodInput.transform.localPosition = Vector3.up;
        food = foodInput;
        target = home;
    }
    
    public bool DeliverFood()
    {
        if (food != null)
        {
            if (food.tag == "ant")
            {
                bool result = food.GetComponent<Ant>().DeliverFood();
                GameControl.GameController.deadAnts--;
                GameControl.GameController.antsConsumed++;
                GameControl.GameController.foodConsumed += food.GetComponent<Ant>().antStrength;
            }
            //TODO - make sure to take the food's strength here
            if (food.tag != "ant")
                GameControl.GameController.foodConsumed += food.GetComponent<Food>().foodamt;
            food.transform.parent = null;
            food.SetActive(false);
            food = null;
        }
        return true;
    }

    public void AssignTarget(GameObject targetInput)
    {
        target = targetInput;
    }
}
