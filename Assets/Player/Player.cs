using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Spawner spawner;
    private BuildingManager buildingManager;
    private UnitManager unitManager;
    private EnemyManager enemyManager;
    private bool isDrawSelectionBox = false;
    private bool isClickSelection = false;
    private  bool isShift = false;
    private bool buildingGUI = false;
    private  Vector2 selectionBoxStart = new Vector2();
    private Vector2 selectionBoxEnd = new Vector2();
    private GUIDisplayer guiDisplayer;
    [SerializeField]
    private MapGrid mapGrid = null;
    private RessourceManager ressourceManager;
    private bool buildingBlocked = false;
    private GameObject woodWallManager;

    void Awake()
    {
        buildingManager = transform.gameObject.GetComponent<BuildingManager>();
        spawner = transform.gameObject.GetComponent<Spawner>();
        unitManager = transform.gameObject.GetComponent<UnitManager>();
        guiDisplayer = GameObject.FindGameObjectWithTag("GUI").GetComponent<GUIDisplayer>();
        ressourceManager = GetComponent<RessourceManager>();
        spawner.onStart();
        woodWallManager = spawner.getWoodWallManager();
        enemyManager = GetComponent<EnemyManager>();
        mapGrid.setPlayer(this);
        mapGrid.setGrid();
        unitManager.onStart();
    }

    void Update()
    { 
        spawn();
        selection();
        move();
    }

    /// <summary>
    /// chekcs for unit move command (rmb)
    /// </summary>
    private void move()
    {
        if (Input.GetMouseButtonDown(1))
        {
            unitManager.move(); 
        }
    }

    /// <summary>
    /// checks for spawn commands
    /// </summary>
    void spawn()
    {

        if (Input.GetKeyDown(KeyCode.A) && !buildingGUI)
        {
            Vector2 mousePos = CalcUtil.getMousePos();
            buildingGUI = true;
            spawner.spawnGhostHouse(mousePos);
        }

        if (Input.GetKeyDown(KeyCode.S) && !buildingGUI)
        {   
            Vector2 mousePos = CalcUtil.getMousePos();
            buildingGUI = true;
            spawner.spawnGhostMainBuilding(mousePos);
        }

        if (Input.GetKeyDown(KeyCode.D) && !buildingGUI)
        {
            Vector2 mousePos = CalcUtil.getMousePos();
            buildingGUI = true;
            spawner.spawnGhostBarrack(mousePos);
        }
        if (Input.GetKeyDown(KeyCode.F) && !buildingGUI)
        {
            woodWallManager.GetComponent<WoodWallManager>().startBuilding(true);
            buildingBlocked = true;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            spawner.spawnZombie1(mapGrid.getGrid()[mapGrid.getMapSize()[0] / 2, mapGrid.getMapSize()[1] / 2].getPos());
        }

        //if (Input.GetKeyDown(KeyCode.G) && !buildingGUI)
        //{
        //    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    buildingGUI = true;
        //    spawner.spawnGhostWoodWallSide(mousePos);
        //}
        //if (Input.GetKeyDown(KeyCode.H) && !buildingGUI)
        //{
        //    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    buildingGUI = true;
        //    spawner.spawnGhostWoodWallCorner(mousePos);
        //}

    }

    
    /// <summary>
    /// checks for selection commands (unit and buildidng selection)
    /// </summary>
    void selection()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isShift = true;
           
        }
        else
        {
            isShift = false;
        }
       
        if (Input.GetMouseButtonDown(0) && !buildingGUI && !buildingBlocked)
        {
            selectionBoxStart = CalcUtil.getMousePos();
            isDrawSelectionBox = true;
            isClickSelection = true;
        }

        if (Input.GetMouseButtonDown(1) && buildingGUI)
        {
            buildingGUI = false;
            Destroy(buildingManager.getCurrentGhostBuilding().gameObject);
            buildingManager.addCurrentGhostBuilding(null);
            buildingManager.SetCurrentGhostBuildingName(null);
        }

        if (Input.GetMouseButtonDown(0) && buildingGUI)
        {
            GhostBuilding gh = buildingManager.getCurrentGhostBuilding().GetComponent<GhostBuilding>();
            if (!buildingBlocked && gh.isBuildable())
            {
                GameObject b = spawner.spawnBuildingForGhost(gh.transform.position);
                List<Grid> grids = gh.GetComponent<GhostBuilding>().blockFieldOn(b);
                b.GetComponent<Building>().setGridsOn(grids);
                if (gh.GetComponent<GhostBuilding>().isStorage())
                {
                    b.GetComponent<Building>().setStorageGrids(gh.GetComponent<GhostBuilding>().getStoragesOn());
                }
                ressourceManager.removeRessources(gh.GetComponent<GhostBuilding>().getRessourceRequirements());
                buildingGUI = false;
                Destroy(buildingManager.getCurrentGhostBuilding().gameObject);
                if (isShift)
                {
                    spawner.spawnGhostForGhost(gh.transform.position);
                    buildingGUI = true;
                }
                else
                {
                    buildingManager.addCurrentGhostBuilding(null);
                    buildingManager.SetCurrentGhostBuildingName(null);
                }

            }
        }


            if (Input.GetMouseButtonUp(0))
            {   
            selectionBoxEnd = CalcUtil.getMousePos();
            isDrawSelectionBox = false;

            if (!isClickSelection)
            {
                 if (!isShift)
                {
                    foreach (GameObject go in unitManager.getUnits())
                    {
                        go.GetComponent<Unit>().setSelected(false);
                    }
                }
                if (selectionBoxStart.x >= selectionBoxEnd.x)
                {
                    float f = selectionBoxStart.x;
                    selectionBoxStart.x = selectionBoxEnd.x;
                    selectionBoxEnd.x = f;
                }
                if (selectionBoxStart.y <= selectionBoxEnd.y)
                {
                    float f = selectionBoxStart.y;
                    selectionBoxStart.y = selectionBoxEnd.y;
                    selectionBoxEnd.y = f;
                }
                foreach (GameObject go in unitManager.getUnits())
                {
                    if (go.transform.position.x >= selectionBoxStart.x && go.transform.position.x <= selectionBoxEnd.x && go.transform.position.y <= selectionBoxStart.y && go.transform.position.y >= selectionBoxEnd.y)
                    {
                        go.GetComponent<Unit>().setSelected(true);
                    }
                }
            }
            else
            {
                if (!isShift)
                {
                    foreach (GameObject go in unitManager.getUnits())
                    {
                        go.GetComponent<Unit>().setSelected(false);
                    }
                    foreach (GameObject go in buildingManager.getBuildings())
                    {
                        go.GetComponent<Building>().setSelected(false);
                    }
                }

               // foreach (GameObject go in unitManager.getUnits())
               // {
                //    if(selectionBoxEnd.x <= go.transform.position.x + go.GetComponent<Unit>().getTriggerCollider().size.x &&
               //        selectionBoxEnd.x >= go.transform.position.x - go.GetComponent<Unit>().getTriggerCollider().size.x &&
               //        selectionBoxEnd.y <= go.transform.position.y + go.GetComponent<Unit>().getTriggerCollider().size.y &&
               //        selectionBoxEnd.y >= go.transform.position.y - go.GetComponent<Unit>().getTriggerCollider().size.y)
               //     {
               //         go.GetComponent<Unit>().setSelected(true);
               //     }
               // }

                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //RaycastHit2D hit = Physics2D.Raycast(ray.origin,ray.direction,Mathf.Infinity);
                
                Vector2 mousePos = CalcUtil.getMousePos();
                Grid grid = mapGrid.getGridOn(mousePos);
                Building bo = null;
                try
                {
                    bo = grid.getBlockingObject().GetComponent<Building>();
                }
                catch
                {

                }
                if (bo != null)
                {
                    bo.setSelected(true);
                }
                
                //if (hit)
               // {
                 //   foreach (GameObject go in unitManager.getUnits())
                 //   {
                //        if (hit.collider.gameObject == go)
                //        {
                //            hit.collider.transform.gameObject.GetComponent<Unit>().setSelected(true);
               //         }
               //     }
               //     foreach (GameObject go in buildingManager.getBuildings())
               //     {
               //         if (hit.collider.gameObject == go)
              //          {
              //              hit.collider.transform.gameObject.GetComponent<Building>().setSelected(true);
              //          }
              //      }
              //  }

               // foreach (GameObject go in buildingManager.getBuildings())
               // {
                   
                   // if (selectionBoxEnd.x <= go.transform.position.x + go.GetComponent<Building>().getTriggerCollider().size.x &&
                   //    selectionBoxEnd.x >= go.transform.position.x - go.GetComponent<Building>().getTriggerCollider().size.x &&
                   //    selectionBoxEnd.y <= go.transform.position.y + go.GetComponent<Building>().getTriggerCollider().size.y &&
                   //    selectionBoxEnd.y >= go.transform.position.y - go.GetComponent<Building>().getTriggerCollider().size.y)
                   // {
                   //     go.GetComponent<Building>().setSelected(true);
                   // }
               // }

               // isClickSelection = false;
            }
        }

    }
    /// <summary>
    /// returns the players unit manager so other objects can access it
    /// </summary>
    /// <returns></returns>
    public UnitManager getUnitManager()
    {
        return unitManager;
    }
    /// <summary>
    /// returns the players ressource manager so other objects can access it
    /// </summary>
    /// <returns></returns>
    public RessourceManager GetRessourceManager()
    {
        return ressourceManager;
    }
    /// <summary>
    /// returns the players map grid so other objects can access it
    /// </summary>
    /// <returns></returns>
    public MapGrid getMapGrid()
    {
        return mapGrid;
    }
    /// <summary>
    /// returns the players spawner, with the prefabs assigned, so other objects can access it
    /// </summary>
    /// <returns></returns>
    public Spawner getSpawner()
    {
        return spawner;
    }
    /// <summary>
    /// returns the players building manager so other objects can access it
    /// </summary>
    /// <returns></returns>
    public BuildingManager GetBuildingManager()
    {
        return buildingManager;             
    }
    /// <summary>
    /// activates or deactivates the building gui
    /// </summary>
    /// <returns></returns>
    public void setbuildingGUI(bool set)
    {
        buildingGUI = set;
    }

    /// <summary>
    /// returns the players gui displayer so other objects can access it
    /// </summary>
    /// <returns></returns>
    public GUIDisplayer getGuiDisplayer()
    {
        return guiDisplayer;
    }
    /// <summary>
    /// activate or deactivate building blocking (player cant build)
    /// </summary>
    /// <returns></returns>
    public void setBuildingBlocked(bool b)
    {
        buildingBlocked = b;
    }
    /// <summary>
    /// returns the players enemy manager so other objects can access it
    /// </summary>
    /// <returns></returns>
    public EnemyManager getEnemyManager()
    {
        return enemyManager;
    }
    /// <summary>
    /// draws the selection box
    /// </summary>
    /// <returns></returns>
    void OnGUI()
    {
        if (isDrawSelectionBox)
        {
            selectionBoxEnd = CalcUtil.getMousePos();

            if (Mathf.Abs(selectionBoxEnd.x - selectionBoxStart.x) >= 0.1 || (Mathf.Abs(selectionBoxEnd.y - selectionBoxStart.y) >= 0.1))
            {
            Vector3 boxPosHiLeftCamera = Camera.main.WorldToScreenPoint(selectionBoxStart);
            Vector3 boxPosLowRightCamera = Camera.main.WorldToScreenPoint(selectionBoxEnd);

            if (boxPosHiLeftCamera.x >= boxPosLowRightCamera.x)
            {
                (boxPosLowRightCamera.x, boxPosHiLeftCamera.x) = (boxPosHiLeftCamera.x, boxPosLowRightCamera.x);
            }
            if (boxPosHiLeftCamera.y <= boxPosLowRightCamera.y)
            {
                (boxPosLowRightCamera.y, boxPosHiLeftCamera.y) = (boxPosHiLeftCamera.y, boxPosLowRightCamera.y);
            }

            float width = boxPosLowRightCamera.x - boxPosHiLeftCamera.x ;
            float height = boxPosHiLeftCamera.y - boxPosLowRightCamera.y;

            isClickSelection = false;

            GUI.Box(new Rect(boxPosHiLeftCamera.x, Screen.height - boxPosHiLeftCamera.y, width, height), "");
            }
            else
            {
                
            }
        }
    }
}
