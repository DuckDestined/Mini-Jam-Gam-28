using UnityEngine;

namespace Assets.Scripts.Gameplay
{
    public class Extensions
    {
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        public static Direction ToDirection(Vector2 direction)
        {
            switch (direction.x)
            {
                case > 0:
                    return Direction.Right;
                case < 0:
                    return Direction.Left;
            }

            switch (direction.y)
            {
                case > 0:
                    return Direction.Up;
                case < 0:
                    return Direction.Down;
            }

            return Direction.Up;
        }
    }
}
    