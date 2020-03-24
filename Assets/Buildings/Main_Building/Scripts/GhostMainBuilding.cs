using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMainBuilding : GhostBuilding
{
    private static Vector2[] newGridOffsets = { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-2, 0), new Vector2(1, 0), new Vector2(2, 0), new Vector2(1, 1), new Vector2(2, 1), new Vector2(0, 1), new Vector2(-1, 1), new Vector2(-2, 1) };
    private static Vector2 newOffset = new Vector2(0, 2.3f);
    private static Vector2[] newStorageOffset = { new Vector2(0, -1)};
    private int[] newRessourceRequirements = { 1000, 300 };
    private List<Grid> storageGrids = new List<Grid>();

    new void Start()
    {
        gridOffsets = newGridOffsets;
        ressourceRequirements = newRessourceRequirements;
        offset = newOffset;
        storage = true;
        base.Start();
    }

    /// <summary>
    /// returns the offset of the mainbuilding, to achieve the right position on the grids
    /// </summary>
    /// <returns></returns>
    static public Vector2 getOffset()
    {
        return newOffset;
    }

    /// <summary>
    /// returns a list of all the grids this building should be on relative to the maingrid it is on
    /// </summary>
    /// <returns></returns>
    static  public Vector2[] getGridOffsets()
    {
        return newGridOffsets;
    }

    /// <summary>
    /// returns the offset of the storage grid (grid where workers can store materials)
    /// </summary>
    /// <returns></returns>
    public static Vector2[] getStorageOffset()
    {
        return newStorageOffset;
    }
    /// <summary>
    /// block the grids on
    /// </summary>
    /// <param name="go">building to block with</param>
    /// <returns></returns>
    public override List<Grid> blockFieldOn(GameObject go)
    {
        foreach (Grid g in gridsOn)
        {
            g.blockPath();
            g.block(go);
            g.setIsBuilding(true);
            mapGrid.removeEmptyGrid(g);
        }
        storagesOn = new List<Grid>();
        foreach (Vector2 offset in newStorageOffset)
        {
            Grid s = mapGrid.getGrid()[(int)(gridOn.getIndex().x + offset.x), (int)(gridOn.getIndex().y + offset.y)];
            storageGrids.Add(s);
            mapGrid.addStorages(s);
            storagesOn.Add(s);
        }

        return gridsOn;
    }

    /// <summary>
    /// returns the storage grids
    /// </summary>
    /// <returns></returns>
    public List<Grid> getStorageGrids()
    {
        return storageGrids;
    }

}
