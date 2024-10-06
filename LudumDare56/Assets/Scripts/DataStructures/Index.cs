using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


// This class creates the Index object which is used for creating a 2D array of edible objects for the ants
public class FoodIndex
{
    public bool isEmpty, canBeEaten;
    public Vector2 pos;
    public List<FoodIndex> neighbors;
    public Color background;
    public Color foreground;
    public int strength;


    // default constructor
    public FoodIndex()
    {
        pos = Vector2.zero;
        neighbors = new List<FoodIndex>();
        background = Color.clear;
        foreground = Color.clear;
        strength = -1;
        isEmpty = false;
        canBeEaten = false;
    }

    // constructor
    public FoodIndex(Vector2 pos, List<FoodIndex> neighbors, Color background, Color foreground, int strength, bool isEmpty, bool canBeEaten)
    {
        this.pos = pos;
        this.neighbors = neighbors;
        this.background = background;
        this.foreground = foreground;
        this.strength = strength;
        this.isEmpty = isEmpty;
        this.canBeEaten = canBeEaten;
    }

    // getters
    #region
    public FoodIndex getIndex()
    {
        return this;
    }
    public Vector2 getPos() { return pos; }
    public List<FoodIndex> getNeighbors() {  return neighbors; }
    public Color getBackground() { return background; }
    public Color getForeground() { return foreground; }
    public int getStrength() { return strength; }
    public bool getEmpty() { return isEmpty; }
    public bool getCanBeEaten() {  return canBeEaten; }
    #endregion

    // setters
    #region
    public void setEmpty(bool isEmpty)
        {
            this.isEmpty = isEmpty;
        }
        public void setCanBeEaten(bool canBeEaten)
        {
            this.canBeEaten = canBeEaten;
        }
        public void setStrength(int strength)
        {
            this.strength = strength;
        }
        public void setBackground(Color background)
        {
            this.background = background;
        }
        public void setForeground(Color foreground)
        {
            this.foreground = foreground;
        }
        public void setNeighbors(List<FoodIndex> neighbors)
        {
            this.neighbors = neighbors;
        }
    #endregion

}
