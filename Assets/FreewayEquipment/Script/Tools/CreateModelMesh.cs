using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyRoads3Dv3;

public class CreateModelMesh : MonoBehaviour
{
    public Material mat;
    public GameObject pt1;
    public GameObject pt2;
    public float width = 1;
    public float height = 1;

    public void DrawCubeObject()
    {
        GameObject go = new GameObject();
        go.name = "SZ_CUBE";
        go.AddComponent<MeshRenderer>();
        go.AddComponent<MeshFilter>();

        go.GetComponent<MeshRenderer>().material = mat;
        Mesh mesh = new Mesh();
        mesh.Clear();
        //按照预设的顶点索引值（从0到7） 设置mesh中的顶点数组信息
        //设置顶点索引值是不分顺逆时针的
        mesh.vertices = new Vector3[]
        {   new Vector3(0, 0, 0),   // 0
            new Vector3(0, 1, 0),   // 1
            new Vector3(1, 0, 0),   // 2
            new Vector3(1, 1, 0),   // 3

            new Vector3(1, 0, 1),   // 4
            new Vector3(1, 1, 1),   // 5
            new Vector3(0, 0, 1),   // 6
            new Vector3(0, 1, 1),   // 7
        };
        //设置三角形顶点顺序，顺时针设置
        mesh.triangles = new int[]
        {
           // 六个面，每一个面有2个三角形;
            1,0,2, 3,1,2,
            3,2,4, 4,5,3,
            6,7,4, 7,5,4,
            7,6,1, 6,0,1,
            7,1,3, 3,5,7,
            0,6,2, 6,4,2
        };

        go.GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    public void DrawPlaneObject()
    {
        GameObject go = new GameObject();
        go.name = "SZ_PLANE";
        go.AddComponent<MeshRenderer>();
        go.AddComponent<MeshFilter>();

        go.GetComponent<MeshRenderer>().material = mat;
        Mesh mesh = new Mesh();
        mesh.Clear();
        ////按照预设的顶点索引值（从0到7） 设置mesh中的顶点数组信息
        ////设置顶点索引值是不分顺逆时针的
        //mesh.vertices = new Vector3[]
        //{   new Vector3(0, 0, 0),   // 0
        //    new Vector3(0, 0, 1),   // 1
        //    new Vector3(1, 0, 0),   // 2
        //    new Vector3(1, 0, 1),   // 3
        //};
        mesh.vertices = new Vector3[]
        {   new Vector3(0, 0, 0),   // 0
            new Vector3(0, 0, 1),   // 1
            new Vector3(0, 0, 2),   // 2
            new Vector3(1, 0, 2),   // 3
            new Vector3(1, 0, 1),   // 4
            new Vector3(1, 0, 0),   // 5
        };
        ////设置三角形顶点顺序，顺时针设置
        //mesh.triangles = new int[]
        //{
        //    //六个面，每一个面有2个三角形;
        //    //1,0,2, 3,1,2,
        //    0,1,2, 3,2,1    // 注意, 面有正反里外，规律，右手定则，四指为顶点顺序方向，则大拇指为可视方向。
        //};

        mesh.triangles = new int[]
        {
            //六个面，每一个面有2个三角形;
            //1,0,2, 3,1,2,
            0,1,4,
            1,2,3,
            0,4,5,
            1,3,4,
            // 注意, 面有正反里外，规律，右手定则，四指为顶点顺序方向，则大拇指为可视方向。
        };

        mesh.uv = new Vector2[]
        {
            //new Vector2(0, 0),
            //new Vector2(0, 0.5f),
            //new Vector2(0, 1),
            //new Vector2(1, 1),
            //new Vector2(1, 0.5f),
            //new Vector2(1, 0)
            new Vector2(1, 0),
            new Vector2(0.5f, 0),
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(0.5f, 1),
            new Vector2(1, 1)
        };

        go.GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    public void DrawPlaneBetweenTwoPoints()
    {
        float dist = (pt1.transform.position - pt2.transform.position).magnitude;
        //Debug.Log(dist);

        Vector3 pt_mid = (pt1.transform.position + pt2.transform.position) / 2;
        //Debug.Log(pt_mid);

        GameObject go = new GameObject();
        go.name = "SZ_PLANE_pts";

        ////go.transform.LookAt(pt2.transform.position);

        go.AddComponent<MeshRenderer>();
        go.AddComponent<MeshFilter>();

        go.GetComponent<MeshRenderer>().material = mat;
        Mesh mesh = new Mesh();
        mesh.Clear();

        mesh.vertices = new Vector3[]
        {
            //new Vector3(0, 0, 1),   // 1-0
            //new Vector3(0, 0, 2),   // 2-1
            //new Vector3(dist, 0, 2),   // 3-2
            //new Vector3(dist, 0, 1),   // 4-3
            //new Vector3(dist, 0, 0),   // 5-4
            //new Vector3(0, 0, 0),   // 0-5

            new Vector3(-dist/2, 0, 0),   // 1-0
            new Vector3(-dist/2, 0, width),   // 2-1
            new Vector3(dist/2, 0, width),   // 3-2
            new Vector3(dist/2, 0, 0),   // 4-3
            new Vector3(dist/2, 0, -1*width),   // 5-4
            new Vector3(-dist/2, 0, -1*width),   // 0-5
        };


        mesh.triangles = new int[]
        {
            //六个面，每一个面有2个三角形;
            //1,0,2, 3,1,2,
            5,0,3,//0,1,4,
            0,1,2,//1,2,3,
            5,3,4,//0,4,5,
            0,2,3,//1,3,4,
            // 注意, 面有正反里外，规律，右手定则，四指为顶点顺序方向，则大拇指为可视方向。
        };

        mesh.uv = new Vector2[]
        {
            //new Vector2(0, 0),
            //new Vector2(0, 0.5f),
            //new Vector2(0, 1),
            //new Vector2(1, 1),
            //new Vector2(1, 0.5f),
            //new Vector2(1, 0)
            new Vector2(0.5f, 0),
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(0.5f, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
        };

        go.GetComponent<MeshFilter>().sharedMesh = mesh;
        go.transform.position = pt_mid;
        
        go.transform.LookAt(pt2.transform);
        go.transform.Rotate(new Vector3(0, -90, 0));

        go.AddComponent<BoxCollider>();
    }

    public void DrawClubBetweenTwoPoints()
    {
        // float dist = (pt1.transform.position - pt2.transform.position).magnitude;

        // Vector3 pt_mid = (pt1.transform.position + pt2.transform.position) / 2;

        GameObject go = new GameObject();
        go.name = "SZ_Inner_box";

        Vector3 v0, v1, v2, v3, v4, v5, v6, v7;
        // v0, v1, v2, v3 正面; 0,1,3, 1,2,3;
        // v4, v5, v6, v7 背面: 4,5,7, 5,6,7;
        // v1, v6, v5, v2 顶面: 1,6,2, 6,5,2;
        // v0, v7, v4, v3 底面: 0,3,7, 7,3,4;
        // v0, v1, v6, v7 左面: 0,6,1, 6,1,7;
        // v3, v2, v5, v4 右面: 3,2,5, 5,3,4;
        v0 = pt1.transform.position + Vector3.right * (-1) * width;
        v1 = pt1.transform.position + Vector3.right * (-1) * width + Vector3.up * height;
        v2 = pt1.transform.position + Vector3.right * width + Vector3.up * height;
        v3 = pt1.transform.position + Vector3.right * width;

        v4 = pt2.transform.position + Vector3.right * (-1) * width;
        v5 = pt2.transform.position + Vector3.right * (-1) * width + Vector3.up * height;
        v6 = pt2.transform.position + Vector3.right * width + Vector3.up * height;
        v7 = pt2.transform.position + Vector3.right * width;


        go.AddComponent<MeshRenderer>();
        go.AddComponent<MeshFilter>();
        // go.GetComponent<MeshRenderer>().material = mat;

        Mesh mesh = new Mesh();
        mesh.Clear();

        mesh.vertices = new Vector3[]
        {
            v0, v1, v2, v3, v4, v5, v6, v7
        };


        mesh.triangles = new int[]
        {
            0,1,3, 1,2,3,
            4,5,7, 5,6,7,
            1,6,2, 6,5,2,
            0,3,7, 7,3,4,
            0,6,1, 6,1,7,
            3,2,5, 5,3,4,
        };


        go.GetComponent<MeshFilter>().sharedMesh = mesh;

        //go.AddComponent<BoxCollider>();
    }
}
