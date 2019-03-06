using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowPolyWaterMovement : MonoBehaviour {

    public float power;
    public float timeScale = 1;
    public float scale = 1;
    private MeshFilter myMesh;
    private float xOffset, yOffset;
    public float water_Offset_Mod;
    
    //this is the offset of the 
    //this code was made with the help of a low poly water tutorial on unity

	// Use this for initialization
	void Start () {
        myMesh = GetComponent<MeshFilter>();
        NoiseGenerator();
	}
	
	// Update is called once per frame
	void Update () {
        NoiseGenerator();
        xOffset += Time.deltaTime * timeScale;
        yOffset += Time.deltaTime * timeScale;
	}

     void LateUpdate()
    {
        MaterialOffset();
    }

    void MaterialOffset()
    {
        float waterOffset = Time.time * water_Offset_Mod;
        GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", new Vector2(0, waterOffset));
    }

    void NoiseGenerator()
    {
        Vector3[] verticies = myMesh.mesh.vertices;

        for (int i = 0; i < verticies.Length; i++)
        {
            verticies[i].y = Mathf.PerlinNoise(verticies[i].x * scale * xOffset, verticies[i].z * scale * yOffset) * power;
        }
        myMesh.mesh.vertices = verticies;
    }


    
}
