using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyMeshNxM : MonoBehaviour {
    LineSegment[] normals;

    struct Triangle 
    {
        public int v1;
        public int v2;
        public int v3;
        public Vector3 normal;
    };

    void InitNormals(Vector3[] v, Vector3[] n)
    {
        // Destroy all existing normals so new ones can be created
        if (normals != null)
        {
            foreach (LineSegment normal in normals)
            {
                Destroy(normal.gameObject);
            }
        }
        normals = new LineSegment[v.Length];
        for (int i = 0; i < v.Length; i++)
        {
            GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            o.name = "Normal" + i.ToString();
            normals[i] = o.AddComponent<LineSegment>();
            normals[i].SetWidth(0.05f);
            normals[i].transform.SetParent(this.transform);
        }
        UpdateNormals(v, n);
    }

    void UpdateNormals(Vector3[] v, Vector3[] n)
    {
        for (int i = 0; i < v.Length; i++)
        {
            normals[i].SetEndPoints(v[i], v[i] + 1.0f * n[i]);
        }
    }

    // Calculates the normal of each triangle
    Vector3 FaceNormal(Vector3[] v, int i0, int i1, int i2)
    {
        Vector3 a = v[i1] - v[i0];
        Vector3 b = v[i2] - v[i0];
        return Vector3.Cross(a, b).normalized;
    }

    // Computes the normals of each vertex
    void ComputeNormals(Vector3[] v, Vector3[] n, int[] t, int Vn, int Vm)
    {
        Vector3[] triNormal = new Vector3[t.Length / 3];  // t has 3 indices per triangle, so divide by 3
        Triangle[] triangles = new Triangle[t.Length / 3];
        triangles[0].v1 = 1;

        for (int i = 0; i < triangles.Length; i++)
        {
            // triangles[0] = t[0], t[1], t[2]; triangles[1] = t[3], t[4], t[5]; triangles[i] = t[i * 3], t[(i * 3) + 1], t[(i * 3) + 2]
            triangles[i].v1 = t[i * 3];
            triangles[i].v2 = t[(i * 3) + 1];
            triangles[i].v3 = t[(i * 3) + 2];
            triangles[i].normal = FaceNormal(v, t[i * 3], t[(i * 3) + 1], t[(i * 3) + 2]);
        }

        foreach (Triangle tri in triangles)
        {
            n[tri.v1] += tri.normal;
            n[tri.v2] += tri.normal;
            n[tri.v3] += tri.normal;
        }
        for (int i = 0; i < n.Length; i++)
        {
            n[i] = n[i].normalized;
        }

        UpdateNormals(v, n);
    }
}