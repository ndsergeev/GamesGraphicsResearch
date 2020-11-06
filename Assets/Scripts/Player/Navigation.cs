using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public static class Navigation
    {
        private static readonly Vector3 Forward = Vector3.forward;
        private static readonly Vector3 Back = -Vector3.forward;
        private static readonly Vector3 Right = Vector3.right;
        private static readonly Vector3 Left = -Vector3.right;

        private static readonly Vector3 FAndR = (Forward + Right).normalized;
        private static readonly Vector3 FAndL = (Forward + Left).normalized;
        private static readonly Vector3 BAndR = (Back + Right).normalized;
        private static readonly Vector3 BAndL = (Back + Left).normalized;
        
        #region General Movement Behaviour

        private const float SpeedMultiplier = 5f;

        public static void Move(Rigidbody rb, Transform transform, Vector2 direction2, float speed = SpeedMultiplier)
        {
            var direction3 = new Vector3(direction2.x, 0, direction2.y);
        
            AddVelocity(rb, transform, direction3, speed);
        }
    
        public static void Move(Rigidbody rb, Transform transform, Vector3 direction3, float speed = SpeedMultiplier)
        {
            AddVelocity(rb, transform, direction3, speed);
        }
    
        private static void AddVelocity(Rigidbody rb, Transform transform, Vector3 direction3, float speed)
        {
            if (direction3 != Vector3.zero)
            {
                // heading
                transform.rotation = Quaternion.LookRotation(direction3);

                rb.velocity = direction3 * speed;
            } else {
                rb.velocity = Vector3.zero;
            }
        }

        #endregion

        #region ML Discrete and Contnuous Action Space Movement

#if CONTINUOUS_ACTION_SPACE
        public static void MoveMLAgent(Rigidbody rb, Transform transform, IReadOnlyList<float> act, float speed = SpeedMultiplier)
        {
            // Discrete Action Space Move:
        
            var action = Mathf.FloorToInt(act[0]);

            switch (action)
            {
                case 1:
                    Move(rb, transform, Forward, speed);
                    break;
                case 2:
                    Move(rb, transform, Back, speed);
                    break;
                case 3:
                    Move(rb, transform, Right, speed);
                    break;
                case 4:
                    Move(rb, transform, Left, speed);
                    break;
                /* Note:
                 * these 4 are options to move in 4 more directions
                 * for the training the number can be controlled via
                 * Unity's GUI in Action Vector array size 4 or 8 */
                case 5:
                    Move(rb, transform, FAndL, speed);
                    break;
                case 6:
                    Move(rb, transform, FAndR, speed);
                    break;
                case 7:
                    Move(rb, transform, BAndL, speed);
                    break;
                case 8:
                    Move(rb, transform, BAndR, speed);
                    break;
            }
        }
#else
    public static void MoveMLAgent(Rigidbody rb, Transform transform, IReadOnlyList<float> act, float speed=SpeedMultiplier)
    {
        /* Continuous Action Space Move:
        act [0] back - forward, -1 -> 1
        act [1] left - right  , -1 -> 1
        */
        
        var direction3 = new Vector3(act[0], 0, act[1]).normalized;
        
        Move(rb, transform, direction3, speed);
    }

    public static void MoveRotateMLAgent(Rigidbody rb, Transform transform, IReadOnlyList<float> act,
        float speed = SpeedMultiplier)
    {
        /* Continuous Action Space Rotate:
        act [0] left - right  , -1 -> 1
        Continuous Action Space Move:
        act [1] back - forward, -1 -> 1
        act [2] left - right  , -1 -> 1
        */
        
        var rotation = transform.up * act[0];
        transform.Rotate(rotation, Time.fixedDeltaTime * 180f);

        // #1
        // var x = act[1];
        // var z = Mathf.Clamp01(act[2]);
        // var direction3 = new Vector3(x, 0, z).normalized;
        // direction3 *= speed;
        
        // #2
        // var direction3 = new Vector3(act[1], 0, act[2]).normalized;
        // direction3 *= speed;

        // var direction3 = Mathf.Clamp01(act[1]) * Vector3.forward * speed;
        var direction3 = Mathf.Clamp01(act[1]) * Vector3.forward;
        rb.AddRelativeForce(direction3, ForceMode.VelocityChange);
    }
#endif

        #endregion
        
        #region General Jump Behaviour
        
        /* Right now Jumping behaviour is a bit unhealthy
         The better implementation is when an agent touches the upper part of a surface to reset IsGrounded bool 
         variable. However is shouldn't reset when an agent collides with a side of the same obstacle while in the air.
         */
        private const float FallMultiplier = 2.5f;
        
        /// <summary>
        /// Designed to be changed in FixedUpdate at it takes fixedDeltaTime
        /// </summary>
        /// <param name="rb"></param>
        public static void OnFall(Rigidbody rb)
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (FallMultiplier - 1) * Time.fixedDeltaTime;
            }
        }

        #endregion
    }
}
