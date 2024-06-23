using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SectorRange : MonoBehaviour
{
    //扇形范围的材质
    [SerializeField] private Material material;

    //扇形角度，限制为1~360
    [SerializeField] [Range(1.0f, 400.0f)] private float angle = 360;
    public float Angle => angle;

    //扇形半径，限制为0.1~20
    [SerializeField] [Range(0.1f, 20.0f)] private float radius = 3.0f;
    public float Radius => radius;
    //扇形网格的质量，限制为1~60
    [SerializeField] [Range(1f, 60)] private int quality = 6;

    //存放网格数据的组件
    private MeshFilter _meshFilter;

    //渲染网格的组件
    private MeshRenderer _meshRenderer;

    //网格对象
    [HideInInspector]
    public GameObject _sectorObj;
    
    public void InitSector()
    {
        _sectorObj = CreateSector();
    }
    
    public GameObject CreateSector()
    {
        return GetSector(Vector3.zero, angle, radius, quality);
    }

    private GameObject GetSector(Vector3 center, float angle, float radius, int triangleCount)
    {
        //每个三角形的角度
        float eachAngle = angle / triangleCount;
        //挽歌顶点数组
        List<Vector3> vertices = new List<Vector3>();
        //添加第一个顶点为扇形的圆心
        vertices.Add(center);
        //获取扇形的所有顶点位置
        for (int i = 0; i < triangleCount; i++)
        {
            Vector3 vertex = Quaternion.Euler(0, 0, angle / 2 - eachAngle * i) * Vector3.up * radius;
            vertices.Add(vertex);
        }

        //根据顶点数组创建网络
        return CreateSectorMesh(vertices);
    }

    private GameObject CreateSectorMesh(List<Vector3> vertices)
    {
        //三角形顶点索引数组
        int[] triangles;
        //三角形的数量
        int triangleAmount = vertices.Count - 2;
        //三角形顶点总数=3*三角形数量
        triangles = new int[3 * triangleAmount];
        //设置网格三角形顶点排列
        for (int i = 0; i < triangleAmount; i++)
        {
            triangles[3 * i] = 0;
            triangles[3 * i + 1] = i + 1;
            triangles[3 * i + 2] = i + 2;
        }

        //设置网格uv数组
        Vector2[] uvs = new Vector2[vertices.Count];
        uvs[0] = new Vector2(vertices[0].x, vertices[0].y);
        for (int i = 1; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, 1);
        }

        //扇形对象为空时，新建一个对象
        if (_sectorObj == null)
        {
            _sectorObj = new GameObject("Sector");
            _sectorObj.transform.SetParent(transform, false);
            _meshFilter = _sectorObj.AddComponent<MeshFilter>();
            _meshRenderer = _sectorObj.AddComponent<MeshRenderer>();
        }

        //新建一个网格，设置网格
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;
        mesh.uv = uvs;
        //指定扇形对象的网格和材质m
        _meshFilter.mesh = mesh;
        _meshRenderer.material = material;
        return _sectorObj;
    }
    
    //显示扇形范围
    public void ShowSector()
    {
        if (_sectorObj != null)
        {
            _sectorObj.SetActive(true);
        }
        else
        {
            Debug.LogError("当前的_sectorObj为"+_sectorObj);
        }
    }

    //隐藏扇形范围
    public void HideSector()
    {
        if (_sectorObj != null)
        {
            _sectorObj.SetActive(false);
        }
    }
    
}
