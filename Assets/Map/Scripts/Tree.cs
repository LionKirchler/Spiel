using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    private int wood = 500;
    protected float spawnOffset = 0;
    private List<Unit> units = new List<Unit>();
    private Grid gridOn;
    private Grid nextTree;
    private MapGrid mapGrid;
    private Player player;

    /// <summary>
    /// heal the tree
    /// </summary>
    /// <param name="hp">heal amount</param>
    public void healTree(int hp)
    {
        wood += hp;
        if (wood <= 0)
        {
            destroyTree();
        }
    }

    /// <summary>
    /// sets a mapgrid for the tree to work with
    /// </summary>
    /// <param name="mg"></param>
    public void setMapGrid(MapGrid mg)
    {
        mapGrid = mg;
    }

    /// <summary>
    /// damages the tree
    /// </summary>
    /// <param name="hp">damage</param>
    /// <returns>damage amount done</returns>
    public int damageTree(int hp)
    {
        int ret = 0;
        if(hp >= wood)
        {
            ret = wood;
        }
        else
        {
            ret = hp;
        }
        wood -= hp;
       
        return ret;
    }

    /// <summary>
    /// sets the grid this tree is on
    /// </summary>
    /// <param name="g"></param>
    public void setGridOn(Grid g)
    {
        gridOn = g;
    }
    /// <summary>
    /// checks if the tree is alive, if it isnt, the tree is destroyed, and the workers are sent to another one
    /// </summary>
    public void checkAlive()
    {

        if (wood <= 0)
        {
            findNearestTree();
        }
    }

    /// <summary>
    /// finds the closest tree, sends the workers to it, and destroyes this tree
    /// </summary>
    public void findNearestTree()
    {
        mapGrid.removeTree(gridOn);

        PathFinderManager pathFinderManager = player.getSpawner().getPathFinderManager().GetComponent<PathFinderManager>();

        nextTree = pathFinderManager.FindNearestGrid(gridOn, mapGrid.getTrees(), mapGrid);

        List<Unit> unitsC = new List<Unit>(units);
        for (int i = 0; i < unitsC.Count; i++)
        {
            nextTree.getBlockingObject().GetComponent<Tree>().addUnit(unitsC[i]);
            if (unitsC[i].GetComponent<Worker>().isLumberjack())
            {
                unitsC[i].GetComponent<Worker>().setWorkGrid(nextTree, "wood");
                if (unitsC[i].GetComponent<Worker>().getWorkPhase() == 1 && !unitsC[i].GetComponent<Worker>().isWoodFull())
                {
                    unitsC[i].GetComponent<Worker>().startWorking(false);
                    unitsC[i].GetComponent<Worker>().startWorking(true);
                }
                
            }
        }

        destroyTree();
    }
    

    /// <summary>
    /// sets the player for this tree to work with
    /// </summary>
    /// <param name="player"></param>
    public void setPlayer(Player player)
    {
        this.player = player;
    }
    /// <summary>
    /// returns the spawn offet of this tree, so it fits on the grid
    /// </summary>
    /// <returns></returns>
    public float getSpawnOffset()
    {
        return spawnOffset;
    }

    /// <summary>
    /// destroys the tree
    /// </summary>
    public void destroyTree()
    {
        gridOn.unBlock();
        Destroy(gameObject);
    }

    /// <summary>
    /// removes a unit working on the tree
    /// </summary>
    /// <param name="u"></param>
    public void removeUnit(Unit u)
    {
        try
        {
            units.Remove(u);
        }
        catch { }
        
    }

    /// <summary>
    /// returns the units working on the tree
    /// </summary>
    /// <returns></returns>
    public List<Unit> getUnits()
    {
        return units;
    }

    /// <summary>
    /// adds a unit thats working on this tree
    /// </summary>
    /// <param name="u"></param>
    public void addUnit(Unit u)
    {
        removeUnit(u);
        units.Add(u);
    }

}
