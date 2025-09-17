using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class DisparoJugador : MonoBehaviour {
    public float velocidad=1.0f;
    protected Rigidbody2D rigid;
    protected Transform trans;
    protected SpriteRenderer spriteRender;

    // Use this for initialization
    void Start ()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector2(0, velocidad);
        trans = GetComponent<Transform>();
        spriteRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update ()
    {
        Vector2 vector2max=Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        //tengo en cuanta el borde inferior del sprite
        if (vector2max.y < spriteRender.bounds.min.y)
            Destroy(this.gameObject);
    }
}