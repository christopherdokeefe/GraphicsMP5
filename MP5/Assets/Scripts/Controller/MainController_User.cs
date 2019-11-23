using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public partial class MainController : MonoBehaviour
{
    public GameObject AxisFrame;
    GameObject selectedObject;
    GameObject selectedAxis;

    RaycastHit hit;
    Ray ray;

    Color storedVertexColor;
    Color storedAxisFrameColor;

    Color vertexHighlightColor;
    Color axisFrameHighlightColor;

    Vector3 delta = Vector3.zero;
    Vector3 mouseDownPos = Vector3.zero;

    bool verticesDisplayed = false;

    // Checks if a vertex has been selected or an axis is being dragged
    // And allows the user to manipulate the vertex position by dragging the axis
    void checkObjectSelection()
    {
        if (EventSystem.current.IsPointerOverGameObject()) {return;}  // Prevents selecting an object that is behind a UI element

        // All selects require the user to hold LeftControl and not be holding LeftAlt (camera controls)
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftAlt))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // Raycast
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.name.StartsWith("Vertex")) // Check if hit vertex
            {
                selectVertex(); 
            }
            else if (Physics.Raycast(ray, out hit) && hit.transform.parent.gameObject.name == "AxisFrame") // Check if hit AxisFrame
            {
                selectAxisFrame();
            }
            // If neither vertex nor axis frame is hit with raycast, deselect vertex and hide the Axis Frame
            else if (selectedObject != null)  
            {
                selectedObject.GetComponent<Renderer>().material.color = storedVertexColor;
                selectedObject = null;
                AxisFrame.SetActive(false);
            }
        }
        // If user is holding the mouse button and an axis is selected, allow them to move it
        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftAlt))
        {
            if (selectedAxis != null)
            {
                controlAxis();
            }
        }
        // If user isn't holding the mouse button, deselect the axis
        else if (selectedAxis != null)
        {
            selectedAxis.GetComponent<Renderer>().material.color = storedAxisFrameColor;
            selectedAxis = null;
        }
    }
    
    // Behavior for when a vertex is selected
    void selectVertex()
    {
        // If a vertex is already selected, reset that vertex's color
        if (selectedObject != null)  
        {
            selectedObject.GetComponent<Renderer>().material.color = storedVertexColor;
        }
        selectedObject = hit.transform.gameObject;  // Set selected object
        storedVertexColor = selectedObject.GetComponent<Renderer>().material.color;     // Store its color for when it gets deselected
        selectedObject.GetComponent<Renderer>().material.color = vertexHighlightColor;  // Change its color to the highlighted vertex color
        AxisFrame.transform.position = selectedObject.transform.position;
        AxisFrame.SetActive(true);
    }
    
    // Behavior for when an axis frame is selected
    // Left mouse button must be held down
    void selectAxisFrame()
    {
        delta = Vector3.zero;
        mouseDownPos = Input.mousePosition;
        
        selectedAxis = hit.transform.gameObject;  // Set selected axis
        storedAxisFrameColor = hit.transform.gameObject.GetComponent<Renderer>().material.color;    // Store axis color for when it gets deselected
        hit.transform.gameObject.GetComponent<Renderer>().material.color = axisFrameHighlightColor; // Change its color to the highlighted axis color
    }

    // Allows the user to control the axis position by moving the mouse
    void controlAxis()
    {
        delta = Input.mousePosition - mouseDownPos;  // Change in mouse position
        mouseDownPos = Input.mousePosition;          // Update mouse position
        if (selectedAxis.name == "X")
        {
            moveX();
        }
        else if (selectedAxis.name == "Y")
        {
            moveY();
        }
        else if (selectedAxis.name == "Z")
        {
            moveZ();
        }
    }
    // Changes the vertex's x position based on how much the user moves the mouse in the x direction
    void moveX()
    {
        selectedObject.transform.position = new Vector3(selectedObject.transform.position.x + (delta.x / 50), 
                                                selectedObject.transform.position.y, selectedObject.transform.position.z);
        AxisFrame.transform.position = selectedObject.transform.position;
    }
    // Changes the vertex's y position based on how much the user moves the mouse in the y direction
    void moveY()
    {
        selectedObject.transform.position = new Vector3(selectedObject.transform.position.x, 
                                                selectedObject.transform.position.y + (delta.y / 50), selectedObject.transform.position.z);
        AxisFrame.transform.position = selectedObject.transform.position;
    }
    // // Changes the vertex's z position based on how much the user moves the mouse in the x direction, since there is no mouse z direction
    void moveZ()
    {
        selectedObject.transform.position = new Vector3(selectedObject.transform.position.x, 
                                                selectedObject.transform.position.y, selectedObject.transform.position.z  - (delta.x / 50));
        AxisFrame.transform.position = selectedObject.transform.position;
    }

    void checkVertexDisplay()
    {
        if (Input.GetKey(KeyCode.LeftControl) && verticesDisplayed == false)
        {
            for (int i = 0; i < QuadNxM.transform.childCount; i++)
            {
                QuadNxM.transform.GetChild(i).gameObject.SetActive(true);
            }
            verticesDisplayed = true;
        }
        if (!Input.GetKey(KeyCode.LeftControl) && selectedObject == null)
        {
            for (int i = 0; i < QuadNxM.transform.childCount; i++)
            {
                QuadNxM.transform.GetChild(i).gameObject.SetActive(false);
            }
            verticesDisplayed = false;
            AxisFrame.SetActive(false);
        }
    }
}
