using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderSorter : MonoBehaviour
{
    private int renderoffset = 5000;
    private SpriteRenderer sr;
    [SerializeField]
    private float offset = 0;
    [SerializeField]
    private bool once = true;
    public void Awake()
    {
        sr = transform.gameObject.GetComponentInChildren<SpriteRenderer>();
    }
    /// <summary>
    /// checks the positon of the object and assignes its render layer, im using this because unitys built in system is not working for me
    /// </summary>
    public void LateUpdate()
    {
        sr.sortingOrder = (int)((renderoffset - transform.position.y - offset) * 10);
        if (once)
        {
            Destroy(this);
        }
    }
}
