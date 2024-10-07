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
    public FoodIndex[,] foodMap = null;
    public int appliedStrength;
    public bool finishInitial = true;
    public int totalFood;
    // Start is called before the first frame update
    void Awake()
    {       
        spriteRenderer = new SpriteRenderer();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && finishInitial)
        {

            for (int i = 0; i < 50; i ++)
            {
                // pick a random open index object
                FoodIndex randomFood = chooseFoodIndex();
                eatFoodIndex(randomFood);
            }
        }
    }

    // assigns foodMap based on the given index (generated on partition screen finish)
    public void assignFoodMap()
    {
        switch(mapIndex)
        {
            case 0:
                foodMap = GameControl.GameController.leafMap;
                totalFood = GameControl.GameController.totalLeafFood;
                break;
            case 1:
                foodMap = GameControl.GameController.appleMap;
                totalFood = GameControl.GameController.totalAppleFood;
                break;
            case 2:
                foodMap = GameControl.GameController.meatMap;
                totalFood = GameControl.GameController.totalMeatFood;
                break;
            case 3:
                foodMap = GameControl.GameController.carterMap;
                totalFood = GameControl.GameController.totalCarterFood;
                break;
            case 4:
                foodMap = GameControl.GameController.atmMap;
                totalFood = GameControl.GameController.totalAtmFood;
                break;
            case 5:
                foodMap = GameControl.GameController.carMap;
                totalFood = GameControl.GameController.totalCarFood;
                break;
            case 6:
                foodMap = GameControl.GameController.houseMap;
                totalFood = GameControl.GameController.totalHouseFood;
                break;
            case 7:
                foodMap = GameControl.GameController.nuclearMap;
                totalFood = GameControl.GameController.totalNuclearFood;
                break;
        }
        finishInitial = true;
        Debug.Log("Total Food: " + totalFood);
    }

    private void assignMainMap()
    {
        switch (mapIndex)
        {
            case 0:
                GameControl.GameController.leafMap = foodMap;
                GameControl.GameController.totalLeafFood = totalFood;
                break;
            case 1:
                GameControl.GameController.appleMap = foodMap; 
                GameControl.GameController.totalAppleFood = totalFood;
                break;
            case 2:
                GameControl.GameController.meatMap = foodMap;
                GameControl.GameController.totalMeatFood = totalFood;
                break;
            case 3:
                GameControl.GameController.carterMap = foodMap; 
                GameControl.GameController.totalCarterFood = totalFood;
                break;
            case 4:
                GameControl.GameController.atmMap = foodMap;
                GameControl.GameController.totalAtmFood = totalFood;
                break;
            case 5: 
                GameControl.GameController.carMap = foodMap;
                GameControl.GameController.totalCarFood = totalFood;
                break;
            case 6:
                GameControl.GameController.houseMap = foodMap;
                GameControl.GameController.totalHouseFood = totalFood;
                break;
            case 7:
                GameControl.GameController.nuclearMap = foodMap;
                GameControl.GameController.totalNuclearFood = totalFood;
                break;
        }
        // Debug.Log("Total Food: " + totalFood);
    }

    public FoodIndex eatFoodIndex(FoodIndex indexToEat)
    {
        // remove index from all possible food Indices
        removeFoodIndex(indexToEat);
        // draw circle to represent this
        createBite(indexToEat);

        // Debug.Log(indexToEat.getBackground());

        // update game controller map values
        assignMainMap();

        return indexToEat;
    }




    // draw circle
    public void createBite(FoodIndex food)
    {
        // draw circle
        GameObject fillCircle = new GameObject();
        fillCircle.AddComponent<SpriteRenderer>();
        fillCircle.GetComponent<SpriteRenderer>().sprite = circle;
        fillCircle.GetComponent<SpriteRenderer>().color = new Color(0.38f, 0.55f, 0.76f);
        fillCircle.GetComponent<SpriteRenderer>().sortingLayerName = "MidGround";
        //Debug.Log("Eating at: " + availableFood[randInd].getPos());
        fillCircle.transform.position = new Vector3(food.getPos().x, food.getPos().y, 0);
        fillCircle.transform.localScale = new Vector3(cellSize * 1.5f, cellSize * 1.5f);
    }

    public FoodIndex chooseFoodIndex()
    {
        // instantiate new list to hold all food we can eat
        List<FoodIndex> availableFood = new List<FoodIndex>();
        int randInd = 0;

        for (int i = 0; i < foodMap.GetLength(0); i++)
            for (int j = 0; j < foodMap.GetLength(1); j++)
                if (foodMap[i, j].getIsOccupied() == false && foodMap[i, j].getHasNeighbor() == true)
                    availableFood.Add(foodMap[i, j]);

        // randomly choose an index to start eating from
        if (availableFood.Count > 0)
        {
            randInd = Random.Range(0, availableFood.Count);
            return availableFood[randInd];
        }
        else
        {
            Debug.Log("FoodIndexMap is empty");
        }
        return null;
    }



    // this function generates a list of all canBeEaten = true indices, puts them into a list and chooses an index at random to remove
    // isEmpty is turned from false to true and all neighbors canBeEaten are true
    private void removeFoodIndex(FoodIndex food)
    {
        // only thing left is to find the position of the food we ate, draw a circle, and remove it from
        // controller's foodIndexMap
        for (int i = 0; i < foodMap.GetLength(0); i++)
        {
            for (int j = 0; j < foodMap.GetLength(1); j++)
            {
                // found right piece of food
                if (food.getPos() == foodMap[i, j].getPos())
                {
                    // remove it from map, and update neighbors to be able to be eaten
                    totalFood--;
                    foodMap[i, j].setIsOccupied(true);
                    foodMap[i, j].setHasNeighbor(false);
                    //Debug.Log(foodMap[i, j].getNeighbors().Count);
                    for (int k = 0; k < foodMap[i, j].getNeighbors().Count; k++)
                        foodMap[i, j].getNeighbors()[k].setHasNeighbor(true);

                    // once we have done all of this, no reason to stay in loop so return function here
                    return;
                }


            }
        }
    }
}
