using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building
{
  
    public House()
    {
        unitSpace = 5;
    }
    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();
    }

    /// <summary>
    /// destory this house
    /// </summary>
    public override void kill()
    {

        player.GetBuildingManager().removeBuilding(gameObject);
        player.GetBuildingManager().removeHouse(gameObject);
        foreach (var g in gridsOn)
        {
            g.setIsBuilding(false);
            g.unBlock();
            g.unBlockPath();
        }
        Destroy(gameObject);
    }

}
