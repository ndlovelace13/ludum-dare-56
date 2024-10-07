using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This class creates the Index object which is used for creating a 2D array of edible objects for the ants
public class FoodIndex
{
    private bool isOccupied, hasNeighbor;
    private Vector2 pos;
    private List<FoodIndex> neighbors;
    private Color background;
    private int strength;


    // default constructor
    public FoodIndex()
    {
        pos = Vector2.zero;
        neighbors = new List<FoodIndex>();
        background = Color.clear;
        strength = -1;
        isOccupied = false;
        hasNeighbor = false;
    }

    // constructor
    public FoodIndex(Vector2 pos, List<FoodIndex> neighbors, Color background, int strength, bool isOccupied, bool hasNeighbor)
    {
        this.pos = pos;
        this.neighbors = neighbors;
        this.background = background;
        this.strength = strength;
        this.isOccupied = isOccupied;
        this.hasNeighbor = hasNeighbor;
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
    public int getStrength() { return strength; }
    public bool getIsOccupied() { return isOccupied; }
    public bool getHasNeighbor() {  return hasNeighbor; }
    #endregion

    // setters
    #region
    public void setIsOccupied(bool isOccupied)
        {
            this.isOccupied = isOccupied;
        }
        public void setHasNeighbor(bool hasNeighbor)
        {
            this.hasNeighbor = hasNeighbor;
        }
        public void setStrength(int strength)
        {
            this.strength = strength;
        }
        public void setBackground(Color background)
        {
            this.background = background;
        }
        public void setNeighbors(List<FoodIndex> neighbors)
        {
            this.neighbors = neighbors;
        }
    #endregion

}
