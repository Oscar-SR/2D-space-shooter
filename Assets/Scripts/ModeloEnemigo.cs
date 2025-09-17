using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeloEnemigo {
    protected int vida=25;
    protected float velocidad=0.2f;

    public int pVida{
    get{return vida;}
    set{vida = value;}
    }
    
    public float pVelocidad{
    get{return velocidad;}
    set{velocidad = value;}
    }
}