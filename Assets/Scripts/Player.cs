//#define IS_GROUNDED
//#if UNITY_STANDALONE, UNITY_IOS

using UnityEngine;

namespace InputSystem
{

    public class Player : MonoBehaviour
    {
        public const float SPEED_MULT = 5F;

        private Rigidbody m_rb;
        private Controls m_controls;

        private void Awake()
        {
            m_controls = new Controls();
            m_controls.AnyPlayer.Move.performed += context => Move();

            m_rb = GetComponent<Rigidbody>();
        }

        private void OnEnable() => m_controls.Enable();

        private void OnDisable() => m_controls.Disable();

        private void Update()
        {
            Move();
        }

#if !IS_GROUNDED
        private void Move()
        {
            {
                // 0 in new velocity might delay movements in Y axes
                Vector2 dir2 = m_controls.AnyPlayer.Move.ReadValue<Vector2>();
                Vector3 dir3 = new Vector3(dir2.x, 0, dir2.y);

                // heading
                transform.rotation = Quaternion.LookRotation(dir3);

                m_rb.velocity = dir3 * SPEED_MULT;
            }
        }
#else
        private bool m_isGrounded = false;

        private void Start()
        {
            Debug.Log("IS_GROUNDED is defined");
        }

        private void Move()
        {
            if (m_isGrounded)
            {
                Vector2 dir = m_controls.AnyPlayer.Move.ReadValue<Vector2>();
                m_rb.velocity = new Vector3(dir.x, 0, dir.y) * SPEED_MULT;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                m_isGrounded = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                m_isGrounded = false;
            }
        }
#endif
    }
}