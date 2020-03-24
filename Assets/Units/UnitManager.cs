using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{

    private List<GameObject> units = new List<GameObject>(10);
    [SerializeField]
    private GameObject canvas = null;
    private GUIDisplayer guiDisplayer;
    private int recruitingUnitCount = 0;
    private Player player;
    private MapGrid mapGrid;
    private Grid workGrid;
    private Grid closestStorage;
    private Grid closestTree;
    private int unitLimitCount = 10;
    private int unitCount = 0;
    private PathFinderManager pathFinderManager;
    public void onStart()
    {
        guiDisplayer = canvas.GetComponent<GUIDisplayer>();
        player = GetComponent<Player>();
        mapGrid = player.getMapGrid();
        pathFinderManager = player.getSpawner().getPathFinderManager();
    }

    void Update()
    {
    }

    /// <summary>
    /// adds a unit to the unit list
    /// </summary>
    /// <param name="unit">unit</param>
    public void addUnit(GameObject unit)
    {
        units.Add(unit);
    }

    /// <summary>
    /// removes a unit from the units list
    /// </summary>
    /// <param name="unit">unit</param>
    public void removeUnit(GameObject unit)
    {
        units.Remove(unit);
    }

    /// <summary>
    /// retuns the unit list
    /// </summary>
    /// <returns></returns>
    public List<GameObject> getUnits()
    {
        return units;
    }

    /// <summary>
    /// returns all selected units
    /// </summary>
    /// <returns></returns>
    public List<GameObject> getSelectedUnits()
    {
        List<GameObject> selected = new List<GameObject>();
        foreach (GameObject unit in units)
        {
            if (unit.GetComponent<Unit>().isSelected())
            {
                selected.Add(unit);
            }
        }
        return selected;
    }

    /// <summary>
    /// returns all selected workers
    /// </summary>
    /// <returns></returns>
    public List<GameObject> getSelectedWorkers()
    {
        List<GameObject> selected = new List<GameObject>();
        foreach (GameObject unit in units)
        {
            if (unit.GetComponent<Worker>() != null)
            {
                if (unit.GetComponent<Worker>().isSelected())
                {
                    selected.Add(unit);
                }
            }
        }
        return selected;
    }

    /// <summary>
    /// returns all selected combat units
    /// </summary>
    /// <returns></returns>
    public List<GameObject> getSelectedCombatUnits()
    {
        List<GameObject> selected = new List<GameObject>();
        foreach (GameObject unit in units)
        {
            if (unit.GetComponent<Unit>().isCombatUnit())
            {
                if (unit.GetComponent<Unit>().isSelected())
                {
                    selected.Add(unit);
                }
            }
        }
        return selected;
    }

    /// <summary>
    /// returns all workers
    /// </summary>
    /// <returns></returns>
    public List<GameObject> getWorkers()
    {
        List<GameObject> wrks = new List<GameObject>();
        foreach (GameObject unit in units)
        {
            if (unit.GetComponent<Worker>() != null)
            {
                wrks.Add(unit);
            }
        }
        return wrks;
    }

    /// <summary>
    /// moves all selected units (if clicked on enemy tree etc, they attack,work etc.)
    /// </summary>
    public void move()
    {
        Vector2 mousePos = CalcUtil.getMousePos();
        Grid grid = mapGrid.getGridOn(mousePos);
        workGrid = null;
        Tree to = null;
        IronOre io = null;
        string gridType = "";
        try
        {
           to = grid.getBlockingObject().GetComponent<Tree>();
           io = grid.getBlockingObject().GetComponent<IronOre>();
        }
        catch{ }
        
        if(to != null)
        {
            workGrid = grid;
            gridType = "wood";
            
        }else if(io != null)
        {
            workGrid = grid;
            gridType = "ironOre";
        }
        GameObject enemy = enemyClicked(mousePos);
        if (workGrid != null)
        {

            List<GameObject> workers = getSelectedWorkers();

            findNearestStorage();

            foreach (GameObject worker in workers)
            {
                if (gridType.Equals("wood"))
                {
                    if (worker.GetComponent<Worker>().isLumberjack())
                    {
                        to.addUnit(worker.GetComponent<Unit>());
                        worker.GetComponent<Worker>().setWorkGrid(grid, "wood");
                        worker.GetComponent<Worker>().startWorking(true);
                    }
                }else if (gridType.Equals("ironOre"))
                {
                    if (worker.GetComponent<Worker>().isIronOreMiner())
                    {
                        io.addUnit(worker.GetComponent<Unit>());
                        worker.GetComponent<Worker>().setWorkGrid(grid, "ironOre");
                        worker.GetComponent<Worker>().startWorking(true);
                    }
                }
            }
        }else if (enemy != null)
        {
            foreach (GameObject unit in getSelectedCombatUnits())
            {
                unit.GetComponent<Unit>().attack(enemy);
            }
        }
        else
        {
            moveRectForation(mousePos);
            foreach(GameObject worker in getSelectedWorkers())
            {
               worker.GetComponent<Worker>().startWorking(false);
            }
        }
    }


    /// <summary>
    /// returns the nearest storage
    /// </summary>
    public void findNearestStorage()
    {

        closestStorage = pathFinderManager.FindNearestGrid(workGrid, mapGrid.getStorages(), mapGrid);

        List<GameObject> workers = getSelectedWorkers();
        foreach (GameObject worker in workers)
        {

            if (worker.GetComponent<Worker>().isLumberjack())
            {
                worker.GetComponent<Worker>().setClosestStorage(closestStorage);
                worker.GetComponent<Worker>().startWorking(true);
            }
        }
    }

    /// <summary>
    /// returns the closest tree
    /// </summary>
    public void findNearestTree()
    {

        closestTree = pathFinderManager.FindNearestGrid(workGrid, mapGrid.getTrees(), mapGrid);

        List<GameObject> workers = getSelectedWorkers();
        foreach (GameObject worker in workers)
        {

            if (worker.GetComponent<Worker>().isLumberjack())
            {
                worker.GetComponent<Worker>().setClosestStorage(closestStorage);
                worker.GetComponent<Worker>().startWorking(true);
            }
        }
    }

    /// <summary>
    /// makes units move in rectangular formation
    /// </summary>
    /// <param name="destination">destination</param>
    public void moveRectForation(Vector2 destination)
   {
        int side = Mathf.CeilToInt(Mathf.Sqrt(getSelectedUnits().Count));
        int dist = 1;
        destination.x -= dist / 2f;
        destination.x += side / 2f;
        float sideDown = 0;
        //Debug.Log("count : " + getSelectedUnits().Count);
        //Debug.Log("side : " + side);
        for(int j = getSelectedUnits().Count; j > 0; j -= side)
        {
            //Debug.Log("j : " + j);
            sideDown++;
            //Debug.Log("sidedown : " + sideDown);
        }

        destination.y += dist / 2f;
        destination.y += sideDown / 2f;

        int i = 0;
        foreach(GameObject unit in getSelectedUnits())
        {
            if(i % side == 0)
            {
                destination.y -= dist;
                destination.x -= side;
            }

            destination.x += dist;
            if (!mapGrid.getGridOn(destination).isPathBlocked())
            {
                Path path = pathFinderManager.FindPath(mapGrid.getGridOn(getSelectedUnits()[i].transform.position), destination, mapGrid);
                getSelectedUnits()[i].GetComponent<Unit>().moveAction(path);
                unit.GetComponent<Unit>().moveAction(path);
            }
            i++;
        }
    }

    /// <summary>
    /// ckecks if a enemy is clicked
    /// </summary>
    /// <param name="pos">position</param>
    /// <returns>clicked enemy (null if none is clicked)</returns>
    public GameObject enemyClicked(Vector2 pos)
    {
        GameObject ret = null;

        RaycastHit2D[] hits = Physics2D.RaycastAll(pos, Vector2.zero);

        if(hits != null && hits.Length > 0)
        {
            foreach(RaycastHit2D hit in hits)
            {
                if(hit.transform.gameObject.tag == "Enemy")
                {
                    ret = hit.transform.gameObject;
                    break;
                }
            }
        }

        return ret;
    }

    /// <summary>
    /// adds unit count
    /// </summary>
    /// <param name="amount">unit count to add</param>
    public void addUnitCount(int amount)
    {
        unitCount += amount;
        guiDisplayer.updateUnitCount(unitCount);
    }

    /// <summary>
    /// returns the unit count
    /// </summary>
    /// <returns></returns>
    public int getUnitCount()
    {
        return unitCount;
    }

    /// <summary>
    /// increases the unit limit
    /// </summary>
    /// <param name="amount">add amount</param>
    public void addUnitLimit(int amount)
    {
        unitLimitCount += amount;
        guiDisplayer.updateUnitLimit(unitLimitCount);
    }
    /// <summary>
    /// returns the unit limit
    /// </summary>
    /// <returns></returns>
    public int getUnitLimit()
    {
        return unitLimitCount;
    }

    /// <summary>
    /// returns the recruiting unit count
    /// </summary>
    /// <returns></returns>
    public int getRectruitingUnitCount()
    {
        return recruitingUnitCount;
    }

    /// <summary>
    /// adds recruiting unit count
    /// </summary>
    /// <param name="amount">amount</param>
    public void addRecruitingUnitCount(int amount)
    {
        recruitingUnitCount += amount;
    }

    /// <summary>
    /// removes unit recruiting amount
    /// </summary>
    /// <param name="amount">amount</param>
    public void removeRecruitingUnitCount(int amount)
    {
        recruitingUnitCount -= amount;
    }

    /// <summary>
    /// returns the gui displayer
    /// </summary>
    /// <returns>gui displayer</returns>
    public GUIDisplayer getGuiDisplayer()
    {
        return guiDisplayer;
    }

}
