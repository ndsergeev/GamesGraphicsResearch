using System;
using Data.Util;
using Player;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class MLRayPrey : MLPlayer
{
    private Rigidbody _rb;
    
    [SerializeField]
    private const float TinyReward = 0.002f;
    // [SerializeField]
    // private const float MidReward = 0.4f;
    [SerializeField]
    private const float BigReward = 1f;

    private RayPerceptionSensorComponent3D _rayPerception;

    #region ML Agent
    
    public override void Initialize()
    {
        playerType = PlayerType.Prey;
        enemyType = playerType == PlayerType.Prey ? PlayerType.Predator : PlayerType.Prey;
        Speed = playerType == PlayerType.Prey ? 7.5f : 8.0f;
        
        _rb = GetComponent<Rigidbody>();
        _rayPerception = GetComponent<RayPerceptionSensorComponent3D>();
        
        // _rayPerception.
        // _rayPerception.RaySensor

        AddToSubject();
    }

    public override void OnEpisodeBegin()
    {
        ResetPosition();
    }

    // public override void CollectObservations(VectorSensor sensor)
    // { }

    public override void OnActionReceived(float[] vectorAction)
    {
        Navigation.MoveMLAgent(_rb, transform, vectorAction);
        
        // encourage for being alive!
        AddReward(TinyReward);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground")) { return; }
        
        // Prey
        var mlPlayer = collision.gameObject.GetComponent<MLPlayer>();
        if (enemyType == mlPlayer.playerType)
        {
            AddReward(-BigReward);
            
            // ToDo: make health system 
            collision.gameObject.SetActive(false);
            GameManager.Instance.IncrementPreyCount();
        }
        else if (playerType == mlPlayer.playerType)
        {
            // Don't touch your neighbours
            AddReward(-BigReward/3);
        }
    }
    
    #endregion
    
    #region Observed

    public override void HandleOnNotify()
    {
        Debug.Log("Object name " + gameObject.name);
    }

    #endregion
}