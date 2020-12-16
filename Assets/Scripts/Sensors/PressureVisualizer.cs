using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureVisualizer : MonoBehaviour
{

    private Mesh mesh;

    //Get color gradient
    public Gradient colorGradient;
    
    //Get pressure array from sensors
    public PressureMeasure pressureData;

    //parameters of a plane
    public float width;
    public float height;
    public int nx;
    public int ny;
    public Vector3 center;



    // Start is called before the first frame update
    void Start()
    {
        //Generate mesh
        GeneratePlane(width, height, nx, ny, center);
        //Get Mesh
        mesh = GetComponent<MeshFilter>().mesh;

        //Debug
        Debug.Log(mesh.colors.Length);
        Debug.Log(mesh.vertexCount);
    }

    // Update is called once per frame
    void Update()
    {
        
        List<Color> colors = new List<Color>();

        //Assign color 
        for (int i = 0; i < pressureData.Pressure2Array.GetLength(0); i++)
        {
            for (int j = 0; j < pressureData.Pressure2Array.GetLength(1); j++)
            {
                //The value of pressure is from 0 to 255
                float value = (float)pressureData.Pressure2Array[i, j] / (float)255.0;
                Color c = colorGradient.Evaluate(value);
                colors.Add(c);
            }
        }
        mesh.SetColors(colors);
    }

    void GeneratePlane(float width, float height, int nx, int ny, Vector3 center)
    {
        //Empty mesh
        Mesh mesh = new Mesh();
        List<Vector3> verList = new List<Vector3>();
        verList.Clear();

        float dx, dy;
        dx = width / (float)nx;
        dy = height / (float)ny;

        //Generate plane
        for (int i = 0; i < nx; i++)
        {
            for (int j = 0; j < ny; j++)
            {
                verList.Add(new Vector3(i * dx,  0, j * dy));
            }
        }
        
        //Move the plane to the specified center
        Vector3 currentCenter=new Vector3(0,0,0);
        foreach(Vector3 p in verList)
        {
            currentCenter += p;
        }
        currentCenter /= verList.Count;
        

        Vector3 move = center-currentCenter ;
        for (int i=0;i<verList.Count;i++)
        {
            verList[i] += move;
        }

        //Set vertices
        mesh.SetVertices(verList);

        //Generate triangle connectivity information
        List<int> triList = new List<int>();
        triList.Clear();
        int pointIndex = 0;
        for (int i = 0; i < nx - 1; i++)
        {
            for (int j = 0; j < ny - 1; j++)
            {
                pointIndex = ny * i + j;
                //Tri1
                triList.Add(pointIndex);
                triList.Add(pointIndex + ny + 1);
                triList.Add(pointIndex + ny);

                //Tri2
                triList.Add(pointIndex);
                triList.Add(pointIndex + 1);
                triList.Add(pointIndex + ny + 1);
            }
        }

        //Set Triangle
        mesh.triangles = triList.ToArray();

        //Assign generated mesh to the mesh filter of an object that this script is attached to
        GetComponent<MeshFilter>().mesh = mesh;

    }
}
