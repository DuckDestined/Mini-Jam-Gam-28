using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts
{
    
    public class Moving : MonoBehaviour
    {
        [SerializeField] private TileManager tileManager;
        protected Vector2 currentDirection;
        private Vector3 _targetLocation;
        
        private protected void Move(Vector2 direction)
        {
            if (Math.Abs(direction.x - direction.y) > 0.01f)
            {
                _targetLocation = new Vector3(Mathf.RoundToInt(direction.x),Mathf.RoundToInt(direction.y)) + transform.position;
                if (tileManager.IsTileAvailable(this.transform.position,currentDirection,direction,_targetLocation))
                {
                    transform.position = _targetLocation;
                } 
                
            }
        }
    }
}
