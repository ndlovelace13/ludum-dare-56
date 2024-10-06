using System.Collections;
using System.Collections.Generic;
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
        StartCoroutine(NewTarget());
    }

    IEnumerator NewTarget()
    {
        while (true)
        {
            if (targetingAnts.Count > 0)
            {
                Ant currentAnt = targetingAnts.Dequeue();
                int choice = Random.Range(0, foodSources.Count);
                Debug.Log(targetingAnts.Count);
                GameObject target = foodSources[choice];
                if (target.tag == "ant")
                    foodSources.Remove(target);
                currentAnt.AssignTarget(target);
            }
            yield return new WaitForFixedUpdate();
        }

    }

    
}
