using UnityEngine;

namespace Player
{
    public static class Navigation
    {
        private const float SpeedMultiplier = 5f;

        public static void Move(Rigidbody rb, Transform transform, Vector2 direction2)
        {
            var direction3 = new Vector3(direction2.x, 0, direction2.y);
        
            AddVelocity(rb, transform, direction3);
        }
    
        public static void Move(Rigidbody rb, Transform transform, Vector3 direction3)
        {
            AddVelocity(rb, transform, direction3);
        }
    
        private static void AddVelocity(Rigidbody rb, Transform transform, Vector3 direction3)
        {
            if (direction3 != Vector3.zero)
            {
                // heading
                transform.rotation = Quaternion.LookRotation(direction3);

                rb.velocity = direction3 * SpeedMultiplier;
            } else {
                rb.velocity = Vector3.zero;
            }
        }
    }
}
