using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyMeshCylinder : MonoBehaviour
{
    GameObject[] vertexObjects;  // Sphere objects representing the vertices of the mesh
    float sphereScale = 0.2f;    // Scale of the vertexObjects to make the mesh easier to manipulate
    public Material GhostVertexMaterial;

    // Creates a sphere at each vertex of the mesh
    // Allows the mesh to later be manipulated by moving the spheres
    void createVertexObjects(Vector3[] v, int Vn, int Vm)
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

        // Name all vertices
        // Start of the row: Vertex[i], only vertices whose name starts with "Vertex" can be clicked and manipulated (see MainController_User)
        // Rest of the row: (Ghost) Vertex, cannot be clicked and manipulated because it does not start with "Vertex" (see MainController_User)
        for (int i = 0; i < Vm; i++)
        {
            vertexObjects[i * Vn] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            vertexObjects[i * Vn].name = "Vertex" + i.ToString();
            for(int j = 1; j < Vn; j++)
            {
                vertexObjects[(i * Vn) + j] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                vertexObjects[(i * Vn) + j].name = "(Ghost) Vertex" + i.ToString();
                vertexObjects[(i * Vn) + j].GetComponent<Renderer>().material = GhostVertexMaterial;
            }
        }

        // Position all vertex objects at the corresponding vertex
        for (int i = 0; i < v.Length; i++)
        {
            vertexObjects[i].transform.position = v[i];
            vertexObjects[i].transform.localScale *= sphereScale;

            vertexObjects[i].transform.parent = this.transform;  // vertexObjects become children of the mesh object
        }
    }
}
