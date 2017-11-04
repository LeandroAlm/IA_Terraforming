using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMap : MonoBehaviour {

    int X, Y;
    public Canvas CanvasStart;
    public Text Xtext, Ytext;
    public Slider Xslider, Yslider;
    public GameObject prefab;

	void Start ()
    {
        
	}
		

	void Update ()
    {
        Xtext.text = "" + Xslider.value;

        Ytext.text = "" + Yslider.value;
    }

    void DrawBase()
    {
        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                Instantiate(prefab, new Vector3(i, -0.5F, j), Quaternion.identity);
            }
        }
    }

    public void GetValeus()
    {
        X = (int)Xslider.value;
        Y = (int)Yslider.value;

        CanvasStart.gameObject.SetActive(false);

        DrawBase();
    }
}
