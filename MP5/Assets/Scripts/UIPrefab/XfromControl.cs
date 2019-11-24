using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XfromControl : MonoBehaviour {
    public Toggle T, R, S;
    public SliderWithEcho X, Y, Z;
    public Text ObjectName;

    private GameObject mSelected;
    private Vector3 mPreviousSliderValues = Vector3.zero;

    // variables that track transform values
    private float translationX;
    private float translationY;
    private float scaleX = 1;
    private float scaleY = 1;
    private float rotationZ;

    Mesh myMesh;
    private Vector2[] originalUV;

    // Use this for initialization
    void Start () {
        T.onValueChanged.AddListener(SetToTranslation);
        R.onValueChanged.AddListener(SetToRotation);
        S.onValueChanged.AddListener(SetToScaling);
        X.SetSliderListener(XValueChanged);
        Y.SetSliderListener(YValueChanged);
        Z.SetSliderListener(ZValueChanged);

        T.isOn = true;
        R.isOn = false;
        S.isOn = false;
        SetToTranslation(true);
        mSelected = GameObject.Find("QuadNxM");
        ObjectName.text = "Selected: Texture";
        myMesh = mSelected.GetComponent<MeshFilter>().mesh;
        originalUV = myMesh.uv;
    }

    // checks to see if the amount of vertices have changed to keep the transform consistent
    void Update()
    {
        if (myMesh != null && originalUV.Length != myMesh.uv.Length)
        {
            originalUV = myMesh.uv;
            TransformTexture();
        }
    }
	
    //---------------------------------------------------------------------------------
    // Initialize slider bars to specific function
    void SetToTranslation(bool v)
    {
        Vector3 p = ReadObjectXfrom();
        mPreviousSliderValues = p;
        X.InitSliderRange(-4, 4, p.x);
        Y.InitSliderRange(-4, 4, p.y);
        Z.InitSliderRange(0, 0, 0);
    }

    void SetToScaling(bool v)
    {
        Vector3 s = ReadObjectXfrom();
        mPreviousSliderValues = s;
        X.InitSliderRange(0.1f, 10, s.x);
        Y.InitSliderRange(0.1f, 10, s.y);
        Z.InitSliderRange(0, 0, 0);
    }

    void SetToRotation(bool v)
    {
        Vector3 r = ReadObjectXfrom();
        mPreviousSliderValues = r;
        X.InitSliderRange(0, 0, 0);
        Y.InitSliderRange(0, 0, 0);
        Z.InitSliderRange(-180, 180, r.z);
        mPreviousSliderValues = r;
    }
    //---------------------------------------------------------------------------------

    //---------------------------------------------------------------------------------
    // resopond to sldier bar value changes
    void XValueChanged(float v)
    {
        Vector3 p = ReadObjectXfrom();
        p.x = v;
        mPreviousSliderValues.x = v;
        UISetObjectXform(ref p);
    }
    
    void YValueChanged(float v)
    {
        Vector3 p = ReadObjectXfrom();
        p.y = v;
        mPreviousSliderValues.y = v;
        UISetObjectXform(ref p);
    }

    void ZValueChanged(float v)
    {
        Vector3 p = ReadObjectXfrom();
        p.z = v;
        mPreviousSliderValues.z = v;
        UISetObjectXform(ref p);
    }
    //---------------------------------------------------------------------------------

    public void ObjectSetUI()
    {
        Vector3 p = ReadObjectXfrom();
        X.SetSliderValue(p.x);  // do not need to call back for this comes from the object
        Y.SetSliderValue(p.y);
        Z.SetSliderValue(p.z);
    }

    // retrieves the correct slider values based on what toggle is active
    private Vector3 ReadObjectXfrom()
    {
        Vector3 p;

        if (T.isOn)
        {
            if (mSelected != null)
                p = new Vector3(translationX, translationY, 0);
            else
                p = Vector3.zero;
        }
        else if (S.isOn)
        {
            if (mSelected != null)
                p = new Vector3(scaleX, scaleY, 0);
            else
                p = Vector3.one;
        }
        else
        {
            p = Vector3.zero;
        }
        return p;
    }

    // changes the transform values based on which toggle is active
    private void UISetObjectXform(ref Vector3 p)
    {
        if (mSelected == null)
            return;

        if (T.isOn)
        {
            translationX = p.x;
            translationY = p.y;
        }
        else if (S.isOn)
        {
            if (p.x > 0)
                scaleX = p.x;
            if (p.y > 0)
                scaleY = p.y;
        }
        else
        {
            rotationZ = p.z;
        }
        TransformTexture();
    }

    // Creates a 3x3Matrix to calculate get new UV values from the original values
    private void TransformTexture()
    {
        Matrix3x3 TRS = Matrix3x3Helpers.CreateTRS(new Vector2(translationX, translationY), rotationZ, new Vector2(scaleX, scaleY));

        if (originalUV.Length != myMesh.uv.Length)
        {
            originalUV = myMesh.uv;
        }
        Vector2[] uv = new Vector2[originalUV.Length];
        for (int i = 0; i < uv.Length; i++)
        {
            uv[i] = TRS * originalUV[i];
        }
        myMesh.uv = uv;
    }
}