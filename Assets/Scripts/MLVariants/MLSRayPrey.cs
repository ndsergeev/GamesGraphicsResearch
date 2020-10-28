using System;
using Player;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class MLSRayPrey : MLPlayer
{
    private const float HealthMax = 100f;
    private float _currentHealth = HealthMax;
    public HealthBar healthBar;
    
    private bool _hasThrowable = false;
    
    // ToDo: Call function based on the action space
    // private BehaviorParameters _behaviorParameters;

    #region ML Agent
    
    public override void Initialize()
    {
        // ToDo: Call function based on the action space 
        // _behaviorParameters = GetComponent<BehaviorParameters>();
        Rb = GetComponent<Rigidbody>();

        AddToSubject();
        
        playerType = PlayerType.Prey;
        enemyType = PlayerType.Predator;
        Speed = playerType == PlayerType.Prey ? 0.6f : 0.65f;

        if (healthBar)
        {
            healthBar.SetMaxHealth(HealthMax);
        }
    }

    public override void OnEpisodeBegin()
    {
        ResetPosition();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(_hasThrowable);
        sensor.AddObservation(_currentHealth/HealthMax);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // Navigation.MoveMLAgent(_rb, transform, vectorAction, Speed);
        Navigation.MoveRotateMLAgent(Rb, transform, vectorAction, Speed);
        
        var gameManager = GameManager.Instance;
        if (gameManager.PreyCount < gameManager.nPreys/1.5f)
        {
            AddReward(-TinyReward);
        }
        else
        {
            AddReward(TinyReward);
        }
    }

    #endregion

    #region Collision

    private void OnTriggerEnter(Collider trigger)
    {
        if (!trigger.gameObject.CompareTag("Pickup") || _hasThrowable) return;
        
        trigger.gameObject.GetComponent<Pickup>().Hide();
        _hasThrowable = true;
        
        AddReward(MidReward);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) return;
        
        if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-HugeReward/2);
            return;
        }

        // Enemy tag is same as opposite tag or not same as mine
        var collidedMLPlayer = collision.gameObject.GetComponent<MLPlayer>();
        if (playerType == collidedMLPlayer.playerType)
        {
            AddReward(-HugeReward/6);
            return;
        }
        
        // because there are TWO types of enum
        _currentHealth -= 25f;
        healthBar.SetHealth(_currentHealth);
        AddReward(-BigReward);
        if (_currentHealth > 0f) return;
        
        // if no Health Points
        GameManager.Instance.IncrementPreyCount();
        AddReward(-HugeReward-HugeReward);
        gameObject.SetActive(false);
    }
    
    // private void OnCollisionStay(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Wall"))
    //     {
    //         AddReward(-TinyReward*2);
    //     }
    // }
    
    #endregion
    
    #region Observed

    public override void HandleOnNotify()
    {
        Debug.Log("Object name " + gameObject.name);
    }

    #endregion
}