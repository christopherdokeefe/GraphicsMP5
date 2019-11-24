using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyMeshCylinder : MonoBehaviour
{
    int columns;
    int rows;
    float height = 2f;
    float radius = 1f;
    float thetaInterval;

    // Vn: number of verticies around circumfrance
    // Vm: number of verticies on y-axis
    public void InitializeMesh(int Vn, int Vm, float theta)
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;   // get the mesh component
        theMesh.Clear();    // delete whatever is there!!

        columns = Vn;
        rows = Vm;

        Vector3[] v = new Vector3[Vn * Vm];   // n by n mesh needs (n + 1) by (n + 1) vertices
        Vector3[] n = new Vector3[Vn * Vm];   // Vertex normals - Must be the same size as v

        // Number of triangles: (n - 1) by (n - 1) mesh and 
        // 2 triangles on each mesh-unit and 3 indexes per triangle
        int[] t = new int[(Vn - 1) * (Vm - 1) * 2 * 3];

        // Initialize the vertices with x and z from -1 to 1                                        v[12] v[13] v[14] v[15]
        // Note: Calculates from 0 to 2 for convenience, then subtracts 1 in inner for loop         v[8]  v[9]  v[10] v[11]
        // When looking at Unity x-z plane, Vertex0 is at the bottom left, (-1, 0, -1)              v[4]  v[5]  v[6]  v[7]
        // Vertices go from Left to Right, Bottom to Top                                            v[0]  v[1]  v[2]  v[3]
        float intervalM = 2f / (Vm - 1);  // Distance between rows
        thetaInterval = theta / (Vn - 1);  // Angle between columns

        float r = radius;      // radius of the cylinder

        for (int i = 0; i < Vm; i++)
        {
            for (int j = 0; j < Vn; j++)
            {
                // Following line to change to something like: 
                //
                // (r * cos(theta), j * height, (r * sin(theta))
                // radius = r
                // theta = the value is an angle between 10 and 360 degrees divided by the Vm (rows)

                // v[(i * Vn) + j] = new Vector3((j * intervalN) - 1, 0, (i * intervalM) - 1) * scale;

                v[(i * Vn) + j] = new Vector3(radius * Mathf.Cos(thetaInterval * j * Mathf.Deg2Rad), ((i * intervalM) - 1) * height, 
                                              radius * Mathf.Sin(thetaInterval * j * Mathf.Deg2Rad));
                //r * Mathf.Cos(thetaInterval * i), 0, r * Mathf.Sin(thetaInterval * i)
            }
        }

        // Initialize the normals of each vertex
        for (int i = 0; i < v.Length; i++)
        {
            n[i] = new Vector3(0, 1, 0);
        }

        // if Vn = 4, this = 18 since there are 3 mesh units per row * 2 triangles per mesh unit * 3 indices per triangle)
        int triangleIndicesPerRow = (Vn - 1) * 2 * 3;

        // Initialize the triangles using the vertices' indexes
        // For loop explanation: For this explanation, assume Vn = 4 (4 vertices per row)
        // INDEX: int i is used to offset the indices for each row. i = 0 starts with the first row, i = 18 starts with the second
        // i is incremented by 18 each time in this example because it is a 3x3 mesh, so 3 mesh units per row (6 triangles total, 18 triangle indexes)
        // j is used to move the index between mesh units, which are 6 triangles each (that is why it is j * 6)
        // Then 0-5 is added to set each individual triangle index
        // VALUES: Each index is set to match the pattern of how triangles are drawn
        //         The first triangle takes v[0], v[n] (the one below v[0]), and v[n + 1] (right next to v[n])
        //         The second triangle takes v[0], v[n + 1], and v[1] (right next to v[0])
        //         Together, these form a mesh unit. j is incremented, and the same thing is done for each mesh unit in the row
        //         i is then incremented, and the same thing is done for each row
        //         Note: ((i / triangleIndecesPerRow) * Vn) is what offsets the rows to get the correct vertex
        //               Otherwise, all sets of triangles would be based on the first row instead of each row
        for (int i = 0; i < t.Length; i += triangleIndicesPerRow)  // Goes through each row of vertices
        {
            for (int j = 0; j < Vn - 1; j++)  // Goes through each mesh unit (2 triangles) in a row
            {
                // Creates first triangle of the mesh unit
                t[i + (j * 6)] = (0) + j + ((i / triangleIndicesPerRow) * Vn);
                t[i + (j * 6) + 1] = (Vn) + j + ((i / triangleIndicesPerRow) * Vn);
                t[i + (j * 6) + 2] = (Vn + 1) + j + ((i / triangleIndicesPerRow) * Vn);

                // Creates second triangle of the mesh unit
                t[i + (j * 6) + 3] = (0) + j + ((i / triangleIndicesPerRow) * Vn);
                t[i + (j * 6) + 4] = (Vn + 1) + j + ((i / triangleIndicesPerRow) * Vn);
                t[i + (j * 6) + 5] = (1) + j + ((i / triangleIndicesPerRow) * Vn);
            }
        }

        theMesh.vertices = v; //  new Vector3[3];
        theMesh.triangles = t; //  new int[3];
        theMesh.normals = n;

        // Uses MyMesh_Controllers partial class to create all the spheres to control the vertices and their normals
        createVertexObjects(v, Vn, Vm);
        InitNormals(v, n);
    }

    // Update is called once per frame
    public void UpdateMesh()
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        Vector3[] v = theMesh.vertices;
        Vector3[] n = theMesh.normals;
        int[] t = theMesh.triangles;

        int Vn = columns;
        int Vm = rows;

        if (vertexObjects != null)
        {
            for (int i = 0; i < Vm; i++)
            {
                v[i * Vn] = vertexObjects[i * Vn].transform.localPosition;  // Set vertex position to vertexObject's position
                float r = Mathf.Sqrt((v[i * Vn].x * v[i * Vn].x) + (v[i * Vn].z * v[i * Vn].z)); // Calculate new radius

                for (int j = 1; j < Vn; j++)
                {
                    // Calculate the position of each vertexObject in the same row so they all move outwards and inwards together
                    vertexObjects[(i * Vn) + j].transform.localPosition = new Vector3(r * Mathf.Cos(thetaInterval * j * Mathf.Deg2Rad),
                                     vertexObjects[i * Vn].transform.localPosition.y, r * Mathf.Sin(thetaInterval * j * Mathf.Deg2Rad));

                    v[i * Vn + j] = vertexObjects[i * Vn + j].transform.localPosition;  // Set vertex position to newly positioned vertex object
                }
            }
            ComputeNormals(v, n, t, rows, columns);

            theMesh.vertices = v;
            theMesh.normals = n;
        }
    }

    public void UpdateRotation(float theta, int Vn, int Vm)
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        Vector3[] v = theMesh.vertices;
        float[] currentRadiuses = new float[Vm];  // Distance from center of each movable vertex
        
        thetaInterval = theta / (Vn - 1);  // Angle between columns
        for (int i = 0; i < Vm; i++)
        {
            currentRadiuses[i] = Mathf.Sqrt((v[i * Vn].x * v[i * Vn].x) + (v[i * Vn].z * v[i * Vn].z)); // Calculate current radiuses
            for (int j = 1; j < Vn; j++)
            {
                // Following line to change to something like: 
                //
                // (r * cos(theta), j * height, (r * sin(theta))
                // radius = r
                // theta = the value is an angle between 10 and 360 degrees divided by the Vm (rows)

                // v[(i * Vn) + j] = new Vector3((j * intervalN) - 1, 0, (i * intervalM) - 1) * scale;

                v[(i * Vn) + j] = new Vector3(currentRadiuses[i] * Mathf.Cos(thetaInterval * j * Mathf.Deg2Rad), v[(i * Vn) + j].y, 
                                              currentRadiuses[i] * Mathf.Sin(thetaInterval * j * Mathf.Deg2Rad));
                //r * Mathf.Cos(thetaInterval * i), 0, r * Mathf.Sin(thetaInterval * i)
            }
        }
    }

    // Clear the mesh and delete all vertex objects
    public void DeleteMesh()
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        theMesh.Clear();

        if (vertexObjects != null)
        {
            foreach (GameObject obj in vertexObjects)
            {
                Destroy(obj);
            }
        }
    }
}


