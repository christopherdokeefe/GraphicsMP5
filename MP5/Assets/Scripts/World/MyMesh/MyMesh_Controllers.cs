using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyMesh : MonoBehaviour
{
    GameObject[] vertexObjects;  // Sphere objects representing the vertices of the mesh
    float sphereScale = 0.2f;    // Scale of the vertexObjects to make the mesh easier to manipulate

    // Creates a sphere at each vertex of the mesh
    // Allows the mesh to later be manipulated by moving the spheres
    void createVertexObjects(Vector3[] v)
    {
        // Delete old vertex objects
        if (vertexObjects != null)
        {
            foreach (GameObject obj in vertexObjects)
            {
                Destroy(obj);
            }
        }
        vertexObjects = new GameObject[v.Length];   

        for (int i = 0; i < v.Length; i++)
        {
            vertexObjects[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            vertexObjects[i].name = "Vertex" + i.ToString();

            vertexObjects[i].transform.position = v[i];
            vertexObjects[i].transform.localScale *= sphereScale;

            vertexObjects[i].transform.parent = this.transform;  // vertexObjects become children of the mesh object
        }
    }
}
