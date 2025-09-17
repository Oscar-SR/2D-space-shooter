using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class BossFinal : MonoBehaviour
{
    protected Rigidbody2D rigid;
    protected ModeloBossFinal modeloBossFinal;
    protected SpriteRenderer spriteRender;
    protected Animator anim;
    public AudioSource audioExplosion;
    public AudioSource audioDisparo;
    protected Transform trans;
    protected bool apareciendo = true;
    public GameObject prefabDisaroEnemigo;
    public float intervaloDisparo = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        modeloBossFinal = new ModeloBossFinal();
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector2(0, -0.2f);
        spriteRender = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        trans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        spriteRender.color = Color.white;
        Vector2 vector2max=Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        if(apareciendo && (spriteRender.bounds.max.y < vector2max.y)){
            apareciendo=false;
            StartCoroutine(movimiento(intervaloDisparo));
        }
    }

    private IEnumerator movimiento(float intervaloDisparo)
    {
        rigid.velocity = Vector2.zero; // Establecer la velocidad a cero

        yield return new WaitForSeconds(1);

        float velocidad = modeloBossFinal.pVelocidad;
        bool invertir = false;
        float origen = -0.7f;
        float destino = 0.7f;

        StartCoroutine(disparar(intervaloDisparo));

        while (true)
        {
            // Verificar si el enemigo debe moverse hacia la derecha o la izquierda
            if (!invertir)
            {
                // Mover hacia la derecha
                rigid.velocity = new Vector2(transform.position.x < destino ? velocidad : -velocidad, rigid.velocity.y);
                // Comprobar si ha alcanzado el borde derecho
                if (Mathf.Abs(transform.position.x - destino) < 0.1f)
                    invertir = true; // Cambiar dirección
            }
            else
            {
                // Mover hacia la izquierda
                rigid.velocity = new Vector2(transform.position.x > origen ? -velocidad : velocidad, rigid.velocity.y);
                // Comprobar si ha alcanzado el borde izquierdo
                if (Mathf.Abs(transform.position.x - origen) < 0.1f)
                    invertir = false; // Cambiar dirección
            }

            yield return null; // Esperar hasta el siguiente frame
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(apareciendo){
            if (other.gameObject.name.StartsWith("DisparoJugador"))
                Destroy(other.gameObject);
        }else{
            if (other.gameObject.name.StartsWith("DisparoJugador"))
            {
                modeloBossFinal.pVida -= 1;
                Destroy(other.gameObject);
                spriteRender.color = Color.red;
            }
            if (modeloBossFinal.pVida <= 0)
            {
                //desactivamos colisiones y mandamos morir
                Collider2D collider = GetComponent<Collider2D>();
                collider.enabled = false;
                anim.SetBool("Explotar", true);
                audioExplosion.Play();
            }
        }
    }

    void destruirInstancia()
    {
        Destroy(gameObject);
        //Suma la puntuación en el jugador
        Jugador jugador = GameObject.Find("Jugador")?.GetComponent<Jugador>();
        if (jugador != null){
            ModeloJugador.Instancia.pPuntuacion += 100;
            Text textPuntuacion = (Text)GameObject.Find("Puntuacion").GetComponent<Text>();
            textPuntuacion.text="Puntuación: "+ModeloJugador.Instancia.pPuntuacion;
        }
    }

    private IEnumerator disparar(float intervaloDisparo)
    {
        while(true){
            audioDisparo.Play();
            anim.SetBool("Disparar", true);
            Instantiate(prefabDisaroEnemigo, new Vector2(trans.GetChild(0).position.x, trans.GetChild(0).position.y), Quaternion.identity);
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
            anim.SetBool("Disparar", false);
            yield return new WaitForSeconds(intervaloDisparo);
        }
    }
}
