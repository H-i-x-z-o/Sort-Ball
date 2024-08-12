using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class BallGraphic : MonoBehaviour
{
    public SpriteRenderer cicleSprite;
    public SpriteRenderer borderSprite;
    public Sprite[] sprites;

    private void Awake()
    {
        cicleSprite = GetComponent<SpriteRenderer>();
        borderSprite = transform.Find("Border").GetComponent<SpriteRenderer>();
    }

    public void SetColor(int type)
    {
        if(type == 0)
        {
            cicleSprite.sprite = sprites[0];
            cicleSprite.color = new Color(0,0,0,0);
            borderSprite.color = new Color(0,0,0,0);
        }
        else
        {
            cicleSprite.sprite = sprites[type];
            cicleSprite.color = Color.white;
            borderSprite.color = new Color(0, 0, 0, 255);
        }

    }

}
