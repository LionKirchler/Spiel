using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Unit
{

    protected bool lumberjack = true;
    protected bool ironOreMiner = true;
    protected Grid workGrid;
    protected string workType = "none";
    protected Grid closestStorage;
    protected int workPhase = -1;
    protected bool didAlready = false;
    protected int[] ironOreStorage = { 0, 100, 1, 0, 10};
    protected int[] woodStorage = { 0, 100, 1, 0, 10};
    private static int[] cost = { 100, 100 };
    //protected List<Grid> storagePath;
    //protected List<Grid> workGridPath;
    protected Tree tr = null;
    protected IronOre io = null;

    public Worker()
    {
        speed = 0.01f;
        unitCountSize = 1;
    }

    new void Start()
    {
        defaultColor = transform.gameObject.GetComponentInChildren<SpriteRenderer>().color;
        base.Start();
    }


    new private void Update()
    {
        //work phases, moving to workgrid, working, moving to storage etc.
        switch (workPhase)
        {
            
            case 0:
                if (!didAlready)
                {
                    //Debug.Log("working");
                    moveTo(workGrid);
                    didAlready = true;
                }
                else
                {
                    if (!moving)
                    {
                        workPhase = 1;
                        didAlready = false;
                    }
                }
                break;

            case 1:
                if (!didAlready)
                {
                    if (workType.Equals("wood")){
                        animator.SetBool("WoodChopping", true);
                    }else if (workType.Equals("ironOre"))
                    {
                        animator.SetBool("IronOreMining", true);
                    }
                    didAlready = true;
                }
                else
                {
                    if (workType.Equals("wood"))
                    {
                        if (woodStorage[3] >= woodStorage[4])
                        {
                            try
                            {
                                woodStorage[0] += workGrid.getBlockingObject().GetComponent<Tree>().damageTree(woodStorage[2]);

                                workGrid.getBlockingObject().GetComponent<Tree>().checkAlive();
                            }
                            catch { }
                            woodStorage[3] = 0;
                        }
                        else
                        {
                            woodStorage[3]++;
                        
                        }
                        tr = null;
                        try
                        {
                            tr = workGrid.getBlockingObject().GetComponent<Tree>();
                        }
                        catch { }
                           
                       
                        if (woodStorage[0] >= woodStorage[1] || tr == null)
                        {
                            if(woodStorage[0] >= woodStorage[1])
                            {
                                woodStorage[0] = woodStorage[1];
                            }
                            didAlready = false;
                            animator.SetBool("WoodChopping", false);
                            workPhase = 2;
                        }
                    }else if (workType.Equals("ironOre"))
                    {
                        if (ironOreStorage[3] >= ironOreStorage[4])
                        {
                            try
                            {
                                ironOreStorage[0] += workGrid.getBlockingObject().GetComponent<IronOre>().damageIronOre(ironOreStorage[2]);

                                workGrid.getBlockingObject().GetComponent<IronOre>().checkAlive();
                            }
                            catch { }
                            ironOreStorage[3] = 0;
                        }
                        else
                        {
                            ironOreStorage[3]++;
                        }
                        io = null;
                        try
                        {
                            io = workGrid.getBlockingObject().GetComponent<IronOre>();
                        }
                        catch { }
                       
                        if (ironOreStorage[0] >= ironOreStorage[1] || io == null)
                        {
                            if (ironOreStorage[0] >= ironOreStorage[1])
                            {
                                ironOreStorage[0] = ironOreStorage[1];
                            }
                            didAlready = false;
                            animator.SetBool("IronOreMining", false);
                            workPhase = 2;
                        }
                    }
                }
                break;
            case 2:
                if (!didAlready)
                {
                    moveTo(closestStorage);
                    didAlready = true;
                }
                else
                {
                    if (!moving)
                    {
                        workPhase = 3;
                        didAlready = false;
                    }
                }
                break;
            case 3:
                if (!didAlready)
                {

                    player.GetRessourceManager().addWood((int)woodStorage[0]);
                    woodStorage[0] = 0;
                    player.GetRessourceManager().addIronOre((int)ironOreStorage[0]);
                    ironOreStorage[0] = 0;
                    if (workType.Equals("wood"))
                    {
                        tr = null;
                        try
                        {
                            tr = workGrid.getBlockingObject().GetComponent<Tree>();
                        }
                        catch { }

                        if (tr == null){
                            workPhase = -1;
                        }
                        else
                        {
                            workPhase = 0;
                        }
                       
                    }else if (workType.Equals("ironOre"))
                    {
                        io = null;
                        try
                        {
                            io = workGrid.getBlockingObject().GetComponent<IronOre>();
                        }
                        catch { }

                        if (io == null)
                        {
                            workPhase = -1;
                        }
                        else
                        {
                            workPhase = 0;
                        }
                    }
                }
               
                break;
        }

        base.Update();
    }

    /// <summary>
    /// returns the workphase of the worker
    /// </summary>
    /// <returns>phase</returns>
    public int getWorkPhase()
    {
        return workPhase;
    }

    /// <summary>
    /// returns if this unit is a lumberjack
    /// </summary>
    /// <returns></returns>
    public bool isLumberjack()
    {
        return lumberjack;
    }
    /// <summary>
    /// returns if this unit is a ironoreminer
    /// </summary>
    /// <returns></returns>
    public bool isIronOreMiner()
    {
        return ironOreMiner;
    }
    /// <summary>
    /// sets the workgrid
    /// </summary>
    /// <param name="grid">grid</param>
    /// <param name="wt">work type (wood,ironore etc.)</param>
    public void setWorkGrid(Grid grid,string wt)
    {

        if (workGrid != null)
        {
            try
            {
                workGrid.getBlockingObject().GetComponent<Tree>().removeUnit(this.GetComponent<Unit>());
            }
            catch { }
        }
        workGrid = grid;
        workType = wt;
    }

    /// <summary>
    /// sets the closest storage the worker should bring the supplies to
    /// </summary>
    /// <param name="g"></param>
    public void setClosestStorage(Grid g)
    {
        closestStorage = g;
    }

    /// <summary>
    /// sets the working phase
    /// </summary>
    /// <param name="ph"></param>
    public void setWorkingPhase(int ph)
    {
        workPhase = ph;
    }

    /// <summary>
    /// make the worker start working
    /// </summary>
    /// <param name="b"></param>
    public void startWorking(bool b)
    {
        if (b)
        {
            workPhase = 0;
        }
        else
        {
            workPhase = -1;
            animator.SetBool("WoodChopping", false);
            animator.SetBool("IronOreMining", false);
            didAlready = false; 
        }
            
    }

    /// <summary>
    /// checks if wood is full
    /// </summary>
    /// <returns></returns>
    public bool isWoodFull()
    {
        return woodStorage[0] >= woodStorage[1];
    }

    /// <summary>
    /// checks if iron ore is full
    /// </summary>
    /// <returns></returns>
    public bool isIronOreFull()
    {
        return ironOreStorage[0] >= ironOreStorage[1];
    }

    /// <summary>
    /// returns the cost of the worker
    /// </summary>
    /// <returns></returns>
    public static int[] getCost()
    {
        return cost;
    }

}

