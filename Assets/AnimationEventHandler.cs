using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{

    bool attackingEnded = false;

    void endAttack()
    {
        attackingEnded = true;
    }

    void startAttack()
    {
        attackingEnded = false;
    }

    public bool isAttackEnded()
    {
        return attackingEnded;
    }

}
