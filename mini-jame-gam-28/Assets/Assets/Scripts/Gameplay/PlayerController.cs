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
            public MoveData(Transform playerTransform,Vector2 direction)
            {
                Position = playerTransform.position;
                Direction = direction;
            }
            
            public Vector3 Position;
            public Vector2 Direction;
        }
        
        private PlayerInput _playerInput;
        private int _remainingMoves;
        private int suns = 0;
        
        
        
        
        private bool _isResetting;
        [SerializeField] private int elapsedTimeResettingInMilliseconds;

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite up;
        [SerializeField] private Sprite down;
        [SerializeField] private Sprite right;
        [SerializeField] private Sprite left;

        private int _currentResetMove;

        public delegate void PlayerMoveAction(int remainingMoves,Vector2 currentDirection);
        public static PlayerMoveAction OnPlayerMove;        
        
        public delegate void PlayerResetAction();
        public static PlayerResetAction OnPlayerReset;

        [SerializeField] private List<MoveData> playerMoves;
        
        // Start is called before the first frame update
        void Start()
        {
            _playerInput = new PlayerInput();
            _playerInput.Enable();
            _playerInput.Player.Move.performed += MoveOnPerformed;
            TileManager.OnCollectedSun += TileManagerOnonCollectedSun;
            OnPlayerMove?.Invoke(_remainingMoves,new Vector2());
            playerMoves = new List<MoveData>();
            _remainingMoves = 0;
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
            var newMove = new MoveData(this.transform, new Vector2(0,0));
            playerMoves.Add(newMove);
        }
        private void MoveOnPerformed(InputAction.CallbackContext obj)
        {
            var direction = obj.ReadValue<Vector2>();
            
            if (_remainingMoves > 0) {
                Move(direction);
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
            OnPlayerMove?.Invoke(_remainingMoves,currentDirection);
        }

        private void ChangeFacingDirection(Vector2 direction)
        {
            switch (direction.x)
            {
                case > 0:
                    spriteRenderer.sprite = right;
                    return;
                case < 0:
                    spriteRenderer.sprite = left;
                    return;            
            }            
            switch (direction.y)
            {
                case > 0:
                    spriteRenderer.sprite = up;
                    return;                
                case < 0:
                    spriteRenderer.sprite = down;
                    return;            
            }

            spriteRenderer.sprite = up;
        }
        
        public void ResetRemainingMoves()
        {
            _remainingMoves = 10;
            OnPlayerMove?.Invoke(_remainingMoves,currentDirection);
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

