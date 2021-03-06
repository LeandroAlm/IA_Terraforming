﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMap : MonoBehaviour
{

    int X, Y;
    int min, max;
    public Terrain terreno;
    public Canvas CanvasStart;
    public Text Xtext, Ytext;
    public Slider Xslider, Yslider;
    public GameObject prefab;
    float[,] heights;

    public Transform Player;
    Vector3 posDefault = new Vector3(16.5f, 20f, 16.5f);

    float random;

    float scale = 10f;

	void Start ()
    {
        GetValeus();
    }
		

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HeightRandom();
            Player.position = new Vector3(Random.Range(1f, 256.0f), 20, Random.Range(1f, 256.0f));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ResetHeight();
            Player.position = posDefault;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SayHeights();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            FixHeights();
        }
    }

    public void GetValeus()
    {
        X = 257;
        Y = 257;
        terreno.terrainData.heightmapResolution = X ;
        terreno.terrainData.size = new Vector3(X, scale, Y);
        heights = new float[X,Y];

        // Alterar locais do terreno!!!
        terreno.terrainData.SetHeights(0, 0, heights);
    }

    void CorrectHeights(int x, int y)
    {
        if ((x % 4) != 0 || (y % 4) != 0)
        {
            int xT = x;
            int yT = y;
            while (xT % 4 != 0) xT--;
            while (yT % 4 != 0) yT--;

            if (xT < 249 && yT < 249)
            {
                float altura = 0;
                Vector2 vetor1, vetor2, vetor3, vetor4;
                float Y1 = 0, Y2 = 0, Y3 = 0, Y4 = 0;


                vetor1 = new Vector2(xT, yT + 4);
                vetor2 = new Vector2(xT + 4, yT + 4);
                vetor3 = new Vector2(xT, yT);
                vetor4 = new Vector2(xT + 4, yT);


                //alturas
                Y1 = heights[xT, yT + 4];
                Y2 = heights[xT + 4, yT + 4];
                Y3 = heights[xT, yT];
                Y4 = heights[xT + 4, yT + 4];

                // interpolacao bilinear
                float altura12 = (1 - (x - vetor1.x)) * Y1 + (x - vetor1.x) * Y2;
                float altura34 = (1 - (x - vetor3.x)) * Y3 + (x - vetor3.x) * Y4;
                float altura13 = (1 - (x - vetor1.x)) * Y1 + (x - vetor1.x) * Y3;
                float altura24 = (1 - (x - vetor2.x)) * Y2 + (x - vetor2.x) * Y4;
                //altura = (1 - (y - vetor1.y)) * altura12 + (y - vetor1.y) * altura34;
                //float alturaAux = (1 - (y - vetor1.y)) * altura13 + (y - vetor1.y) * altura24;
                //float Height = (1 - (y - vetor1.y)) * altura + (y - vetor1.y) * alturaAux;
                float med = (heights[xT, yT] + heights[xT + 4, yT] + heights[xT, yT + 4] + heights[xT + 4, yT + 4]) / 4f;
                float Height = ((heights[xT, yT] - heights[xT + 4, yT]) * altura34 +
                    (heights[xT + 4, yT + 4] - heights[xT + 4, yT]) * altura24 +
                     (heights[xT, yT + 4] - heights[xT + 4, yT + 4]) * altura12 +
                     (heights[xT, yT + 4] - heights[xT, yT] * altura13)) / 4f;
                heights[x, y] = Height + med;
            }
        }
    }

    void SayHeights()
    {
        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                //if (heights[i,j] == 0)
                if (i % 4 == 0 && j % 4 == 0)
                    Debug.Log("(" + i + "," + j + ") -> " + heights[i,j]);
            }
        }
    }

    void FixHeights()
    {
        for (int i = 0; i < X-1; i++)
        {
            for (int j = 0; j < Y-1; j++)
            {
                if (i == 0 && j > 0)
                    if (heights[i, j] > ((heights[i, j - 1] + heights[i, j + 1]) / 2f) + 0.1f)
                        heights[i, j] -= 0.05f; 
                else if (i > 0 && j == 0)
                        if (heights[i, j] > ((heights[i - 1, j] + heights[i + 1, j]) / 2f) + 0.1f)
                            heights[i, j] -= 0.05f;
                else if (i > 0 && j > 0)
                        if (heights[i, j] > ((heights[i-1, j] + heights[i-1, j-1] + heights[i, j-1] + heights[i+1, j-1]
                                            + heights[i+1, j] + heights[i+1, j+1] + heights[i, j+1] + heights[i-1, j+1]) / 8f) + 0.1f)
                            heights[i, j] -= 0.05f;
            }
        }
    }

    void HeightRandom()
    {
        Debug.Log("Alterando as alturas!");
        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                //random = Random.Range(0.0f, 1.0f);
                //DoSmoth(i, j);
                //heights[i,j] = random;
                DoRandom1(i, j);
            }
        }
        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                //random = Random.Range(0.0f, 1.0f);
                //DoSmoth(i, j);
                //heights[i,j] = random;
                //DoRandom2(i, j);
                CorrectHeights(i, j);
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

    private void DoRandom1(int x, int y)
    {
        if ((x % 4) == 0 && (y % 4) == 0)
        {
            //os nr de 4 em 4 seram random 0, 4, 8, 12, 16, 20, 24, 28, 32

            if (x == 0 && y == 0)
                random = Random.Range(0.0f, 3.0f);
            else if (x > 0 && y == 0)
                random = Random.Range(heights[x - 4, 0] - 0.15f, heights[x - 4, 0] + 0.15f);
            else if (x == 0 && y > 0)
                random = Random.Range(heights[0, y - 4] - 0.15f, heights[0, y - 4] + 0.15f);
            else if (x > 0 && y > 0)
                random = Random.Range((heights[x - 4, y] + heights[x, y - 4]) / 2f - 0.15f, (heights[x - 4, y] + heights[x, y - 4]) / 2f + 0.15f);

            //random = Random.Range(0.0f, 1.0f);
            heights[x, y] = random;
        }
    }
    private void DoRandom2(int x, int y)
    {
        if ((x % 4) != 0 || (y % 4) != 0)
        {
            // os nr nao multiplos de 4... procura se o multiplo de 4 mais atraz e mais abaixo e faz se a media!

            int xT = x;
            int yT = y;

            while (xT % 4 != 0) xT--;
            while (yT % 4 != 0) yT--;

            float h = 0;
            
            int tempX, tempY;

            if (xT < 249 && yT < 249)
            {
                tempX = x - xT;
                tempY = y - yT;

                

                float deltaX = (x - xT) / 4f;
                float deltaY = (y - yT) / 4f;

                float med = (heights[xT, yT] + heights[xT + 4, yT] + heights[xT, yT + 4] + heights[xT + 4, yT + 4]) / 4f;
                //h = ((heights[xT, yT] - heights[xT + 4, yT]) * deltaX +
                //    (heights[xT + 4, yT] - heights[xT + 4, yT + 4]) * deltaY +
                //     (heights[xT, yT + 4] - heights[xT + 4, yT + 4]) * deltaX +
                //     (heights[xT, yT] - heights[xT, yT + 4] * deltaY)) / 4f;

                //if (tempX == 0)
                //{
                //    if (tempY == 1)
                //        h = (heights[xT, yT] * 0.9f + heights[xT, yT + 4] * 0.1f) / 2;
                //    else if (tempY == 2)
                //        h = (heights[xT, yT] + heights[xT, yT + 4]) / 2;
                //    else if (tempY == 3)
                //        h = (heights[xT, yT] * 0.1f + heights[xT, yT + 4] * 0.9f) / 2;
                //}
                //else if (tempX == 1)
                //{
                //    if (tempY == 0)
                //        h = (heights[xT, yT] * 0.9f + heights[xT + 4, yT] * 0.1f) / 2;
                //    else if (tempY == 1)
                //        h = ((heights[xT, yT]) * 0.55f + (heights[xT, yT + 4]) * 0.2f + (heights[xT + 4, yT] + heights[xT + 4, yT + 4]) * 0.25f) / 4f;
                //    else if (tempY == 2)
                //        h = ((heights[xT, yT] + heights[xT, yT + 4]) * 0.75f + (heights[xT + 4, yT] + heights[xT + 4, yT + 4]) * 0.25f) / 4f;
                //    else if (tempY == 3)
                //        h = ((heights[xT, yT]) * 0.2f + (heights[xT, yT + 4]) * 0.55f + (heights[xT + 4, yT] + heights[xT + 4, yT + 4]) * 0.25f) / 4f;
                //}
                //else if (tempX == 2)
                //{
                //    if (tempY == 0)
                //        h = (heights[xT, yT] + heights[xT + 4, yT]) / 2;
                //    else if (tempY == 1)
                //        h = ((heights[xT, yT] + heights[xT + 4, yT]) * 0.75f + (heights[xT, yT + 4] + heights[xT + 4, yT + 4]) * 0.25f) / 4f;
                //    else if (tempY == 2)
                //        h = (heights[xT, yT] + heights[xT + 4, yT] + heights[xT, yT + 4] + heights[xT + 4, yT + 4]) / 4f;
                //    else if (tempY == 3)
                //        h = ((heights[xT, yT] + heights[xT + 4, yT]) * 0.25f + (heights[xT, yT + 4] + heights[xT + 4, yT + 4]) * 0.75f) / 4f;
                //}
                //else if (tempX == 3)
                //{
                //    if (tempY == 0)
                //        h = (heights[xT, yT] * 0.1f + heights[xT + 4, yT] * 0.9f) / 2;
                //    else if (tempY == 1)
                //        h = (((heights[xT, yT]) + (heights[xT, yT + 4])) * 0.25f + (heights[xT + 4, yT] * 0.55f + heights[xT + 4, yT + 4]) * 0.2f) / 4f;
                //    else if (tempY == 2)
                //        h = ((heights[xT, yT] + heights[xT, yT + 4]) * 0.25f + (heights[xT + 4, yT] + heights[xT + 4, yT + 4]) * 0.75f) / 4f;
                //    else if (tempY == 3)
                //        h = (((heights[xT, yT]) + (heights[xT, yT + 4])) * 0.25f + (heights[xT + 4, yT] * 0.2f + heights[xT + 4, yT + 4]) * 0.55f) / 4f;
                //}

                //h = (heights[xT, yT] + heights[xT + 4, yT] + heights[xT, yT + 4] + heights[xT + 4, yT + 4]) / 4f;
                //heights[x, y] = h;
                heights[x, y] = med;
                //heights[x, y] = h + med;
            }

        }
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
