using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Gameplay;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
   public class Level : MonoBehaviour
   
   {
   [SerializeField] private int id;
   [SerializeField] private Vector3 initialPlayerPosition;
   public delegate void OnLevelLoadedAction(int id, Vector3 initialPlayerLocation);
   public static event OnLevelLoadedAction OnLevelLoaded;

   
   public void StartLevel()
   {
      OnLevelLoaded?.Invoke(id, initialPlayerPosition);
   }
   
   public List<GameObject> LoadTiles()
   {
      var allChildren = GetComponentsInChildren<Transform>();

      return (from child in allChildren where child.gameObject != this.gameObject select child.gameObject).ToList();
   }
   }
}
