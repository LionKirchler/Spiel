using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodWall : Building
{
    private string type = "front";
    [SerializeField]
    private GameObject front = null;
    [SerializeField]
    private GameObject side = null;
    [SerializeField]
    private GameObject corner = null;
    private GameObject active = null;
    private SpriteRenderer sr = null;
    private MapGrid mg = null;
    private Grid gridOn = null;
    new void Start()
    {
        base.Start();
    }

    /// <summary>
    /// refreshes the connections to change the visuals
    /// </summary>
    public void refresh()
    {
        setType("front");
        if ((mg.getGrid()[(int)gridOn.getIndex().x, (int)gridOn.getIndex().y - 1].isWallOrGhostwall() || mg.getGrid()[(int)gridOn.getIndex().x, (int)gridOn.getIndex().y + 1].isWallOrGhostwall()) && !mg.getGrid()[(int)gridOn.getIndex().x - 1, (int)gridOn.getIndex().y].isWallOrGhostwall() && !mg.getGrid()[(int)gridOn.getIndex().x + 1, (int)gridOn.getIndex().y].isWallOrGhostwall())
        {
            setType("side");
        }
        else if ((mg.getGrid()[(int)gridOn.getIndex().x - 1, (int)gridOn.getIndex().y].isWallOrGhostwall() || mg.getGrid()[(int)gridOn.getIndex().x + 1, (int)gridOn.getIndex().y].isWallOrGhostwall()) && !mg.getGrid()[(int)gridOn.getIndex().x, (int)gridOn.getIndex().y + 1].isWallOrGhostwall() && !mg.getGrid()[(int)gridOn.getIndex().x, (int)gridOn.getIndex().y - 1].isWallOrGhostwall())
        {
            setType("front");
        }
        else if (mg.getGrid()[(int)gridOn.getIndex().x - 1, (int)gridOn.getIndex().y].isWallOrGhostwall() && (mg.getGrid()[(int)gridOn.getIndex().x, (int)gridOn.getIndex().y + 1].isWallOrGhostwall() || mg.getGrid()[(int)gridOn.getIndex().x, (int)gridOn.getIndex().y - 1].isWallOrGhostwall()) && !mg.getGrid()[(int)gridOn.getIndex().x + 1, (int)gridOn.getIndex().y].isWallOrGhostwall())
        {
            setType("cornerl");
        }
        else if (mg.getGrid()[(int)gridOn.getIndex().x + 1, (int)gridOn.getIndex().y].isWallOrGhostwall() && (mg.getGrid()[(int)gridOn.getIndex().x, (int)gridOn.getIndex().y + 1].isWallOrGhostwall() || mg.getGrid()[(int)gridOn.getIndex().x, (int)gridOn.getIndex().y - 1].isWallOrGhostwall()) && !mg.getGrid()[(int)gridOn.getIndex().x - 1, (int)gridOn.getIndex().y].isWallOrGhostwall())
        {
            setType("cornerr");
        }
    }

    /// <summary>
    /// ses the mapgrid for the wall to work with
    /// </summary>
    /// <param name="mg">mapgrid</param>
    public void setMapGrid(MapGrid mg)
    {
        this.mg = mg;
    }

    /// <summary>
    /// sets the grid this wall is on
    /// </summary>
    /// <param name="gridOn">grid</param>
    public void setGridOn(Grid gridOn)
    {
        this.gridOn = gridOn;
    }

    /// <summary>
    /// sets the type of this wall
    /// </summary>
    /// <param name="type">type name</param>
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
            active.transform.rotation = Quaternion.Euler(0, 180, 0);
            sr = active.GetComponent<SpriteRenderer>();
        }
    }

    /// <summary>
    /// destroyes the wall
    /// </summary>
    public override void kill()
    {

        player.GetBuildingManager().removeBuilding(gameObject);
        player.GetBuildingManager().removeWoodWall(gameObject);
        foreach (var g in gridsOn)
        {
            g.setIsBuilding(false);
            g.unBlock();
            g.unBlockPath();
        }
        Destroy(gameObject);
    }

}
