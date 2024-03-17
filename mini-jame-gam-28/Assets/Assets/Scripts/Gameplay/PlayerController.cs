using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class PlayerController : Moving
    {
        private PlayerInput _playerInput;
        private int _remainingMoves;
        private int suns = 0; 

        public delegate void PlayerMoveAction(int remainingMoves);
        public static PlayerMoveAction OnPlayerMove;

        [SerializeField] private List<Vector3> playerMoves;
        
        // Start is called before the first frame update
        void Start()
        {
            _playerInput = new PlayerInput();
            _playerInput.Enable();
            _playerInput.Player.Move.performed += MoveOnPerformed;
            TileManager.OnCollectedSun += TileManagerOnonCollectedSun;
            if (OnPlayerMove != null) OnPlayerMove(_remainingMoves);
            _remainingMoves = 0;
        }

        private void TileManagerOnonCollectedSun()
        {
            suns++;
        }

        public void InitPlayer(Vector3 position)
        {
            _remainingMoves = 10; ;
            transform.position = position;
            playerMoves.Add(position);
        }
        private void MoveOnPerformed(InputAction.CallbackContext obj)
        {
            var input = obj.ReadValue<Vector2>();
            
            if (_remainingMoves > 0) {
                Move(input);
                playerMoves.Add(this.transform.position);
                _remainingMoves--;
            }
            else
            {
                ResetPlayer();
            }
            if (OnPlayerMove != null) OnPlayerMove(_remainingMoves);
        }
        
        
        public void ResetRemainingMoves()
        {
            _remainingMoves = 10;
            if (OnPlayerMove != null) OnPlayerMove(_remainingMoves);
        }

        public void ResetPlayer()
        {
            ResetRemainingMoves();
            transform.position = playerMoves[0];
            
        }

    }
}
