using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTile : MonoBehaviour
{
    [SerializeField] private Image Background;
    [SerializeField] private Transform Lock;
    [SerializeField] private Transform Right;
    [SerializeField] private int IndexLevel;

    private void Awake()
    {
        Background = transform.Find("background").GetComponent<Image>();
        Lock = transform.Find("Lock Icon");
        Right = transform.Find("Right Icon");
    }

    public void SetIndex(int idx)
    {
        IndexLevel = idx;
    }

    public int GetIndex() => IndexLevel;

    public void Locked()
    {
        var color = Background.color;
        color.a = 0;
        Background.color = color;
        Lock.gameObject.SetActive(true);
        Right.gameObject.SetActive(false);
    }

    public void Unlocked()
    {
        var color = Background.color;
        color.a = 255;
        Background.color = color;
        Lock.gameObject.SetActive(false);
        Right.gameObject.SetActive(false);
    }

    public void Completed()
    {
        var color = Background.color;
        color.a = 255;
        Background.color = color;
        Lock.gameObject.SetActive(false);
        Right.gameObject.SetActive(true);
    }
}
