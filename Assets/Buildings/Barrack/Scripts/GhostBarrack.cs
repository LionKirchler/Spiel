using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBarrack : GhostBuilding
{
    private Vector2[] newGridOffsets = { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1), new Vector2(-1, 1) };
    private int[] newRessourceRequirements = { 400, 100 };
    public GhostBarrack()
    {
        gridOffsets = newGridOffsets;
        offset = new Vector2(0, -0.15f);
        ressourceRequirements = newRessourceRequirements;
    }

}
