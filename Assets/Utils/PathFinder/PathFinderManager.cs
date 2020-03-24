using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinderManager : MonoBehaviour
{

    private int searchLimit = 1000;
    private int nearestGridLimit = 100;
    private Grid[,] pathGrid = null;
    private MapGrid mg = null;



    //public void copyGrid()
    //{
    //    pathGrid = new Grid[mg.getMapSize()[0], mg.getMapSize()[1]];
    //    copyGrid(mg, pathGrid);
    //}

    /// <summary>
    /// the mapgrid for the pathfinder to work with
    /// </summary>
    /// <param name="mg"></param>
    public void setMapGrid(MapGrid mg)
    {
        this.mg = mg;
        pathGrid = mg.getGrid();
    }
    /// <summary>
    /// finds closest grid with units on
    /// </summary>
    /// <param name="sta">staring grid</param>
    /// <param name="mg">mapgrid to work with</param>
    /// <param name="limit">search limit/range</param>
    /// <returns>found grid</returns>
    public Grid FindNearestUnitGrid(Grid sta, MapGrid mg, int limit)
    {
        if (sta == null || mg == null)
        {
            return null;
        }

        Vector2[] pattern = { new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1) };
        List<Grid> usedGrid = new List<Grid>();
        Grid found = null;
        List<Grid> outerGrid = new List<Grid>();
        outerGrid.Add(pathGrid[(int)sta.getIndex().x, (int)sta.getIndex().y]);
        for (int i = 0; i < limit && found == null; i++)
        {
            found = expandGridFindUnitsOn(outerGrid, pathGrid, usedGrid, pattern, mg);
        }
        resetGrid(usedGrid);

        return found;
    }

    /// <summary>
    /// finds closest grid with a building on
    /// </summary>
    /// <param name="sta">starting grid</param>
    /// <param name="mg">mapgrid to work with</param>
    /// <param name="limit">search limit/range</param>
    /// <returns>found grid</returns>
    public Grid[] FindNearestBuildGrid(Grid sta, MapGrid mg, int limit)
    {

        if (sta == null || mg == null)
        {
            return null;
        }

        Vector2[] pattern = { new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1) };
        List<Grid> usedGrid = new List<Grid>();
        Grid[] found = new Grid[2];
        List<Grid> outerGrid = new List<Grid>();
        outerGrid.Add(pathGrid[(int)sta.getIndex().x, (int)sta.getIndex().y]);
        for (int i = 0; i < limit && found[0] == null; i++)
        {
            found = expandGridFindBlocked(outerGrid, pathGrid, usedGrid, pattern, mg);
        }
        resetGrid(usedGrid);
        return found;
    }

    /// <summary>
    /// finds closest grid on list of grids
    /// </summary>
    /// <param name="sta">starting grid</param>
    /// <param name="grd">list of grids to search for</param>
    /// <param name="mg">mapgrid to work with</param>
    /// <returns></returns>
    public Grid FindNearestGrid(Grid sta, List<Grid> grd, MapGrid mg)
    {
        if (sta == null || grd == null || mg == null || grd.Count <= 0)
        {
            return null;
        }
        if (grd.Count == 1)
        {
            return grd[0];
        }
        Vector2[] pattern = { new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1) };
        List<Grid> usedGrid = new List<Grid>();
        Grid found = null;
        List<Grid> outerGrid = new List<Grid>();
        outerGrid.Add(pathGrid[(int)sta.getIndex().x, (int)sta.getIndex().y]);
        for (int i = 0; i < nearestGridLimit && found == null; i++)
        {
            found = expandGridFind(outerGrid, pathGrid, usedGrid, pattern, grd, mg);
        }
        resetGrid(usedGrid);
        return found;
    }

    /// <summary>
    /// create a path
    /// </summary>
    /// <param name="sta">starting grid</param>
    /// <param name="dest">destination position</param>
    /// <param name="mg">mapgrid to work with</param>
    /// <returns>created path</returns>
    public Path FindPath(Grid sta, Vector2 dest, MapGrid mg)
    {

        Grid des = mg.getGridOn(dest);

        if (sta == null || des == null || mg == null)
        {
            return null;
        }

        Vector2[] pattern = { new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, -1), new Vector2(-1, -1) };
        List<Grid> outerGrid = new List<Grid>();
        List<Grid> usedGrid = new List<Grid>();
        Grid start = pathGrid[(int)sta.getIndex().x, (int)sta.getIndex().y];
        Grid destination = pathGrid[(int)des.getIndex().x, (int)des.getIndex().y];
        List<Grid> retraceGrid;
        if (start.getIndex() != destination.getIndex())
        {

            outerGrid.Add(start);
            expandGrid(outerGrid, pathGrid, usedGrid, outerGrid[0], pattern, destination, start, mg);
            Grid endGrid = null;

            for (int i = 0; i < searchLimit && endGrid == null; i++)
            {
                Grid eg = getGridWithLowestDistanceValue(outerGrid);
                endGrid = expandGrid(outerGrid, pathGrid, usedGrid, eg, pattern, destination, start, mg);
            }

            retraceGrid = retracePath(endGrid, mg);
        }
        else
        {
            retraceGrid = new List<Grid>();
            retraceGrid.Add(start);
        }

        Path path = new Path(retraceGrid, dest);

        resetGrid(usedGrid);
        return path;

    }

    /// <summary>
    /// expands the grid in a pattern searching for destination grid (used for pathfinding)
    /// </summary>
    /// <param name="outerGrid">outergrid/grid that hasnt been expanded</param>
    /// <param name="pathGrid">the created grid the pathmanager works with</param>
    /// <param name="usedGrid">grids that already have been expanded</param>
    /// <param name="expandGrid">grid to expand</param>
    /// <param name="pattern">expand pattern</param>
    /// <param name="dest">destination</param>
    /// <param name="start">start</param>
    /// <param name="mg">mapgrid to work with</param>
    /// <returns>returns grid if destination reached</returns>
    private Grid expandGrid(List<Grid> outerGrid, Grid[,] pathGrid, List<Grid> usedGrid, Grid expandGrid, Vector2[] pattern, Grid dest, Grid start, MapGrid mg)
    {
        Grid ret = null;
        if (expandGrid == null)
        {
            return null;
        }
        int[] index = { (int)expandGrid.getIndex().x, (int)expandGrid.getIndex().y };
        if (pathGrid[index[0], index[1]].getIndex() == dest.getIndex())
        {
            ret = pathGrid[index[0], index[1]];
        }
        else
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if ((int)(index[0] + pattern[i].x) < mg.getMapSize()[0] && (int)(index[0] + pattern[i].x) >= 0 && (int)(index[1] + pattern[i].y) < mg.getMapSize()[1] && (int)(index[1] + pattern[i].y) >= 0 && !pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)].isPathBlocked() && !pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)].isPathSelected())
                {
                    outerGrid.Add(pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)]);
                    if (pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)].getIndex() != start.getIndex())
                    {
                        if (pathGrid[index[0], index[1]].getPathOrigin() == null)
                        {
                            pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)].setPathOrigin(pathGrid[index[0], index[1]]);
                        }
                        else if (pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)].getIndex() != pathGrid[index[0], index[1]].getPathOrigin().getIndex())
                        {
                            pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)].setPathOrigin(pathGrid[index[0], index[1]]);
                        }
                    }
                    setDistanceToDestination(pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)], dest);
                    setDistanceToStart(pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)], start);
                    pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)].setDistanceValue();
                    pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)].setPathSelected(true);
                    usedGrid.Add(pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)]);


                    if (pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)].getIndex() == dest.getIndex())
                    {
                        ret = pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)];
                    }
                }
            }
            outerGrid.Remove(expandGrid);
        }
        return ret;
    }

    /// <summary>
    /// expands the grid in a pattern searching for multiple destination grids (used for pathfinding)
    /// </summary>
    /// <param name="outerGrid">outergrid/grid that hasnt been expanded</param>
    /// <param name="pathGrid">the created grid the pathmanager works with</param>
    /// <param name="usedGrid">grids that already have been expanded</param>
    /// <param name="pattern">expand pattern</param>
    /// <param name="dest">destination grids</param>
    /// <param name="mg">mapgrid to work with</param>
    /// <returns>returns grid if any destination reached</returns>
    private Grid expandGridFind(List<Grid> outerGrid, Grid[,] pathGrid, List<Grid> usedGrid, Vector2[] pattern, List<Grid> dest, MapGrid mg)
    {
        Grid ret = null;
        if (outerGrid == null)
        {
            return null;
        }
        List<Grid> outerGridC = new List<Grid>(outerGrid);
        foreach (Grid og in outerGridC)
        {
            int[] index = { (int)og.getIndex().x, (int)og.getIndex().y };
            foreach (Grid g in dest)
            {
                if (pathGrid[index[0], index[1]].getIndex() == g.getIndex())
                {
                    ret = pathGrid[index[0], index[1]];
                    goto End;
                }
            }
            for (int i = 0; i < pattern.Length; i++)
            {
                if ((int)(index[0] + pattern[i].x) < mg.getMapSize()[0] && (int)(index[0] + pattern[i].x) >= 0 && (int)(index[1] + pattern[i].y) < mg.getMapSize()[1] && (int)(index[1] + pattern[i].y) >= 0 && !pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)].isPathBlocked() && !pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)].isPathSelected())
                {
                    outerGrid.Add(pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)]);

                    pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)].setPathSelected(true);
                    usedGrid.Add(pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)]);

                    foreach (Grid g in dest)
                    {
                        if (pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)].getIndex() == g.getIndex())
                        {
                            ret = pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)];
                            goto End;
                        }
                    }
                }
            }
            outerGrid.Remove(og);
        }
    End:
        return ret;
    }

    /// <summary>
    /// expands the grid in a pattern searching for blocked grids(used for pathfinding)
    /// </summary>
    /// <param name="outerGrid">outergrid/grid that hasnt been expanded</param>
    /// <param name="pathGrid">the created grid the pathmanager works with</param>
    /// <param name="usedGrid">grids that already have been expanded</param>
    /// <param name="pattern">expand pattern</param>
    /// <param name="mg">mapgrid to work with</param>
    /// <returns>returns grid if any blocked grid reached</returns>
    public Grid[] expandGridFindBlocked(List<Grid> outerGrid, Grid[,] pathGrid, List<Grid> usedGrid, Vector2[] pattern, MapGrid mg)
    {
        Grid[] ret = new Grid[2];
        if (outerGrid == null)
        {
            return null;
        }
        List<Grid> outerGridC = new List<Grid>(outerGrid);
        foreach (Grid og in outerGridC)
        {
            int[] index = { (int)og.getIndex().x, (int)og.getIndex().y };

            for (int i = 0; i < pattern.Length; i++)
            {
                if ((int)(index[0] + pattern[i].x) < mg.getMapSize()[0] && (int)(index[0] + pattern[i].x) >= 0 && (int)(index[1] + pattern[i].y) < mg.getMapSize()[1] && (int)(index[1] + pattern[i].y) >= 0 && !pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)].isPathSelected())
                {
                    outerGrid.Add(pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)]);

                    pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)].setPathSelected(true);
                    usedGrid.Add(pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)]);

                    if (pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)].isBuilding())
                    {
                        ret[0] = mg.getGrid()[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)];
                        ret[1] = mg.getGrid()[(index[0]), (index[1])];
                        goto End;
                    }

                }
            }
            outerGrid.Remove(og);
        }
    End:

        return ret;

    }

    /// <summary>
    /// expands the grid in a pattern searching for grids with units on(used for pathfinding)
    /// </summary>
    /// <param name="outerGrid">outergrid/grid that hasnt been expanded</param>
    /// <param name="pathGrid">the created grid the pathmanager works with</param>
    /// <param name="usedGrid">grids that already have been expanded</param>
    /// <param name="pattern">expand pattern</param>
    /// <param name="mg">mapgrid to work with</param>
    /// <returns>returns grid if any grids with units on have been reached</returns>
    public Grid expandGridFindUnitsOn(List<Grid> outerGrid, Grid[,] pathGrid, List<Grid> usedGrid, Vector2[] pattern, MapGrid mg)
    {
        Grid ret = null;
        if (outerGrid == null)
        {
            return null;
        }
        List<Grid> outerGridC = new List<Grid>(outerGrid);
        foreach (Grid og in outerGridC)
        {
            int[] index = { (int)og.getIndex().x, (int)og.getIndex().y };

            if (pathGrid[index[0], index[1]].hasUnitsOn())
            {
                ret = pathGrid[index[0], index[1]];
                goto End;
            }

            for (int i = 0; i < pattern.Length; i++)
            {
                if ((int)(index[0] + pattern[i].x) < mg.getMapSize()[0] && (int)(index[0] + pattern[i].x) >= 0 && (int)(index[1] + pattern[i].y) < mg.getMapSize()[1] && (int)(index[1] + pattern[i].y) >= 0 && !pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)].isPathBlocked() && !pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)].isPathSelected())
                {
                    outerGrid.Add(pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)]);

                    pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)].setPathSelected(true);
                    usedGrid.Add(pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)]);

                    if (pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)].hasUnitsOn())
                    {
                        ret = pathGrid[(int)(index[0] + pattern[i].x), (int)(index[1] + pattern[i].y)];
                        goto End;
                    }

                }
            }
            outerGrid.Remove(og);
        }
    End:
        return ret;

    }

    /// <summary>
    /// returns the grid with the lowest distance value (to expand it)
    /// </summary>
    /// <param name="grid">grids to check</param>
    /// <returns>grids with lowest distance value</returns>
    public Grid getGridWithLowestDistanceValue(List<Grid> grid)
    {
        float closestV = Mathf.Infinity;
        Grid closestG = null;
        for (int i = 0; i < grid.Count; i++)
        {
            if (grid[i].getDistanceValue() < closestV)
            {
                closestG = grid[i];
                closestV = grid[i].getDistanceValue();
            }
        }

        return closestG;
    }

    /// <summary>
    /// sets the distance to destination for mulitple grids
    /// </summary>
    /// <param name="grid">grids</param>
    /// <param name="dest">destination</param>
    private void setDistanceToDestination(List<Grid> grid, Grid dest)
    {
        for (int i = 0; i < grid.Count; i++)
        {
            grid[i].setPathDestinationDistance(Mathf.Sqrt(Mathf.Pow(Mathf.Sqrt(Mathf.Sqrt(Mathf.Pow(dest.getIndex().x - grid[i].getIndex().x, 4)) + Mathf.Sqrt(Mathf.Pow(dest.getIndex().y - grid[i].getIndex().y, 4))), 2)));
        }
    }

    /// <summary>
    /// sets the distance to the destingation for grid
    /// </summary>
    /// <param name="grid">grid</param>
    /// <param name="dest">destination</param>
    private void setDistanceToDestination(Grid grid, Grid dest)
    {
        grid.setPathDestinationDistance(Mathf.Sqrt(Mathf.Pow(Mathf.Sqrt(Mathf.Sqrt(Mathf.Pow(dest.getIndex().x - grid.getIndex().x, 4)) + Mathf.Sqrt(Mathf.Pow(dest.getIndex().y - grid.getIndex().y, 4))), 2)));
        //Debug.Log(Mathf.Sqrt(Mathf.Pow(Mathf.Sqrt(Mathf.Sqrt(Mathf.Pow(dest.getIndex().x - grid.getIndex().x, 4)) + Mathf.Sqrt(Mathf.Pow(dest.getIndex().y - grid.getIndex().y, 4))), 2)));
    }

    /// <summary>
    /// retraces the path, to return to its origin and get the final path
    /// </summary>
    /// <param name="g">grid</param>
    /// <param name="mg">mapgrid to work with</param>
    /// <returns>path</returns>
    public List<Grid> retracePath(Grid g, MapGrid mg)
    {
        List<Grid> ret = new List<Grid>();

        Grid current = g;

        while (current != null)
        {
            ret.Add(mg.getGrid()[(int)current.getIndex().x, (int)current.getIndex().y]);
            current = current.getPathOrigin();
        }

        return ret;
    }

    /// <summary>
    /// sets the grids distance to start (not direct)
    /// </summary>
    /// <param name="grid">grid</param>
    /// <param name="start">start</param>
    private void setDistanceToStart(Grid grid, Grid start)
    {
        Grid current = grid;
        int dist = 0;
        while (current != null && dist <= 100)
        {
            if (current.getPathOrigin() != null)
            {
                dist++;
            }
            current = current.getPathOrigin();
            //Debug.Log(dist + "  " + current);
            //Debug.Log(2 + "  " + current.getPathOrigin().getPathOrigin());
        }

        grid.setPathStartingDistance(dist);
    }

    /// <summary>
    /// copy the map grid
    /// </summary>
    /// <param name="mg">mapgird</param>
    /// <param name="grid">array to copy to</param>
    public void copyGrid(MapGrid mg, Grid[,] grid)
    {
        for (int x = 0; x < mg.getMapSize()[0]; x++)
        {
            for (int y = 0; y < mg.getMapSize()[1]; y++)
            {
                grid[x, y] = new Grid(mg.getGrid()[x, y]);
            }
        }

    }

    /// <summary>
    /// indicates grids by color (for testing)
    /// </summary>
    /// <param name="mg">mapgrid</param>
    /// <param name="g">grids to indicate</param>
    /// <param name="color">color</param>
    public void indicateGridByColor(MapGrid mg, List<Grid> g, Color color)
    {
        for (int i = 0; i < g.Count; i++)
        {
            mg.getGrid()[(int)g[i].getIndex().x, (int)g[i].getIndex().y].getGrass().GetComponentInChildren<SpriteRenderer>().color = color;
        }
    }

    /// <summary>
    /// indicates grid by color (for testing)
    /// </summary>
    /// <param name="mg">mapgrid</param>
    /// <param name="g">grid to indicate</param>
    /// <param name="color">color</param>
    public void indicateGridByColor(MapGrid mg, Grid g, Color color)
    {
        mg.getGrid()[(int)g.getIndex().x, (int)g.getIndex().y].getGrass().GetComponentInChildren<SpriteRenderer>().color = color;
    }

    /// <summary>
    /// resets the values of the grid
    /// </summary>
    /// <param name="usedGrid">used grids to clear</param>
    public void resetGrid(List<Grid> usedGrid)
    {
        foreach(Grid g in usedGrid)
        {
            g.setPathSelected(false);
            g.setPathStartingDistance(0);
            g.setPathOrigin(null);
            g.setPathDestinationDistance(0);
        }
    }

    //old pathfinding used with astar
    /*
    public void Update()
    {
        if (!startSearch)
        {
            return;
        }

        if (framePassed == 0)
        {
            Debug.Log(storages.Count);
            foreach (Grid g in storages)
            {
                if (g != start)
                {
                    GameObject pf = spawner.SpawnPathFinder();
                    pathFinders.Add(pf);
                    pf.GetComponent<PathFinderUtil>().SetPositions(start, g);
                }
            }
        }

        if (framePassed >= 20)
        {
            closestGrid = null;
            float closest = -1;


            int k = 0;
            Debug.Log(pathFinders.Count);
            foreach (GameObject g in pathFinders)
            {
                Debug.Log(g.GetComponent<PathFinderUtil>().GetDistance() + " | " + k);
                k++;
                if (g.GetComponent<PathFinderUtil>().GetDistance() < closest || closest == -1)
                {
                    closest = g.GetComponent<PathFinderUtil>().GetDistance();
                    closestGrid = g.GetComponent<PathFinderUtil>().GetStorage();
                    Debug.Log("xxx " + g.GetComponent<PathFinderUtil>().GetDistance() + " | " + k);
                }
            }
            if (unitManager != null)
            {
                unitManager.RecieveNearest(closestGrid, type);
            }
            else if (treeManager != null)
            {
                treeManager.RecieveNearest(closestGrid, type);
            }

            foreach (GameObject g in pathFinders)
            {
                Destroy(g);
            }
            Destroy(transform.gameObject);
        }
        framePassed++;
    }
    */
}

