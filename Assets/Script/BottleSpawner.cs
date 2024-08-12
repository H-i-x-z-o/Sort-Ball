using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BottleSpawner : MonoBehaviour
{
    public Vector3 spawnPosition = new Vector3(0,0,0); // Vị trí spawn object
    public float spacingX = 1.5f; // Khoảng cách giữa các cột
    public float spacingY = 4.3f; // Khoảng cách giữa các hàng
    public int DefaultMaxObjectsPerRow = 5; // Số lượng object tối đa trên một hàng
    public int numberBottles;


    public List<BottleGraphic> SpawnBottles(int numObjects, BottleGraphic prefab, Transform parent = null)
    {
        List<BottleGraphic> bottles = new List<BottleGraphic>(); // Danh sách object đã spawn
        int numObjectsPerRow = DefaultMaxObjectsPerRow; // Số object tối đa trên một hàng
        if (numObjects >= 6 && numObjects <= 8) numObjectsPerRow = 4;
        else if (numObjects > 10) numObjectsPerRow = 6;
        int numRows = Mathf.CeilToInt((float)numObjects / numObjectsPerRow); // Tính số hàng cần thiết
        int objectsInLastRow = numObjects % numObjectsPerRow == 0 ? numObjectsPerRow : numObjects % numObjectsPerRow; // Số object ở hàng cuối
        // Debug.Log(numRows);
        for (int row = 0; row < numRows; row++)
        {
            int numObjectsInRow = (row == numRows - 1) ? objectsInLastRow : numObjectsPerRow; // Số object trong hàng hiện tại
            float rowWidth = (numObjectsInRow - 1) * spacingX; // Chiều rộng của hàng

            // Tính toán vị trí bắt đầu của hàng
            float startX = spawnPosition.x - rowWidth / 2;
            for (int i = 0; i < numObjectsInRow; i++)
            {
                Vector3 spawnPosition = new Vector3(startX + i * spacingX, transform.position.y - row * spacingY, 0f);
                BottleGraphic bottle = Instantiate(prefab, spawnPosition, Quaternion.identity);
                bottle.transform.SetParent(parent);
                bottle.index = row * numObjectsPerRow + i;
                bottles.Add(bottle);
                // Debug.Log("Spawning " + numObjects + " objects");
            }
        }
        FillSize.Instance.Fill(numObjectsPerRow, numRows);
        return bottles;
    }
}
