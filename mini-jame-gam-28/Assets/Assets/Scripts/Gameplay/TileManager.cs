using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Assets.Scripts;
using Assets.Scripts.Gameplay;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using static Assets.Scripts.Gameplay.Extensions;
using static Assets.Scripts.Gameplay.Extensions.Direction;

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
    

    public bool IsTileAvailable(Vector3 currentLocation, Direction currentDirection, Direction movementDirection, Vector3 tileLocation)
    {
        var tileObject = _currentTiles[Mathf.RoundToInt(tileLocation.x) + 5, Mathf.RoundToInt(tileLocation.y) + 4];
        if (tileObject == null)
        {
            var spawnedStick = SpawnStick(currentLocation, currentDirection, movementDirection);
            _currentTiles[Mathf.RoundToInt(currentLocation.x + 5 ), Mathf.RoundToInt(currentLocation.y + 4)] = spawnedStick;
            return true;
        }
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
                Start();
                return false;
            }
            default:
            {
                var spawnedStick = SpawnStick(currentLocation, currentDirection, movementDirection);
                _currentTiles[Mathf.RoundToInt(currentLocation.x + 5 ), Mathf.RoundToInt(currentLocation.y + 4)] = spawnedStick;
                return true;
            }
        }
    }


    private GameObject SpawnStick(Vector3 location, Direction currentDirection, Direction movementDirection)
    {
        var spawnedStick = Instantiate(stick);
        spawnedStick.transform.position = location;
        var spriteRenderer = spawnedStick.GetComponent<SpriteRenderer>();
        switch (currentDirection)
        {
            case Direction.Up:
            { 
                switch (movementDirection)
                {
                    case Direction.Up:
                        spriteRenderer.sprite = vertical;
                        break;
                    case Direction.Down:
                        return null;
                    case Direction.Left:
                        spriteRenderer.sprite = up_left;
                        break;
                    case Direction.Right:
                        spriteRenderer.sprite = up_right;
                        break;
                }

                break;
            }
            case Direction.Down:
            {
                switch (movementDirection)
                {
                    case Direction.Down:
                        spriteRenderer.sprite = vertical;
                        break;
                    case Direction.Left:
                        spriteRenderer.sprite = down_right;
                        break;
                    case Direction.Right:
                        spriteRenderer.sprite = down_left;
                        break;
                }

                break;
            }
            case Direction.Left:
            {
                switch (movementDirection)
                {
                    case Direction.Up:
                        spriteRenderer.sprite = left_right;
                        break;
                    case Direction.Down:
                        spriteRenderer.sprite = left_left;
                        break;
                    case Direction.Left:
                        spriteRenderer.sprite = horizontal;
                        break;
                    case Direction.Right:
                        return null;
                }

                break;
            }
            case Direction.Right:
            {
                switch (movementDirection)
                {
                    case Direction.Up:
                    {
                        spriteRenderer.sprite = right_left;
                        break;
                    }
                    case Direction.Down:
                    {
                        spriteRenderer.sprite = right_right;
                        break;
                    }
                    case Direction.Left:
                        return null;
                    case Direction.Right:
                    {
                        spriteRenderer.sprite = horizontal;
                        break;
                    }
                }

                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(currentDirection), currentDirection, null);
        }

        return spawnedStick;
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
