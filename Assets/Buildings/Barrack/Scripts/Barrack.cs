using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barrack : Building
{

    private List<UnitTimer> unitTimers = new List<UnitTimer>();
    [SerializeField]
    private Image spawnProgressBar = null;
    private float nextLoadingBarStep;
    private int recruitingLimit = 5;
    private int recruitingCount = 0;
    private float KnightRecruitTime = 5;
    private float ArchetRecruitTime = 7;
    private const int KNIGHTID = 0;
    private const int ARCHERID = 1;
    public Barrack()
    {
        unitSpace = 0;
    }

    new void Start()
    {
        nextLoadingBarStep = loadingBarSteps;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        base.Start();
    }


    new void Update()
    {
        //Add Knight to queue by pressing 1
        if (selected && Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (player.getUnitManager().getUnitCount() + player.getUnitManager().getRectruitingUnitCount() < player.getUnitManager().getUnitLimit() && player.GetRessourceManager().hasRessources(Knight.getCost()) && recruitingCount < recruitingLimit)
            {
                if (unitTimers.Count > 0)
                {
                    unitTimers.Add(new UnitTimer(unitTimers[unitTimers.Count - 1].getEndTime() + KnightRecruitTime, KNIGHTID, KnightRecruitTime));
                    player.getUnitManager().addRecruitingUnitCount(player.getSpawner().getKnightComponent().getunitCountSize());
                }
                else
                {
                    unitTimers.Add(new UnitTimer(Time.time + KnightRecruitTime, KNIGHTID, KnightRecruitTime));
                    player.getUnitManager().addRecruitingUnitCount(player.getSpawner().getKnightComponent().getunitCountSize());
                }
                player.GetRessourceManager().removeRessources(Knight.getCost());
                recruitingCount++;
            }

        }
        //Add Archer to queue by pressing 1
        else if (selected && Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (player.getUnitManager().getUnitCount() + player.getUnitManager().getRectruitingUnitCount() < player.getUnitManager().getUnitLimit() && player.GetRessourceManager().hasRessources(Archer.getCost()) && recruitingCount < recruitingLimit)
            {
                if (unitTimers.Count > 0)
                {
                    unitTimers.Add(new UnitTimer(unitTimers[unitTimers.Count - 1].getEndTime() + ArchetRecruitTime, ARCHERID, ArchetRecruitTime));
                    player.getUnitManager().addRecruitingUnitCount(player.getSpawner().getArcherComponent().getunitCountSize());
                }
                else
                {
                    unitTimers.Add(new UnitTimer(Time.time + ArchetRecruitTime, ARCHERID, ArchetRecruitTime));
                    player.getUnitManager().addRecruitingUnitCount(player.getSpawner().getArcherComponent().getunitCountSize());
                }
                player.GetRessourceManager().removeRessources(Archer.getCost());
                recruitingCount++;
            }

        }
        //Spawning if queuetime is over
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

                        RecruitKnight();
                        break;
                    case 1:

                        RecruitArcher();
                        break;

                }
                unitTimers.RemoveAt(0);
                recruitingCount--;
            }

        }
        base.Update();
    }

    /// <summary>
    /// Spwawn a knight
    /// </summary>
    void RecruitKnight()
    {
        player.getSpawner().spawnKnight(transform.position);
        player.getUnitManager().removeRecruitingUnitCount(player.getSpawner().getKnightComponent().getunitCountSize());
    }

    /// <summary>
    /// spawn a archer
    /// </summary>
    void RecruitArcher()
    {
        player.getSpawner().spawnArcher(transform.position);
        player.getUnitManager().removeRecruitingUnitCount(player.getSpawner().getArcherComponent().getunitCountSize());
    }

    /// <summary>
    /// Destroy the building
    /// </summary>
    public override void kill()
    {
        player.GetBuildingManager().removeBuilding(gameObject);
        player.GetBuildingManager().removeBarrack(gameObject);
        foreach (var g in gridsOn)
        {
            g.setIsBuilding(false);
            g.unBlock();
            g.unBlockPath();
        }
        Destroy(gameObject);

    }

}

