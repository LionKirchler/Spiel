using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostWoodWall : GhostBuilding
{
    string type = "front";
    [SerializeField]
    GameObject front = null;
    [SerializeField]
    GameObject side = null;
    [SerializeField]
    GameObject corner = null;
    GameObject active = null;
    int woodMat = int.MaxValue;
    int ironMat = int.MaxValue;

    public GhostWoodWall()
    {
        ressourceRequirements = new int[] { 50, 0 };
        gridOffsets = new Vector2[]{ new Vector2(0, 0)};
        offset = new Vector2(0, 0.93f);
    }

    new void Start()
    {
        active = front;
        base.Start();
    }

    public override void Update()
    {
        
    }

    /// <summary>
    /// returns if this is buildable
    /// (should only be used for single build, but not for building multiple)
    /// </summary>
    /// <returns></returns>
    public override bool isBuildable()
    {
        return !isGridBlocked && hasMaterials(this.woodMat, this.ironMat);
    }

    /// <summary>
    /// returns if this is buildable
    /// used for multiple building, see woodwall manager script for example how to use it
    /// </summary>
    /// <param name="woodMat">woodmaterial required</param>
    /// <param name="ironMat">iron required</param>
    /// <returns></returns>
    public bool isBuildable(int woodMat, int ironMat)
    {
        if (gridOn == null)
        {
            return false;
        }
        this.woodMat = woodMat;
        this.ironMat = ironMat;
        gridsOn.Clear();
        transform.position = gridOn.getPos() + offset;
        bool anyBlocked = false;

        if (gridOn.isBlocked())
        {
            anyBlocked = true;
        }

        if (anyBlocked)
        {
            isGridBlocked = true;
            sr.color = colorColliding;
            return false;
        }
        else if (!hasMaterials(woodMat, ironMat))
        {
            isGridBlocked = false;
            sr.color = colorColliding;
            return false;
        }
        else
        {
            isGridBlocked = false;
            sr.color = colorNotColliding;
            return true;
        }

    }

    /// <summary>
    /// sets the grids this is on
    /// </summary>
    /// <param name="g">grid</param>
    public void setGrid(Grid g)
    {
        this.gridOn = g;
        gridsOn.Clear();
        gridsOn.Add(g);
        transform.position = gridOn.getPos() + offset;
    }

    /// <summary>
    /// sets the type of the wall (visual)
    /// </summary>
    /// <param name="type">type</param>
    public void setType(string type)
    {
        this.type = type;
        if (type.Equals("front"))
        {
            front.SetActive(true);
            side.SetActive(false);
            corner.SetActive(false);
            active = front;
            sr = active.GetComponent<SpriteRenderer>();
        }
        else if (type.Equals("side"))
        {
            side.SetActive(true);
            front.SetActive(false);
            corner.SetActive(false);
            active = side;
            sr = active.GetComponent<SpriteRenderer>();
        }
        else if (type.Equals("cornerl"))
        {
            corner.SetActive(true);
            front.SetActive(false);
            side.SetActive(false);
            active = corner;
            active.transform.rotation = Quaternion.Euler(0, 0, 0);
            sr = active.GetComponent<SpriteRenderer>();
        }
        else if (type.Equals("cornerr"))
        {
            corner.SetActive(true);
            front.SetActive(false);
            side.SetActive(false);
            active = corner;
            active.transform.rotation = Quaternion.Euler(0,180,0);
            sr = active.GetComponent<SpriteRenderer>();
        }
    }

    /// <summary>
    /// spawn a real wall
    /// </summary>
    /// <param name="spawner">spawner to use for spawning</param>
    /// <returns></returns>
    public GameObject spawnWall(Spawner spawner)
    {
        GameObject ret = null;
        if (type.Equals("front"))
        {
            ret = spawner.spawnWoodWall(transform.position, "front");
        }
        else if (type.Equals("side"))
        {
            ret = spawner.spawnWoodWall(transform.position, "side");
        }
        else if (type.Equals("cornerl"))
        {
            ret = spawner.spawnWoodWall(transform.position, "cornerl");
        }
        else if (type.Equals("cornerr"))
        {
            ret = spawner.spawnWoodWall(transform.position, "cornerr");
        }

        

        return ret;
    }

    /// <summary>
    /// retfreshes nearby walls, to fix the visuals
    /// </summary>
    public void refreshclose()
    {
        if (mapGrid.getGrid()[(int)gridOn.getIndex().x - 1, (int)gridOn.getIndex().y].isWall())
        {
            mapGrid.getGrid()[(int)gridOn.getIndex().x - 1, (int)gridOn.getIndex().y].getBlockingObject().GetComponent<WoodWall>().refresh();
        }
        if (mapGrid.getGrid()[(int)gridOn.getIndex().x + 1, (int)gridOn.getIndex().y].isWall())
        {
            mapGrid.getGrid()[(int)gridOn.getIndex().x + 1, (int)gridOn.getIndex().y].getBlockingObject().GetComponent<WoodWall>().refresh();
        }
        if (mapGrid.getGrid()[(int)gridOn.getIndex().x, (int)gridOn.getIndex().y - 1].isWall())
        {
            mapGrid.getGrid()[(int)gridOn.getIndex().x, (int)gridOn.getIndex().y - 1].getBlockingObject().GetComponent<WoodWall>().refresh();
        }
        if (mapGrid.getGrid()[(int)gridOn.getIndex().x, (int)gridOn.getIndex().y + 1].isWall())
        {
            mapGrid.getGrid()[(int)gridOn.getIndex().x, (int)gridOn.getIndex().y + 1].getBlockingObject().GetComponent<WoodWall>().refresh();
        }
    }

    /// <summary>
    /// returns the type of the wall
    /// </summary>
    /// <returns></returns>
    public string getType()
    {
        return type;
    }

    /// <summary>
    /// return the maingrid this wall is on
    /// </summary>
    /// <returns></returns>
    public Grid getGridOn()
    {
        return gridOn;
    }

    /// <summary>
    /// return the ressources required to build a wall
    /// </summary>
    /// <returns></returns>
    public override int[] getRessourceRequirements()
    {
        return ressourceRequirements;
    }

    /// <summary>
    /// returns the wood required to build a wall
    /// </summary>
    /// <returns></returns>
    public int getWoodCost()
    {
        return ressourceRequirements[0];
    }

    /// <summary>
    /// returns the iron required to build a wall
    /// </summary>
    /// <returns></returns>
    public int getIronCost()
    {
        return ressourceRequirements[1];
    }

    /// <summary>
    /// check if materials are available
    /// </summary>
    /// <param name="woodMat">wood</param>
    /// <param name="ironMat">iron</param>
    /// <returns></returns>
    public bool hasMaterials(int woodMat, int ironMat)
    {
        return player.GetRessourceManager().getWood() >= ressourceRequirements[0] + woodMat && player.GetRessourceManager().getIronOre() >= ressourceRequirements[1] + ironMat;
    }

}
