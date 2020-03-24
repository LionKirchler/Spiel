using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    private List<GameObject> buildings = new List<GameObject>();
    private List<GameObject> houses = new List<GameObject>();
    private List<GameObject> barracks = new List<GameObject>();
    private List<GameObject> woodWalls = new List<GameObject>();
    private List<GameObject> mainBuildings = new List<GameObject>();
    private GameObject currentGhostBuilding;
    private string currentGhostBuildingName = "";

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>
    /// sets current ghostbuilding used by the player, if this is called the name should also be set in the SetCurrentGhostBuildingName() method
    /// </summary>
    /// <param name="house">the ghostbuilding to be set</param>
    public void addCurrentGhostBuilding(GameObject house)
    {
        currentGhostBuilding = house;
    }

    /// <summary>
    /// returns the ghostbuilding currently used by the player
    /// </summary>
    /// <returns></returns>
    public GameObject getCurrentGhostBuilding()
    {      
            return currentGhostBuilding;
    }

    /// <summary>
    /// adds a house to the house list
    /// </summary>
    /// <param name="house">house to add</param>
    public void addHouse(GameObject house)
    {
        houses.Add(house);
    }

    /// <summary>
    /// removes a house from the house list, for example after it beeing destroyed
    /// </summary>
    /// <param name="house">house to remove</param>
    public void removeHouse(GameObject house)
    {
        try
        {
            houses.Remove(house);
        }
        catch { }
    }

    /// <summary>
    /// adds a barrack to the barrack list
    /// </summary>
    /// <param name="barrack">barrack to add</param>
    public void addBarrack(GameObject barrack)
    {
        barracks.Add(barrack);
    }

    /// <summary>
    /// removes a barrack from the barrack list, for example after it beeing destroyed
    /// </summary>
    /// <param name="barrack">barrack to remove</param>
    public void removeBarrack(GameObject barrack)
    {
        try
        {
            barracks.Remove(barrack);
        }
        catch { }
    }

    /// <summary>
    /// adds a wood wall to the wood wall list
    /// </summary>
    /// <param name="woodWall">wood wall to add</param>
    public void addWoodWall(GameObject woodWall)
    {
        woodWalls.Add(woodWall);
    }

    /// <summary>
    /// removes a wood wall from the wood wall list, for example after it beeing destroyed
    /// </summary>
    /// <param name="woodWall">wood wall to remove</param>
    public void removeWoodWall(GameObject woodWall)
    {
        try
        {
            woodWalls.Remove(woodWall);
        }
        catch { }
    }

    /// <summary>
    /// adds a main building to the mainbuilding list
    /// </summary>
    /// <param name="mainBuilding">mainbuilding to add</param>
    public void addMainBuilding(GameObject mainBuilding)
    {
        mainBuildings.Add(mainBuilding);
    }

    /// <summary>
    /// removes a mainbuilding from the mainbuilding list, for example after it beeing destroyed
    /// </summary>
    /// <param name="mainBuilding">mainbuilding to remove</param>
    public void removeMainBuilding(GameObject mainBuilding)
    {
        try
        {
            mainBuildings.Remove(mainBuilding);
        }
        catch { }
    }

    /// <summary>
    /// adds a building to the general building list, this contains all buildings
    /// </summary>
    /// <param name="building">building to add</param>
    public void addBuilding(GameObject building)
    {
        buildings.Add(building);
    }

    /// <summary>
    /// removes a building from the general building list, for example after it beeing destroyed
    /// </summary>
    /// <param name="building">building to remove</param>
    public void removeBuilding(GameObject building)
    {
        try
        {
            buildings.Remove(building);
        }
        catch { }
    }

    /// <summary>
    /// returns the list of buildings
    /// </summary>
    /// <returns></returns>
    public List<GameObject> getBuildings()
    {
        return buildings;
    }
    /// <summary>
    /// returns the list of houses
    /// </summary>
    /// <returns></returns>
    public List<GameObject> getHouses()
    {
        return houses;
    }
    /// <summary>
    /// returns the list of barracks
    /// </summary>
    /// <returns></returns>
    public List<GameObject> getBarracks()
    {
        return barracks;
    }

    /// <summary>
    /// returns the name of the current ghost building, used to check what it is
    /// </summary>
    /// <returns></returns>
    public string GetCurrentGhostBuildingName()
    {
        return currentGhostBuildingName;
    }

    /// <summary>
    /// sets the name of the current ghost building, should be done if the ghostbuiling is beeing set
    /// </summary>
    /// <param name="name">name to be set</param>
    public void SetCurrentGhostBuildingName(string name)
    {
        currentGhostBuildingName = name;
    }
}
