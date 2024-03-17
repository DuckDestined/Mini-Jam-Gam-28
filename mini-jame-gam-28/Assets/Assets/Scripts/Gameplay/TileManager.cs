using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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


    [SerializeField] private GameObject stick;
    [SerializeField] private Sprite up_left;
    [SerializeField] private Sprite up_right;
    [SerializeField] private Sprite down_left;
    [SerializeField] private Sprite down_right;
    [SerializeField] private Sprite left_left;
    [SerializeField] private Sprite left_right;
    [SerializeField] private Sprite right_left;
    [SerializeField] private Sprite right_right;
    [SerializeField] private Sprite vertical;
    [SerializeField] private Sprite horizontal;

    private static readonly Vector2 Up = new Vector2(0, 1);
    private static readonly Vector2 Down = new Vector2(0, -1);
    private static readonly Vector2 Left = new Vector2(-1, 0);
    private static readonly Vector2 Right = new Vector2(1, 0);


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
    

    public bool IsTileAvailable(Vector3 currentLocation,Vector2 currentDirection, Vector2 movementDirection, Vector3 tileLocation)
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
            default:
            {
                var spawnedStick = Instantiate(stick);
                var spriteRenderer = stick.GetComponent<SpriteRenderer>();
                spawnedStick.transform.position = currentLocation;
                
                return true;
            }
        }
    }


    public void SpawnStick(Vector3 location, Vector2 currentDirection, Vector2 movementDirection)
    {
        var spawnedStick = Instantiate(stick);
        spawnedStick.transform.position = location;
        var spriteRenderer = spawnedStick.GetComponent<SpriteRenderer>();
        if (currentDirection == Up)
        {
            if (movementDirection == Up)
            {
                spriteRenderer.sprite = vertical;
            }

            if (movementDirection == Down)
            {
                return;
            }

            if (movementDirection == Left)
            {
                spriteRenderer.sprite = up_left;
            }

            if (movementDirection == Right)
            {
                spriteRenderer.sprite = up_right;

            }
        }

        if (currentDirection == Down)
        {
            if (movementDirection == Up)
            {
                return;
            }

            if (movementDirection == Down)
            {
                spriteRenderer.sprite = vertical;
            }

            if (movementDirection == Left)
            {
                spriteRenderer.sprite = down_left;

            }

            if (movementDirection == Right)
            {
                spriteRenderer.sprite = down_right;

            }
        }

        if (currentDirection == Left)
        {
            if (movementDirection == Up)
            {
                spriteRenderer.sprite = left_right;
            }

            if (movementDirection == Down)
            {
                spriteRenderer.sprite = left_left;
            }

            if (movementDirection == Left)
            {
                spriteRenderer.sprite = horizontal;
            }

            if (movementDirection == Right)
            {
                return;
            }
        }

        if (currentDirection == Right)
        {
            if (movementDirection == Up)
            {
                spriteRenderer.sprite = right_left;

            }

            if (movementDirection == Down)
            {
                spriteRenderer.sprite = right_right;

            }

            if (movementDirection == Left)
            {
                return;
            }

            if (movementDirection == Right)
            {
                spriteRenderer.sprite = vertical;
            }
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
