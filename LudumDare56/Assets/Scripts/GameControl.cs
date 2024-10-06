using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl GameController;
    public FoodIndex[,] leafMap;
    public FoodIndex[,] appleMap;
    public FoodIndex[,] meatMap;
    public FoodIndex[,] carterMap;
    public FoodIndex[,] atmMap;
    public FoodIndex[,] carMap;
    public FoodIndex[,] houseMap;
    public FoodIndex[,] nuclearMap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        GameController = this;
        DontDestroyOnLoad(gameObject);
    }
}
