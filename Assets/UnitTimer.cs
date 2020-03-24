using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTimer
{
    private float endTime;
    private int id;
    private float totalTime;

    public UnitTimer(float endTime, int id, float totalTime)
    {
        this.endTime = endTime;
        this.id = id;
        this.totalTime = totalTime;

    }

    /// <summary>
    /// returns the time the recruiting has ended
    /// </summary>
    /// <returns></returns>
    public float getEndTime()
    {
        return endTime;
    }

    /// <summary>
    /// returns the id of the unit
    /// </summary>
    /// <returns></returns>
    public int getId()
    {
        return id;
    }

    /// <summary>
    /// returns the total amount of units
    /// </summary>
    /// <returns></returns>
    public float getTotalTime()
    {
        return totalTime;
    }

}
