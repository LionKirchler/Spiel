using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    private List<Grid> path = null;
    private Vector2 dest;

    public Path(List<Grid> path, Vector2 dest)
    {
        this.path = path;
        this.dest = dest;
    }

    /// <summary>
    /// returns the path grid list
    /// </summary>
    /// <returns></returns>
    public List<Grid> getPath()
    {
        return path;
    }

    /// <summary>
    /// returns the destination positon
    /// </summary>
    /// <returns></returns>
    public Vector2 getDest()
    {
        return dest;
    }
}
