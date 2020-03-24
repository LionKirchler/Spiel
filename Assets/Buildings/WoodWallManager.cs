using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodWallManager : MonoBehaviour
{

    private Grid start;
    private Grid end;
    private MapGrid mg;
    private Spawner spawner;
    private Player player;
    private List<GameObject> walls = new List<GameObject>();
    private List<Grid> gridsOn = new List<Grid>();
    private bool building = false;
    private int woodMat = 0;
    private int ironMat = 0;
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            GameObject g = spawner.spawnGhostWoodWall(Vector2.zero);
            g.SetActive(false);
            walls.Add(g);
        }
    }


    void LateUpdate()
    {
        //sets the start and end pos, sets the grids the walls should be on, sets the ghost walls, and on mouse release spawns the walls
        if (!building)
        {
            return;
        }

        Vector2 mousePos = CalcUtil.getMousePos();
        for (int i = 0; i < gridsOn.Count; i++)
        {
            gridsOn[i].setGhostWall(false);
        }
        gridsOn.Clear();

        if (Input.GetMouseButtonDown(0))
        {
            start = mg.getGridOn(mousePos);
        }

        
        Grid endt = mg.getGridOn(mousePos);
        if (endt != null)
        {
            end = endt;
        }
        if (start != null)
        {
            setGridsOn();
            spawnGhostWall();
        }
        if (Input.GetMouseButtonUp(0))
        {
            spawnWall();
            start = null;
            player.setBuildingBlocked(false);
            building = false;
            gridsOn.Clear();
            start = null;
            end = null;
        }

        if (Input.GetMouseButtonDown(1))
        {
            foreach (GameObject w in walls)
            {
                w.SetActive(false);
            }
            player.setBuildingBlocked(false);
            this.building = false;
            gridsOn.Clear();
            start = null;
            end = null;
        }
    }

    /// <summary>
    /// sets the grids the walls should be on
    /// </summary>
    public void setGridsOn()
    {
        int x0 = (int)start.getIndex().x;
        int y0 = (int)start.getIndex().y;
        int x1 = (int)end.getIndex().x;
        int y1 = (int)end.getIndex().y;

        if(x1 < x0)
        {
            (x0, x1) = (x1, x0);
            (y0, y1) = (y1, y0);
            x0--;
        }

        int dx = x1 - x0;
        int dy = y1 - y0;
        bool first = true;
        int y2 = 0;
        Grid g = null;
        int y;

        if (dx == 0)
        {
            if (y0 <= y1)
            {
                for (y = y0; y <= y1; y++)
                {
                    try
                    {
                        g = mg.getGrid()[x0, y];
                        gridsOn.Add(g);
                        g.setGhostWall(true);
                    }
                    catch { }
                }
            }
            else
            {
                for (y = y0; y >= y1; y--)
                {
                    try
                    {
                        g = mg.getGrid()[x0, y];
                        gridsOn.Add(g);
                        g.setGhostWall(true);
                    }
                    catch { }
                }
            }
        }
        else
        {
            for (int x = x0; x <= x1;)
            {

                y = y1 + dy * (x - x1) / dx;
                if (first)
                {
                    y2 = y;
                    first = false;
                }
                try
                {
                    g = mg.getGrid()[x, y2];
                    gridsOn.Add(g);
                    g.setGhostWall(true);
                }
                catch { }
                if (y2 == y)
                {
                    x++;
                }
                else
                {
                    if (y2 < y)
                    {
                        y2++;
                    }
                    else
                    {
                        y2--;
                    }
                }

            }
        }

    }

    /// <summary>
    /// spawns ghost wood walls on grids
    /// </summary>
    public void spawnGhostWall()
    {
        woodMat = 0;
        ironMat = 0;
        int i;
        if (gridsOn[0].getIndex() == start.getIndex())
        {
            for (i = 0; i < gridsOn.Count; i++)
            {
                GameObject go = walls[i];
                go.GetComponent<GhostWoodWall>().setGrid(gridsOn[i]);
                go.GetComponent<GhostWoodWall>().setType("front");
                bool t = false;
                try
                {
                    if ((mg.getGrid()[(int)gridsOn[i].getIndex().x, (int)gridsOn[i].getIndex().y - 1].isWallOrGhostwall() || mg.getGrid()[(int)gridsOn[i].getIndex().x, (int)gridsOn[i].getIndex().y + 1].isWallOrGhostwall()) && !mg.getGrid()[(int)gridsOn[i].getIndex().x - 1, (int)gridsOn[i].getIndex().y].isWallOrGhostwall() && !mg.getGrid()[(int)gridsOn[i].getIndex().x + 1, (int)gridsOn[i].getIndex().y].isWallOrGhostwall())
                    {
                        go.GetComponent<GhostWoodWall>().setType("side");
                        t = true;
                    }
                }
                catch { }
                try
                {
                    if (!t && (mg.getGrid()[(int)gridsOn[i].getIndex().x - 1, (int)gridsOn[i].getIndex().y].isWallOrGhostwall() || mg.getGrid()[(int)gridsOn[i].getIndex().x + 1, (int)gridsOn[i].getIndex().y].isWallOrGhostwall()) && !mg.getGrid()[(int)gridsOn[i].getIndex().x, (int)gridsOn[i].getIndex().y + 1].isWallOrGhostwall() && !mg.getGrid()[(int)gridsOn[i].getIndex().x, (int)gridsOn[i].getIndex().y - 1].isWallOrGhostwall())
                    {
                        go.GetComponent<GhostWoodWall>().setType("front");
                        t = true;
                    }
                }
                catch { }
                try
                {
                    if (!t && mg.getGrid()[(int)gridsOn[i].getIndex().x - 1, (int)gridsOn[i].getIndex().y].isWallOrGhostwall() && (mg.getGrid()[(int)gridsOn[i].getIndex().x, (int)gridsOn[i].getIndex().y + 1].isWallOrGhostwall() || mg.getGrid()[(int)gridsOn[i].getIndex().x, (int)gridsOn[i].getIndex().y - 1].isWallOrGhostwall()) && !mg.getGrid()[(int)gridsOn[i].getIndex().x + 1, (int)gridsOn[i].getIndex().y].isWallOrGhostwall())
                    {
                        go.GetComponent<GhostWoodWall>().setType("cornerl");
                        t = true;
                    }
                }
                catch { }
                try
                {
                    if (!t && mg.getGrid()[(int)gridsOn[i].getIndex().x + 1, (int)gridsOn[i].getIndex().y].isWallOrGhostwall() && (mg.getGrid()[(int)gridsOn[i].getIndex().x, (int)gridsOn[i].getIndex().y + 1].isWallOrGhostwall() || mg.getGrid()[(int)gridsOn[i].getIndex().x, (int)gridsOn[i].getIndex().y - 1].isWallOrGhostwall()) && !mg.getGrid()[(int)gridsOn[i].getIndex().x - 1, (int)gridsOn[i].getIndex().y].isWallOrGhostwall())
                    {
                        go.GetComponent<GhostWoodWall>().setType("cornerr");
                    }
                }
                catch { }
                go.GetComponent<GhostWoodWall>().isBuildable(woodMat, ironMat);
                woodMat += go.GetComponent<GhostWoodWall>().getWoodCost();
                ironMat += go.GetComponent<GhostWoodWall>().getIronCost();
                go.SetActive(true);
            }
        }
        else
        {
            for (i = gridsOn.Count - 1; i >= 0; i--)
            {
                GameObject go = walls[gridsOn.Count - i];
                go.GetComponent<GhostWoodWall>().setGrid(gridsOn[i]);
                go.GetComponent<GhostWoodWall>().setType("front");
                bool t = false;
                try
                {
                    if ((mg.getGrid()[(int)gridsOn[i].getIndex().x, (int)gridsOn[i].getIndex().y - 1].isWallOrGhostwall() || mg.getGrid()[(int)gridsOn[i].getIndex().x, (int)gridsOn[i].getIndex().y + 1].isWallOrGhostwall()) && !mg.getGrid()[(int)gridsOn[i].getIndex().x - 1, (int)gridsOn[i].getIndex().y].isWallOrGhostwall() && !mg.getGrid()[(int)gridsOn[i].getIndex().x + 1, (int)gridsOn[i].getIndex().y].isWallOrGhostwall())
                    {
                        go.GetComponent<GhostWoodWall>().setType("side");
                        t = true;
                    }
                }
                catch { }
                try
                {
                    if (!t && (mg.getGrid()[(int)gridsOn[i].getIndex().x - 1, (int)gridsOn[i].getIndex().y].isWallOrGhostwall() || mg.getGrid()[(int)gridsOn[i].getIndex().x + 1, (int)gridsOn[i].getIndex().y].isWallOrGhostwall()) && !mg.getGrid()[(int)gridsOn[i].getIndex().x, (int)gridsOn[i].getIndex().y + 1].isWallOrGhostwall() && !mg.getGrid()[(int)gridsOn[i].getIndex().x, (int)gridsOn[i].getIndex().y - 1].isWallOrGhostwall())
                    {
                        go.GetComponent<GhostWoodWall>().setType("front");
                        t = true;
                    }
                }
                catch { }
                try
                {
                    if (!t && mg.getGrid()[(int)gridsOn[i].getIndex().x - 1, (int)gridsOn[i].getIndex().y].isWallOrGhostwall() && (mg.getGrid()[(int)gridsOn[i].getIndex().x, (int)gridsOn[i].getIndex().y + 1].isWallOrGhostwall() || mg.getGrid()[(int)gridsOn[i].getIndex().x, (int)gridsOn[i].getIndex().y - 1].isWallOrGhostwall()) && !mg.getGrid()[(int)gridsOn[i].getIndex().x + 1, (int)gridsOn[i].getIndex().y].isWallOrGhostwall())
                    {
                        go.GetComponent<GhostWoodWall>().setType("cornerl");
                        t = true;
                    }
                }
                catch { }
                try
                {
                    if (!t && mg.getGrid()[(int)gridsOn[i].getIndex().x + 1, (int)gridsOn[i].getIndex().y].isWallOrGhostwall() && (mg.getGrid()[(int)gridsOn[i].getIndex().x, (int)gridsOn[i].getIndex().y + 1].isWallOrGhostwall() || mg.getGrid()[(int)gridsOn[i].getIndex().x, (int)gridsOn[i].getIndex().y - 1].isWallOrGhostwall()) && !mg.getGrid()[(int)gridsOn[i].getIndex().x - 1, (int)gridsOn[i].getIndex().y].isWallOrGhostwall())
                    {
                        go.GetComponent<GhostWoodWall>().setType("cornerr");
                    }
                }
                catch { }
                go.GetComponent<GhostWoodWall>().isBuildable(woodMat, ironMat);
                woodMat += go.GetComponent<GhostWoodWall>().getWoodCost();
                ironMat += go.GetComponent<GhostWoodWall>().getIronCost();
                go.SetActive(true);
            }
        }
        for (i = gridsOn.Count; i < walls.Count; i++)
        {
            walls[i].SetActive(false);
        }
    }

    /// <summary>
    /// spawns the real wood walls
    /// </summary>
    public void spawnWall()
    {

        List<GameObject> blt = new List<GameObject>();
        for (int i = 0; i < walls.Count; i++)
        {
            if (walls[i].activeInHierarchy && walls[i].GetComponent<GhostWoodWall>().isBuildable())
            {
                blt.Add(walls[i]);
            }
        }

        foreach(GameObject b in blt)
        {
            var go = b.GetComponent<GhostWoodWall>().spawnWall(spawner);
            Debug.Log(go);
            List<Grid> grids = new List<Grid>();
            grids.Add(b.GetComponent<GhostWoodWall>().getGridOn());
            b.GetComponent<GhostWoodWall>().setGridsOn(grids);
            go.GetComponent<WoodWall>().setGridsOn(grids);
            b.GetComponent<GhostWoodWall>().blockFieldOn(go);
            player.GetRessourceManager().removeRessources(b.GetComponent<GhostWoodWall>().getRessourceRequirements());
        }

        foreach(GameObject w in walls)
        {
            w.SetActive(false);
        }

    }

    /// <summary>
    /// sets the mapgrid for the woodwallmanager to work with
    /// </summary>
    /// <param name="mg">mapgrid</param>
    public void setMapGrid(MapGrid mg)
    {
        this.mg = mg;
    }

    /// <summary>
    /// sets the spawner the woodwallmanager should work with
    /// </summary>
    /// <param name="spawner"></param>
    public void setSpawner(Spawner spawner)
    {
        this.spawner = spawner;
    }

    /// <summary>
    /// sets the player the woodwallmanager should work with
    /// </summary>
    /// <param name="player">player</param>
    public void setPlayer(Player player)
    {
        this.player = player;
    }

    /// <summary>
    /// start building
    /// </summary>
    /// <param name="b">true for start, false for stop</param>
    public void startBuilding(bool b)
    {
        this.building = b;
    }
}
