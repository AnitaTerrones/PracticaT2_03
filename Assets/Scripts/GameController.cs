using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text scoreText;
    public Text lifesText;

    private int score = 0;
    private int lifes = 5; //inicia con 5 vidas
    
    void Start()
    {
        //Mostramos el score acumulado
        scoreText.text = "Score: " + score;
        PrintLifes();
    }

    void Update()
    { }
    public int GetScore()
    {
        return this.score;
    }
    public void PlusScore(int var)
    {
        this.score += var;
        scoreText.text = "Score: " + score;
    }
    
    //pierde vida
    public void LoseLife()
    {
        lifes -=1;
        PrintLifes();
    }

    public int GetLifes()
    {
        return lifes;
    }

    private void PrintLifes()
    {
        var text = "Lives: ";
        for (var i = 0; i < lifes; i++)//i para las vidas que tenga
        {
            text += "I ";
        }
        lifesText.text = text;
    }
    
}
