//#define IS_GROUNDED
//#if UNITY_STANDALONE, UNITY_IOS

using InputSystem;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float SpeedMultiplier = 5F;

    private Rigidbody _rb;
    private Controls _controls;

    private void Awake()
    {
        _controls = new Controls();
        _controls.AnyPlayer.Move.performed += context => Move();

        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable() => _controls.Enable();

    private void OnDisable() => _controls.Disable();

    private void Update()
    {
        Move();
    }

#if !IS_GROUNDED
    private void Move()
    {
        // 0 in new velocity might delay movements in Y axes
        var dir2 = _controls.AnyPlayer.Move.ReadValue<Vector2>();

        if (dir2 != Vector2.zero)
        {
            var dir3 = new Vector3(dir2.x, 0, dir2.y);
                
            // heading
            transform.rotation = Quaternion.LookRotation(dir3);

            _rb.velocity = dir3 * SpeedMultiplier;
        }
        else
        {
            _rb.velocity = Vector3.zero;
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