using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MakeCircle : MonoBehaviour {

	// Use this for initialization
	void Start () {
        MakeaCircle(10);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void MakeaCircle(int numOfPoints)
    {
        float angleStep = 360.0f / (float)numOfPoints;
        List<Vector3> vertexList = new List<Vector3>();
        List<int> triangleList = new List<int>();
        Quaternion quaternion = Quaternion.Euler(0.0f, 0.0f, angleStep);
        // Make first triangle.
        vertexList.Add(new Vector3(0.0f, 0.0f, 0.0f));  // 1. Circle center.
        vertexList.Add(new Vector3(0.0f, 0.5f, 0.0f));  // 2. First vertex on circle outline (radius = 0.5f)
        vertexList.Add(quaternion * vertexList[1]);     // 3. First vertex on circle outline rotated by angle)
                                                        // Add triangle indices.
        triangleList.Add(0);
        triangleList.Add(1);
        triangleList.Add(2);
        for (int i = 0; i < numOfPoints - 1; i++)
        {
            triangleList.Add(0);                      // Index of circle center.
            triangleList.Add(vertexList.Count - 1);
            triangleList.Add(vertexList.Count);
            vertexList.Add(quaternion * vertexList[vertexList.Count - 1]);
        }
        GameObject TestCircle = new GameObject();
        TestCircle.AddComponent<MeshRenderer>();
        TestCircle.AddComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mesh.vertices = vertexList.ToArray();
        mesh.triangles = triangleList.ToArray();
        //mesh.RecalculateNormals();
        MeshFilter mf = TestCircle.GetComponent<MeshFilter>();
        MeshRenderer mr = TestCircle.GetComponent<MeshRenderer>();
        mf.mesh = mesh;
        mr.material.color = Color.white;
        
    }
}
