using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    // UNKNOWN PROBLEM WITH GETTING MyMesh COMPONENT
    // INITIALIZING AND UPDATING MESH IN MyMesh FOR NOW
    public GameObject Quad;
    private MyMesh myMesh;

    public SliderWithEcho vertexSlider;
    // Start is called before the first frame update
    void Start()
    {
        
        myMesh = Quad.GetComponent<MyMesh>();
        myMesh.InitializeMesh((int) vertexSlider.GetSliderValue());
        vertexSlider.TheSlider.onValueChanged.AddListener(resizeMesh);
        
    }

    // Update is called once per frame
    void Update()
    {
        myMesh.UpdateMesh();
    }

    void resizeMesh(float v)
    {
        int vertices = (int) v; // Change value to an int for InitializeMesh to use
        myMesh.InitializeMesh(vertices);
    }
}
