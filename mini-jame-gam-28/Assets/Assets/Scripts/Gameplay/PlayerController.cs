using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Gameplay
{
    [Serializable]
    public class PlayerController : Moving
    {

        [Serializable]
        private class MoveData
        {
            public MoveData(Transform playerTransform,Extensions.Direction direction)
            {
                Position = playerTransform.position;
                Direction = direction;
            }
            
            public Vector3 Position;
            public Extensions.Direction Direction;
        }
        
        private PlayerInput _playerInput;
        private int _remainingMoves;
        private int suns = 0;
        
        
        
        
        [SerializeField] private bool _isResetting;
        [SerializeField] private int elapsedTimeResettingInMilliseconds;

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite up;
        [SerializeField] private Sprite down;
        [SerializeField] private Sprite right;
        [SerializeField] private Sprite left;

        private int _currentResetMove;

        public delegate void PlayerMoveAction(int remainingMoves);
        public static PlayerMoveAction OnPlayerMove;        
        
        public delegate void PlayerResetAction();
        public static PlayerResetAction OnPlayerReset;

        [SerializeField] private List<MoveData> playerMoves;
        
        // Start is called before the first frame update
        void Awake()
        {
            _playerInput = new PlayerInput();
            _playerInput.Enable();
            _playerInput.Player.Move.performed += MoveOnPerformed;
            TileManager.OnCollectedSun += TileManagerOnonCollectedSun;
            Level.OnLevelLoaded += LevelOnOnLevelLoaded;
            OnPlayerMove?.Invoke(_remainingMoves);
            playerMoves = new List<MoveData>();
            _remainingMoves = 0;
        }

        private void LevelOnOnLevelLoaded(int _, Vector3 initialPlayerLocation)
        {
            InitPlayer(initialPlayerLocation);
        }

        private void Update()
        {
            if (_isResetting)
            {
                 elapsedTimeResettingInMilliseconds += Mathf.FloorToInt(Time.deltaTime * 1000);
                if (elapsedTimeResettingInMilliseconds >= 100)
                {
                    elapsedTimeResettingInMilliseconds = 0;
                    var moveData = playerMoves[_currentResetMove];
                    this.transform.position = moveData.Position;
                    ChangeFacingDirection(moveData.Direction);
                    
                    _currentResetMove--;
                    if (_currentResetMove == -1)
                    {
                        _isResetting = false;
                        _playerInput.Player.Enable();
                        playerMoves = new List<MoveData>();
                    }
                }
                
            }
        }

        private void TileManagerOnonCollectedSun()
        {
            suns++;
        }

        public void InitPlayer(Vector3 position)
        {
            _remainingMoves = 10; ;
            this.transform.position = position;
            var newMove = new MoveData(this.transform, Extensions.Direction.Up);
            playerMoves.Add(newMove);
        }
        private void MoveOnPerformed(InputAction.CallbackContext obj)
        {
            var input = obj.ReadValue<Vector2>();
            var direction = Extensions.ToDirection(input);
            
            if (_remainingMoves > 0) {
                Move(input);
                var newMove = new MoveData(this.transform,direction);
                ChangeFacingDirection(direction);
                playerMoves.Add(newMove);
                _remainingMoves--;
            }
            else
            {
                ResetPlayer();
            }
            if (_remainingMoves == 0) ResetPlayer();
            OnPlayerMove?.Invoke(_remainingMoves);
        }

        private void ChangeFacingDirection(Extensions.Direction direction)
        {
            switch (direction)
            {
               case Extensions.Direction.Right:
                    spriteRenderer.sprite = right;
                    break;
               case Extensions.Direction.Left:
                    spriteRenderer.sprite = left;
                    break;

               case Extensions.Direction.Up:
                    spriteRenderer.sprite = up;
                    break;

               case Extensions.Direction.Down:
                    spriteRenderer.sprite = down;
                    break;
            }

            currentDirection = direction;
        }
        
        public void ResetRemainingMoves()
        {
            _remainingMoves = 10;
            OnPlayerMove?.Invoke(_remainingMoves);
        }

        public void ResetPlayer()
        {
            ResetRemainingMoves();
            OnPlayerReset?.Invoke();
            _playerInput.Player.Disable();
            _isResetting = true;
            _currentResetMove = playerMoves.Count-1;
            elapsedTimeResettingInMilliseconds = 0;
        }
        
        
    }
    

    
}

