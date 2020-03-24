using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //[SerializeField]
    //private GameObject testUnit = null;
    [SerializeField]
    private GameObject knight = null;
    [SerializeField]
    private GameObject house = null;
    [SerializeField]
    private GameObject ghostHouse = null;
    [SerializeField]
    private GameObject barrack = null;
    [SerializeField]
    private GameObject ghostBarrack = null;
    [SerializeField]
    private GameObject mainBuilding = null;
    [SerializeField]
    private GameObject ghostMainBuilding = null;
    [SerializeField]
    private GameObject workerMan = null;
    [SerializeField]
    private GameObject pathFinder = null;
    [SerializeField]
    private GameObject pathFinderManager = null;
    [SerializeField]
    private GameObject woodWallFront = null;
    [SerializeField]
    private GameObject ghostWoodWallFront = null;  
    [SerializeField]
    private GameObject woodWallSide = null;
    [SerializeField]
    private GameObject ghostWoodWallSide = null;
    [SerializeField]
    private GameObject woodWallCorner = null;
    [SerializeField]
    private GameObject ghostWoodWallCorner = null;
    [SerializeField]
    private GameObject woodWallManager = null;
    [SerializeField]
    private GameObject GhostWoodWall = null;
    [SerializeField]
    private GameObject woodWall = null;  
    [SerializeField]
    private GameObject zombie1 = null;  
    [SerializeField]
    private GameObject archer = null;
    [SerializeField]
    private GameObject archerArrow = null;
    [SerializeField]
    private GameObject mapManager = null;
    private Player player;
    private UnitManager unitManager;
    private GUIDisplayer guiDisplayer;
    private GameObject woodWallManagerSpawned = null;
    private PathFinderManager pathFinderManagerMain = null;

    public void onStart()
    {
        player = transform.gameObject.GetComponent<Player>();
        guiDisplayer = transform.gameObject.GetComponent<UnitManager>().getGuiDisplayer();
        woodWallManagerSpawned = spawnWoodWallManager(Vector2.zero);
        unitManager = player.getUnitManager();
    }


    /// <summary>
    /// returns the main pathfindermanager
    /// </summary>
    /// <returns></returns>
    public PathFinderManager getPathFinderManager()
    {
        return pathFinderManagerMain;
    }

    /// <summary>
    /// spawns a pathfindermanager
    /// </summary>
    /// <returns></returns>
    public GameObject SpawnPathFinderManager()
    {
        GameObject go = Instantiate(pathFinderManager);
        pathFinderManagerMain = go.GetComponent<PathFinderManager>();
        pathFinderManagerMain.setMapGrid(player.getMapGrid());
        return go;
    }

    /// <summary>
    /// returns the main wood wall manager
    /// </summary>
    /// <returns></returns>
    public GameObject getWoodWallManager()
    {
        return woodWallManagerSpawned;
    }

    /// <summary>
    /// spawns a archer arrow
    /// </summary>
    /// <param name="pos">spawn position</param>
    /// <returns></returns>
    public GameObject spawnArcherArrow(Vector2 pos)
    {
        GameObject go = Instantiate(archerArrow, pos, archerArrow.transform.rotation);
        return go;
    }

    /// <summary>
    /// spawns a knight
    /// </summary>
    /// <param name="pos">spawn positon</param>
    public void spawnKnight(Vector2 pos)
    {
        GameObject go = Instantiate(knight, pos, knight.transform.rotation);
        go.GetComponent<Unit>().setMapGrid(mapManager.GetComponent<MapGrid>());
        player.getUnitManager().addUnit(go);
        player.getUnitManager().addUnitCount(knight.GetComponent<Unit>().getunitCountSize());
        go.GetComponent<Unit>().PassSpawner(this);
        go.GetComponent<Unit>().setPlayer(player);
        go.GetComponent<Unit>().setUnitManager(unitManager);
        go.GetComponent<Unit>().setGridOn();
    }

    /// <summary>
    /// spawns a archer
    /// </summary>
    /// <param name="pos">spawn position</param>
    public void spawnArcher(Vector2 pos)
    {
        GameObject go = Instantiate(archer, pos, archer.transform.rotation);
        go.GetComponent<Unit>().setMapGrid(mapManager.GetComponent<MapGrid>());
        player.getUnitManager().addUnit(go);
        player.getUnitManager().addUnitCount(archer.GetComponent<Unit>().getunitCountSize());
        go.GetComponent<Unit>().PassSpawner(this);
        go.GetComponent<Unit>().setPlayer(player);
        go.GetComponent<Unit>().setUnitManager(unitManager);
        go.GetComponent<Unit>().setGridOn();
    }
    /// <summary>
    /// spawns a zombie
    /// </summary>
    /// <param name="pos">spawn position</param>
    public GameObject spawnZombie1(Vector2 pos)
    {
        GameObject go = Instantiate(zombie1, pos, zombie1.transform.rotation);
        go.GetComponent<Enemy>().setMapGrid(mapManager.GetComponent<MapGrid>());
        player.getEnemyManager().addEnemy(go);
        go.GetComponent<Enemy>().PassSpawner(this);
        go.GetComponent<Enemy>().setPlayer(player);
        go.GetComponent<Enemy>().setMapGrid(player.getMapGrid());
        go.GetComponent<Enemy>().setGridOn();
        return go;
    }

    /// <summary>
    /// spawns a male worker
    /// </summary>
    /// <param name="pos">spawn position</param>
    public void spawnWorkerMan(Vector2 pos)
    {
        GameObject go = Instantiate(workerMan, pos, workerMan.transform.rotation);
        go.GetComponent<Unit>().setMapGrid(mapManager.GetComponent<MapGrid>());
        player.getUnitManager().addUnit(go);
        player.getUnitManager().addUnitCount(knight.GetComponent<Unit>().getunitCountSize());
        go.GetComponent<Unit>().PassSpawner(this);
        go.GetComponent<Unit>().setPlayer(player);
        go.GetComponent<Unit>().setUnitManager(unitManager);
        go.GetComponent<Unit>().setGridOn();
    }

    /// <summary>
    /// spawns a house
    /// </summary>
    /// <param name="pos">spawn position</param>
    /// <returns>spawned house</returns>
    public GameObject spawnHouse(Vector2 pos)
    {
        GameObject go = Instantiate(house, pos, house.transform.rotation);
        player.GetBuildingManager().addBuilding(go);
        player.GetBuildingManager().addHouse(go);
        player.getUnitManager().addUnitLimit(go.GetComponent<Building>().getUnitSpace());
        go.GetComponent<Building>().setPlayer(player);

        refreshMap();
        return go;
    }

    /// <summary>
    /// spawnes a ghosthouse
    /// </summary>
    /// <param name="pos">spawn position</param>
    /// <returns>spawned ghosthouse</returns>
    public GameObject spawnGhostHouse(Vector2 pos)
    {
        GameObject go = Instantiate(ghostHouse, pos, ghostHouse.transform.rotation);
        player.GetBuildingManager().addCurrentGhostBuilding(go);
        player.GetBuildingManager().SetCurrentGhostBuildingName("House");
        go.GetComponent<GhostBuilding>().setPlayer(player);
        return go;
    }

    /// <summary>
    /// spawns a barrack
    /// </summary>
    /// <param name="pos">spawn position</param>
    /// <returns>spawned barrack</returns>
    public GameObject spawnBarrack(Vector2 pos)
    {
        GameObject go = Instantiate(barrack, pos, barrack.transform.rotation);
        player.GetBuildingManager().addBuilding(go);
        player.GetBuildingManager().addBarrack(go);
        player.getUnitManager().addUnitLimit(barrack.GetComponent<Building>().getUnitSpace());
        refreshMap();
        go.GetComponent<Building>().setPlayer(player);

        return go;
    }
    
    /// <summary>
    /// spawnes a ghostbarrack
    /// </summary>
    /// <param name="pos">spawn position</param>
    /// <returns>spawned ghostbarrack</returns>
    public GameObject spawnGhostBarrack(Vector2 pos)
    {
        GameObject go = Instantiate(ghostBarrack, pos, ghostBarrack.transform.rotation);
        player.GetBuildingManager().addCurrentGhostBuilding(go);
        player.GetBuildingManager().SetCurrentGhostBuildingName("Barrack");
        go.GetComponent<GhostBuilding>().setPlayer(player);
        return go;
    }

    /// <summary>
    /// spawnes a ghostwoodwall
    /// </summary>
    /// <param name="pos">spawn position</param>
    /// <returns>spawned ghostwoodwall</returns>
    public GameObject spawnGhostWoodWall(Vector2 pos)
    {
        GameObject go = Instantiate(GhostWoodWall, pos, GhostWoodWall.transform.rotation);
        //player.GetBuildingManager().addCurrentGhostBuilding(go);
        //player.GetBuildingManager().SetCurrentGhostBuildingName("GhostWoodWall");
        go.GetComponent<GhostBuilding>().setPlayer(player);
        return go;
    }

    /// <summary>
    /// spawns a ghostmainBuilding
    /// </summary>
    /// <param name="pos">spawn position</param>
    /// <returns>spawned ghostmainbuilding</returns>
    public GameObject spawnGhostMainBuilding(Vector2 pos)
    {
        GameObject go = Instantiate(ghostMainBuilding, pos, ghostMainBuilding.transform.rotation);
        player.GetBuildingManager().addCurrentGhostBuilding(go);
        player.GetBuildingManager().SetCurrentGhostBuildingName("MainBuilding");
        go.GetComponent<GhostBuilding>().setPlayer(player);
        return go;
    }

    /// <summary>
    /// spawn a ghostwoodwallfront
    /// should not be used, use ghost wood wall types
    /// </summary>
    /// <param name="pos">spawn position</param>
    /// <returns>spawned ghostwoodwallfront</returns>
    public GameObject spawnGhostWoodWallFront(Vector2 pos)
    {
        GameObject go = Instantiate(ghostWoodWallFront, pos, ghostWoodWallFront.transform.rotation);
        player.GetBuildingManager().addCurrentGhostBuilding(go);
        player.GetBuildingManager().SetCurrentGhostBuildingName("WoodWallFront");
        go.GetComponent<GhostBuilding>().setPlayer(player);
        return go;
    }

    /// <summary>
    /// spawn a woodwallfront
    /// should not be used, use wood wall types
    /// </summary>
    /// <param name="pos">spawn position</param>
    /// <returns>spawned woodwallfront</returns>
    public GameObject spawnWoodWallFront(Vector2 pos)
    {
        GameObject go = Instantiate(woodWallFront, pos, woodWallFront.transform.rotation);
        //player.GetBuildingManager().addBuilding(go);
        player.GetBuildingManager().addWoodWall(go);
        refreshMap();
        return go;
    }
    /// <summary>
    /// spawn a ghostwoodwallside
    /// should not be used, use ghost wood wall types
    /// </summary>
    /// <param name="pos">spawn position</param>
    /// <returns>spawned ghostwoodwallside</returns>
    public void spawnGhostWoodWallSide(Vector2 pos)
    {
        GameObject go = Instantiate(ghostWoodWallSide, pos, ghostWoodWallSide.transform.rotation);
        player.GetBuildingManager().addCurrentGhostBuilding(go);
        player.GetBuildingManager().SetCurrentGhostBuildingName("WoodWallSide");
        go.GetComponent<GhostBuilding>().setPlayer(player);
    }

    /// <summary>
    /// spawns a wood wall manager (used to build wood walls)
    /// </summary>
    /// <param name="pos">spawn position</param>
    /// <returns>spawned woodwallmanager</returns>
    private GameObject spawnWoodWallManager(Vector2 pos)
    {
        GameObject go = Instantiate(woodWallManager, pos, woodWallManager.transform.rotation);
        go.GetComponent<WoodWallManager>().setMapGrid(player.getMapGrid());
        go.GetComponent<WoodWallManager>().setSpawner(this);
        go.GetComponent<WoodWallManager>().setPlayer(player);
        return go;
    }

    /// <summary>
    /// spawns a wood wall
    /// </summary>
    /// <param name="pos">spawn position</param>
    /// <param name="type">spawn type (side, corner etc.)</param>
    /// <returns>spawned woodwall</returns>
    public GameObject spawnWoodWall(Vector2 pos, string type)
    {
        GameObject go = Instantiate(woodWall, pos, woodWall.transform.rotation);
        go.GetComponent<WoodWall>().setType(type);
        go.GetComponent<Building>().setPlayer(player);
        player.GetBuildingManager().addBuilding(go);
        player.GetBuildingManager().addBarrack(go);
        return go;
    }

    /// <summary>
    /// spawn a woodwallside
    /// should not be used, use wood wall types
    /// </summary>
    /// <param name="pos">spawn position</param>
    /// <returns>spawned woodwallside</returns>
    public GameObject spawnWoodWallSide(Vector2 pos)
    {
        GameObject go = Instantiate(woodWallSide, pos, woodWallSide.transform.rotation);
        //player.GetBuildingManager().addBuilding(go);
        player.GetBuildingManager().addWoodWall(go);
        refreshMap();
        return go;
    }

    /// <summary>
    /// spawn a ghostwoodwallcorner
    /// should not be used, use ghost wood wall types
    /// </summary>
    /// <param name="pos">spawn position</param>
    /// <returns>spawned ghostwoodwallcorner</returns>
    public void spawnGhostWoodWallCorner(Vector2 pos)
    {
        GameObject go = Instantiate(ghostWoodWallCorner, pos, ghostWoodWallCorner.transform.rotation);
        // player.GetBuildingManager().addCurrentGhostBuilding(go);
        player.GetBuildingManager().SetCurrentGhostBuildingName("WoodWallCorner");
        go.GetComponent<GhostBuilding>().setPlayer(player);
    }
    /// <summary>
    /// spawn a woodwallcorner
    /// should not be used, use wood wall types
    /// </summary>
    /// <param name="pos">spawn position</param>
    /// <returns>spawned woodwallcorner</returns>
    public GameObject spawnWoodWallCorner(Vector2 pos)
    {
        GameObject go = Instantiate(woodWallCorner, pos, woodWallCorner.transform.rotation);
        //player.GetBuildingManager().addBuilding(go);
        player.GetBuildingManager().addWoodWall(go);
        refreshMap();
        return go;
    }

    /// <summary>
    /// spawnes a main building
    /// </summary>
    /// <param name="pos">spawn position</param>
    /// <returns>spawned main building</returns>
    public GameObject spawnMainBuilding(Vector2 pos)
    {
        GameObject go = Instantiate(mainBuilding, pos, mainBuilding.transform.rotation);
        player.GetBuildingManager().addBuilding(go);
        player.GetBuildingManager().addMainBuilding(go);
        player.getUnitManager().addUnitLimit(go.GetComponent<Building>().getUnitSpace());
        refreshMap();
        go.GetComponent<Building>().setPlayer(player);
        return go;
    }

    /// <summary>
    /// spawn building for curren ghostbuilding
    /// </summary>
    /// <param name="pos">spawn position</param>
    /// <returns>spawned building</returns>
    public GameObject spawnBuildingForGhost(Vector2 pos)
    {
        if (player.GetBuildingManager().GetCurrentGhostBuildingName().Equals("Barrack"))
        {
            return spawnBarrack(pos);
        }
        else if (player.GetBuildingManager().GetCurrentGhostBuildingName().Equals("House"))
        {
            return spawnHouse(pos);
        }
        else if (player.GetBuildingManager().GetCurrentGhostBuildingName().Equals("MainBuilding"))
        {
            return spawnMainBuilding(pos);
        }
        else if (player.GetBuildingManager().GetCurrentGhostBuildingName().Equals("WoodWallFront"))
        {
            return spawnWoodWallFront(pos);
        }
        else if (player.GetBuildingManager().GetCurrentGhostBuildingName().Equals("WoodWallSide"))
        {
            return spawnWoodWallSide(pos);
        }
        else if (player.GetBuildingManager().GetCurrentGhostBuildingName().Equals("WoodWallCorner"))
        {
            return spawnWoodWallCorner(pos);
        }
        return null;
    }

    /// <summary>
    /// spawnes a ghostbuilding for a ghostbuilding
    /// </summary>
    /// <param name="pos">spawn position</param>
    /// <returns>spawned ghostbuilding</returns>
    public GameObject spawnGhostForGhost(Vector2 pos)
    {
        if (player.GetBuildingManager().GetCurrentGhostBuildingName().Equals("Barrack"))
        {
            return spawnGhostBarrack(pos);
        }
        else if (player.GetBuildingManager().GetCurrentGhostBuildingName().Equals("House"))
        {
            return spawnGhostHouse(pos);
        }
        else if (player.GetBuildingManager().GetCurrentGhostBuildingName().Equals("MainBuilding"))
        {
            return spawnGhostMainBuilding(pos);
        }

        return null;
    }

    /// <summary>
    /// returns a knight component as a sample
    /// </summary>
    /// <returns></returns>
    public Unit getKnightComponent()
    {
        return knight.GetComponent<Unit>();
    }

    /// <summary>
    /// returns a archer component as a spample
    /// </summary>
    /// <returns></returns>
    public Unit getArcherComponent()
    {
        return archer.GetComponent<Unit>();
    }

    /// <summary>
    /// returns a worker man component as a sample
    /// </summary>
    /// <returns></returns>
    public Unit getworkerManComponent()
    {
        return workerMan.GetComponent<Unit>();
    }

    /// <summary>
    /// returns a ghostmainbuilding component as a sample
    /// </summary>
    /// <returns></returns>
    public GhostMainBuilding getGhostMainBuildingComponent()
    {
        return ghostMainBuilding.GetComponent<GhostMainBuilding>();
    }

    /// <summary>
    /// refreshes the map
    /// outdated, it does currently nothing
    /// </summary>
    public void refreshMap()
    {
        //astarPath.Scan();
    }

}
