using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private Vector2 pos;
    private Vector2 size;
    private Vector2 index;
    private bool blocked = false;
    private GameObject blockingObject;
    private GameObject grass;
    private bool building = false;
    private bool pathBlocked = false;
    
    private bool pathSelected = false;
    private float pathDestDistance = -1;
    private float pathStartDistance = -1;
    private Grid pathOrigin = null;
    private bool wall = false;
    private bool ghostwall = false;
    private List<GameObject> unitsOn = new List<GameObject>(5);
    private List<GameObject> enemysOn = new List<GameObject>(5);

    public Grid()
    {

    }

    /// <summary>
    /// clone grid
    /// </summary>
    /// <param name="g">grid to clone</param>
    public Grid(Grid g)
    {
        this.pos = g.getPos();
        this.size = g.getSize();
        this.index = g.getIndex();
        this.blocked = g.isBlocked();
        this.blockingObject = g.getBlockingObject();
        this.grass = g.getGrass();
        this.pathBlocked = g.isPathBlocked();
        this.pathSelected = g.isPathSelected();
        this.pathDestDistance = g.getPathDestinationDistance();
        this.pathStartDistance = g.getPathStartingDistance();
        this.pathOrigin = g.getPathOrigin();
        this.unitsOn = new List<GameObject>(g.getUnitsOn());
        this.enemysOn = new List<GameObject>(g.getEnemysOn());
        this.building = g.isBuilding();
    }

    /// <summary>
    /// initializing the grid
    /// </summary>
    /// <param name="sPos">pos this grid is on</param>
    /// <param name="sSize">size of this grid</param>
    /// <param name="sIsBlocked">if this grid is blocked</param>
    /// <param name="sindex">index of this grid</param>
    public void intitGrid(Vector2 sPos,Vector2 sSize, bool sIsBlocked, Vector2 sindex)
    {
        pos = sPos;
        blocked = sIsBlocked;
        size = sSize;
        index = sindex;
    }

    /// <summary>
    /// assignes a grass to this grid
    /// </summary>
    /// <param name="gs"></param>
    public void setGrass(GameObject gs)
    {
        grass = gs;
    }

    /// <summary>
    /// color the grass associated to this grid in a color (use for testing)
    /// </summary>
    /// <param name="c">Color</param>
    public void colorGrass(Color c)
    {
        getGrass().GetComponentInChildren<SpriteRenderer>().color = c;
    }

    /// <summary>
    /// returns the grass this grid is on
    /// </summary>
    /// <returns></returns>
    public GameObject getGrass()
    {
        return grass;
    }

    /// <summary>
    /// returns the pos of this grid
    /// </summary>
    /// <returns></returns>
    public Vector2 getPos()
    {
        return pos;
    }

    /// <summary>
    /// returns if this grid has a building on
    /// </summary>
    /// <returns></returns>
    public bool isBuilding()
    {
        return building;
    }

    /// <summary>
    /// set if this grid has a building on
    /// </summary>
    /// <param name="b"></param>
    public void setIsBuilding(bool b)
    {
        building = b;
    }

    /// <summary>
    /// unblock the path of this grid / making it movable
    /// </summary>
    public void unBlockPath()
    {
        pathBlocked = false;
    }

    /// <summary>
    /// returns the index of this grid
    /// </summary>
    /// <returns></returns>
    public Vector2 getIndex()
    {
        return index;
    }

    /// <summary>
    /// return if this grid is blocked
    /// </summary>
    /// <returns></returns>
    public bool isBlocked()
    {
        return blocked;
    }

    /// <summary>
    /// set if the grid has a wall on it
    /// </summary>
    /// <param name="b"></param>
    public void setWall(bool b)
    {
        wall = b;
    }

    /// <summary>
    /// returns if the grid has a wall on it
    /// </summary>
    /// <returns></returns>
    public bool isWall()
    {
        return wall;
    }

    /// <summary>
    /// returns if the grid has a wall or ghostwall on it
    /// </summary>
    /// <returns></returns>
    public bool isWallOrGhostwall()
    {
        return wall || ghostwall;
    }

    /// <summary>
    /// returns if the grid has a ghostwall on it
    /// </summary>
    /// <returns></returns>
    public bool isGhostWall()
    {
        return ghostwall;
    }

    /// <summary>
    /// sets the ghostwall on this grid
    /// </summary>
    /// <param name="b"></param>
    public void setGhostWall(bool b)
    {
        ghostwall = b;
    }

    /// <summary>
    /// sets the distance value of this grid (used for pathfinding)
    /// </summary>
    public void setDistanceValue()
    {
        pathDestDistance = pathStartDistance + pathDestDistance;
    }

    /// <summary>
    /// returns the distance value of this grid (used for pathfinding)
    /// </summary>
    /// <returns></returns>
    public float getDistanceValue()
    {
        return pathDestDistance;
    }

    /// <summary>
    /// returns if this grid is movable
    /// </summary>
    /// <returns></returns>
    public bool isPathBlocked()
    {
        return pathBlocked;
    }

    /// <summary>
    /// set if the path is already selected by the pathmanager (used for pathfinding)
    /// </summary>
    /// <param name="b"></param>
    public void setPathSelected(bool b)
    {
        pathSelected = b;
    }

    /// <summary>
    /// retunrns if the path is already selected (used for pathfinding)
    /// </summary>
    /// <returns></returns>
    public bool isPathSelected()
    {
        return pathSelected;
    }

    /// <summary>
    /// block the grid with a object
    /// </summary>
    /// <param name="go"></param>
    public void block(GameObject go)
    {
        blockingObject = go;
        blocked = true;
    }

    /// <summary>
    /// block the path of this grid to make it unmovable
    /// </summary>
    public void blockPath()
    {
        pathBlocked = true;
    }

    /// <summary>
    /// returns the object this grid is beeing blocked by
    /// </summary>
    /// <returns></returns>
    public GameObject getBlockingObject()
    {
        return blockingObject;
    }

    /// <summary>
    /// sets the path destination distance (used by pathfinding)
    /// </summary>
    /// <param name="f"></param>
    public void setPathDestinationDistance(float f)
    {
        pathDestDistance = f;
    }

    /// <summary>
    /// returns the path destination distance (used by pathfinding)
    /// </summary>
    /// <returns></returns>
    public float getPathDestinationDistance()
    {
        return pathDestDistance;
    }

    /// <summary>
    /// sets the starting path distance (used by pathfinding)
    /// </summary>
    /// <param name="f"></param>
    public void setPathStartingDistance(float f)
    {
        pathStartDistance = f;
    }

    /// <summary>
    /// returnds the starting path distance (used by pathfinding)
    /// </summary>
    /// <returns></returns>
    public float getPathStartingDistance()
    {
        return pathStartDistance;
    }

    /// <summary>
    /// sets the path origin of this grid (used by pathfinding)
    /// </summary>
    /// <param name="g"></param>
    public void setPathOrigin(Grid g)
    {
        pathOrigin = g;
    }

    /// <summary>
    /// returns the path origin of this grid (used by pathfinding)
    /// </summary>
    /// <returns></returns>
    public Grid getPathOrigin()
    {
        return pathOrigin;
    }

    /// <summary>
    /// unblock this grid
    /// </summary>
    public void unBlock()
    {
        building = false;
        blockingObject = null;
        blocked = false;
    }

    /// <summary>
    /// get the size of this grid
    /// </summary>
    /// <returns></returns>
    public Vector2 getSize()
    {
        return size;
    }

    /// <summary>
    /// add a unit that is on this grid
    /// </summary>
    /// <param name="go"></param>
    public void addUnitOn(GameObject go)
    {
        unitsOn.Add(go);
    }

    /// <summary>
    /// removes a unit that is on this grid
    /// </summary>
    /// <param name="go">unit</param>
    public void removeUnitOn(GameObject go)
    {
        unitsOn.Remove(go);
    }

    /// <summary>
    /// removes a unit that is on this grid
    /// </summary>
    /// <param name="i">index</param>
    public void removeUnitOn(int i)
    {
        unitsOn.RemoveAt(i);
    }

    /// <summary>
    /// returns the units on this grid
    /// </summary>
    /// <returns></returns>
    public List<GameObject> getUnitsOn()
    {
        return unitsOn;
    }

    /// <summary>
    /// used to check if this grid has units on
    /// </summary>
    /// <returns></returns>
    public bool hasUnitsOn()
    {
        return unitsOn.Count > 0;
    }

    /// <summary>
    /// add a enemy that is on this grid
    /// </summary>
    /// <param name="go">enemy</param>
    public void addEnemyOn(GameObject go)
    {
        enemysOn.Add(go);
    }

    /// <summary>
    /// remove a enemy that is on this grid
    /// </summary>
    /// <param name="go">enemy</param>
    public void removeEnemyOn(GameObject go)
    {
        enemysOn.Remove(go);
    }

    /// <summary>
    /// remove a enemy that is on this grid
    /// </summary>
    /// <param name="i">index</param>
    public void removeEnemyOn(int i)
    {
        enemysOn.RemoveAt(i);
    }

    /// <summary>
    /// returns the enemys on this grid
    /// </summary>
    /// <returns></returns>
    public List<GameObject> getEnemysOn()
    {
        return enemysOn;
    }

    /// <summary>
    /// check if this grid has enemys on it
    /// </summary>
    /// <returns></returns>
    public bool hasEnemysOn()
    {
        return enemysOn.Count > 0;
    }
}
