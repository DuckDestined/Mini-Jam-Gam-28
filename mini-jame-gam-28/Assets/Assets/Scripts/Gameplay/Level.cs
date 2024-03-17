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
      
   [SerializeField] private GameObject player;
   [SerializeField] private int id;
   [SerializeField] private Vector3 initialPlayerPosition;
   private PlayerController _playerController;
   public delegate void OnLevelLoadedAction(int id);
   public static event OnLevelLoadedAction OnLevelLoaded;
   private void Start()
   {
      _playerController = player.GetComponent<PlayerController>();
      StartLevel();
   }
    
   public void StartLevel()
   {
      if (_playerController == null)
      {
         _playerController = player.GetComponent<PlayerController>();
         return;
      }
      if (_playerController != null) _playerController.InitPlayer(initialPlayerPosition);
      OnLevelLoaded?.Invoke(id);
   }
   
   
   public List<GameObject> LoadTiles()
   {
      var allChildren = GetComponentsInChildren<Transform>();

      return (from child in allChildren where child.gameObject != this.gameObject select child.gameObject).ToList();
   }
   }
}
