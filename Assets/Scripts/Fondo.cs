using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fondo : MonoBehaviour
{
    protected MeshRenderer meshRender;
    public float velocidadFondo = 0.02f;
    public float velocidadMedio = 0.08f;
    public float velocidadFrente = 0.1f;

    // Use this for initialization
    void Start ()
    {
        meshRender = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update ()
    {
        meshRender.materials[0].SetTextureOffset("_MainTex", new Vector2(0, Time.time * velocidadFondo));
        meshRender.materials[1].SetTextureOffset("_MainTex", new Vector2(0, Time.time * velocidadMedio));
        meshRender.materials[2].SetTextureOffset("_MainTex", new Vector2(0, Time.time * velocidadFrente));
    }
}