using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeloJugador : MonoBehaviour
{
    // Instancia estática para el singleton
    public static ModeloJugador Instancia { get; private set; }

    protected int vida = 100;
    protected int puntuacion = 0;
    protected float velocidad = 0.7f;

    // Asegúrate de que la instancia sea accesible
    public int pVida
    {
        get { return vida; }
        set { vida = Mathf.Max(0, value); }
    }

    public int pPuntuacion
    {
        get { return puntuacion; }
        set { puntuacion = value; }
    }

    public float pVelocidad
    {
        get { return velocidad; }
        set { velocidad = value; }
    }

    private void Awake()
    {
        // Verifica si ya existe una instancia
        if (Instancia == null)
        {
            Instancia = this; // Asigna la instancia
            DontDestroyOnLoad(gameObject); // Mantiene este objeto entre escenas
        }
        else
        {
            Destroy(gameObject); // Destruye este objeto si ya hay una instancia
        }
    }
}