using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBuilding : MonoBehaviour
{

    protected SpriteRenderer sr;
    protected Color colorColliding = Color.red;
    protected Color colorNotColliding = Color.green;
    protected bool isGridBlocked = true;
    protected GameObject mapManager;
    protected MapGrid mapGrid;
    protected Vector2 offset = new Vector2(0, 0f);
    protected Grid gridOn;
    protected Vector2[] gridOffsets = { new Vector2(0, 0) };
    protected List<Grid> gridsOn = new List<Grid>();
    protected int[] ressourceRequirements;
    protected Player player;
    protected bool storage = false;
    protected List<Grid> storagesOn;
    public void Start()
    {
        GameObject[] objects = FindObjectsOfType<GameObject>();
        foreach(GameObject o in objects)
        {
            if(o.tag == "MapManager")
            {
                mapManager = o;
                mapGrid = o.GetComponent<MapGrid>();
            }
        }
        sr = transform.gameObject.GetComponentInChildren<SpriteRenderer>();

        sr.color = colorColliding;

    }



    //sets the grids the ghostbuilding is on, and checks if its buildable
    public virtual void Update()
    {

        Vector2 mousePos = CalcUtil.getMousePos();
        gridsOn.Clear();
        Grid g = mapGrid.getGridOn(mousePos);
        gridOn = g;
        transform.position = g.getPos() + offset;

        bool anyBlocked = false;
        int x = (int)g.getIndex().x;
        int y = (int)g.getIndex().y;

        for (int i = 0; i < gridOffsets.Length; i++)
        {
            Grid gOffset = null;
            if (x + (int)gridOffsets[i].x < mapGrid.getMapSize()[0] && y + (int)gridOffsets[i].y < mapGrid.getMapSize()[1] && x + (int)gridOffsets[i].x >= 0 && y + (int)gridOffsets[i].y >= 0)
            {
                gOffset = mapGrid.getGrid()[x + (int)gridOffsets[i].x, y + (int)gridOffsets[i].y];
                gridsOn.Add(gOffset);
            }
            else
            {
                anyBlocked = true;
            }
                        
        }
        foreach(Grid grid in gridsOn)
        {
            if (grid.isBlocked())
            {
                anyBlocked = true;
            }
        }

        if (anyBlocked)
        {
            isGridBlocked = true;
            sr.color = colorColliding;
        }else if (!hasMaterials())
        {
            isGridBlocked = false;
            sr.color = colorColliding;
        }
        else
        {
            isGridBlocked = false;
            sr.color = colorNotColliding;
        }
    }

    /// <summary>
    /// return the ressources required to build the associated building
    /// </summary>
    /// <returns></returns>
    public virtual int[] getRessourceRequirements()
    {
        return ressourceRequirements;
    }

    /// <summary>
    /// blocks the fields this ghostbuilding is one from movement and further building
    /// </summary>
    /// <param name="go">building to be blocked with (the associated building spawned with this ghostbuilding)</param>
    /// <returns>blocked grids</returns>
    public virtual List<Grid> blockFieldOn(GameObject go)
    {
        foreach (Grid g in gridsOn)
        {
            g.block(go);
            g.blockPath();
            g.setIsBuilding(true);
            mapGrid.removeEmptyGrid(g);
        }
        return gridsOn;
    }

    /// <summary>
    /// returns if this ghost building can be used as a storage
    /// </summary>
    /// <returns></returns>
    public bool isStorage()
    {
        return storage;
    }

    /// <summary>
    /// returnes the storage grids this ghostbuilding has
    /// </summary>
    /// <returns></returns>
    public List<Grid> getStoragesOn()
    {
        return storagesOn;
    }

    /// <summary>
    /// returns if this building can be build / if a real building can be spawned
    /// </summary>
    /// <returns></returns>
    public virtual bool isBuildable()
    {
        return !isGridBlocked && hasMaterials();
    }

    /// <summary>
    /// returns if the player has enogh materials to build it
    /// </summary>
    /// <returns></returns>
    public bool hasMaterials()
    {
        return player.GetRessourceManager().getWood() >= ressourceRequirements[0] && player.GetRessourceManager().getIronOre() >= ressourceRequirements[1];
    }

    /// <summary>
    /// sets the player this ghostbuilding should work with
    /// </summary>
    /// <param name="player"></param>
    public void setPlayer(Player player)
    {
        this.player = player;
    }

    /// <summary>
    /// returns the grids this ghostbuilding is on
    /// </summary>
    /// <returns></returns>
    public List<Grid> getGridsOn()
    {
        return gridsOn;
    }

    /// <summary>
    /// set the grids this ghostbuilding is on
    /// </summary>
    /// <param name="grids"></param>
    public void setGridsOn(List<Grid> grids)
    {
        gridsOn = new List<Grid>(grids);
    }

}
