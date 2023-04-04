using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Candy : MonoBehaviour
{
    public CandyData Data;
    public Animator Animator;
    public Sprite[] CandySprites;
    private Image m_image;
    

    public void Initialize(int vScore, int hScore, CandyColor color, Sprite candySprite)
    {
        Animator = GetComponent<Animator>();
        m_image = GetComponent<Image>();
        m_image.sprite = candySprite;
        Data = new CandyData(vScore, hScore, color);
    }

    



}


