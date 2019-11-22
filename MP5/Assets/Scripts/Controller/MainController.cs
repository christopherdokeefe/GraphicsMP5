﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MainController : MonoBehaviour
{
    // UNKNOWN PROBLEM WITH GETTING MyMesh COMPONENT
    // INITIALIZING AND UPDATING MESH IN MyMesh FOR NOW
    public GameObject Quad;
    private MyMesh myMesh;

    public GameObject QuadNxM;
    private MyMeshNxM myMeshNxM;

    public SliderWithEcho vertexPerRowSlider;
    public SliderWithEcho vertexPerColumnSlider;
    // Start is called before the first frame update
    void Start()
    {
        myMesh = Quad.GetComponent<MyMesh>();
        myMesh.InitializeMesh((int) vertexPerRowSlider.GetSliderValue());

        myMeshNxM = QuadNxM.GetComponent<MyMeshNxM>();
        myMeshNxM.InitializeMesh((int) vertexPerRowSlider.GetSliderValue(), (int) vertexPerColumnSlider.GetSliderValue());

        vertexPerRowSlider.TheSlider.onValueChanged.AddListener(resizeMesh);
        vertexPerColumnSlider.TheSlider.onValueChanged.AddListener(resizeMesh);

        vertexHighlightColor = new Color(0.5f, 0f, 0.3f, 0.5f);
        axisFrameHighlightColor = new Color(0.8f, 0.8f, 0.1f, 0.5f);
        AxisFrame.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        myMesh.UpdateMesh();
        myMeshNxM.UpdateMesh();
        checkObjectSelection();  // Select any vertexObject that has been clicked
    }

    void resizeMesh(float v)
    {
        int vertexRows = (int) vertexPerRowSlider.GetSliderValue();  // Change value to an int for InitializeMesh to use
        int vertexColumns = (int) vertexPerColumnSlider.GetSliderValue();
        myMesh.InitializeMesh(vertexRows);
        myMeshNxM.InitializeMesh(vertexRows, vertexColumns);
    }
}
