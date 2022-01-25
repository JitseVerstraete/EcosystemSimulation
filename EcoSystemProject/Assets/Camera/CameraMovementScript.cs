using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        m_WorldSize = SimulationScript.Instance.GetWorldSize();
        m_MaxZoomSize = m_WorldSize.y;
        Camera.main.orthographicSize = m_MaxZoomSize;
    }

    // Update is called once per frame
    void Update()
    {
        //ZOOMING
        float newCameraZoom = Camera.main.orthographicSize - m_ZoomSpeed * Input.mouseScrollDelta.y;

        Camera.main.orthographicSize = Mathf.Clamp(newCameraZoom, m_MinZoomSize, m_MaxZoomSize);



        //MOVING

        //get camera pos and size
        m_HalfHeight = Camera.main.orthographicSize;
        m_Halfwidth = m_HalfHeight * Camera.main.aspect;

        //camera movement
        if (Input.GetMouseButtonDown(1))
            m_MouseDown = true;
        else if (Input.GetMouseButtonUp(1))
            m_MouseDown = false;

        m_CurrentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);




        Vector3 camPos = Camera.main.transform.position;
        float xMin = -m_WorldSize.x + m_Halfwidth;
        float xMax = m_WorldSize.x - m_Halfwidth;
        float yMin = -m_WorldSize.y + m_HalfHeight;
        float yMax = m_WorldSize.y - m_HalfHeight;

        float newX = camPos.x;
        float newY = camPos.y;

        if (m_MouseDown)
        {
            newX += m_PreviousMousePosition.x - m_CurrentMousePosition.x;
            newY += m_PreviousMousePosition.y - m_CurrentMousePosition.y;

        }

        newX = Mathf.Clamp(newX, xMin, xMax);
        newY = Mathf.Clamp(newY, yMin, yMax);
        Camera.main.transform.position = new Vector3(newX, newY, 0f);



        //set the current mouse position as previous mouse position
        m_PreviousMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    //DATA MEMEBERS

    public float m_MinZoomSize = 5f;
    private float m_MaxZoomSize = 0f;

    //private members
    private Vector3 m_PreviousMousePosition;
    private Vector3 m_CurrentMousePosition;
    private float m_ZoomSpeed = 1f;
    private bool m_MouseDown = false;

    private float m_HalfHeight;
    private float m_Halfwidth;

    private Vector2 m_WorldSize;
}
