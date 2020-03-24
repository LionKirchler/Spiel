using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private MapSpawner mapSpawner;
    private MapGrid mapGrid;
    private int woodSpotAmount = 2;
    private int ironOreSpotAmount = 3;
    private List<Grid> woodSpots = new List<Grid>();
    private List<Grid> ironOreSpots = new List<Grid>();
    private Spawner spawner;

    /// <summary>
    /// initializes the mapgenerator
    /// </summary>
    public void init()
    {
        spawner = GameObject.FindGameObjectWithTag("Player").GetComponent<Spawner>();
        mapSpawner = transform.gameObject.GetComponent<MapSpawner>();
        mapSpawner.setGrassArray();
        mapSpawner.setTreeArray();
        mapSpawner.setIronOreArray();
        mapGrid = transform.gameObject.GetComponent<MapGrid>();
    }

    /// <summary>
    /// generate woods
    /// </summary>
    /// <param name="grids">mapgrid to generate on</param>
    /// <param name="emptyGrids">still empty grids</param>
    public void generateWoods(Grid[,] grids,List<Grid> emptyGrids)
    {

        for (; woodSpotAmount > 0; woodSpotAmount--)
        {
            Grid gr = emptyGrids[Random.Range(0, emptyGrids.Count)];
            Vector2 pos = gr.getIndex();
            woodSpots.Add(grids[(int)pos.x, (int)pos.y]);
            GameObject go = mapSpawner.spawnRandomTree(grids[(int)pos.x, (int)pos.y].getPos());
            mapGrid.addTrees(grids[(int)pos.x, (int)pos.y]);
            grids[(int)pos.x, (int)pos.y].block(go);
            go.GetComponent<Tree>().setMapGrid(mapGrid);
            mapGrid.removeEmptyGrid(grids[(int)pos.x, (int)pos.y]);
            go.GetComponent<Tree>().setGridOn(grids[(int)pos.x, (int)pos.y]);

        }

       
        foreach (Grid woodspot in woodSpots) {
            int woodProbability = 200;
            bool reset = false;
            while (Random.Range(0,100) <= woodProbability)
            {
                reset = false;
                int side = Random.Range(0, 3);
                int x = 0;
                int y = 0;
                switch (side)
                {
                    case 0:
                        x = -1;
                        y = 0;
                        break;
                    case 1:
                        x = 1;
                        y = 0;
                        break;
                    case 2:
                        x = 0;
                        y = -1;
                        break;
                    case 3:
                        x = 0;
                        y = 1;
                        break;
                }

                int randY = 0;
                int randX = 0;
                Vector2 cSpot = new Vector2((int)woodspot.getIndex().x, (int)woodspot.getIndex().y);
                while (grids[(int)cSpot.x, (int)cSpot.y].isBlocked())
                {
                    if(x == 0)
                    {
                        randX = Random.Range(-2, 2);
                        if (randX < -1)
                        {
                            randX = -1;
                        }else if (randX > 1)
                        {
                            randX = 1;
                        }
                    }
                    else if(y == 0)
                    {
                        randY = Random.Range(-2, 2);
                        if (randY < -1)
                        {
                            randY = -1;
                        }else if (randY > 1)
                        {
                            randY = 1;
                        }
                    }

                    if(cSpot.x + x > mapGrid.getMapSize()[0] - 1 || cSpot.x + x < 0 || cSpot.x + randX > mapGrid.getMapSize()[0] - 1 || cSpot.x + randX < 0 || cSpot.y + y > mapGrid.getMapSize()[1] - 1 || cSpot.y + y < 0 || cSpot.y + randY > mapGrid.getMapSize()[1] - 1 || cSpot.y + randY < 0)
                    {
                        reset = true;
                        break;
                    }
                    cSpot.x += x;
                    cSpot.y += y;
                    cSpot.y += randY;
                    cSpot.x += randX;
                }
                if (!reset)
                {
                    GameObject t = mapSpawner.spawnRandomTree(grids[(int)cSpot.x, (int)cSpot.y].getPos());
                    grids[(int)cSpot.x, (int)cSpot.y].block(t) ;
                    mapGrid.removeEmptyGrid(grids[(int)cSpot.x, (int)cSpot.y]);
                    mapGrid.addTrees(grids[(int)cSpot.x, (int)cSpot.y]);
                    t.GetComponent<Tree>().setMapGrid(mapGrid);
                    t.GetComponent<Tree>().setGridOn(grids[(int)cSpot.x, (int)cSpot.y]);
                    woodProbability -= 2;
                }
            }
        }
    }

    /// <summary>
    /// generates iron ores
    /// </summary>
    /// <param name="grids">grids to generate on</param>
    /// <param name="emptyGrids">still empty grids</param>
    public void generateIronOre(Grid[,] grids, List<Grid> emptyGrids)
    {

        for (; ironOreSpotAmount > 0; ironOreSpotAmount--)
        {
            Grid gr = emptyGrids[Random.Range(0, emptyGrids.Count)];
            Vector2 pos = gr.getIndex();
            ironOreSpots.Add(grids[(int)pos.x, (int)pos.y]);
            GameObject go = mapSpawner.spawnRandomIronOre(grids[(int)pos.x, (int)pos.y].getPos());
            mapGrid.addIronOres(grids[(int)pos.x, (int)pos.y]);
            grids[(int)pos.x, (int)pos.y].block(go);
            go.GetComponent<IronOre>().setMapGrid(mapGrid);
            mapGrid.removeEmptyGrid(grids[(int)pos.x, (int)pos.y]);
            go.GetComponent<IronOre>().setGridOn(grids[(int)pos.x, (int)pos.y]);
        }


        foreach (Grid ironSpot in ironOreSpots)
        {
            int ironOreProbability = 100;
            bool reset = false;
            while (Random.Range(0, 100) <= ironOreProbability)
            {
                reset = false;
                int side = Random.Range(0, 3);
                int x = 0;
                int y = 0;
                switch (side)
                {
                    case 0:
                        x = -1;
                        y = 0;
                        break;
                    case 1:
                        x = 1;
                        y = 0;
                        break;
                    case 2:
                        x = 0;
                        y = -1;
                        break;
                    case 3:
                        x = 0;
                        y = 1;
                        break;
                }

                int randY = 0;
                int randX = 0;
                Vector2 cSpot = new Vector2((int)ironSpot.getIndex().x, (int)ironSpot.getIndex().y);
                while (grids[(int)cSpot.x, (int)cSpot.y].isBlocked())
                {
                    if (x == 0)
                    {
                        randX = Random.Range(-2, 2);
                        if (randX < -1)
                        {
                            randX = -1;
                        }
                        else if (randX > 1)
                        {
                            randX = 1;
                        }
                    }
                    else if (y == 0)
                    {
                        randY = Random.Range(-2, 2);
                        if (randY < -1)
                        {
                            randY = -1;
                        }
                        else if (randY > 1)
                        {
                            randY = 1;
                        }
                    }

                    if (cSpot.x + x > mapGrid.getMapSize()[0] - 1 || cSpot.x + x < 0 || cSpot.x + randX > mapGrid.getMapSize()[0] - 1 || cSpot.x + randX < 0 || cSpot.y + y > mapGrid.getMapSize()[1] - 1 || cSpot.y + y < 0 || cSpot.y + randY > mapGrid.getMapSize()[1] - 1 || cSpot.y + randY < 0)
                    {
                        reset = true;
                        break;
                    }
                    cSpot.x += x;
                    cSpot.y += y;
                    cSpot.y += randY;
                    cSpot.x += randX;
                }
                if (!reset)
                {
                    GameObject go = mapSpawner.spawnRandomIronOre(grids[(int)cSpot.x, (int)cSpot.y].getPos());
                    grids[(int)cSpot.x, (int)cSpot.y].block(go); 
                    mapGrid.removeEmptyGrid(grids[(int)cSpot.x, (int)cSpot.y]);
                    mapGrid.addIronOres(grids[(int)cSpot.x, (int)cSpot.y]);
                    go.GetComponent<IronOre>().setMapGrid(mapGrid);
                    go.GetComponent<IronOre>().setGridOn(grids[(int)cSpot.x, (int)cSpot.y]);
                    ironOreProbability -= 10;
                }
            }
        }
    }

    /// <summary>
    /// generates a main building (buggy)
    /// </summary>
    /// <param name="grids">grids to generate on</param>
    /// <param name="emptyGrids">empty grids</param>
    public void generateMainBuilding(Grid[,] grids, List<Grid> emptyGrids, Vector2 size)
    {
        int r = 0;
        bool isInGrid = true;
        do {
            r = Random.Range(0, emptyGrids.Count);
            Vector2 index = emptyGrids[r].getIndex();
            isInGrid = true;
            foreach (Vector2 offset in GhostMainBuilding.getGridOffsets())
            {
                if(index.x + offset.x >= mapGrid.getMapSize()[0] || index.x + offset.x < 0 ||  index.y + offset.y >= mapGrid.getMapSize()[1] || index.y + offset.y < 0)
                {
                    isInGrid = false;
                }
            }
        } while (!isInGrid);

        GameObject building = spawner.spawnMainBuilding(new Vector2(emptyGrids[r].getPos().x + GhostMainBuilding.getOffset().x, emptyGrids[r].getPos().y + GhostMainBuilding.getOffset().y));
        Vector2 mainIndex = emptyGrids[r].getIndex();
        foreach (Vector2 offset in GhostMainBuilding.getGridOffsets())
        {
            grids[(int)(mainIndex.x + offset.x), (int)(mainIndex.y + offset.y)].block(building);
            mapGrid.removeEmptyGrid(grids[(int)(mainIndex.x + offset.x), (int)(mainIndex.y + offset.y)]);
        }

        foreach (Vector2 offset in GhostMainBuilding.getStorageOffset())
        {
            mapGrid.addStorages(mapGrid.getGrid()[(int)(mainIndex.x + offset.x), (int)(mainIndex.y + offset.y)]);
        }

    }

    /// <summary>
    /// generates grass
    /// </summary>
    /// <param name="grids">grids to generate on</param>
    public void generateGrass(Grid[,] grids)
    {
        foreach(Grid grid in grids)
        {
            GameObject grass = mapSpawner.spawnRandomGrass(grid.getPos());
            grid.setGrass(grass);
            grass.GetComponent<Grass>().setGridOn(grid);
        }
    }



    /// <summary>
    /// generates the map
    /// </summary>
    public void generateMap()
    {
        Color[,] map = new Color[mapGrid.getMapSize()[0], mapGrid.getMapSize()[1]];

        Vector2 ironOreOffset = new Vector2(Random.Range(1, 100), Random.Range(1, 100));
        Vector2 ironOreScale = new Vector2(20, 20);
        float ironOreThreshold = 0.25f;
        Color ironOreColor = new Color(255, 0, 0);
        int ironOreType = 0;
        Vector2 woodOffset = new Vector2(Random.Range(1, 100), Random.Range(1, 100));
        Vector2 woodScale = new Vector2(5, 5);
        float woodThreshold = 0.4f;
        Color treeColor = new Color(0, 255, 0);
        int treeType = 1;
        int type = -1;

        for (int x = 0; x < mapGrid.getMapSize()[0]; x++)
        {
            for (int y = 0; y < mapGrid.getMapSize()[1]; y++)
            {
                type = -1;

                float percol = Mathf.PerlinNoise((float)x / mapGrid.getMapSize()[0] * woodScale.x + woodOffset.x, (float)y / mapGrid.getMapSize()[1] * woodScale.y + woodOffset.y);
                if (percol <= woodThreshold)
                {
                    //map[x,y] = treeColor;
                    type = treeType;
                }

                percol = Mathf.PerlinNoise((float)x / mapGrid.getMapSize()[0] * ironOreScale.x + ironOreOffset.x, (float)y / mapGrid.getMapSize()[1] * ironOreScale.y + ironOreOffset.y);
                if (percol <= ironOreThreshold)
                {
                    //map[x,y] = ironOreColor;
                    type = ironOreType;
                }

                if(type == treeType)
                {
                    spawnTree(x,y);
                }else if(type == ironOreType)
                {
                    spawnIronOre(x, y);
                }
            }
        }

    }

    /// <summary>
    /// spawnes a iron ore
    /// </summary>
    /// <param name="x">x index</param>
    /// <param name="y">y index</param>
    /// <returns>spawned ore</returns>
    public GameObject spawnIronOre(int x, int y)
    {
        GameObject go = mapSpawner.spawnRandomIronOre(mapGrid.getGrid()[x, y].getPos());
        mapGrid.getGrid()[x, y].block(go);
        mapGrid.removeEmptyGrid(mapGrid.getGrid()[x, y]);
        mapGrid.addIronOres(mapGrid.getGrid()[x, y]);
        go.GetComponent<IronOre>().setMapGrid(mapGrid);
        go.GetComponent<IronOre>().setGridOn(mapGrid.getGrid()[x, y]);

        return go;
    }

    /// <summary>
    /// spawns a tree
    /// </summary>
    /// <param name="x">x index</param>
    /// <param name="y">y index</param>
    /// <returns>spawned tree</returns>
    public GameObject spawnTree(int x, int y)
    {
        GameObject t = mapSpawner.spawnRandomTree(mapGrid.getGrid()[x, y].getPos());
        mapGrid.getGrid()[x, y].block(t);
        mapGrid.removeEmptyGrid(mapGrid.getGrid()[x, y]);
        mapGrid.addTrees(mapGrid.getGrid()[x, y]);
        t.GetComponent<Tree>().setMapGrid(mapGrid);
        t.GetComponent<Tree>().setGridOn(mapGrid.getGrid()[x, y]);

        return t;
    }

}
