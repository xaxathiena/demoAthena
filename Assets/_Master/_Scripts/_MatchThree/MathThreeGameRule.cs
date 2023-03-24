using System.Collections.Generic;
using UnityEngine;

public class MathThreeGameRule
{
    public (List<Vector2Int>, bool) DestroyBlocks()
    {
        List<Vector2Int> tempResult;
        BlockUIControl tempBlock;
        int n = 10;
        int haftN = 10 / 2;
        for (int i = -haftN; i < haftN; i++)
        {
            for (int j = -haftN; j < haftN; j++)
            {
                tempBlock = BoardUIManager.instance.GetBlock(i, j);
                if(tempBlock == null) continue;
                tempResult = HorizoltalSameBlocks(tempBlock);
                if (tempResult.Count >= 2)
                {
                    tempResult.Add(new Vector2Int(i,j));
                    return (tempResult, true);
                }
                else
                {
                    tempResult = VerticalSameBlocks(tempBlock);
                    if (tempResult.Count >= 2)
                    {
                        tempResult.Add(new Vector2Int(i,j));
                        return (tempResult, false);
                    }
                }
            }
        }
        return (null, false);
    } 

    public List<Vector2Int> VerticalSameBlocks(BlockUIControl block)
    {
        List<Vector2Int> results = new List<Vector2Int>();
        BlockUIControl nextBlock;
        for (int i = 1; i < 10; i++)
        {
            nextBlock = BoardUIManager.instance.GetBlock(block.CurrentPosition.x + i, block.CurrentPosition.y); 
            if (nextBlock != null && nextBlock.type == block.type)
            {
                results.Add(nextBlock.CurrentPosition);
            }
            else break;
        }
        return results;
    }
    //HorizoltalSameBlocks
    public List<Vector2Int> HorizoltalSameBlocks(BlockUIControl block)
    {
        List<Vector2Int> results = new List<Vector2Int>();
        BlockUIControl nextBlock;
        for (int i = 1; i < 10; i++)
        {
            nextBlock = BoardUIManager.instance.GetBlock(block.CurrentPosition.x, block.CurrentPosition.y + i); 
            if (nextBlock != null && nextBlock.type == block.type)
            {
                results.Add(nextBlock.CurrentPosition);
            }
            else break;
        }
        return results;
    }
}
