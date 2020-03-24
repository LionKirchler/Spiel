using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    Grid gridOn = null;

    public void setGridOn(Grid g)
    {
        gridOn = g;
    }
    public Grid getGridOn()
    {
        return gridOn;
    }

}
