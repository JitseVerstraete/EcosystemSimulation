using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //camera movement
        if(Input.GetMouseButtonDown(1))
            m_MouseDown = true;
        else if(Input.GetMouseButtonUp(1))
            m_MouseDown = false;

        m_CurrentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
       

        if (m_MouseDown)
        {
            Camera.main.transform.position +=  m_PreviousMousePosition - m_CurrentMousePosition;
        }

       m_PreviousMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);



        //zooming
        float newCameraZoom = Camera.main.orthographicSize - m_ZoomSpeed * Input.mouseScrollDelta.y;
       
        Camera.main.orthographicSize = Mathf.Clamp(newCameraZoom, 1f, 10f); ;
        print(Camera.main.orthographicSize);



    }

    private Vector3 m_PreviousMousePosition;
    private Vector3 m_CurrentMousePosition;
    private float m_ZoomSpeed =1f;
    private bool m_MouseDown = false;
}
