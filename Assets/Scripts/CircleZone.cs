using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleZone : MonoBehaviour {
    MeshRenderer mr;
    MeshFilter mf;
    Mesh mesh;
    static Vector3 _pos;
    static Vector3 _planetPos;
    static float _PlanetRadius;
    static Material _DarkMat;
    // Use this for initialization
    void Start () {
        mr = GetComponent<MeshRenderer>();
        mf = GetComponent<MeshFilter>();
	}

    public static GameObject CreateNewZone(Transform _ParentPlanet,Vector3 Pos,Material DarkMat)
    {
        _PlanetRadius = Vector3.Distance(Pos, _ParentPlanet.position);
        _planetPos = _ParentPlanet.position;
        _pos = Pos;
        _DarkMat = DarkMat;
        //CreateObject
        GameObject TestCircle = new GameObject();
        TestCircle.name = "zone";
        MeshRenderer mr = TestCircle.AddComponent<MeshRenderer>();
        MeshFilter mf = TestCircle.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();
        /////
        mesh = CreateShapeOnplanet.Create(_pos, _PlanetRadius, _planetPos, 1);
        ////
        mf.mesh = mesh;
        mr.material = _DarkMat;

        TestCircle.transform.position = Pos;
        TestCircle.transform.parent = _ParentPlanet;
        TestCircle.AddComponent<CircleZone>();
        return TestCircle;
    }
    public void UpdateCircleZone(float Raduis)
    {
        mesh = new Mesh();
        mesh = CreateShapeOnplanet.Create(_pos, _PlanetRadius, _planetPos,Raduis);
        mesh.RecalculateNormals();

        mf.mesh = mesh;
        mr.material= _DarkMat;
    }
    
}
