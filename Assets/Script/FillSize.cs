using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillSize : MonoBehaviour
{
    public static FillSize Instance;
    public Camera mainCamera;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Fill(int numberObjectPerRow, int numberRows)
    {
        mainCamera.orthographicSize = (float)(numberObjectPerRow == 4 ? 7 : 9);
        if(numberObjectPerRow == 6) mainCamera.orthographicSize = 10;
        mainCamera.transform.position = numberRows == 1 ? new Vector3(0, 0, -10) : new Vector3(0, -1.5f, -10f);
    }
}
