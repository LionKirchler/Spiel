using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected bool selected = false;
    protected int health = 100;
    protected float speed = 0;
    protected Color selectionColor = new Color(50 / 255f, 210 / 255f, 50 / 255f, 100 / 255f);
    protected Color defaultColor;
    protected bool lookingRight = true;
    protected Animator animator;
    protected BoxCollider2D triggerCollider;
    protected int unitCountSize = 1;
    protected MapGrid mapGrid;
    protected Spawner spawner;
    protected bool moving = false;
    protected Path movingPath = null;
    protected Player player;
    protected Grid gridOn = null;
    protected Grid lastGridOn = null;
    protected UnitManager unitManager = null;
    protected bool attacking = false;
    protected bool canAttack = false;
    protected float attackstart = 0;
    protected GameObject targetUnit;
    protected Enemy targetUnitUnit;
    protected float attackDuration = 1;
    protected int damage = 50;
    protected float range = 1f;
    protected Grid targetGrid = null;
    protected bool damageTaken = false;
    protected float damageTakenStart = 0;
    protected float damageTakenDuration = 0.3f;
    protected float flashAmount = 0;
    protected SpriteRenderer sr;
    protected AnimationEventHandler animationEventHandler;
    protected PathFinderManager pathFinderManager;
    protected Vector2 moveToPos;

    protected int type = -1;
    protected string types = "";
    public static readonly int WALKINGTYPE = 0;
    public static readonly string WALKINGTYPES = "Walking";
    public static readonly string ATTACKINGS = "Attacking";

    public void Start()
    {
        animator = transform.gameObject.GetComponentInChildren<Animator>();
        triggerCollider = transform.gameObject.GetComponentInChildren<BoxCollider2D>();
        sr = transform.gameObject.GetComponentInChildren<SpriteRenderer>();
        defaultColor = sr.color;
        animationEventHandler = GetComponentInChildren<AnimationEventHandler>();
        pathFinderManager = player.getSpawner().getPathFinderManager();
    }

    int i = -1;
    public void Update()
    {
        //if damage taken, it does a visual effect
        if(damageTaken)
        {
            if(Time.time <= damageTakenStart +  damageTakenDuration)
            {

                
                if (Time.time <= damageTakenStart + damageTakenDuration / 2) {
                    flashAmount = ((Time.time - damageTakenStart) / (damageTakenDuration / 2));
                }
                else
                {
                    flashAmount =  1 - ((Time.time - (damageTakenStart + damageTakenDuration / 2)) / (damageTakenDuration / 2));
                }
                sr.material.SetFloat("Flash", flashAmount);
            }
            else
            {
                damageTaken = false;
                sr.material.SetFloat("Flash", 0);
            }
        }

        //if attacking it does a animation and deals damage to the target
        if (attacking)
        {

            if (animator.GetBool(ATTACKINGS) == false)
            {
                animator.SetBool(ATTACKINGS, true);
                attackstart = Time.time;
            }
            else
            {
                if (animationEventHandler.isAttackEnded())
                {
                    damageTarget(damage);
                    attacking = false;
                    animator.SetBool(ATTACKINGS, false);
                }
            }
            return;
        }

        //if moving it moves to the move-to-location
        if (moving)
        {

            //if targets grid changed, refresh path
            if (targetUnitUnit != null && targetUnit.activeInHierarchy && targetUnitUnit.getGridOn() != targetGrid)
            {
                attack(targetUnit);
                return;
            }

            //if in range with the target, attack it
            if (targetUnit != null && targetUnit.activeInHierarchy && CalcUtil.getAbsDist(gameObject, targetUnit) <= range)
            {
                attacking = true;
            }

            //moving
            if (Mathf.Sqrt(Mathf.Pow(transform.position.x - moveToPos.x, 2)) >= 0.01 || Mathf.Sqrt(Mathf.Pow(transform.position.y - moveToPos.y, 2)) >= 0.01)
            {
                if (transform.position.x - moveToPos.x <= -0.01)
                {
                    transform.position += new Vector3(speed, 0);
                }
                else if (transform.position.x - moveToPos.x >= 0.01)
                {
                    transform.position += new Vector3(-speed, 0);
                }

                if (transform.position.y - moveToPos.y <= -0.01)
                {
                    transform.position += new Vector3(0, speed);
                }
                else if (transform.position.y - moveToPos.y >= 0.01)
                {
                    transform.position += new Vector3(0, -speed);
                }
               
            }
            //if moved to destination get next postiontion in path and move to it, until path is gone
            else
                {
                if (i <= 0)
                {
                    moving = false;
                    animator.SetBool(WALKINGTYPES, false);
                    setGridOn(movingPath.getPath()[i]);
                }
                else
                {
                    setGridOn(movingPath.getPath()[i]);

                    i--;
                    if (i != 0)
                    {
                        moveToPos = movingPath.getPath()[i].getPos();
                    }
                    else
                    {
                        moveToPos = movingPath.getDest();
                    }
                    if (movingPath.getPath()[i].isPathBlocked())
                    {
                        moveTo(movingPath.getPath()[i]);
                    }
                    //looking in destinations direction
                    if (transform.position.x <= moveToPos.x)
                    {
                        lookingRight = true;
                        transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
                    }
                    else
                    {
                        lookingRight = false;
                        transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);

                    }
                }
            }

        }
    }

    /// <summary>
    /// returns if the unit is selected
    /// </summary>
    /// <returns></returns>
    public bool isSelected()
    {
        return selected;
    }

    /// <summary>
    /// passes a spawner to to the unit to work with
    /// </summary>
    /// <param name="s">spawner</param>
    public void PassSpawner(Spawner s)
    {
        spawner = s;
    }

    /// <summary>
    /// sets the mapgrid for the unit to work with
    /// </summary>
    /// <param name="mg">mapgrid</param>
    public void setMapGrid(MapGrid mg)
    {
        mapGrid = mg;
    }

    /// <summary>
    /// returns true if this unit can fight (is a combat unit)
    /// </summary>
    /// <returns></returns>
    public bool isCombatUnit()
    {
        return canAttack;
    }

    /// <summary>
    /// toggs the selection of the unit
    /// </summary>
    public void toggleSelected()
    {
        if (selected)
        {
            selected = false;
        }
        else
        {
            selected = true;
        }
    }

    /// <summary>
    /// selects/deselects the unit
    /// </summary>
    /// <param name="b"></param>
    public void setSelected(bool b)
    {
        if (b)
        {
            selected = true; 
        }
        else
        {
            selected = false;
        }
    }

    /// <summary>
    /// damages the units target
    /// </summary>
    /// <param name="damage"></param>
    public virtual void damageTarget(int damage)
    {
        if (targetUnit != null && targetUnitUnit != null && targetUnit.activeInHierarchy)
        {
            targetUnitUnit.damageUnit(damage);
        }
    }

    /// <summary>
    /// moves the unit with a path, and disables attacking
    /// </summary>
    /// <param name="path">path</param>
    public void moveAction(Path path)
    {
        if (canAttack)
        {
            attacking = false;
            animator.SetBool(ATTACKINGS, false);
            targetUnitUnit = null;
            targetGrid = null;
            targetUnit = null;
        }
        moveTo(path);
    }

    /// <summary>
    /// moves the unit to a grid
    /// </summary>
    /// <param name="g">grid</param>
    public void moveTo(Grid g)
    {
        Path path = pathFinderManager.FindPath(mapGrid.getGridOn(transform.position), g.getPos(), mapGrid);
        moveTo(path);
    }

    /// <summary>
    /// moves the unit to a enemy
    /// </summary>
    /// <param name="g">enemy</param>
    public void moveTo(Enemy g)
    {
        Path path = pathFinderManager.FindPath(mapGrid.getGridOn(transform.position), (Vector2)g.transform.position, mapGrid);
        moveTo(path);
    }

    /// <summary>
    /// moves the unit to a path
    /// </summary>
    /// <param name="patho"></param>
    public void moveTo(Path patho)
    {
        List<Grid> path;
        if (patho != null)
        {
            path = patho.getPath();
        }
        else
        {
            return;
        }
        if(path == null || path.Count == 0)
        {
            return; 
        }
        moving = true;
        movingPath = patho;
        animator.SetBool(WALKINGTYPES, true);
        setGridOn();
        if (path.Count >= 2)
        {
            i = path.Count - 2;
            moveToPos = movingPath.getPath()[i].getPos();
        }
        else
        {
            moveToPos = patho.getDest();
        }

        if (transform.position.x <= patho.getPath()[i].getPos().x)
        {
            lookingRight = true;
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }
        else
        {
            lookingRight = false;
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);

        }
    }


    /// <summary>
    /// attack a enemy
    /// </summary>
    /// <param name="go">enemys gameobject</param>
    public void attack(GameObject go)
    {
        targetUnit = go;
        targetUnitUnit = go.GetComponent<Enemy>();
        targetGrid = targetUnitUnit.getGridOn();
        moveTo(targetUnitUnit);
    }

    /// <summary>
    /// sets the grid this unit is on
    /// </summary>
    public void setGridOn()
    {
        bool last = false;

        if (gridOn != null)
        {
            gridOn.removeUnitOn(gameObject);
            lastGridOn = gridOn;
        }
        else
        {
            last = true;
        }
        gridOn = mapGrid.getGridOn(transform.position);
        gridOn.addUnitOn(gameObject);
        if (last)
        {
            lastGridOn = gridOn;
        }
    }

    /// <summary>
    /// sets the  grid this unit is on
    /// </summary>
    /// <param name="g">grid</param>
    public void setGridOn(Grid g)
    {
        bool last = false;
        if (gridOn != null)
        {
            gridOn.removeUnitOn(gameObject);
            lastGridOn = gridOn;
        }
        else
        {
            last = true;
        }
        gridOn = g;
        gridOn.addUnitOn(gameObject);
        if (last)
        {
            lastGridOn = gridOn;
        }
        //Debug.Log(g.getUnitsOn().Count);
        //g.getGrass().GetComponentInChildren<SpriteRenderer>().color = Color.blue;
    }

    /// <summary>
    /// returns the health of this unit
    /// </summary>
    /// <returns></returns>
    public int getHealth()
    {
        return health;
    }

    /// <summary>
    /// damages the unit
    /// </summary>
    /// <param name="damage">damage</param>
    public void damageUnit(int damage)
    {
        health -= damage;
        indicateDamage();
        if(health <= 0)
        {
            kill();
        }
    }

    /// <summary>
    /// indicates the damage done (visual effect)
    /// </summary>
    public void indicateDamage()
    {
        damageTaken = true;
        damageTakenStart = Time.time;
    }

    /// <summary>
    /// kills this unit
    /// </summary>
    public void kill()
    {
        try
        {
            unitManager.removeUnit(gameObject);
        }
        catch { }
        try
        {
            unitManager.addUnitCount(-unitCountSize);
        }
        catch { }
        try
        {
            gridOn.removeUnitOn(gameObject);
        }
        catch { }
        try
        {
            Destroy(gameObject);
        }
        catch { }

    }

    /// <summary>
    /// heals this unit
    /// </summary>
    /// <param name="heal"></param>
    public void healUnit(int heal)
    {
        health += heal;
       
    }

    /// <summary>
    /// returns the trigger collider of this unit (for click detection for example
    /// </summary>
    /// <returns></returns>
    public BoxCollider2D getTriggerCollider()
    {
        return triggerCollider;
    }

    /// <summary>
    /// returns the unit count size (space needet for unit)
    /// </summary>
    /// <returns></returns>
    public int getunitCountSize()
    {
        return unitCountSize;
    }

    /// <summary>
    /// sets the player for the unit to work with
    /// </summary>
    /// <param name="player"></param>
    public void setPlayer(Player player)
    {
        this.player = player;
    }

    /// <summary>
    /// sets the unit manager for the unit to work with
    /// </summary>
    /// <param name="unitManager"></param>
    public void setUnitManager(UnitManager unitManager)
    {
        this.unitManager = unitManager;
    }

    /// <summary>
    /// returns the grid the unit is on
    /// </summary>
    /// <returns></returns>
    public Grid getGridOn()
    {
        return gridOn;
    }

    /// <summary>
    /// returns the last grid the unit was on
    /// </summary>
    /// <returns></returns>
    public Grid getLastGridOn()
    {
        return lastGridOn;
    }

}
