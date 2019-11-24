using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MainController : MonoBehaviour
{
    // UNKNOWN PROBLEM WITH GETTING MyMesh COMPONENT
    // INITIALIZING AND UPDATING MESH IN MyMesh FOR NOW
    public GameObject QuadNxM;
    public GameObject Cylinder;
    private MyMeshNxM myMeshNxM;
    private MyMeshCylinder myMeshCylinder;

    public SliderWithEcho quadVertexPerRowSlider;
    public SliderWithEcho quadVertexPerColumnSlider;

    public SliderWithEcho cylinderVertexPerRowSlider;
    public SliderWithEcho cylinderVertexPerColumnSlider;
    public SliderWithEcho cylinderRotationSlider;
    // Start is called before the first frame update
    void Start()
    {
        myMeshNxM = QuadNxM.GetComponent<MyMeshNxM>();
        myMeshNxM.InitializeMesh((int) quadVertexPerRowSlider.GetSliderValue(), (int) quadVertexPerColumnSlider.GetSliderValue());

        myMeshCylinder = Cylinder.GetComponent<MyMeshCylinder>();
        myMeshCylinder.InitializeMesh((int)cylinderVertexPerRowSlider.GetSliderValue(), 
                                      (int)cylinderVertexPerColumnSlider.GetSliderValue(),
                                      cylinderRotationSlider.GetSliderValue());


        quadVertexPerRowSlider.TheSlider.onValueChanged.AddListener(resizeQuadMesh);
        quadVertexPerColumnSlider.TheSlider.onValueChanged.AddListener(resizeQuadMesh);

        cylinderVertexPerRowSlider.TheSlider.onValueChanged.AddListener(resizeCylinderMesh);
        cylinderVertexPerColumnSlider.TheSlider.onValueChanged.AddListener(resizeCylinderMesh);
        cylinderRotationSlider.TheSlider.onValueChanged.AddListener(updateCylinderRotation);

        vertexHighlightColor = new Color(0.5f, 0f, 0.3f, 0.5f);
        axisFrameHighlightColor = new Color(0.8f, 0.8f, 0.1f, 0.5f);
        AxisFrame.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        myMeshNxM.UpdateMesh();
        myMeshCylinder.UpdateMesh();

        checkObjectSelection();  // Select any vertexObject that has been clicked
        checkVertexDisplay(QuadNxM);
        checkVertexDisplay(Cylinder);
    }

    void resizeQuadMesh(float v)
    {
        int vertexRows = (int)quadVertexPerRowSlider.GetSliderValue();  // Change value to an int for InitializeMesh to use
        int vertexColumns = (int)quadVertexPerColumnSlider.GetSliderValue();
        myMeshNxM.InitializeMesh(vertexColumns, vertexRows);
    }

    void resizeCylinderMesh(float v)
    {
        int vertexRows = (int)cylinderVertexPerRowSlider.GetSliderValue();  // Change value to an int for InitializeMesh to use
        int vertexColumns = (int)cylinderVertexPerColumnSlider.GetSliderValue();
        float theta = cylinderRotationSlider.GetSliderValue();
        myMeshCylinder.InitializeMesh(vertexColumns, vertexRows, theta);
    }

    void updateCylinderRotation(float v)
    {
        int vertexRows = (int)cylinderVertexPerRowSlider.GetSliderValue();  // Change value to an int for InitializeMesh to use
        int vertexColumns = (int)cylinderVertexPerColumnSlider.GetSliderValue();
        float theta = cylinderRotationSlider.GetSliderValue();
        myMeshCylinder.UpdateRotation(theta, vertexColumns, vertexRows);
    }
}
