using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private float cameraSpeed = 20;
    private int screenEdgeOffset = 10;
    private float minZoom = 60f;
    private float maxZoom = 173f;
    private float ZoomSens = 15f;
    void Update()
    {
        //movement (horizontal, vertical)
        if(Input.mousePosition.x <= screenEdgeOffset || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= new Vector3(cameraSpeed * Time.deltaTime, 0);
        }
        if (Input.mousePosition.x >= Screen.width - screenEdgeOffset || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(cameraSpeed * Time.deltaTime, 0);
        }
        if (Input.mousePosition.y >= Screen.height - screenEdgeOffset || Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0,cameraSpeed * Time.deltaTime);
        }
        if (Input.mousePosition.y <= screenEdgeOffset || Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= new Vector3(0,cameraSpeed * Time.deltaTime);
        }

        //zooming
        float size = Camera.main.fieldOfView;
        size -= Input.GetAxis("Mouse ScrollWheel") * ZoomSens;
        size = Mathf.Clamp(size, minZoom, maxZoom);
        Camera.main.fieldOfView = size;

    }
}
