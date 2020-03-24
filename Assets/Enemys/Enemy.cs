using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected int health = 100;
    protected int maxhealth;
    protected float attackingSpeed = 0;
    protected float walkingSpeed = 0;
    protected bool lookingRight = true;
    protected Vector2 destination;
    protected Animator animator;
    protected BoxCollider2D triggerCollider;
    protected MapGrid mapGrid;
    protected Spawner spawner;
    protected bool moving = false;
    protected bool agressiveMoving = false;
    protected Path movingPath = null;
    protected Player player;
    protected int viewRange = 4;
    protected Grid targetBuilding = null;
    protected Grid targetBuildingMoveGrid = null;
    protected GameObject targetUnit = null;
    protected Unit targetUnitUnit = null;
    protected Grid targetGrid = null;
    protected Grid gridOn = null;
    protected float scanDelay = 3;
    protected float lastScan;
    protected int chaseDistance = 6;
    protected float range = 1;
    protected int damage = 10;
    protected float attackstart = 0;
    protected bool attacking = false;
    protected float attackDuration = 2;
    protected AnimationEventHandler animationEventHandler;
    protected EnemyManager enemyManager;
    protected bool damageTaken = false;
    protected float damageTakenStart = 0;
    protected float damageTakenDuration = 0.3f;
    protected float flashAmount = 0;
    protected Color damageColor = Color.white;
    protected SpriteRenderer sr;
    protected int nextWalkingRange = 5;
    protected PathFinderManager pathFinderManager;
    protected bool movingBuilding = false;
    protected Vector2 moveToPos;
    protected bool attackingBuilding = false;
    protected float buildingAttackRange = 0.2f;

    protected int type = -1;
    protected string types = "";
    public static readonly int AGRESSIVEWALKINGTYPE = 0;
    public static readonly int WALKINGTYPE = 1;
    public static readonly string AGRESSIVEWALKINGTYPES = "AgressiveWalking";
    public static readonly string WALKINGTYPES = "Walking";
    public static readonly int ATTACKINBUILDINGTYPE = 2;
    public static readonly string ATTACKINGS = "Attacking";

    public void Start()
    {
        animator = transform.gameObject.GetComponentInChildren<Animator>();
        triggerCollider = transform.gameObject.GetComponentInChildren<BoxCollider2D>();
        lastScan = Time.time;
        animationEventHandler = GetComponentInChildren<AnimationEventHandler>();
        enemyManager = player.getEnemyManager();
        sr = GetComponentInChildren<SpriteRenderer>();
        pathFinderManager = player.getSpawner().getPathFinderManager();
        maxhealth = health;
    }

    int i = -1;
    public void Update()
    {
        //if not actively attacking etc, move randomly
        if (!moving && !agressiveMoving && !attacking && !movingBuilding)
        {
            walkToRandom();
        }

        //if damage taken it triggeres a effect
        if (damageTaken)
        {
            if (Time.time <= damageTakenStart + damageTakenDuration)
            {

                if (Time.time <= damageTakenStart + damageTakenDuration / 2)
                {
                    flashAmount = ((Time.time - damageTakenStart) / (damageTakenDuration / 2));
                }
                else
                {
                    flashAmount = 1 - ((Time.time - (damageTakenStart + damageTakenDuration / 2)) / (damageTakenDuration / 2));
                }
                sr.material.SetFloat("Flash", flashAmount);
            }
            else
            {
                damageTaken = false;
                sr.material.SetFloat("Flash", 0);
            }
        }

        //after the scanningTime has passed it scans for units/buildings to attack
        if (Time.time - lastScan >= scanDelay)
        {
            bool found = false;
            if (!agressiveMoving && !attacking && !movingBuilding)
            {
                GameObject go = scanForUnitsOn();
                if (go != null)
                {
                    found = true;
                    moving = false;
                    attack(go);
                }
                else
                {

                    Grid[] g = scanForBuildings();
                    if (g != null && g[0] != null && g[1] != null)
                    {
                        found = true;
                        moving = false;
                        targetBuilding = g[0];
                        targetBuildingMoveGrid = g[1];
                        Vector2 pos = getBuildingMovePos();
                        attackBuilding(pos);
                    }
                }
            }
            if (!found)
            {
                lastScan = Time.time;
            }
        }

        //triggers the attack animation and deals damage to the target
        if (attacking || attackingBuilding)
        {
            if (animator.GetBool(ATTACKINGS) == false)
            {
                animator.SetBool(ATTACKINGS, true);
            }
            else
            {
                if (animationEventHandler.isAttackEnded())
                {
                    if (attacking)
                    {
                        damageTarget(damage);
                        attacking = false;
                        animator.SetBool(ATTACKINGS, false);
                    }
                    else if (attackingBuilding)
                    {
                        damageTargetBuilding(damage);
                        attackingBuilding = false;
                        animator.SetBool(ATTACKINGS, false);
                    }
                }
            }
            return;
        }


        //movements (walking, attacking unit, attacking building)
        if (moving || agressiveMoving || movingBuilding)
        {

            if (moving)
            {
                type = WALKINGTYPE;
                types = WALKINGTYPES;
            }
            else if (agressiveMoving)
            {
                type = AGRESSIVEWALKINGTYPE;
                types = AGRESSIVEWALKINGTYPES;
            }
            else if (movingBuilding)
            {
                type = ATTACKINBUILDINGTYPE;
                types = AGRESSIVEWALKINGTYPES;
            }

            //if attacking unit
            if (agressiveMoving)
            {
                if (targetUnitUnit != null)
                {
                    //check if the unit chaged grid/is moving, if it is recalculate the path
                    if (targetUnitUnit.getGridOn() != targetGrid)
                    {
                        attack(targetUnit);
                    }
                    //check if close enogh to attack
                    if (targetUnit != null && CalcUtil.getAbsDist(gameObject, targetUnit) <= range)
                    {
                        attacking = true;
                    }
                }
                else
                {
                    //if target is destroyed,  clear agressive behaviour
                    clearAgressiveMoving();
                    return;
                }
            }
            //if attacking building
            else if (movingBuilding)
            {
                if (targetBuilding != null && targetBuilding.isBuilding() && targetBuilding.getBlockingObject() != null)
                {
                    //check if close enogh to attack
                    if (CalcUtil.getAbsDist(transform.position.x, transform.position.y, targetBuildingMoveGrid.getPos().x, targetBuildingMoveGrid.getPos().y) <= buildingAttackRange)
                    {
                        moving = false;
                        attackingBuilding = true;
                    }
                }
                else
                {
                    //if building is destroyed, clear agressive behaviour
                    clearAttackingBuilding();
                }
            }

            //if distance to the next move position is greater 0.1 move to it, else swap to the next one
            if (CalcUtil.getAbsDist(transform.position.x, transform.position.y, moveToPos.x, moveToPos.y) >= 0.1)
            {
                //if movepos is on the right, move right
                if (transform.position.x - moveToPos.x <= -0.01)
                {
                    if (type == AGRESSIVEWALKINGTYPE || type == ATTACKINBUILDINGTYPE)
                    {
                        transform.position += new Vector3(attackingSpeed, 0);
                    }
                    else if (type == WALKINGTYPE)
                    {
                        transform.position += new Vector3(walkingSpeed, 0);
                    }
                }
                //left
                else if (transform.position.x - moveToPos.x >= 0.01)
                {
                    if (type == AGRESSIVEWALKINGTYPE || type == ATTACKINBUILDINGTYPE)
                    {
                        transform.position += new Vector3(-attackingSpeed, 0);
                    }
                    else if (type == WALKINGTYPE)
                    {
                        transform.position += new Vector3(-walkingSpeed, 0);
                    }
                }
                //top
                if (transform.position.y - moveToPos.y <= -0.01)
                {
                    if (type == AGRESSIVEWALKINGTYPE || type == ATTACKINBUILDINGTYPE)
                    {
                        transform.position += new Vector3(0, attackingSpeed);
                    }
                    else if (type == WALKINGTYPE)
                    {
                        transform.position += new Vector3(0, walkingSpeed);
                    }
                }
                //bot
                else if (transform.position.y - moveToPos.y >= 0.01)
                {
                    if (type == AGRESSIVEWALKINGTYPE || type == ATTACKINBUILDINGTYPE)
                    {
                        transform.position += new Vector3(0, -attackingSpeed);
                    }
                    else if (type == WALKINGTYPE)
                    {
                        transform.position += new Vector3(0, -walkingSpeed);
                    }
                }

            }
            else
            {
                //if path index is 0 / path has ended, stop moving
                if (i <= 0)
                {
                    moving = false;
                    agressiveMoving = false;
                    movingBuilding = false;
                    animator.SetBool(types, false);
                    setGridOn(movingPath.getPath()[i]);
                }
                //else, get next pose in path to move to
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
                        if (type == AGRESSIVEWALKINGTYPE && targetUnit != null)
                        {
                            attack(targetUnit);
                        }
                        else if (type == WALKINGTYPE)
                        {
                            walkToRandom();
                        }
                        else if (type == ATTACKINBUILDINGTYPE)
                        {
                            attackBuilding(getBuildingMovePos());
                        }
                    }
                    //look to destination
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
    /// scans for near building in range
    /// </summary>
    /// <returns>found building grids, index 0: grid building is on; 1: grid before it, usually grid to be moven to</returns>
    public Grid[] scanForBuildings()
    {
        Grid[] grids = pathFinderManager.FindNearestBuildGrid(mapGrid.getGridOn(transform.position), mapGrid, viewRange);
        return grids;

    }

    /// <summary>
    /// get pos the the unit should move to to get to the building
    /// </summary>
    /// <returns>pos</returns>
    public Vector2 getBuildingMovePos()
    {
        if (targetBuilding != null && targetBuildingMoveGrid != null)
        {
            //return targetBuildingMoveGrid.getPos() + (targetBuilding.getPos() - targetBuildingMoveGrid.getPos());
            return targetBuildingMoveGrid.getPos();
        }
        else
        {
            return Vector2.positiveInfinity;
        }
    }

    /// <summary>
    /// scans for near unit in range
    /// </summary>
    /// <returns>units Gameobject</returns>
    public GameObject scanForUnitsOn()
    {
        Grid g = pathFinderManager.FindNearestUnitGrid(mapGrid.getGridOn(transform.position), mapGrid, viewRange);

        if (g != null)
        {
            targetUnit = g.getUnitsOn()[0];
        }
        return targetUnit;
    }

    /// <summary>
    /// sets the grid this enemy is on
    /// </summary>
    public void setGridOn()
    {
        if (gridOn != null)
        {
            gridOn.removeEnemyOn(gameObject);
        }
        gridOn = mapGrid.getGridOn(transform.position);
        gridOn.addEnemyOn(gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>grid this unit is on</returns>
    public Grid getGridOn()
    {
        return gridOn;
    }

    /// <summary>
    /// stops agressive moving/attacking unit
    /// </summary>
    public void clearAgressiveMoving()
    {
        targetGrid = null;
        targetUnitUnit = null;
        targetUnit = null;
        agressiveMoving = false;
        animator.SetBool(AGRESSIVEWALKINGTYPES, false);
    }
    /// <summary>
    /// stops enemy from attacking building
    /// </summary>
    public void clearAttackingBuilding()
    {
        targetBuilding = null;
        targetBuildingMoveGrid = null;
        movingBuilding = false;
        animator.SetBool(AGRESSIVEWALKINGTYPES, false);
    }

    /// <summary>
    /// sets the grid, more performance friendly
    /// </summary>
    /// <param name="g">Grid to be set</param>
    public void setGridOn(Grid g)
    {

        if (gridOn != null)
        {
            gridOn.removeEnemyOn(gameObject);
        }
        gridOn = g;
        gridOn.addEnemyOn(gameObject);
        //Debug.Log(g.getUnitsOn().Count);
        //g.getGrass().GetComponentInChildren<SpriteRenderer>().color = Color.blue;
    }

    /// <summary>
    /// set the spawner this enemy should use
    /// </summary>
    /// <param name="s"></param>
    public void PassSpawner(Spawner s)
    {
        spawner = s;
    }

    /// <summary>
    /// attack a unit
    /// </summary>
    /// <param name="go">Unit to attack</param>
    public void attack(GameObject go)
    {
        targetUnit = go;
        targetUnitUnit = go.GetComponent<Unit>();
        targetGrid = targetUnitUnit.getGridOn();
        moveTo(targetUnitUnit, AGRESSIVEWALKINGTYPE);
    }
    /// <summary>
    /// move to building, building itself is not set here
    /// </summary>
    /// <param name="g"></param>
    public void attackBuilding(Vector2 g)
    {
        movingBuilding = true;
        moveTo(g, ATTACKINBUILDINGTYPE);
    }

    /// <summary>
    /// sets the mapGrid to be used by this enemy, for example to search buidlings/units
    /// </summary>
    /// <param name="mg">mapgrid</param>
    public void setMapGrid(MapGrid mg)
    {
        mapGrid = mg;
    }

    /// <summary>
    /// moveTo a position, in a specific type
    /// </summary>
    /// <param name="g">position to move to</param>
    /// <param name="type">movement type (walking, agressive etc.)</param>
    public void moveTo(Vector2 g, int type)
    {
        Path path = pathFinderManager.FindPath(mapGrid.getGridOn(transform.position), g, mapGrid);
        moveTo(path, type);
    }

    /// <summary>
    /// moveTo a grid, in a specific type
    /// </summary>
    /// <param name="g">position to move to</param>
    /// <param name="type">movement type (walking, agressive etc.)</param>
    public void moveTo(Grid g, int type)
    {
        Path path = pathFinderManager.FindPath(mapGrid.getGridOn(transform.position), g.getPos(), mapGrid);
        moveTo(path, type);
    }

    /// <summary>
    /// moveTo a unit, in a specific type
    /// </summary>
    /// <param name="g">unit to move to</param>
    /// <param name="type">movement type (walking, agressive etc.)</param>
    public void moveTo(Unit g, int type)
    {
        Path path = pathFinderManager.FindPath(mapGrid.getGridOn(transform.position), g.transform.position, mapGrid);
        moveTo(path, type);
    }

    /// <summary>
    /// makes enemy walk to a random near unblocked grid
    /// </summary>
    public void walkToRandom()
    {
        Grid g = mapGrid.GetRandomEmptyGridInRange(gridOn, nextWalkingRange);
        moveTo(g, WALKINGTYPE);
    }

    /// <summary>
    /// move the given path
    /// </summary>
    /// <param name="patho">path to move</param>
    /// <param name="type">movement type (walking, agressive etc.)</param>
    public void moveTo(Path patho, int type)
    {
        if (patho == null)
        {
            Debug.Log("path Null");
            return;
        }

        List<Grid> path = patho.getPath();
        if (path == null || path.Count == 0)
        {
            Debug.Log("path not valid");
            return;
        }
        string types = "";
        if (type == WALKINGTYPE)
        {
            moving = true;
            types = WALKINGTYPES;
        }
        else if (type == AGRESSIVEWALKINGTYPE)
        {
            if (chaseDistance <= path.Count)
            {
                clearAgressiveMoving();
                walkToRandom();
                return;
            }
            agressiveMoving = true;
            types = AGRESSIVEWALKINGTYPES;
        }
        else if (type == ATTACKINBUILDINGTYPE)
        {
            movingBuilding = true;
            types = AGRESSIVEWALKINGTYPES;
        }
        movingPath = patho;


        animator.SetBool(types, true);
        if (movingPath.getPath().Count >= 2)
        {
            i = movingPath.getPath().Count - 2;
            moveToPos = movingPath.getPath()[i].getPos();
        }
        else
        {
            i = 0;
            moveToPos = patho.getDest();
        }
        if (movingPath.getPath()[i].isPathBlocked())
        {
            if (type == WALKINGTYPE)
            {
                walkToRandom();
            }
            else if (type == AGRESSIVEWALKINGTYPE)
            {
                if (targetUnit != null)
                {
                    attack(targetUnit);
                }
                else
                {
                    walkToRandom();
                }
            }
        }

        setGridOn();

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

    /// <summary>
    /// returns the health of this enemy
    /// </summary>
    /// <returns></returns>
    public int getHealth()
    {
        return health;
    }

    /// <summary>
    /// damages this enemy
    /// </summary>
    /// <param name="damage">damage to be dealt</param>
    public void damageUnit(int damage)
    {
        health -= damage;
        indicateDamage();
        if (health <= 0)
        {
            kill();
        }
    }

    /// <summary>
    /// damage the targeted building
    /// </summary>
    /// <param name="damage">damage to be dealt</param>
    public void damageTargetBuilding(int damage)
    {
        if (targetBuilding != null && targetBuilding.isBuilding())
        {
            targetBuilding.getBlockingObject().GetComponent<Building>().damage(damage);
        }
        else if (!targetBuilding.isBuilding())
        {
            clearAttackingBuilding();
        }

    }

    /// <summary>
    /// damage the target unit
    /// </summary>
    /// <param name="dmg">damage to be dealt</param>
    public void damageTarget(int dmg)
    {
        if (targetUnitUnit != null)
        {
            targetUnitUnit.damageUnit(dmg);
        }

    }

    /// <summary>
    /// indicate the damage taken with a shader effect
    /// </summary>
    public void indicateDamage()
    {
        damageTaken = true;
        damageTakenStart = Time.time;
    }

    /// <summary>
    /// kill this enemy
    /// </summary>
    public void kill()
    {
        enemyManager.poolEnemy(gameObject);
        //Destroy(gameObject);
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
    /// returns the trigger collider of this enemy, can be used for click detection for example
    /// </summary>
    /// <returns>triggerCollider</returns>
    public BoxCollider2D getTriggerCollider()
    {
        return triggerCollider;
    }

    /// <summary>
    /// sets the player this enemy should work with
    /// </summary>
    /// <param name="player"></param>
    public void setPlayer(Player player)
    {
        this.player = player;
    }

    /// <summary>
    /// resets the enemy
    /// </summary>
    public void resetStats()
    {
        health = maxhealth;
        type = -1;
        types = "";
        clearAgressiveMoving();
        clearAttackingBuilding();
    }
}
