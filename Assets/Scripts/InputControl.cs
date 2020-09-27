// using UnityEngine;
// using InputSystem;
// using UnityEngine.InputSystem;
//
// public class InputControl : MonoBehaviour
// {
//     
//     #region Init Controls
//
//     private Controls _controls;
//     private InputAction _moveInputAction;
//     
//     private void OnEnable() => _controls.Enable();
//     private void OnDisable() => _controls.Disable();
//
//     private void InitControls()
//     {
//         _controls = new Controls();
//         
// #if UNITY_EDITOR
//         if (Input.GetJoystickNames().Length > 0)
//         {
//             InitXboxController();
//         }
//         else // Use your PC
//         {
//             InitKeyboardController();
//         }
// #elif UNITY_STANDALONE
//         InitKeyboardController();
// #elif UNITY_XBOXONE
//         InitXboxController();
// #elif UNITY_IPHONE
//         // ToDo: Develop input for iOS platform
//         InitIOSController();
// #endif
//     }
//
//     private void InitKeyboardController()
//     {
//         _controls.PCPlayer.Move.performed += context => Move();
//         _moveInputAction = _controls.PCPlayer.Move;
//     }
//
//     private void InitXboxController()
//     {
//         _controls.XBOXPlayer.Move.performed += context => Move();
//         _moveInputAction = _controls.XBOXPlayer.Move;
//     }
//
//     private void InitIOSController()
//     {
//         // ToDo: Handle input for iOS platform
//         throw new System.NotImplementedException();
//     }
//     #endregion
//     
//     private void Awake()
//     {
//         InitControls();
//     }
//
//     private void Update()
//     {
//         Move();
//     }
//     
//     private bool _isGrounded = false;
//
//     private static void Move()
//     {
//         // if (!_isGrounded) return;
//         
//         // var dir2 = _moveInputAction.ReadValue<Vector2>();
//         // Navigation.Move(rb, transform, dir2);
//     }
// }
//
