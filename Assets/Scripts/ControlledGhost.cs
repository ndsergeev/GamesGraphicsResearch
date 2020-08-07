using System;
using InputSystem;
using Player;
using UnityEngine;

public class ControlledGhost : ControlledPlayer
{
    private Rigidbody _rb;

    #region InitSingleton

    private Controls _controls;
    
    private void OnEnable() => _controls.Enable();
    private void OnDisable() => _controls.Disable();

    private void InitControls()
    {
        _controls = new Controls();
        _controls.AnyPlayer.Move.performed += context => Move();
    }

    #endregion
    

    private void Awake()
    {
        InitControls();
    }

    private void Start()
    {
        AddToSubject();
        _rb = GetComponent<Rigidbody>();
    }

    #region Observed

    public override void HandleOnNotify()
    {
        Debug.Log("Object name " + gameObject.name);
    }

    #endregion
    
    private void Update()
    {
        Move();
    }

#if !IS_GROUNDED

    private void Move()
    {
        var dir2 = _controls.AnyPlayer.Move.ReadValue<Vector2>();
        
        Navigation.Move(_rb, transform, dir2);
    }
    
#else
    private bool _isGrounded = false;

    private void Move()
    {
        if (!_isGrounded) return;
        
        var dir2 = _controls.AnyPlayer.Move.ReadValue<Vector2>();
        Navigation.Move(_rb, transform, dir2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground")) return;
        
        _isGrounded = true;
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground")) return;
        
        _isGrounded = false;
    }
#endif
}