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
    Animator animator;

    //Index obj here

    //timing stuff
    float waitTime = 1f;
    float antSpeed = 2.5f;
    public float deathTimer = 20f;
    public int antStrength = 1;
    public float antSize = 1f;

    //state machine
    antState currentState;
    bool readyToFeed = false;
    bool readyToTarget = false;

    //food
    GameObject food;

    //storing location for flipping
    float prevX;
    float currentX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive && isAlive)
        {
            currentX = transform.localPosition.x;
            if (currentX < prevX)
            {
                Vector3 newScale = transform.localScale;
                newScale.x = -Mathf.Abs(newScale.x);
                transform.localScale = newScale;
            }
            else if (currentX > prevX)
            {
                Vector3 newScale = transform.localScale;
                newScale.x = Mathf.Abs(newScale.x);
                transform.localScale = newScale;
            }
            prevX = currentX;
        }
    }

    private void OnEnable()
    {
        
    }

    public void Activate()
    {
        currentX = transform.localPosition.x;
        prevX = currentX;
        isActive = true;
        isAlive = true;
        animator = GetComponent<Animator>();
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
        waitTime = GameControl.GameController.GetEfficiency();

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
                    animator.SetBool("walk", false);
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
                    animator.SetBool("walk", true);
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
                    animator.SetBool("walk", false);
                    if (currentTime < waitTime && !readyToFeed)
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
                            //Flip();
                        }
                    }
                    break;
                case antState.Returning:
                    animator.SetBool("walk", true);
                    //go to the home location
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, antSpeed * Time.fixedDeltaTime);
                    if (Vector3.Distance(transform.position, target.transform.position) < 0.01f)
                    {
                        Debug.Log("Delivering");
                        currentState = antState.Delivering;
                    }
                    break;
                case antState.Delivering:
                    animator.SetBool("walk", false);
                    //wait for deliverCooldown to be done
                    if (currentTime < waitTime && !readyToTarget)
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
                            //Flip();
                        }
                    }
                    break;
                case antState.Death:
                    animator.SetTrigger("death");
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
        isAlive = false;
        Debug.Log("death Handler reahced");
        //add the currentLocation to the Feeding Locations
        targetControl.GetComponent<FeedingHandler>().foodSources.Add(gameObject);
        if (target != null)
        {
            Debug.Log("target wasn't null");
            if (target.tag == "ant")
                targetControl.GetComponent<FeedingHandler>().foodSources.Add(target);
        }
        Debug.Log("target was null");
        target = null;
        readyToTarget = false;
        readyToFeed = false;
        GameControl.GameController.activeAnts--;
        GameControl.GameController.deadAnts++;
        Debug.Log("Le ant is dead");
        Debug.Log(currentState);
        yield return null;
    }

    public void AssignFood(GameObject foodInput)
    {
        foodInput.transform.SetParent(transform, false);
        foodInput.transform.localPosition = Vector3.up;
        foodInput.transform.localScale = Vector3.one;
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

    public void Flip()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
}
