using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class Jugador : MonoBehaviour {
    public GameObject prefabDisaroJugador;
    protected Transform trans;
    protected Animator anim;
    public AudioSource audioDisparo;
    public AudioSource audioExplosion;
    public AudioSource audioGameOver;
    public Image imageVida;
    protected Text textVida;
    protected bool estaActivo = true;

    // Use this for initialization
    void Start ()
    {
        trans = GetComponent<Transform>();
        anim=GetComponent<Animator>();
        textVida = (Text)GameObject.Find("VidaNumero").GetComponent<Text>();
    }

    // Update is called once per frame
    void  FixedUpdate ()
    {
        if(estaActivo)
        {
            Vector2 vector2 = new Vector2();
            vector2.x = Input.GetAxisRaw("Horizontal");
            vector2.y = Input.GetAxisRaw("Vertical");
            vector2.Normalize(); //el módulo sea 1

            if (Input.GetAxisRaw("Fire1")==1){
                anim.SetBool("Disparar", true);
                disparar();
            }else{
                anim.SetBool("Disparar", false);
                audioDisparo.loop = false;
            }
            mover(vector2);
        }
    }

    void mover(Vector2 vector2)
    {
        //calculamos las coordenadas mínima y máxima en coordenadas del munndo
        Vector2 vector2min=Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 vector2max=Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        vector2.x *= ModeloJugador.Instancia.pVelocidad * Time.deltaTime;
        vector2.y *= ModeloJugador.Instancia.pVelocidad * Time.deltaTime;
        vector2.x=Mathf.Clamp(trans.position.x + vector2.x, vector2min.x, vector2max.x);
        vector2.y=Mathf.Clamp(trans.position.y + vector2.y, vector2min.y, vector2max.y);
        trans.position = new Vector2(vector2.x, vector2.y);
    }

    void disparar()
    {
        //creo una bala en el cañon izquierdo y otra en el derecho
        Instantiate(prefabDisaroJugador, new Vector2(trans.GetChild(0).position.x, trans.GetChild(0).position.y), Quaternion.identity);
        Instantiate(prefabDisaroJugador, new Vector2(trans.GetChild(1).position.x, trans.GetChild(1).position.y), Quaternion.identity);

        if (audioDisparo.loop == false)
        {
            audioDisparo.loop = true;
            audioDisparo.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name.StartsWith("DisparoEnemigo"))
        {
            ModeloJugador.Instancia.pVida -= 1;
            imageVida.rectTransform.sizeDelta = new Vector2(ModeloJugador.Instancia.pVida, imageVida.rectTransform.sizeDelta.y);
            textVida.text=ModeloJugador.Instancia.pVida.ToString();
            Destroy(collider.gameObject);
        }
        if (collider.gameObject.tag.Equals("Enemigo"))
        {
            ModeloJugador.Instancia.pVida -= 20;
            imageVida.rectTransform.sizeDelta = new Vector2(ModeloJugador.Instancia.pVida, imageVida.rectTransform.sizeDelta.y);
            textVida.text=ModeloJugador.Instancia.pVida.ToString();
        }
        if ((ModeloJugador.Instancia.pVida <= 0 || collider.gameObject.name.StartsWith("BossFinal")) && estaActivo){
            StartCoroutine(morir(2f));
        }
    }

    private IEnumerator morir(float tiempo)
    {
        float tiempoTranscurrido = 0f;

        Collider2D colliderJugador = GetComponent<Collider2D>();
        colliderJugador.enabled = false;
        audioDisparo.enabled = false;
        estaActivo = false;
        
        GameObject musicaFondo = GameObject.Find("Sonido");
        AudioSource audioSource = musicaFondo.GetComponent<AudioSource>();
        audioSource.Stop();

        audioGameOver.Play();

        Vector3 escalaInicial = transform.localScale; // Escala original de la nave

        while (tiempoTranscurrido < tiempo)
        {
            // Rota la nave en el eje Z
            transform.Rotate(0f, 0f, 360f * Time.deltaTime);

            // Calcula la nueva escala reducida proporcional al tiempo transcurrido
            float factorEscala = 1f - (tiempoTranscurrido / tiempo); // Factor que va de 1 a 0
            transform.localScale = escalaInicial * factorEscala; // Escala la nave poco a poco

            tiempoTranscurrido += Time.deltaTime; // Aumenta el tiempo transcurrido
            yield return null; // Espera al siguiente frame
        }

        // Detiene el sonido de game over y activa la animación de explosión
        audioGameOver.Stop();
        anim.SetBool("Explotar", true);
        audioExplosion.Play();

        // Restablece la escala a su valor original en caso de que se vuelva a usar el objeto
        transform.localScale = escalaInicial;
    }

    void destruirInstancia()
    {
        if (PlayerPrefs.GetInt("PuntuacionMaxima") < ModeloJugador.Instancia.pPuntuacion)
            PlayerPrefs.SetInt("PuntuacionMaxima", ModeloJugador.Instancia.pPuntuacion);
        Destroy(gameObject);
        SceneManager.LoadScene("EscenaPuntuacion");
    }
}