using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntSpawner : MonoBehaviour
{
    //Spawn Timer
    float spawnDelay = 3f;

    [SerializeField] GameObject antPool;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnHandler());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameControl.GameController.swarmActive)
            spawnDelay = 0.1f;
        else
            spawnDelay = GameControl.GameController.GetSpawn();
    }

    IEnumerator SpawnHandler()
    {
        float currentTime = 0f;
        //set this to be tied to GameState
        while (true)
        {
            if (currentTime >= spawnDelay)
            {
                currentTime = 0f;
                GameObject newAnt = antPool.GetComponent<ObjectPool>().GetPooledObject();
                newAnt.transform.position = transform.position;
                newAnt.transform.localScale = transform.localScale;
                newAnt.SetActive(true);
                newAnt.GetComponent<Ant>().Activate();
                newAnt.GetComponent<Ant>().home = gameObject;
            }
            currentTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
