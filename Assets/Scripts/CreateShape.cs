using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CreateShapeOnplanet {
    private struct TriangleIndices
    {
        public int v1;
        public int v2;
        public int v3;

        public TriangleIndices(int v1, int v2, int v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }
    }

    // return index of point in the middle of p1 and p2
    private static int getMiddlePoint(int p1, int p2, ref List<Vector3> vertices, ref Dictionary<long, int> cache, float radius,Vector3 CenterPoint,Vector3 _ClickedPoint)
    {
        // first check if we have it already
        bool firstIsSmaller = p1 < p2;
        long smallerIndex = firstIsSmaller ? p1 : p2;
        long greaterIndex = firstIsSmaller ? p2 : p1;
        long key = (smallerIndex << 32) + greaterIndex;

        int ret;
        if (cache.TryGetValue(key, out ret))
        {
            return ret;
        }

        // not in cache, calculate it
        Vector3 point1 = vertices[p1];
        Vector3 point2 = vertices[p2];
        Vector3 middle = new Vector3
        (
            (point1.x + point2.x) / 2f,
            (point1.y + point2.y) / 2f,
            (point1.z + point2.z) / 2f 
        );

        // add vertex makes sure point is on unit sphere
        int i = vertices.Count;
        vertices.Add(((middle + _ClickedPoint) - CenterPoint).normalized * (radius+1) - _ClickedPoint);
        // store it, return index
        cache.Add(key, i);
        
        return i;
    }

    public static Mesh Create(Vector3 _Clickedpos,float PlanetRadius,Vector3 PlanetCenter, float wradius)
    {
        Vector3 PointPos;
        wradius = wradius * Mathf.Deg2Rad;
        float _radius; float _polar; float _elevation;
        float O_polar, O_elevation;
        float tempr = 0;
        Mesh mesh = new Mesh();
        mesh.Clear();

        List<Vector3> vertList = new List<Vector3>();
        Dictionary<long, int> middlePointIndexCache = new Dictionary<long, int>();
        int recursionLevel = 3;
        int numOfPoints =20;

        float angleStep = (360 / (float)numOfPoints)*Mathf.Deg2Rad;
        CartesianToSpherical(_Clickedpos, PlanetCenter, out _radius, out _polar, out _elevation);
        SphericalToCartesian(_radius+1, _polar, _elevation, out PointPos);

        vertList.Add(PointPos + (PlanetCenter - _Clickedpos));           //0

        
        O_polar = _polar;
        O_elevation = _elevation;
        
        for (int i = 0; i < numOfPoints; i++)
        {
            _elevation = O_elevation+ wradius*Mathf.Sin(tempr);
            _polar = O_polar + wradius*Mathf.Cos(tempr);
            tempr -= angleStep;

            SphericalToCartesian(PlanetRadius+1, _polar, _elevation, out PointPos);
            vertList.Add((PointPos+(PlanetCenter-_Clickedpos)));
        }

        List<TriangleIndices> faces = new List<TriangleIndices>();
        int FacesCount = 0;
        for (FacesCount = 0; FacesCount <= numOfPoints - 2; FacesCount++)
        {
            faces.Add(new TriangleIndices(0, FacesCount + 1, FacesCount + 2));
        }
        faces.Add(new TriangleIndices(0, numOfPoints, 1));
        //

        // refine triangles
        for (int i = 0; i < recursionLevel; i++)
        {
            List<TriangleIndices> faces2 = new List<TriangleIndices>();
            foreach (var tri in faces)
            {
                // replace triangle by 4 triangles
                int a = getMiddlePoint(tri.v1, tri.v2, ref vertList, ref middlePointIndexCache, PlanetRadius, PlanetCenter, _Clickedpos);
                int b = getMiddlePoint(tri.v2, tri.v3, ref vertList, ref middlePointIndexCache, PlanetRadius, PlanetCenter, _Clickedpos);
                int c = getMiddlePoint(tri.v3, tri.v1, ref vertList, ref middlePointIndexCache, PlanetRadius, PlanetCenter, _Clickedpos);

                faces2.Add(new TriangleIndices(tri.v1, a, c));
                faces2.Add(new TriangleIndices(tri.v2, b, a));
                faces2.Add(new TriangleIndices(tri.v3, c, b));
                faces2.Add(new TriangleIndices(a, b, c));
            }
            faces = faces2;
        }

        mesh.vertices = vertList.ToArray();

        List<int> triList = new List<int>();
        for (int i = 0; i < faces.Count; i++)
        {
            triList.Add(faces[i].v1);
            triList.Add(faces[i].v2);
            triList.Add(faces[i].v3);
        }
        mesh.triangles = triList.ToArray();
        return mesh;
    }
    public static void SphericalToCartesian(float radius, float polar, float elevation, out Vector3 outCart)
    {
        float a = radius * Mathf.Cos(elevation);
        outCart.x = a * Mathf.Cos(polar);
        outCart.y = radius * Mathf.Sin(elevation);
        outCart.z = a * Mathf.Sin(polar);
    }
    public static void CartesianToSpherical(Vector3 cartCoords,Vector3 Center, out float outRadius, out float outPolar, out float outElevation)
    {
        cartCoords = cartCoords - Center;
        if (cartCoords.x == 0)
            cartCoords.x = Mathf.Epsilon;
        outRadius = Mathf.Sqrt((cartCoords.x * cartCoords.x)
                        + (cartCoords.y * cartCoords.y)
                        + (cartCoords.z * cartCoords.z));
        outPolar = Mathf.Atan(cartCoords.z / cartCoords.x);
        if (cartCoords.x < 0)
            outPolar += Mathf.PI;
        outElevation = Mathf.Asin(cartCoords.y / outRadius);
    }
}
