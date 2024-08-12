using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BottleGraphic : MonoBehaviour
{
    public int index;
    public List<BallGraphic> ballGraphics = new List<BallGraphic>();
    public GameGraphic gameGraphic;
    public Transform BottleUptransform;

    private void Awake()
    {
        gameGraphic = FindObjectOfType<GameGraphic>();
        BottleUptransform = transform.Find("BottleUp");
    }

    private void OnMouseUpAsButton()
    {
        if(LevelManager.Instance.GetStateGame() != LevelManager.State.PLAY) return;
        gameGraphic.OnClickBottle(index);
        // Game.instance.ChoseBottle(index);
        // Debug.Log("choose bottle :" + index,gameObject);
    }


    public void SetBottleGraphic(int[] ballTypes)
    {
        for (int i = 0; i < ballGraphics.Count; i++)
        {
            if (i >= ballTypes.Length) SetGraphicNone(i);
            else SetGraphic(i, ballTypes[i]);
        }
    }

    public void SetGraphicNone(int index)
    {
        ballGraphics[index].SetColor(0);
    }

    public void SetGraphic(int index,int ballType)
    {
        ballGraphics[index].SetColor(ballType);
    }

    public Vector3 GetBallPosition(int index)
    {
        return ballGraphics[index].transform.position;
    }

    public Vector3 GetBottleUpPosition()
    {
        return BottleUptransform.position;
    }
}
