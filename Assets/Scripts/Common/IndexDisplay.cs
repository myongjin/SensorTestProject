using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexDisplay : MonoBehaviour
{
    public GameObject textObject;
    public Mesh mesh;
    private TextMesh[] meshes;
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        meshes = new TextMesh[mesh.vertexCount];
        //Game object를 vertex수만큼 만들고
        //각 오브젝트에 textmesh를 할당해야함
        //그리고 각 오브젝트를 해당 vertex위치로 이동 시켜야함
        GenerateTextObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateTextObject()
    {
        for(int i=0;i<mesh.vertexCount;i++)
        {
            textObject.GetComponentInChildren<TextMesh>().text=i.ToString();
            Instantiate(textObject, this.transform.TransformPoint(mesh.vertices[i]), Quaternion.identity);
        }
        
    }
}
