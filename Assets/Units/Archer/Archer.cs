using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit
{
    static int[] cost = { 300, 500 };
    private Vector2 arrowOffest = new Vector2(0.1f,0.1f);

    public Archer()
    {
        canAttack = true;
        speed = 0.01f;
        unitCountSize = 2;
        attackDuration = 1;
        damage = 50;
        range = 4f;
    }

    public static int[] getCost()
    {
        return cost;
    }

    public override void damageTarget(int damage)
    {
        if (targetUnit != null && targetUnitUnit != null)
        {
            Vector2 offset = lookingRight ? arrowOffest : -arrowOffest;
            GameObject arrow = spawner.spawnArcherArrow((Vector2)transform.position + offset);
            arrow.GetComponent<Projectile>().setTarget(targetUnit);
        }
    }

}
