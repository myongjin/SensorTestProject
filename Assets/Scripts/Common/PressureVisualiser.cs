using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureVisualiser : MonoBehaviour
{
    public Gradient colorGradient;
    public Mesh mesh;
    public float testValue = 0;
    // Start is called before the first frame update
    void Start()
    {
        //get mesh of an object this script is attached to
        mesh = GetComponent<MeshFilter>().mesh;
        Debug.Log(mesh.colors.Length);
        Debug.Log(mesh.vertexCount);
    }

    // Update is called once per frame
    void Update()
    {
        //you need to assign pressure value to color array
        List<Color> colors = new List<Color>();

        for(int i=0;i<mesh.vertexCount;i++)
        {
            float value = testValue;
            Color c = colorGradient.Evaluate(value);
            colors.Add(c);
        }

        mesh.SetColors(colors);
        
    }
}
