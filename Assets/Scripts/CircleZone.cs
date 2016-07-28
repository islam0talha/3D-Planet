using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleZone : MonoBehaviour {
    MeshRenderer mr;
    MeshFilter mf;
    Mesh mesh;
    static float Raduis = 2.5f;
    // Use this for initialization
    void Start () {
        mr = GetComponent<MeshRenderer>();
        mf = GetComponent<MeshFilter>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public static GameObject CreateNewZone(Transform _ParentPlanet,Vector3 Pos)
    {
        //CreateObject
        GameObject TestCircle = new GameObject();
        TestCircle.name = "zone";
        MeshRenderer mr = TestCircle.AddComponent<MeshRenderer>();
        MeshFilter mf = TestCircle.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();
        /////
        mesh = CreateShape.Create(1f,Vector3.Distance(_ParentPlanet.position,Pos));
        ////
        mesh.RecalculateNormals();
        mf.mesh = mesh;
        mr.material.color = Color.white;

        TestCircle.transform.position = _ParentPlanet.transform.position;
        TestCircle.transform.LookAt(Pos);
        TestCircle.transform.parent = _ParentPlanet;
        TestCircle.AddComponent<CircleZone>();
        return TestCircle;
    }
    public void UpdateCircle(float Raduis)
    {
        mesh = new Mesh();
        mesh = CreateShape.Create(Raduis,0);
        mesh.RecalculateNormals();

        mf.mesh = mesh;
        mr.material.color = Color.white;
    }
    
}
