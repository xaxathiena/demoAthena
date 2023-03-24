using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BlockUIControl : MonoBehaviour
{
   public MeshRenderer mesh;
   public Vector2Int currentPosition;

   public Vector2Int CurrentPosition
   {
      set
      {
         gameObject.name = $"({value.x}, {value.y})";
         currentPosition = value;
      }
      get => currentPosition;
   }
   public BlockType type;
   public GameObject border;

   public bool IsHighlight
   {
      set => border.SetActive(value);
   }

   public void OnSetup(int x, int y, BlockType type, Color color)
   {
      gameObject.name = $"({x}, {y})";
      this.type = type;
      CurrentPosition = new Vector2Int(x, y);
      mesh.material.color = color;
   }

}
