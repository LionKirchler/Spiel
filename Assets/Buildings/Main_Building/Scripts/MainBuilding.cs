using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBuilding : Building
{
    private Vector2 spawnOffet = new Vector2(0, -2.5f);
    private List<UnitTimer> unitTimers = new List<UnitTimer>();
    [SerializeField]
    private Image spawnProgressBar = null;
    private float nextLoadingBarStep;
    private int recruitingLimit = 5;
    private int recruitingCount = 0;
    private float WorkerManRecruitTime = 5;
    private const int WORKERMANID = 0;
    public MainBuilding()
    {
        unitSpace = 10;
    }

    new void Start()
    {
        nextLoadingBarStep = loadingBarSteps;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        base.Start();
    }


    new void Update()
    {
        //add a worker to the recruite queue by pressing 1
        if (selected && Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (player.getUnitManager().getUnitCount() + player.getUnitManager().getRectruitingUnitCount() < player.getUnitManager().getUnitLimit() && player.GetRessourceManager().hasRessources(Worker.getCost()) && recruitingCount < recruitingLimit)
            {
                if (unitTimers.Count > 0)
                {
                    unitTimers.Add(new UnitTimer(unitTimers[unitTimers.Count - 1].getEndTime() + WorkerManRecruitTime, WORKERMANID, WorkerManRecruitTime));
                    player.getUnitManager().addRecruitingUnitCount(player.getSpawner().getKnightComponent().getunitCountSize());
                }
                else
                {
                    unitTimers.Add(new UnitTimer(Time.time + WorkerManRecruitTime, WORKERMANID, WorkerManRecruitTime));
                    player.getUnitManager().addRecruitingUnitCount(player.getSpawner().getKnightComponent().getunitCountSize());
                }
                player.GetRessourceManager().removeRessources(Worker.getCost());
                recruitingCount++;
            }
        }
        //if units are recruited, spawn them
        if (unitTimers.Count > 0)
        {
            float currentLoadingPoint = (Time.time - (unitTimers[0].getEndTime() - unitTimers[0].getTotalTime())) / unitTimers[0].getTotalTime();
            if (currentLoadingPoint > nextLoadingBarStep)
            {
                spawnProgressBar.fillAmount = nextLoadingBarStep;
                nextLoadingBarStep += loadingBarSteps;
            }

            if (Time.time > unitTimers[0].getEndTime())
            {
                spawnProgressBar.fillAmount = 1;
                nextLoadingBarStep = loadingBarSteps;
                switch (unitTimers[0].getId())
                {
                    case 0:

                        RecruitWorker();
                        break;

                }
                unitTimers.RemoveAt(0);
                recruitingCount--;
            }

        }
        base.Update();
    }

    /// <summary>
    /// spawns a worker
    /// </summary>
    void RecruitWorker()
    {
        player.getSpawner().spawnWorkerMan(new Vector2(transform.position.x + spawnOffet.x, transform.position.y + spawnOffet.y));
        player.getUnitManager().removeRecruitingUnitCount(player.getSpawner().getKnightComponent().getunitCountSize());
    }

    /// <summary>
    /// destroys this building
    /// </summary>
    public override void kill()
    {
        player.GetBuildingManager().removeBuilding(gameObject);
        player.GetBuildingManager().removeMainBuilding(gameObject);
        foreach (var g in gridsOn)
        {
            g.setIsBuilding(false);
            g.unBlock();
            g.unBlockPath();
        }

        foreach (var item in storageGrids)
        {
            player.getMapGrid().removeStorages(item);
        }

        Destroy(gameObject);

    }
}
