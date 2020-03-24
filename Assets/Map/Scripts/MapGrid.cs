using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapGrid : MonoBehaviour
{
    private int mapSizeX = 100;
    private int mapSizeY = 100;
    private int gridTileSizeX = 1;
    private int gridTileSizeY = 1;
    [SerializeField]
    private GameObject GUIgo;
    private bool showGrid = false;
    [SerializeField]
    private Material lineMatBuildable;
    private MapGenerator mapGenerator;
    private Player player;

    private Grid[,] grid;
    private List<Grid> emptyGrid = new List<Grid>(100);
    private List<Grid> storages = new List<Grid>(10);
    private List<List<Vector2>> outLinesX = new List<List< Vector2 >>();
    private List<Vector2> outLinesY = new List<Vector2>();
    private List<Line> lines = new List<Line>();
    private Line[] pooledLines;
    private int maxLines = 100000;
    private int listLenght = 0;
    public GameObject linePrefab;
    public List<Grid> trees = new List<Grid>(10);
    public List<Grid> ironOres = new List<Grid>(10);



    //generates a grid, envirement and indication lines, which arent used anymore
    public void setGrid()
    {
        mapGenerator = GetComponent<MapGenerator>();
        grid = new Grid[mapSizeX, mapSizeY];

        for (int x = -((mapSizeX * gridTileSizeX) / 2),i = 0; x < ((mapSizeX * gridTileSizeX) / 2); x+= gridTileSizeX,i++)
        {
            List<Vector2> currentList = new List<Vector2>();
            List<Vector2> currentList2 = new List<Vector2>();
            bool once = true;
            for (int y = -((mapSizeY * gridTileSizeY) / 2), j = 0; y < ((mapSizeY * gridTileSizeY) / 2); y += gridTileSizeY, j++)
            {
                if(once)
                {
                    currentList.Add(new Vector2(x - gridTileSizeX / 2f, y - gridTileSizeY / 2f));
                    currentList2.Add(new Vector2(x + gridTileSizeX / 2f, y - gridTileSizeY / 2f));
                    once = false;
                }
                currentList2.Add(new Vector2(x + gridTileSizeX / 2f, y + gridTileSizeY / 2f));
                currentList.Add(new Vector2(x - gridTileSizeX / 2f, y + gridTileSizeY / 2f));
                grid[i, j] = new Grid();
                grid[i, j].intitGrid(new Vector2(x, y), new Vector2(gridTileSizeX / 2f,gridTileSizeY / 2f), false, new Vector2(i,j));
            }
            outLinesX.Add(currentList);
            outLinesX.Add(currentList2);
            listLenght+= 2;
        }

        for (int y = -((mapSizeY * gridTileSizeY) / 2), j = 0; y < ((mapSizeY * gridTileSizeY) / 2); y += gridTileSizeY, j++)
        {
            List<Vector2> currentList = new List<Vector2>();
            List<Vector2> currentList2 = new List<Vector2>();
            bool once = true;
            for (int x = -((mapSizeX * gridTileSizeX) / 2), i = 0; x < ((mapSizeX * gridTileSizeX) / 2); x += gridTileSizeX, i++)
                {
                if (once)
                {
                    currentList2.Add(new Vector2(x - gridTileSizeX / 2f, y + gridTileSizeY / 2f));
                    currentList.Add(new Vector2(x - gridTileSizeX / 2f, y - gridTileSizeY / 2f));
                    once = false;
                }
                currentList2.Add(new Vector2(x + gridTileSizeX / 2f, y + gridTileSizeY / 2f));
                currentList.Add(new Vector2(x + gridTileSizeX / 2f, y - gridTileSizeY / 2f));
            }
            outLinesX.Add(currentList);
            outLinesX.Add(currentList2);
            listLenght+= 2;
        }

        pooledLines = new Line[maxLines];

        for (int i = 0; i < maxLines; i++)
        {
            var line = Instantiate(linePrefab);
            line.SetActive(false);
            line.transform.SetParent(transform);
            pooledLines[i] = line.GetComponent<Line>();
        }

        for(int x = 0; x < mapSizeX; x++)
        {
            for(int y = 0; y < mapSizeY; y++)
            {
                emptyGrid.Add(grid[x, y]);
            }
        }

        player.getSpawner().SpawnPathFinderManager();
        mapGenerator.init();
        mapGenerator.generateGrass(grid);
        //mapGenerator.generateMainBuilding(grid, emptyGrid, new Vector2(mapSizeX, mapSizeY));
        //mapGenerator.generateWoods(grid,emptyGrid);
        //mapGenerator.generateIronOre(grid, emptyGrid);
        mapGenerator.generateMap();

    }


    /// <summary>
    /// get the grid the mouse is on
    /// </summary>
    /// <param name="mousePos">mouspos</param>
    /// <returns>grid the mouse is on</returns>
    public Grid getGridOn(Vector2 mousePos)
    {
        Grid g = null;

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.tag.Equals("Grass"))
            {
                //hit.transform.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
                g = hit.transform.gameObject.GetComponent<Grass>().getGridOn();
            }
        }
        return g;
    }

    /// <summary>
    /// returns the grid
    /// </summary>
    /// <returns></returns>
    public Grid[,] getGrid()
    {
        return grid;
    }

    /// <summary>
    /// returns the grid size
    /// </summary>
    /// <returns></returns>
    public int[] getMapSize()
    {
        int[] size = { mapSizeX, mapSizeY };
        return size;
    }

    /// <summary>
    /// sets if the indication lines should be shown (outdated)
    /// </summary>
    /// <param name="b"></param>
    public void setShowGrid(bool b)
    {
        showGrid = b;
    }

    /// <summary>
    /// returns the size of parts in grid
    /// </summary>
    /// <returns></returns>
    public Vector2 getGridTileSize()
    {
        return new Vector2(gridTileSizeX,gridTileSizeY);
    }

    /// <summary>
    /// adds a storage grid to the storage list
    /// </summary>
    /// <param name="go">storage grid</param>
    public void addStorages(Grid go)
    {
        storages.Add(go);
    }

    /// <summary>
    /// sets the player for the mapgrid to work with
    /// </summary>
    public void setPlayer(Player player)
    {
        this.player = player;
    }

    /// <summary>
    /// returns the storage grids
    /// </summary>
    /// <returns>storage grids</returns>
    public List<Grid> getStorages()
    {
        return storages;
    }

    /// <summary>
    /// add a tree to the tree list
    /// </summary>
    /// <param name="go">tree to add</param>
    public void addTrees(Grid go)
    {
        trees.Add(go);
    }

    /// <summary>
    /// remove a tree from the tree list
    /// </summary>
    /// <param name="g">tree to remove</param>
    public void removeTree(Grid g)
    {

        try
        {
            trees.Remove(g);
        }
        catch
        {

        }

    }

    /// <summary>
    /// return all iron ore grids
    /// </summary>
    /// <returns></returns>
    public List<Grid> getIronOres()
    {
        return ironOres;
    }

    /// <summary>
    /// add a iron ore grid to the list
    /// </summary>
    /// <param name="go">iron ore grid</param>
    public void addIronOres(Grid go)
    {
        ironOres.Add(go);
    }

    /// <summary>
    /// removes a storage from the storage list
    /// </summary>
    /// <param name="g">storage to remove</param>
    public void removeStorages(Grid g)
    {
        try
        {
            storages.Remove(g);
        }
        catch { }
    }

    /// <summary>
    /// removes a iron ore from the iron ore list
    /// </summary>
    /// <param name="g">iron ore to remove</param>
    public void removeIronOres(Grid g)
    {

        try
        {
            ironOres.Remove(g);
        }
        catch
        {

        }

    }

    /// <summary>
    /// get tree grids
    /// </summary>
    /// <returns>tree grids</returns>
    public List<Grid> getTrees()
    {
        return trees;
    }

    /// <summary>
    /// returns the list of empty grids
    /// </summary>
    /// <returns></returns>
    public List<Grid> getEmptyGrids()
    {
        return emptyGrid;
    }


    /// <summary>
    /// removes a grid from the empty list (if it was blocked for example)
    /// </summary>
    /// <param name="g"></param>
    public void removeEmptyGrid(Grid g)
    {
       
        try
        {
            emptyGrid.Remove(g);
        }
        catch
        {

        }
        
    }

    /// <summary>
    /// returns a random empty grid in range(rectangular)
    /// </summary>
    /// <param name="st">main grid</param>
    /// <param name="range">range to scan</param>
    /// <returns>random empty grid in range</returns>
    public Grid GetRandomEmptyGridInRange(Grid st, int range)
    {
        List<Grid> grids = new List<Grid>();

        for(int i = (int)st.getIndex().x - range; i < st.getIndex().x + range; i++)
        {
            for (int j = (int)st.getIndex().y - range; j < st.getIndex().y + range; j++)
            {
                if(i >= 0 && i < mapSizeX && j >= 0 && j < mapSizeY && !grid[i, j].isPathBlocked())
                {
                    grids.Add(grid[i, j]);
                }
            }
        }

        return grids[Random.Range(0, grids.Count - 1)];

    }

    void Update()
    {
        //show line indicator for girds if activated (outdated)
        if (showGrid)
        {
            int lineCount = 0;
            for (int j = 0; j < listLenght; j++) {
                for (int i = 0; i < outLinesX[j].Count - 1; i++)
                {
                    pooledLines[lineCount].Initialise(outLinesX[j][i], outLinesX[j][i + 1], 0.02f, Color.white);
                    pooledLines[lineCount].gameObject.SetActive(true);
                    lineCount++;
                }
            }
        }
    }
}
