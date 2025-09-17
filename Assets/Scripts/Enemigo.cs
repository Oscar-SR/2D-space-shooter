using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Enemigo : MonoBehaviour {
    protected Rigidbody2D rigid;
    protected Transform trans;
    protected SpriteRenderer spriteRender;
    protected ModeloEnemigo modeloEnemigo;
    protected AudioSource audioExplosion;
    protected bool disparando = false;
    public GameObject prefabDisaroEnemigo;
    protected Animator anim;
    public float intervaloDisparo = 2.0f;

    // Use this for initialization
    void Start ()
    {
        modeloEnemigo = new ModeloEnemigo();
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector2(0, -modeloEnemigo.pVelocidad);
        trans = GetComponent<Transform>();
        spriteRender = GetComponent<SpriteRenderer>();

        audioExplosion = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void  FixedUpdate ()
    {
        //The bottom-left of the viewport is (0,0); the topright is (1,1).
        Vector2 vector2min=Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 vector2max=Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        //tengo en cuanta el borde inferior del sprite
        if (vector2min.y > spriteRender.bounds.max.y)
            Destroy(this.gameObject);
        
        if (vector2max.y > spriteRender.bounds.min.y && !disparando){
            disparando=true;
            InvokeRepeating("disparar", intervaloDisparo, intervaloDisparo);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.StartsWith("DisparoJugador"))
        {
            modeloEnemigo.pVida -= 1;
            Destroy(other.gameObject);
        }
        if (modeloEnemigo.pVida <= 0 || other.gameObject.name.StartsWith("Jugador"))
        {
            //desactivamos colisiones y mandamos morir
            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = false;
            anim.SetBool("Explotar", true);
            audioExplosion.Play();
        }
    }

    void destruirInstancia()
    {
        Destroy(gameObject);
        //Suma la puntuación en el jugador
        Jugador jugador = GameObject.Find("Jugador")?.GetComponent<Jugador>();
        if (jugador != null){
            ModeloJugador.Instancia.pPuntuacion += 5;
            Text textPuntuacion = (Text)GameObject.Find("Puntuacion").GetComponent<Text>();
            textPuntuacion.text="Puntuación: "+ModeloJugador.Instancia.pPuntuacion;
        }
    }

    void disparar()
    {
        anim.SetBool("Disparar", true);
        Instantiate(prefabDisaroEnemigo, new Vector2(trans.GetChild(0).position.x, trans.GetChild(0).position.y), Quaternion.identity);
        StartCoroutine(detenerAnim());
    }

    private IEnumerator detenerAnim(){
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.SetBool("Disparar", false);
    }
}