using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    protected int unitSpace = 0;
    protected bool selected = true;
    protected int health = 100;
    protected int maxHealth;
    protected BoxCollider2D triggerCollider;
    [SerializeField]
    protected Image healthBar;
    protected float loadingBarSteps = 0.0333f;
    [SerializeField]
    protected GameObject progressBar;
    protected Player player;
    protected List<Grid> gridsOn = new List<Grid>();
    protected List<Grid> storageGrids = new List<Grid>();

    public void Start()
    {
        maxHealth = health;
        triggerCollider = transform.gameObject.GetComponentInChildren<BoxCollider2D>();
    }

    public void Update()
    {
    }

    /// <summary>
    /// set the refferenc to the player
    /// </summary>
    /// <param name="player"></param>
    public void setPlayer(Player player)
    {
        this.player = player;
    }

    /// <summary>
    /// Dealing damage to the Building
    /// </summary>
    /// <param name="dmg">damage that should be dealt</param>
    public void damage(int dmg)
    {
        health -= dmg;
        updateHealthBar();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>Amount of units this building can hold</returns>
    public int getUnitSpace()
    {
        return unitSpace;
    }

    /// <summary>
    /// returns its  trigger collider that can be used to detect clicks etc.
    /// </summary>
    /// <returns></returns>
    public BoxCollider2D getTriggerCollider()
    {
        return triggerCollider;
    }

    /// <summary>
    /// Selecting Unselecting a building
    /// </summary>
    /// <param name="b">true to select, false to unselect</param>
    public void setSelected(bool b)
    {
        selected = b;
        setProgressBarVisible(b);
    }

    /// <summary>
    /// Set the Progress Bar visible
    /// </summary>
    /// <param name="b">true to show, false to hide</param>
    public void setProgressBarVisible(bool b)
    {
        if (progressBar != null)
        {
            progressBar.SetActive(b);
        }
    }

    /// <summary>
    /// updates the healthbar
    /// </summary>
    public void updateHealthBar()
    {
        float i = 1;
        for (; health / (float)maxHealth < i; i -= loadingBarSteps) ;
        
        healthBar.fillAmount = i;
        if(health <= 0)
        {
            kill();
        }
    }

    /// <summary>
    /// Destroys Building, almost every building ovverides it
    /// </summary>
    public virtual void kill()
    {
        player.GetBuildingManager().removeBuilding(gameObject);

        foreach (var g in gridsOn)
        {
            g.setIsBuilding(false);
            g.unBlock();
            g.unBlockPath();
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// set the grids the building is on
    /// </summary>
    /// <param name="gridsOn"></param>
    public void setGridsOn(List<Grid> gridsOn)
    {

        this.gridsOn = gridsOn;

    }

    /// <summary>
    /// set the grids that can be used as storage
    /// </summary>
    /// <param name="storageGrids"></param>
    public void setStorageGrids(List<Grid> storageGrids)
    {
        this.storageGrids = storageGrids;
    }

}
