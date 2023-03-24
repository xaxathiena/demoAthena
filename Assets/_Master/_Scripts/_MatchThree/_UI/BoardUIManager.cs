using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class ColorByType
{
    public BlockType type;
    public Color color;
}
public class BoardUIManager : Singleton<BoardUIManager>
{
    [SerializeField] private DrawBoardControl drawBoardControl;
    [SerializeField] private Vector2 blockSize;
    [SerializeField] private Transform boardPivot;
    [SerializeField] private BlockUIControl blockPrefab;
    [SerializeField] private ColorByType[] _typesColor;
    private Dictionary<BlockType, Color> _typesColorDic = new Dictionary<BlockType, Color>();
    private MathThreeGameRule rule = new MathThreeGameRule();
    private List<BlockUIControl> allBlocks = new List<BlockUIControl>();
    private bool isSwapping = false;
    public List<BlockUIControl> AllBlocks => allBlocks;

    private BlockUIControl AddNewBlock(int x, int y)
    {
        BlockUIControl result = null;
        if (drawBoardControl.CanAddBlock(x + 5, y + 5))
        {
            result = Instantiate(blockPrefab, boardPivot);
            allBlocks.Add(result);
            // go.rect.sizeDelta = blockSize;  
            result.transform.position = new Vector2(x +0.5f , y + 0.5f );
        }

        return result;
    }
    public void OnSetup()
    {
        foreach (var VARIABLE in _typesColor)
        {
            _typesColorDic[VARIABLE.type] = VARIABLE.color;
        }
        rule = new MathThreeGameRule();
        
        
        int n = 10;
        int haftN = 10 / 2;
        for (int i = -haftN; i < haftN; i++)
        {
            for (int j = -haftN; j < haftN; j++)
            {
                var go = AddNewBlock(i, j);
                if(go == null) continue;
                // var go = Instantiate(blockPrefab, boardPivot);
                // allBlocks.Add(go);
                // // go.rect.sizeDelta = blockSize;  
                // go.transform.position = new Vector2(i +0.5f , j + 0.5f );
                BlockType type = (BlockType)Random.Range(0, (int)BlockType.COUNT);
                go.OnSetup(i, j, type, _typesColorDic[type]);
            }
        }
        //CheckBlockDestroy();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            CheckBlockDestroy();
        }
    }

    private void CheckBlockDestroy()
    {
        List<MoveBlockDefine> moevBlockDefines = new List<MoveBlockDefine>();
        var (blockDestroy, side) =rule.DestroyBlocks();
        if(blockDestroy == null) return;
        //Add new block
        MoveBlockDefine tmp;
        if (side)
        {
            //Vertical
            int count = blockDestroy.Count;
            int highestBlock =-999;
            for (int i = 0; i < blockDestroy.Count; i++)
            {
                var go = Instantiate(blockPrefab, boardPivot);
                allBlocks.Add(go);
                // go.rect.sizeDelta = blockSize;  
                go.transform.position = new Vector2(blockDestroy[0].x + 0.5f , 5 + i + 0.5f );
                BlockType type = (BlockType)Random.Range(0, (int)BlockType.COUNT);
                go.OnSetup(blockDestroy[0].x,5 + i,  type, _typesColorDic[type]);
                if (blockDestroy[i].y > highestBlock) highestBlock = blockDestroy[i].y;
                
                
                tmp = new MoveBlockDefine()
                {
                    distance = count,
                    targetPos = new Vector2Int() { x = blockDestroy[0].x, y = 5 + i - count },
                    block = go
                };
                moevBlockDefines.Add(tmp);
            }

            for (int i = highestBlock + 1; i < 5; i++)
            {
                tmp = new MoveBlockDefine()
                {
                    distance = count,
                    targetPos = new Vector2Int() { x = blockDestroy[0].x, y = i - count },
                    block = GetBlock(blockDestroy[0].x, i)
                };
                moevBlockDefines.Add(tmp);
            }
            //All move count
            
        }
        else
        {
            for ( int i = 0; i < blockDestroy.Count; i++ )
            {
                var go = Instantiate(blockPrefab, boardPivot);
                allBlocks.Add(go);
                // go.rect.sizeDelta = blockSize;  
                go.transform.position = new Vector2(blockDestroy[i].x + 0.5f , 5+ 0.5f );
                BlockType type = (BlockType)Random.Range(0, (int)BlockType.COUNT);
                go.OnSetup(blockDestroy[i].x,5,  type, _typesColorDic[type]);
                
                tmp = new MoveBlockDefine()
                {
                    distance = 1,
                    targetPos = new Vector2Int() { x = blockDestroy[i].x, y = 4 },
                    block = go
                };
                moevBlockDefines.Add(tmp);
            }
            for ( int i = 0; i < blockDestroy.Count; i++ )
            {
                var block = GetBlock(blockDestroy[i].x, blockDestroy[i].y);
                var blocks =  MoveFromBlockWithDistance(block, 1);
                if (blocks.Count > 0)
                {
                    moevBlockDefines.AddRange(blocks);
                }
            }
            //Move 1
        }
        //Move all block
        
        for (int i = 0; i < blockDestroy.Count; i++)
        {
            var block = GetBlock(blockDestroy[i].x, blockDestroy[i].y);
            allBlocks.Remove(block);
            Destroy(block.gameObject);
        }

        foreach (var b in moevBlockDefines)
        {
            b.Run();
        }
    }

    private List<MoveBlockDefine> MoveFromBlockWithDistance(BlockUIControl block, int distance)
    {
        List<MoveBlockDefine> result = new List<MoveBlockDefine>();
        MoveBlockDefine tmp;
        for (int i = block.CurrentPosition.y + 1; i < 5; i++)
        {
            tmp = new MoveBlockDefine()
            {
                distance = distance,
                targetPos = new Vector2Int() { x = block.CurrentPosition.x, y = i - distance },
                block = GetBlock(block.CurrentPosition.x, i)
            };
            result.Add(tmp);
        }
        return result;
    }
    
    public BlockUIControl GetBlock(Vector2Int pos)
    {
        return GetBlock(pos.x, pos.y);
    }
    public BlockUIControl GetBlock(int x, int y)
    {
        return allBlocks.Find(b => b.CurrentPosition.x == x && b.CurrentPosition.y == y);
    }
    public void Swap(BlockUIControl block, string side)
    {
        if(isSwapping) return;
        Vector2Int blockConnect = block.CurrentPosition;
        switch (side)
        {
            case "l":
                blockConnect.x = block.CurrentPosition.x - 1;
                break;
            case "r":
                blockConnect.x = block.CurrentPosition.x + 1;
                break;
            case "u":
                blockConnect.y = block.CurrentPosition.y + 1;
                break;
            case "d":
                blockConnect.y = block.CurrentPosition.y - 1;
                break;
        }

        var connectedBlock =
            allBlocks.Find(b => b.CurrentPosition.x == blockConnect.x && b.CurrentPosition.y == blockConnect.y);
        if (connectedBlock != null)
        {
            isSwapping = true;
            Vector3 blockPos = block.transform.position;
            Vector3 blockConnectedPos = connectedBlock.transform.position;
            (connectedBlock.CurrentPosition, block.CurrentPosition) = (block.CurrentPosition, connectedBlock.CurrentPosition);
            block.transform.DOMove(blockConnectedPos, 0.1f).OnComplete(() => isSwapping = false);
            connectedBlock.transform.DOMove(blockPos, 0.1f);
            Debug.Log($"{side}: {block.CurrentPosition} => {blockConnect}");
        }
    }
}

public class MoveBlockDefine
{
    private static float timeMove = 1f;
    public BlockUIControl block;
    public int distance;
    public Vector2Int targetPos;

    public void Run()
    {
        if (block == null)
        {
            int a = 1;
        }
        Vector2 targertPosistion = new Vector2(targetPos.x +0.5f , targetPos.y + 0.5f);
        block.transform.DOMove(targertPosistion, timeMove).OnComplete(() => block.CurrentPosition = targetPos);
    }
}