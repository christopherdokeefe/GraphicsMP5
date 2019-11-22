using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyMeshNxM : MonoBehaviour 
{
    // For passing value into ComputeNormals in UpdateMesh()
    int rows;
    int columns;
    // n by m number of vertices - 3x2 mesh = 4x3 vertices
    // Vn: number of vertices in a row
    // Vm: number of vertices in a column
	public void InitializeMesh(int Vn, int Vm) 
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;   // get the mesh component
        theMesh.Clear();    // delete whatever is there!!

        rows = Vn;  
        columns = Vm;

        Vector3[] v = new Vector3[Vn * Vm];   // n by n mesh needs (n + 1) by (n + 1) vertices
        Vector3[] n = new Vector3[Vn * Vm];   // Vertex normals - Must be the same size as v

        // Number of triangles: (n - 1) by (n - 1) mesh and 
        // 2 triangles on each mesh-unit and 3 indexes per triangle
        int[] t = new int[(Vn - 1) * (Vm - 1) * 2 * 3];         

        // Initialize the vertices with x and z from -1 to 1                                        v[12] v[13] v[14] v[15]
        // Note: Calculates from 0 to 2 for convenience, then subtracts 1 in inner for loop         v[8]  v[9]  v[10] v[11]
        // When looking at Unity x-z plane, Vertex0 is at the bottom left, (-1, 0, -1)              v[4]  v[5]  v[6]  v[7]
        // Vertices go from Left to Right, Bottom to Top                                            v[0]  v[1]  v[2]  v[3]
        float intervalN = 2f / (Vn - 1);
        float intervalM = 2f / (Vm - 1);

        for (int i = 0; i < Vm; i++)
        {
            for (int j = 0; j < Vn; j++)
            {
                // (i * Vn) moves the vertex index to the next row
                //  j moves the vertex's column position, i.e. it moves it along the row
                // Example: if Vn = 4 (4 vertices per row), i = 0 loop sets v[0], v[1], v[2], v[3],  i = 1 sets v[4], v[5], v[6], v[7], and so on
                // (j * interval) - 1 spaces vertices evenly from -1 to 1 on the x axis, and same for (i * interval) - 1
                v[(i * Vn) + j] = new Vector3((j * intervalN) - 1, 1, (i * intervalM) - 1);
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
                t[i + (j * 6)]     = (0) + j + ((i / triangleIndicesPerRow) * Vn);
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

        /* KELVIN SUNG EXAMPLE ONLY FOR REFERENCE, CAN DELETE
        Mesh theMesh = GetComponent<MeshFilter>().mesh;   // get the mesh component
        theMesh.Clear();    // delete whatever is there!!

        Vector3[] v = new Vector3[9];   // 2x2 mesh needs 3x3 vertices
        int[] t = new int[8*3];         // Number of triangles: 2x2 mesh and 2x triangles on each mesh-unit
        Vector3[] n = new Vector3[9];   // MUST be the same as number of vertices

        v[0] = new Vector3(-1, 0, -1);
        v[1] = new Vector3( 0, 0, -1);
        v[2] = new Vector3( 1, 0, -1);

        v[3] = new Vector3(-1, 0, 0);
        v[4] = new Vector3( 0, 0, 0);
        v[5] = new Vector3( 1, 0, 0);

        v[6] = new Vector3(-1, 0, 1);
        v[7] = new Vector3( 0, 0, 1);
        v[8] = new Vector3( 1, 0, 1);

        n[0] = new Vector3(0, 1, 0);
        n[1] = new Vector3(0, 1, 0);
        n[2] = new Vector3(0, 1, 0);
        n[3] = new Vector3(0, 1, 0);
        n[4] = new Vector3(0, 1, 0);
        n[5] = new Vector3(0, 1, 0);
        n[6] = new Vector3(0, 1, 0);
        n[7] = new Vector3(0, 1, 0);
        n[8] = new Vector3(0, 1, 0);

        t[0] = 0; t[1] = 3; t[2] = 4;  // 0th triangle
        t[3] = 0; t[4] = 4; t[5] = 1;  // 1st triangle


        t[6] = 1; t[7] = 4; t[8] = 5;  // 2nd triangle
        t[9] = 1; t[10] = 5; t[11] = 2;  // 3rd triangle





        t[12] = 3; t[13] = 6; t[14] = 7;  // 4th triangle
        t[15] = 3; t[16] = 7; t[17] = 4;  // 5th triangle


        t[18] = 4; t[19] = 7; t[20] = 8;  // 6th triangle
        t[21] = 4; t[22] = 8; t[23] = 5;  // 7th triangle

        theMesh.vertices = v; //  new Vector3[3];
        theMesh.triangles = t; //  new int[3];
        theMesh.normals = n;
        */

        // Uses MyMesh_Controllers partial class to create all the spheres to control the vertices and their normals
        createVertexObjects(v);
        InitNormals(v, n);
    }

    // Update is called once per frame
    public void UpdateMesh()
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        Vector3[] v = theMesh.vertices;
        Vector3[] n = theMesh.normals;
        int [] t = theMesh.triangles;

        for (int i = 0; i < vertexObjects.Length; i++)
        {
		    v[i] = vertexObjects[i].transform.localPosition;
        }

        ComputeNormals(v, n, t, rows, columns);

        theMesh.vertices = v;
        theMesh.normals = n;
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