using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected float speed = 0.06f;
    protected Grid destination;
    protected bool homing = false;
    protected GameObject target;
    protected float hitbox = 0.5f;
    protected int damage = 10;

    void Update()
    {
        //if homing it followes the target
        if (homing && target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed);
            Vector2 direction = target.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rotation;
            checkHit();
        }
        

        if(target == null)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// if its close enogh to the target it deals damage
    /// </summary>
    public void checkHit()
    {
        if(CalcUtil.getAbsDist(gameObject,target) <= hitbox)
        {
            target.GetComponent<Enemy>().damageUnit(damage);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// sets the target of the projectile
    /// </summary>
    /// <param name="target">targets Gameobject</param>
    public void setTarget(GameObject target)
    {
        homing = true;
        this.target = target;
    }
}
