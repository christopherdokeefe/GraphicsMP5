using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManipulation : MonoBehaviour
{
    public GameObject LookAtPosition;
    Vector3 delta = Vector3.zero;
    Vector3 mouseDownPos = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        updateLookAt();
        // Zoom in and out with the camera
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            ProcesssZoom(Input.GetAxis("Mouse ScrollWheel") * -5f);
        }

        // Track the camera (Horizontal and Vertical movement)
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButtonDown(1))
        {
            mouseDownPos = Input.mousePosition;
            delta = Vector3.zero;
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(1))
        {
            delta = mouseDownPos - Input.mousePosition;
            mouseDownPos = Input.mousePosition;
            ProcessTrack(delta / 60f);
        }

        // Tumble the camera (rotating around the LookAtPosition)
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButtonDown(0))
        {
            mouseDownPos = Input.mousePosition;
            delta = Vector3.zero;
        }
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButton(0))
        {
            delta = mouseDownPos - Input.mousePosition;
            mouseDownPos = Input.mousePosition;
            ProcessTumble(delta / 5f);
        }
    }

    void updateLookAt()
    {
        Vector3 dir = (LookAtPosition.transform.localPosition - transform.localPosition).normalized;
        transform.forward = dir;
    }

    public void ProcesssZoom(float delta)
    {
        Vector3 v = LookAtPosition.transform.localPosition - transform.localPosition;
        float dist = v.magnitude;
        dist += delta;
        transform.localPosition = LookAtPosition.transform.localPosition - dist * v.normalized;
    }

    public void ProcessTrack(Vector3 delta)
    {
        Vector3 trackX = transform.right * delta.x;
        Vector3 trackY = transform.up * delta.y;
        transform.position += trackX + trackY;
        LookAtPosition.transform.position += trackX + trackY;
    }

    public void ProcessTumble(Vector3 delta)
    {
        Vector3 v = (LookAtPosition.transform.localPosition - transform.localPosition);
        float dist = v.magnitude;

        // delta.y represents rotating around x-axis
        // -delta.x represents rotating around y-axis in proper direction
        transform.Rotate(delta.y, -delta.x, 0f);

        if (transform.eulerAngles.x > 80 && transform.eulerAngles.x <= 180) 
        { 
            transform.eulerAngles = new Vector3(80, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        else if (transform.eulerAngles.x > 180 && transform.eulerAngles.x < 280)
        {
            transform.eulerAngles = new Vector3(-80, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        Vector3 dir = transform.forward;
        transform.position = LookAtPosition.transform.position - (dir * dist);
    }
}
