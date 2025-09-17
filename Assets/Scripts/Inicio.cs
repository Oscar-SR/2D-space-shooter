using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Inicio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("BotonJugar").GetComponentInChildren<Text>().text = "Jugar";
        GameObject.Find("BotonSalir").GetComponentInChildren<Text>().text = "Salir";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CambiarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    public void Salir()
    {
        Application.Quit(); // Cierra el juego
    }
}
