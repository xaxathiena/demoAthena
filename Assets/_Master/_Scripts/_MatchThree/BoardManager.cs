using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    public const int Size =10;
    private static BoardManager instance;
    public static BoardManager Instance => instance;
    	
    [SerializeField] private List<BlockDefine> blocks = new List<BlockDefine>();
    [SerializeField] private DrawBoardControl drawBoardControl;
    [SerializeField] private BoardUIManager uiManager;
    public ArrayLayout Layout => drawBoardControl.data;
    public List<BlockDefine> Blocks => blocks;
    private void Awake()
    {
        if(instance != null) Destroy(instance.gameObject);
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < drawBoardControl.data.rows.Length; i++)
        {
            for (int j = 0; j < drawBoardControl.data.rows[i].row.Length; j++)
            {
                blocks.Add(new BlockDefine()
                {
                    x= i,
                    y = j,
                    type = (BlockType)Random.Range(0, (int) BlockType.COUNT),
                    isBlock =  drawBoardControl.data.rows[i].row[j]
                });
            }
        }
        uiManager.OnSetup();
    }
}

public enum BlockType
{
    One,
    Two,
    Three,
    Four,
    Five,
    COUNT
}
[System.Serializable]
public class BlockDefine
{
    public int x;
    public int y;
    public BlockType type;
    public bool isBlock;
}