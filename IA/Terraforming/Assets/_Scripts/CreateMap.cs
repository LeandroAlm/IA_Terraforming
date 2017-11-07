using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMap : MonoBehaviour {

    int X, Y;
    int min, max;
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
                //random = Random.Range(0.0f, 1.0f);
                //DoSmoth(i, j);
                //heights[i,j] = random;
                DoRandom(i, j);
            }
        }
        terreno.terrainData.SetHeights(0, 0, heights);
    }

    void DoSmoth(int x, int y)
    {
        float h, h1, h2, h3;

            
        // canto esquerdo
        if (x == 0 && y == 0)
        {
            random = Random.Range(0.0f, 1.0f);
            heights[0, 0] = random;
            
        }
        //vertical esqueda
        else if (x == 0 && y != 0)
        {
            h = heights[0, y - 1];
            if (h > 0.9f)
                random = Random.Range(h - 0.1f, 1.0f);
            else if (h < 0.1f)
                random = Random.Range(0.0f, h +0.1f);
            else
                random = Random.Range(h - 0.1f, h + 0.1f);
            heights[0, y] = random;
        }
        // horizontal de baixo
        else if (x != 0 && y == 0)
        {
            h = heights[x - 1, y];

            if (h > 0.9f)
                random = Random.Range(0.8f, 1.0f);
            else if (h < 0.1f)
                random = Random.Range(0.0f, 0.2f);
            else
                random = Random.Range(h - 0.1f, h + 0.1f);
            heights[x, y] = random;
        }
        //vertical direita
        else if (x > 31 && y != 0)
        {
            h = heights[0, y - 1];
            if (h > 0.9f)
                random = Random.Range(0.8f, 1.0f);
            else if (h < 0.1f)
                random = Random.Range(0.0f, 0.2f);
            else
                random = Random.Range(h - 0.1f, h + 0.1f);
            heights[0, y] = random;
        }
        // horizontal de baixo
        else if (x != 0 && y == 0)
        {
            h = heights[x - 1, y];

            if (h > 0.9f)
                random = Random.Range(0.8f, 1.0f);
            else if (h < 0.1f)
                random = Random.Range(0.0f, 0.2f);
            else
                random = Random.Range(h - 0.1f, h + 0.1f);
            heights[x, y] = random;
        }
        // horizontal de cima
        else if (x != 0 && y > 31)
        {
            h = heights[x - 1, y];

            if (h > 0.9f)
                random = Random.Range(0.8f, 1.0f);
            else if (h < 0.1f)
                random = Random.Range(0.0f, 0.2f);
            else
                random = Random.Range(h - 0.1f, h + 0.1f);
            heights[x, y] = random;
        }
        // td mapa
        else if (x > 0 && x < 31 && y > 0 && y < 31)
        {
            h = heights[x - 1, y];
            h1 = heights[x + 1, y];
            h2 = heights[x, y - 1];
            h3 = heights[x, y + 1];

            h = (h + h1 + h2 + h3) / 4;

            if (h > 0.9f)
                random = Random.Range(0.8f, 1.0f);
            else if (h < 0.1f)
                random = Random.Range(0.0f, 0.2f);
            else
                random = Random.Range(h - 0.1f, h + 0.1f);
            heights[x, y] = random;
        }
        //terreno.terrainData.SetHeights(0, 0, heights);
    }

    private void DoRandom(int x, int y)
    {
        //Debug.Log("x: " + x + "  y: " + y);
        if ((x % 4) == 0 && (y % 4) == 0)
        {
            // os nr de 4 em 4 seram random 0, 4, 8, 12, 16, 20, 24, 28, 32
            random = Random.Range(0.0f, 1.0f);
            heights[x, y] = random;
        }
        else
        {
            // os nr nao multiplos de 4... procura se o multiplo de 4 mais atraz e mais abaixo e faz se a media!

            int xT = x;
            int yT = y;
            
            int ok = 0;

            while (ok == 0)
            {
                if (xT % 4 != 0)
                    xT--;
                else if (xT % 4 == 0)
                {
                    if (yT % 4 != 0)
                        yT--;
                    else if (yT % 4 == 0)
                    {
                        float h;
                        Debug.Log("xT: " + xT + "  yT: " + yT);


                        if (xT == 28 || yT == 28)
                        {
                            h = (heights[xT, yT]);
                        }
                        else
                        {
                            h = (heights[xT, yT] + heights[xT + 4, yT] + heights[xT, yT + 4] + heights[xT + 4, yT + 4]) / 4;
                        }
                        heights[x, y] = h;
                        ok++;
                    }
                }
            }
        }

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
                heights[i, j] = 0.0f;
            }
        }
        terreno.terrainData.SetHeights(0, 0, heights);
    }
}
