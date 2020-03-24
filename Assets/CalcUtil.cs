using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcUtil : MonoBehaviour
{

    /// <summary>
    /// return the distance of 2 gameobjects
    /// </summary>
    /// <param name="u">object 1</param>
    /// <param name="e">object 2</param>
    /// <returns>distance (float)</returns>
    public static float getAbsDist(GameObject u, GameObject e)
    {
        float x1 = u.transform.position.x;
        float x2 = e.transform.position.x;
        float y1 = u.transform.position.y;
        float y2 = e.transform.position.y;

        return Mathf.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
    }
    /// <summary>
    /// return the distance of 2 positions
    /// </summary>
    /// <param name="x1">x1</param>
    /// <param name="y1">y1</param>
    /// <param name="x2">x2</param>
    /// <param name="y2">y2</param>
    /// <returns></returns>
    public static float getAbsDist(float x1, float y1, float x2, float y2)
    {
        return Mathf.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
    }
    /// <summary>
    /// return the mouse pos in the scene
    /// </summary>
    /// <returns>mousepos stored in a vector2</returns>
    public static Vector2 getMousePos()
    {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            return hit.point;
        }
        return Vector2.zero;
    }
}
