using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMap : MonoBehaviour {

    int X, Y;
    public Terrain terreno;
    public Canvas CanvasStart;
    public Text Xtext, Ytext;
    public Slider Xslider, Yslider;
    public GameObject prefab;
    float[,] heights;

    float random;

    float scale = 20f;

	void Start ()
    {
        GetValeus();
    }
		

	void Update ()
    {


        if (Input.GetKeyDown(KeyCode.A))
        {
            HeightRandom();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            ResetHeight();
        }
    }

    public void GetValeus()
    {
        X = 32;
        Y = 32;
        terreno.terrainData.heightmapResolution = X + 1;
        terreno.terrainData.size = new Vector3(X, scale, Y);
        heights = new float[X,Y];

        // Alterar locais do terreno!!!
        terreno.terrainData.SetHeights(0, 0, heights);
        
    }

    void HeightRandom()
    {
        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                Debug.Log("Alterando as alturas!");
                random = Random.Range(0.0f, 1.0f);
                heights[i,j] = CalculateHeight(i, j);
            }
        }
        terreno.terrainData.SetHeights(0, 0, heights);
    }

    
    float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / X * scale;
        float yCoord = (float)y / Y * scale;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }

    void ResetHeight()
    {
        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                Debug.Log("Reset das alturas!");
                random = Random.Range(0.0f, 1.0f);
                heights[i, j] = 0.0f;
            }
        }
        terreno.terrainData.SetHeights(0, 0, heights);
    }
}
