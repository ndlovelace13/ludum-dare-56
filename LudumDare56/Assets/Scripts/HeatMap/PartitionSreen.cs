using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;



// this class is in charge of splitting the screen evenly into a certain amount of points, then generating a heatmap which corresponds to
// each individual object.
//  1. Splits screen into even grid
//  2. Generate a heatmap based on Type of Objects
//  3. Use heatmap to generate multilpe 2D arrays that define food bounds
//  4. Generate a heatmap per food object's bounds, defining toughness for each food object
//  5. Use heatmap to generate a 2D array of index objects





public class PartitionSreen : MonoBehaviour
{
    private Vector2[,] gridMap, locMap;
    int vertical, horizontal, cols, rows;

    [SerializeField] private float scale;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite sprite;
    [SerializeField] private LayerMask bgMask;
    [SerializeField] private GameObject controller;
    [SerializeField] private int mapIndex;

    private List<Color> foundColors = new List<Color>();
    private int[] countColors = new int[200];
    private FoodIndex[,] foodMap;
    private List<FoodIndex> neighbors = new List<FoodIndex>();
    // Start is called before the first frame update
    void Start()
    {

        // Code to populate gridmap and split the background into an even grid
        #region
        spriteRenderer = GetComponent<SpriteRenderer>();


        cols = Mathf.CeilToInt(spriteRenderer.bounds.size.x / scale);
        rows = Mathf.CeilToInt(spriteRenderer.bounds.size.y / scale);

        foodMap = new FoodIndex[cols, rows];

        // instantiate the map with empty default Index objects for intialization's sake
        for (int p = 0; p < cols; p++)
            for (int u = 0; u < rows; u++)
                foodMap[p, u] = new FoodIndex();


        SpriteRenderer renderer;
        Sprite monkey;

        gridMap = new Vector2[cols, rows];
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                gridMap[i, j] = new Vector2(scale / 2 + scale * i, scale / 2 + scale * j);
                //Debug.Log(gridMap[i, j]);

                // now that we have location for our gridmap, we can translate the heatmap and
                // generate the bounds list
                Vector2 point = gridMap[i, j];
                // generate a raycast with origin of point that we are currently at on grid

                Texture2D text = spriteRenderer.sprite.texture;
                Collider2D hit = Physics2D.OverlapPoint(new Vector2((scale * 2 - scale * point.x), (int)(scale * 2 - scale * point.y)), bgMask);

                if (hit)
                {
                    renderer = hit.transform.root.GetComponent<SpriteRenderer>();

                    monkey = renderer.sprite;

                    int x = (int)Mathf.FloorToInt(point.x * monkey.pixelsPerUnit);
                    int y = (int)Mathf.FloorToInt(point.y * monkey.pixelsPerUnit);

                    Color color = monkey.texture.GetPixel(x, y);

                    // if color is not clear, we hit a match for our object
                    if (color != Color.clear)
                    {
                        // manually set alpha to ensure correct color accuracy
                        color.a = 1;
                        // create actual index object at current index with an empty neighbors list and tentative canBeEaten value
                        foodMap[i, j] = new FoodIndex(point, neighbors, Color.blue, color, FindStrength(color), false, false);
                    }
                    // didn't find a color so index is now set to an empty node
                    else
                    {
                        foodMap[i, j] = new FoodIndex(point, neighbors, Color.blue, color, -1, true, false);

                    }


                    // printing functions (not very relevant)
                    #region
                    if (!foundColors.Contains(color))
                    {
                        foundColors.Add(color);
                    }

                    for (int m = 0; m < foundColors.Count; m++)
                    {
                        if (color == foundColors[m])
                        {
                            countColors[m]++;
                        }
                    }
                    #endregion

                }
            }
            #endregion
            // DEPRECIATED -----------------------------------------------
            /* Used to determine rgb values of pixels within the image
            Color[] colurs = spurt.texture.GetPixels();
            Debug.Log(spurt.texture.height + " " + spurt.texture.width);

            for (int y = 0; y < spurt.texture.height; y++)
            {
                for (int x = 0; x < spurt.texture.width; x++)
                {
                    int index = y * spurt.texture.width + x;
                    Debug.Log(colurs[index]);
                }
            }



            // code to raycast heatmap and populate the location map


            /*
            Renderer renderer = hit.collider.GetComponent<SpriteRenderer>();
            Texture2D text2D = renderer.material.mainTexture as Texture2D;

            Vector2 pCoord = hit.textureCoord;

            pCoord.x *= text2D.width;
            pCoord.y *= text2D.height;

            Vector2 tilling = renderer.material.mainTextureScale;
            Color color = text2D.GetPixel(Mathf.FloorToInt(pCoord.x * tilling.x), Mathf.FloorToInt(pCoord.y * tilling.y));

            Debug.Log(color);
            */
        } // end of for loop

        // done initializing everything with required values, can now go through and finalize foodMap
        // with correct neighbor values
        for (int i = 0; i < foodMap.GetLength(0); i++)
        {
            for (int j = 0; j < foodMap.GetLength(1); j++)
            {
                // check if food exists at this index
                if (foodMap[i, j].getEmpty() == false)
                {
                    neighbors = new List<FoodIndex>();
                    // clear neighbors and populate list
                    PopulateNeighbors(i, j);
                    foodMap[i, j].setNeighbors(neighbors);
                    // if neighbor count of node isn't 4, means that it isn't completely landlocked, therefore
                    // it can be eaten
                    if (neighbors.Count < 4)
                    {
                        foodMap[i, j].setCanBeEaten(true);
                    }
                }
            }
        } // end of for loop

        // once finished populating foodMap, assign foodMap to it's assigned global list
        setGlobalMap();
        // printing function (non relevant)
        // prints amt of found colors
        for (int z = 0; z < foundColors.Count; z++)
        {
            Debug.Log(foundColors[z] + " Count: " + countColors[z]);
        }

        // prints generated food map from given sprite and gridmap
        for (int l = 0; l < foodMap.GetLength(0); l++)
        {
            for (int n = 0; n < foodMap.GetLength(1); n++)
            {
                Debug.Log(foodMap[l, n].getNeighbors().Count);
            }
        }   
        
    }   // end of start()

    // helper function to take a Color object and return an integer
    private int FindStrength(Color color)
    {
        if (color == Color.green) return 1;
        else if (color == Color.blue) return 2;
        else if (color == Color.red) return 3;
        else if (color == Color.grey) return 4;
        else if (color == Color.magenta) return 10;
        else if (color.r == 255 && color.g == 255 && color.b == 0) return 20;
        else if (color == Color.cyan) return 40;
        else if (color == Color.black) return 100;
        else return -1;
    }

    // helper function to populate neighbors list when given an index i and j (rows and cols)
    private void PopulateNeighbors(int i, int j)
    {
        // base case, first node can only check South and East (North West Corner)
        if (i == 0 && j == 0)
        {
            Debug.Log("North West Corner");
            if (!foodMap[i + 1, j].getEmpty())
                neighbors.Add(foodMap[i+1, j]);
            if (!foodMap[i, j + 1].getEmpty())
                neighbors.Add(foodMap[i, j + 1]);
            return;
        }
        // edge case for at the very end of the list (can only check North and West) (South East Corner)
        if ((i == foodMap.GetLength(0) - 1) && (j == foodMap.GetLength(1) - 1))
        {
            Debug.Log("South East Corner");
            if (!foodMap[i - 1, j].getEmpty())
                neighbors.Add(foodMap[i-1, j]);
            if (!foodMap[i, j - 1].getEmpty())
                neighbors.Add(foodMap[i, j-1]);
            return;
        }
        // edge case for South West Corner. Can only check North and East
        if (i == 0 && j == foodMap.GetLength(1) - 1)
        {
            Debug.Log("South West Corner");
            if (!foodMap[i, j - 1].getEmpty())
                neighbors.Add(foodMap[i, j - 1]);
            if (!foodMap[i + 1, j].getEmpty())
                neighbors.Add(foodMap[i+1, j]);
            return;
        }
        // edge case for North East Corner Can only check South and West
        if (i == foodMap.GetLength(0) - 1 && j == 0)
        {
            Debug.Log("North East Coner");
            if (!foodMap[i - 1, j].getEmpty())
                neighbors.Add(foodMap[i - 1, j]);
            if (!foodMap[i, j + 1].getEmpty())
                neighbors.Add(foodMap[i, j + 1]);
            return;
        }
        // edge case if we are at the end of the very East side of the list, and NOT at corners
        if (i == foodMap.GetLength(0) - 1 && j > 0 && j < foodMap.GetLength(1) - 1)
        {
            Debug.Log("East Side");
            // if i and j are both positive here, can check South, West, North
            if (!foodMap[i - 1, j].getEmpty())
                neighbors.Add(foodMap[i-1, j]);
            if (!foodMap[i, j - 1].getEmpty())
                neighbors.Add(foodMap[i, j-1]);
            if (!foodMap[i, j + 1].getEmpty())
                neighbors.Add(foodMap[i, j + 1]);
            return;
        }
        // edge case if we are at the very South side of the list and NOT at corners
        if (i > 0 && i < foodMap.GetLength(0) - 1 && j == foodMap.GetLength(1) - 1)
        {
            Debug.Log("South Side");
            // i and j both positive, can check North, East, West
            if (!foodMap[i - 1, j].getEmpty())
                neighbors.Add(foodMap[i - 1, j]);
            if (!foodMap[i + 1, j].getEmpty())
                neighbors.Add(foodMap[i + 1, j]);
            if (!foodMap[i, j - 1].getEmpty())
                neighbors.Add(foodMap[i, j-1]);
            return;
            
        }
        // edge case if we are at the very North side of list and NOT at corners
        if (i > 0 && i < foodMap.GetLength(0) - 1 && j == 0)
        {
            Debug.Log("North Side");
            // j is 0, i is positive, can check South, East, West
            if (!foodMap[i - 1, j].getEmpty())
                neighbors.Add(foodMap[i - 1, j]);
            if (!foodMap[i + 1, j].getEmpty())
                neighbors.Add(foodMap[i + 1, j]);
            if (!foodMap[i, j + 1].getEmpty())
                neighbors.Add(foodMap[i, j + 1]);
            return;
        }
        // edge case if we are at the very West side of list and NOT at corners
        if (i == 0 && j > 0 && j < foodMap.GetLength(1) - 1)
        {
            Debug.Log("West Side");
            // if i is 0, j is positive, can check South, North, East
            if (!foodMap[i + 1, j].getEmpty())
                neighbors.Add(foodMap[i+1,j]);  
            if (!foodMap[i, j + 1].getEmpty())
                neighbors.Add(foodMap[i, j+1]);
            if (!foodMap[i, j - 1].getEmpty())
                neighbors.Add(foodMap[i, j - 1]);
            return;
        }
        // all other normal cases where no edge or border (the body of graph)
        if (i > 0 && j > 0 && i < foodMap.GetLength(0) - 1 && j < foodMap.GetLength(1) - 1)
        {
            Debug.Log("Body");
            if (!foodMap[i - 1, j].getEmpty())
                neighbors.Add(foodMap[i - 1, j]);
            if (!foodMap[i, j - 1].getEmpty())
                neighbors.Add(foodMap[i, j - 1]);
            if (!foodMap[i + 1, j].getEmpty()) 
                neighbors.Add(foodMap[i + 1, j]);
            if (!foodMap[i, j + 1].getEmpty())
                neighbors.Add(foodMap[i, j + 1]);
            return;
        }
    }

    // helper function to set gamecontroller variables 
    private void setGlobalMap()
    {
        switch (mapIndex)
        {
            case 0:
                controller.gameObject.GetComponent<GameControl>().leafMap = foodMap; break;
            case 1:
                controller.gameObject.GetComponent<GameControl>().appleMap = foodMap; break;
            case 2:
                controller.gameObject.GetComponent<GameControl>().meatMap = foodMap; break;
            case 3:
                controller.gameObject.GetComponent<GameControl>().carterMap = foodMap; break;
            case 4:
                controller.gameObject.GetComponent<GameControl>().atmMap = foodMap; break;
            case 5:
                controller.gameObject.GetComponent<GameControl>().carMap = foodMap; break;
            case 6:
                controller.gameObject.GetComponent<GameControl>().houseMap = foodMap; break;
            case 7:
                controller.gameObject.GetComponent<GameControl>().nuclearMap = foodMap; break;
        }
    }
    


}
