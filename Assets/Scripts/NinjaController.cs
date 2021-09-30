using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NinjaController : MonoBehaviour
{
    //public properties
    public float velocityX = 20;
    public float JumpForce = 40;
    
    //limites de salto
    private int saltos = 2;//maximo de saltos
    private int cont = 0;//contar saltos

    //escalera
    private bool escalera = false;

    //Shurenks
    public GameObject rightShuriken;
    public GameObject leftShuriken;
    
    //private components
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator animator;
    private GameController game;
    
    //Const
    private const int ANIMATION_IDLE = 0;//quieto
    private const int ANIMATION_RUN = 1;
    private const int ANIMATION_SLIDE = 2;
    private const int ANIMATION_LANZAR = 3;
    private const int ANIMATION_CLIMB = 4;//trepar
    private const int ANIMATION_GLIDE = 5;//planear
    private const int ANIMATION_JUMP = 6;
    private const int ANIMATION_DEAD = 7;//muere

    private const int LAYER_GROUND = 6;
    private const string TAG_ENEMY = "Enemy";

    void Start()// Start is called before the first frame update
    {
       Debug.Log("Iniciando Game Objet");//mensaje en consola
       sr = GetComponent<SpriteRenderer>();
       rb = GetComponent<Rigidbody2D>();
       animator = GetComponent<Animator>();
       game  = FindObjectOfType<GameController>();
    }

    void Update()// Update is called once per frame
    {
         //cambio velocidad en X mas no en Y
        rb.velocity = new Vector2(0, rb.velocity.y);
        changeAnimation(ANIMATION_IDLE);
        
        rb.gravityScale = 10;
 
        //Ir a la derecha
        if(Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(velocityX, rb.velocity.y);
            sr.flipX = false;
            changeAnimation(ANIMATION_RUN);
        }
            
        //Ir a la izquierda
        if(Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-velocityX, rb.velocity.y);
            sr.flipX = true;
            changeAnimation(ANIMATION_RUN);
        }

        //Deslizar
        if(Input.GetKey(KeyCode.X))
        {
            changeAnimation(ANIMATION_SLIDE);
        }

        //movimiento salto
        if(Input.GetKeyUp(KeyCode.Space) )
        {
            //vemos el limite de salto
            if(cont < saltos)
            {
                rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
                cont++; //sumamos contador de saltos
            }
            changeAnimation(ANIMATION_JUMP);
            Debug.Log(cont);//mensaje en consola
        }

        //Lanzar shuriken
        if (Input.GetKeyUp(KeyCode.E))
        {
            var shuriken = sr.flipX ? leftShuriken : rightShuriken;
            var position = new Vector2(transform.position.x, transform.position.y);
            var rotation = rightShuriken.transform.rotation;
            Instantiate(shuriken, position, rotation);
            changeAnimation(ANIMATION_LANZAR);
        }

        //Subir escalera
        if(escalera == true)
        {
            //Ir arriba
            if(Input.GetKey(KeyCode.W))
            {
                rb.velocity = new Vector2(rb.velocity.x, 10);
                changeAnimation(ANIMATION_CLIMB);
            }
            if(Input.GetKey(KeyCode.S))
            { 
                rb.velocity = new Vector2(rb.velocity.x, -10);
                changeAnimation(ANIMATION_CLIMB);
            }
        }
        if(Input.GetKey(KeyCode.F))
        {                
            rb.velocity = new Vector2(rb.velocity.x,-1); 
            changeAnimation(ANIMATION_GLIDE);
        }
        
    }

    //COLISIONES
    private void OnCollisionEnter2D(Collision2D colision)
    {
        //colisiona y se reseta los saltos a 0  
        if (colision.gameObject.layer == LAYER_GROUND)
        {
            Debug.Log("Collision: " + colision.gameObject.name);
            cont = 0;//reseteo el contador de saltos
        }
        if (colision.gameObject.CompareTag(TAG_ENEMY))
        {
            game.LoseLife();//pierde vida tras colisionar
            changeAnimation(ANIMATION_DEAD);
        }
    }
    void OnTriggerStay2D(Collider2D other)    
    {
        if(other.gameObject.tag == "escalera")
        {
            escalera = true;
            rb.gravityScale = 0;
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }         
    }
    void OnTriggerExit2D(Collider2D other) 
    {
        if(other.gameObject.tag == "escalera")
        {
            escalera = false;
        }
    }
    
    //cambio de estado
    private void changeAnimation(int animation)
    {
        animator.SetInteger("Estado", animation);
    }
}

