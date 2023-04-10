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

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        m_image = GetComponent<Image>();
        StartCoroutine(VisualDelay());
        
    }

    public void Initialize(int vScore, int hScore, CandyColor color, Sprite candySprite)
    {
        m_image.sprite = candySprite;
        Data = new CandyData(vScore, hScore, color);
    }

    private IEnumerator VisualDelay()
    {
        m_image.enabled = false;
        yield return new WaitForSeconds(1f);
        m_image.enabled = true;
    }

    



}


