using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronOre : MonoBehaviour
{
    protected int iron = 500;
    protected float spawnOffset = 0;
    protected MapGrid mapGrid;
    protected Grid gridOn;
    protected List<Unit> units = new List<Unit>();
    protected Player player;

    /// <summary>
    ///heal iron ore (useless)
    /// </summary>
    /// <param name="hp">heal amount </param>
    public void healIronOre(int hp)
    {
        iron += hp;
        if (iron <= 0)
        {
            destroyIronOre();
        }
    }

    /// <summary>
    /// sets the grid this ore is on
    /// </summary>
    /// <param name="g">grid</param>
    public void setGridOn(Grid g)
    {
        gridOn = g;
    }

    /// <summary>
    /// set the map grid this iron ore should work with
    /// </summary>
    /// <param name="mg">mapgrid</param>
    public void setMapGrid(MapGrid mg)
    {
        mapGrid = mg;
    }

    /// <summary>
    /// damage the iron ore (by mining for example)
    /// </summary>
    /// <param name="hp">damage</param>
    /// <returns>damage dealt</returns>
    public int damageIronOre(int hp)
    {
        int ret = 0;
        if (hp >= iron)
        {
            ret = iron;
        }
        else
        {
            ret = hp;
        }
        iron -= hp;

        return ret;
    }

    /// <summary>
    /// check if this is still not destoryed
    /// </summary>
    public void checkAlive()
    {
        if (iron <= 0)
        {
            findNearestIronOre();
        }
    }

    /// <summary>
    /// finds the closest iron and sends current workers to it, and destroys itself
    /// </summary>
    public void findNearestIronOre()
    {
        mapGrid.removeIronOres(gridOn);

        PathFinderManager pathFinderManager = player.getSpawner().getPathFinderManager().GetComponent<PathFinderManager>();

        Grid nextIronOre = pathFinderManager.FindNearestGrid(gridOn, mapGrid.getIronOres(), mapGrid);

        List<Unit> unitsC = new List<Unit>(units);
        for (int i = 0; i < unitsC.Count; i++)
        {
            nextIronOre.getBlockingObject().GetComponent<IronOre>().addUnit(unitsC[i]);
            if (unitsC[i].GetComponent<Worker>().isIronOreMiner())
            {
                unitsC[i].GetComponent<Worker>().setWorkGrid(nextIronOre, "ironOre");
                if (unitsC[i].GetComponent<Worker>().getWorkPhase() == 1 && !unitsC[i].GetComponent<Worker>().isIronOreFull())
                {
                    unitsC[i].GetComponent<Worker>().startWorking(false);
                    unitsC[i].GetComponent<Worker>().startWorking(true);
                }

            }
        }

        destroyIronOre();
    }

    /// <summary>
    /// returns the span offset of this iron ore to fit on the grid
    /// </summary>
    /// <returns></returns>
    public float getSpawnOffset()
    {
        return spawnOffset;
    }

    /// <summary>
    /// destory iron ore
    /// </summary>
    public void destroyIronOre()
    {
        gridOn.unBlock();
        Destroy(gameObject);
    }

    /// <summary>
    /// sets the player this iron ore should work with
    /// </summary>
    /// <param name="player">player</param>
    public void setPlayer(Player player)
    {
        this.player = player;
    }

    /// <summary>
    /// removes a unit working on this iron ore
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
    /// returns the units working on this
    /// </summary>
    /// <returns></returns>
    public List<Unit> getUnits()
    {
        return units;
    }

    /// <summary>
    /// adds a unit working on this
    /// </summary>
    /// <param name="u"></param>
    public void addUnit(Unit u)
    {
        removeUnit(u);
        units.Add(u);
    }

}
