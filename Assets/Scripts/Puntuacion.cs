using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Puntuacion : MonoBehaviour
{
    public Text puntuacionFinal;
    public Text puntuacionMaxima;

    // Start is called before the first frame update
    void Start()
    {
        puntuacionFinal.text = "Puntuación final: " + ModeloJugador.Instancia.pPuntuacion;
        puntuacionMaxima.text = "Máxima puntuación: " + PlayerPrefs.GetInt("PuntuacionMaxima");
        GameObject.Find("BotonVolverAJugar").GetComponentInChildren<Text>().text = "Volver a jugar";
        GameObject.Find("BotonVolverAlInicio").GetComponentInChildren<Text>().text = "Volver al inicio";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CambiarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }
}
