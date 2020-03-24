using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie1 : Enemy
{
    new void Start()
    {
        attackingSpeed = 0.006f;
        walkingSpeed = 0.001f;
        viewRange = 4;
        chaseDistance = 6;
        range = 0.6f;
        damage = 10;
        attackDuration = 1;
        buildingAttackRange = 0.2f;
        base.Start();
    }
}
