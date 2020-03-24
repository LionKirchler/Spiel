using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Knight : Unit
{

    private static int[] cost = { 100, 300 };

    public Knight()
    {
        canAttack = true;
        speed = 0.01f;
        unitCountSize = 1;
        attackDuration = 1;
        damage = 50;
        range = 0.6f;
    }

    /// <summary>
    /// returns the cost of this Knight
    /// </summary>
    /// <returns></returns>
    public static int[] getCost()
    {
        return cost;
    }
    
}
