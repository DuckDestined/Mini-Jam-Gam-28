using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Gameplay;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class TileManager : MonoBehaviour
{

    [SerializeField] private GameObject[,] _currentTiles;
    [SerializeField] private Level currentLevel;
    [SerializeField] private LevelCollection levelCollection;
    public delegate void CollectedSunAction();

    public static event CollectedSunAction OnCollectedSun;   
    
    
    public delegate void PlayerInvalidMoveAction();

    public static event PlayerInvalidMoveAction OnInvalidMove;
    private void Start()
    {
        _currentTiles = new GameObject[11, 9];
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        var nextLevel = levelCollection.NextLevel();
        ReadLevel(nextLevel);
        currentLevel = nextLevel;
        nextLevel.StartLevel();
    }
    

    public bool IsTileAvailable(Vector3 tileLocation)
    {
        var tileObject = _currentTiles[Mathf.RoundToInt(tileLocation.x) + 5, Mathf.RoundToInt(tileLocation.y) + 4];
        if (tileObject == null) return true;
        switch (tileObject.tag)
        {
            case "Obstacle":
            {
                OnInvalidMove?.Invoke();
                return false;
            }
            case "Sun":
            {
                Destroy(tileObject);
                OnCollectedSun?.Invoke();
                LoadNextLevel();
                return true;
            }
            default: return true;
        }
    }
    
    
    private void ReadLevel(Level level)
    {
        foreach (var tile in level.LoadTiles())
        {
            var position = tile.transform.position;
            _currentTiles[Mathf.RoundToInt(position.x) + 5, Mathf.RoundToInt(position.y) + 4] =
                tile;
        }
        
    } 
}
