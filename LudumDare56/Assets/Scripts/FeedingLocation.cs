using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedingLocation : MonoBehaviour
{
    //must have an associated list from gameController to dole out food
    GameObject foodPool;

    public Queue<Ant> harvestQueue;

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
        while (true)
        {
            if (harvestQueue.Count > 0)
            {
                //will need to assign an index and adjust the global lists here
                Ant currentAnt = harvestQueue.Dequeue();
                GameObject currentFood = foodPool.GetComponent<ObjectPool>().GetPooledObject();
                //set the currentFood color to reflect the eaten color
                //instantiate the "bite" circle here as well
                currentFood.SetActive(true);
                currentAnt.AssignFood(currentFood);
            }
            yield return new WaitForFixedUpdate();
        }
    }


}
