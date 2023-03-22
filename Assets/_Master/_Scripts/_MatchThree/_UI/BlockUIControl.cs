using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockUIControl : MonoBehaviour
{
   public RectTransform rect;
   public Vector2 currentPosition;
   private void Awake()
   {
      rect = transform as RectTransform;
   }

   public void OnSetup(int x, int y)
   {
      currentPosition = new Vector2(x, y);
   }
}
