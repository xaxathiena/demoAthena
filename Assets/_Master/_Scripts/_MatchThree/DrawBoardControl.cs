using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBoardControl : MonoBehaviour
{
    public ArrayLayout data;

    public bool CanAddBlock(int x, int y)
    {
        if (data.rows.Length > x)
        {
            if (data.rows[x].row.Length > y)
            {
                return !data.rows[x].row[y];
            }
        }

        return false;
    }
}
