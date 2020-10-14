/// <summary>
/// Grab pass. Credits to Unity User Cician https://forum.unity.com/threads/simple-optimized-blur-shader.185327/#post-1267642
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GrabPass : MonoBehaviour
{
    public float Distortion = 1.25f;
    public Color color = new Color(1, 1, 1, 1);

    // Use this for initialization
    void Awake()
    {
        if (GetComponent<Image>().material != null)
        {
            GetComponent<Image>().material = new Material(Shader.Find("Smkgames/Menu/SimpleGrabPassBlur"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().material.SetFloat("_Distortion", Distortion);
        GetComponent<Image>().material.SetColor("_Color", color);

    }
}