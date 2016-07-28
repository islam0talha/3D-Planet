using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CreateShape {
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
    private static int getMiddlePoint(int p1, int p2, ref List<Vector3> vertices, ref Dictionary<long, int> cache, float radius)
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
        vertices.Add(middle.normalized*radius);

        // store it, return index
        cache.Add(key, i);

        return i;
    }

    public static Mesh Create(float radius,float PlanetRadius)
    {
        Mesh mesh = new Mesh();
        mesh.Clear();

        List<Vector3> vertList = new List<Vector3>();
        Dictionary<long, int> middlePointIndexCache = new Dictionary<long, int>();
        int recursionLevel = 3;
        int numOfPoints = 10;

        float angleStep = 360.0f / (float)numOfPoints;
        Quaternion quaternion = Quaternion.Euler(0.0f, 0.0f, -angleStep);

        //float t = (1f + Mathf.Sqrt(5f)) / 2f;


        //vertList.Add(new Vector3(-1,t,0).normalized * PlanetRadius);           //0
        //vertList.Add(new Vector3(-t, 0,1).normalized * PlanetRadius);    //1
        //for (int i = 1; i < numOfPoints; i++)
        //{
        //    vertList.Add((quaternion * vertList[i]));
        //}

        ///////////////////
        float t = (radius + Mathf.Sqrt(5f)) / 2f;

        vertList.Add(new Vector3(-1f, t, 0f).normalized    * PlanetRadius);
        vertList.Add(new Vector3(1f, t, 0f).normalized     * PlanetRadius);
        vertList.Add(new Vector3(0f, 1f, t).normalized     * PlanetRadius);
        vertList.Add(new Vector3(0f, 1f, -t).normalized    * PlanetRadius);
        vertList.Add(new Vector3(-t, 0f, -1f).normalized   * PlanetRadius);
        vertList.Add(new Vector3(-t, 0f, 1f).normalized    * PlanetRadius);

        // create 5 triangles of the CurvedCircle
        List<TriangleIndices> faces = new List<TriangleIndices>();

        // 5 faces around point 0
        faces.Add(new TriangleIndices(0, 5, 2));
        faces.Add(new TriangleIndices(0, 2, 1));
        faces.Add(new TriangleIndices(0, 1, 3));
        faces.Add(new TriangleIndices(0, 3, 4));
        faces.Add(new TriangleIndices(0, 4, 5));

        ///////////////////
        //List<TriangleIndices> faces = new List<TriangleIndices>();
        //int FacesCount = 0;
        //for (FacesCount = 0; FacesCount <= numOfPoints-2; FacesCount++)
        //{
        //    faces.Add(new TriangleIndices(0, FacesCount + 1, FacesCount + 2));
        //}
        //faces.Add(new TriangleIndices(0, numOfPoints, 1));

        // refine triangles
        for (int i = 0; i < recursionLevel; i++)
        {
            List<TriangleIndices> faces2 = new List<TriangleIndices>();
            foreach (var tri in faces)
            {
                // replace triangle by 4 triangles
                int a = getMiddlePoint(tri.v1, tri.v2, ref vertList, ref middlePointIndexCache, PlanetRadius);
                int b = getMiddlePoint(tri.v2, tri.v3, ref vertList, ref middlePointIndexCache, PlanetRadius);
                int c = getMiddlePoint(tri.v3, tri.v1, ref vertList, ref middlePointIndexCache, PlanetRadius);

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
        //mesh.uv = new Vector2[vertices.Length];

        //Vector3[] normales = new Vector3[vertList.Count];
        //for (int i = 0; i < normales.Length; i++)
        //    normales[i] = vertList[i].normalized;


        //mesh.normals = normales;

        //mesh.RecalculateBounds();
        mesh.Optimize();
        return mesh;
    }
}
