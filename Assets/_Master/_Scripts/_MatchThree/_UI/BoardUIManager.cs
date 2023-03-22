using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardUIManager : MonoBehaviour
{
    [SerializeField] private Vector2 blockSize;
    [SerializeField] private Transform boardPivot;
    [SerializeField] private BlockUIControl blockPrefab;
    
    public void OnSetup()
    {
        int n = 10;
        int haftN = 10 / 2;
        for (int i = -haftN; i < haftN; i++)
        {
            for (int j = -haftN; j < haftN; j++)
            {
                var go = Instantiate(blockPrefab, boardPivot);
                go.rect.sizeDelta = blockSize;  
                go.rect.anchoredPosition = new Vector2(i * blockSize.x + blockSize.x/2 , j * blockSize.y + blockSize.y/2 );
            }
        }
    }
    
}
