using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class EatFood : MonoBehaviour
{
    [SerializeField] private int mapIndex;
    [SerializeField] private Sprite circle;
    [SerializeField] private float cellSize;
    private SpriteRenderer spriteRenderer;
    private FoodIndex[,] foodMap;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = new SpriteRenderer();
    }

    // Update is called once per frame
    void Update()
    {
        // purely used for testing whether or not we can eat from food correctly
        if (Input.GetKeyDown(KeyCode.Space))
        {

            // obtain foodMap of index we want
            assignFoodMap();

            // instantiate a new sphere to have the same color as the returned removed FoodIndex object
            Color antfoodColor = removeFoodIndex();

            Debug.Log(antfoodColor);

            // update game controller map values
            assignMainMap();


        }
    }

    // assigns foodMap based on the given index
    private void assignFoodMap()
    {
        switch(mapIndex)
        {
            case 0:
                foodMap = GameControl.GameController.leafMap; break;
            case 1:
                foodMap = GameControl.GameController.appleMap; break;
            case 2:
                foodMap = GameControl.GameController.meatMap; break;
            case 3:
                foodMap = GameControl.GameController.carterMap; break;
            case 4:
                foodMap = GameControl.GameController.atmMap; break;
            case 5:
                foodMap = GameControl.GameController.carMap; break;
            case 6:
                foodMap = GameControl.GameController.houseMap; break;
            case 7:
                foodMap = GameControl.GameController.nuclearMap; break;
        }
    }

    private void assignMainMap()
    {
        switch (mapIndex)
        {
            case 0:
                GameControl.GameController.leafMap = foodMap; break;
            case 1:
                GameControl.GameController.appleMap = foodMap; break;
            case 2:
                GameControl.GameController.meatMap = foodMap; break;
            case 3:
                GameControl.GameController.carterMap = foodMap; break;
            case 4:
                GameControl.GameController.atmMap = foodMap; break;
            case 5: 
                GameControl.GameController.carMap = foodMap; break;
            case 6:
                GameControl.GameController.houseMap = foodMap; break;
            case 7:
                GameControl.GameController.nuclearMap = foodMap; break;
        }
    }

    // this function generates a list of all canBeEaten = true indices, puts them into a list and chooses an index at random to remove
    // isEmpty is turned from false to true and all neighbors canBeEaten are true
    private Color removeFoodIndex()
    {
        // instantiate new list to hold all food we can eat
        List<FoodIndex> availableFood = new List<FoodIndex>();
        Color foodColor = new Color();
        int randInd = 0;
        
        for (int i = 0; i < foodMap.GetLength(0); i++)
        {
            for (int j = 0; j < foodMap.GetLength(1); j++)
            {
                if (foodMap[i,j].getIsOccupied() == false && foodMap[i, j].getHasNeighbor() == true)
                    availableFood.Add(foodMap[i, j]);
            }
        }

        // randomly choose an index to start eating from
        if (availableFood.Count > 0)
        {
            randInd = Random.Range(0, availableFood.Count);
        }
        else
        {
            Debug.Log("FoodIndexMap is empty");
        }

        // obtain information from available food
        foodColor = availableFood[randInd].getBackground();

        // only thing left is to find the position of the food we ate, draw a circle, and remove it from
        // controller's foodIndexMap
        for (int i = 0; i < foodMap.GetLength(0); i++)
        {
            for (int j = 0; j < foodMap.GetLength(1); j++)
            {
                // found right piece of food
                if (availableFood[randInd].getPos() == foodMap[i, j].getPos())
                {
                    // draw circle
                    GameObject fillCircle = new GameObject();
                    fillCircle.AddComponent<SpriteRenderer>();
                    fillCircle.GetComponent<SpriteRenderer>().sprite = circle;
                    fillCircle.GetComponent<SpriteRenderer>().color = new Color(0.38f, 0.55f, 0.76f);
                    //Debug.Log("Eating at: " + availableFood[randInd].getPos());
                    fillCircle.transform.position = new Vector3(availableFood[randInd].getPos().x, availableFood[randInd].getPos().y, 0);
                    fillCircle.transform.localScale = new Vector3(cellSize * 1.5f, cellSize * 1.5f);

                    // remove it from map, and update neighbors to be able to be eaten
                    foodMap[i, j].setIsOccupied(true);
                    foodMap[i, j].setHasNeighbor(false);
                    Debug.Log(foodMap[i, j].getNeighbors().Count);
                    for (int k = 0; k < foodMap[i, j].getNeighbors().Count; k++)
                        foodMap[i, j].getNeighbors()[k].setHasNeighbor(true);

                    // once we have done all of this, no reason to stay in loop so return function here
                    return foodColor;
                }


            }
        }
        return Color.clear;
    }




}
