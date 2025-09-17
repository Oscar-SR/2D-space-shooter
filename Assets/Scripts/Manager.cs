using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public GameObject gameObjectEnemigo;
    public GameObject gameObjectBossFinal;
    private IEnumerator corutina;
    protected Text textNivel;
    protected AudioSource audioLevelUp;

    // Use this for initialization
    void Start ()
    {
        textNivel = (Text)GameObject.Find("NumeroNivel").GetComponent<Text>();
        audioLevelUp = GetComponent<AudioSource>();
        corutina = coroutinaAvisiones(2);
        StartCoroutine(corutina);
    }

    private IEnumerator coroutinaAvisiones(float waitTime)
    {
        //The bottom-left of the viewport is (0,0); the topright is (1,1).
        Vector2 vector2min=Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 vector2max=Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        int aviones = (int)(Random.value * 5);
        float rangox = vector2max.x - vector2min.x;
        float rangoy = (vector2max.y-vector2min.y)*2 ;
        Vector2 vector2 = new Vector2();
        int nAvionesMaximo;
        int nivel=1;

        while(true){
            nAvionesMaximo=1;
            gameObjectEnemigo.GetComponent<Enemigo>().intervaloDisparo=2.0f/nivel;
            gameObjectBossFinal.GetComponent<BossFinal>().intervaloDisparo=1.0f/nivel;

            while (nAvionesMaximo<20)
            {
                aviones = (int)(Random.value * nAvionesMaximo) + 1;
                nAvionesMaximo+=2;
                for (int i = 0; i < aviones; i++)
                {
                    GameObject gameObjectAvion = Instantiate(gameObjectEnemigo);
                    vector2.x = vector2min.x + rangox * Random.value;
                    vector2.y = vector2max.y + rangoy * Random.value;
                    gameObjectAvion.transform.position = vector2;
                }
                yield return new WaitForSeconds(waitTime);
            }

            yield return new WaitForSeconds(10);

            //Generamos al jefe final
            GameObject instanciaBossFinal = Instantiate(gameObjectBossFinal);
            vector2.x = 0;
            vector2.y = 1;
            instanciaBossFinal.transform.position = vector2;

            //Espera a la muerte del jefe final
            while (instanciaBossFinal != null)
            {
                yield return null;
            }

            nivel++;
            textNivel.text="Nivel "+nivel;

            //Animación de parpadeo del nivel
            int numParpadeos = 20;
            float duracionParpadeo = 0.1f;
            audioLevelUp.Play();
            for (int i = 0; i < numParpadeos; i++)
            {
                // Cambia el color del texto a amarillo
                textNivel.color = Color.yellow;
                yield return new WaitForSeconds(duracionParpadeo / 2);

                // Cambia el color del texto de vuelta al original
                textNivel.color = Color.white;
                yield return new WaitForSeconds(duracionParpadeo / 2);
            }
            audioLevelUp.Stop();
        }
    }
}