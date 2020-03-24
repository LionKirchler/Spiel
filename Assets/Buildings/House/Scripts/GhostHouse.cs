using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHouse : GhostBuilding
{
    //private Vector2[] newGridOffsets = { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1), new Vector2(-1, 1) };
    private Vector2[] newGridOffsets = { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(1, 0)};
    private int[] newRessourceRequirements = { 200, 0 };

    public GhostHouse()
    {
        gridOffsets = newGridOffsets;
        offset = new Vector2(0, -0.1f);
        ressourceRequirements = newRessourceRequirements;
    }

}
