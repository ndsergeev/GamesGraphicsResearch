using System;
using Player;
using UnityEngine;
using Unity.MLAgents.Sensors;
// using Unity.MLAgents.Policies;

public class MLRaySharedBrain : MLPlayer
{
    // ToDo: Call function based on the action space
    // private BehaviorParameters _behaviorParameters;
    
    private Rigidbody _rb;
    
    [SerializeField]
    private const float TinyReward = 0.002f;
    // [SerializeField]
    // private const float MidReward = 0.4f;
    [SerializeField]
    private const float BigReward = 2f;

    #region ML Agent
    
    public override void Initialize()
    {
        // ToDo: Call function based on the action space 
        // _behaviorParameters = GetComponent<BehaviorParameters>();
        _rb = GetComponent<Rigidbody>();
        
        AddToSubject();
        
        playerType = CompareTag("Predator") ? PlayerType.Predator : PlayerType.Prey;
        enemyType = playerType == PlayerType.Prey ? PlayerType.Predator : PlayerType.Prey;
        Speed = playerType == PlayerType.Prey ? 0.6f : 0.65f;
    }

    public override void OnEpisodeBegin()
    {
        ResetPosition();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((int)playerType);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // Navigation.MoveMLAgent(_rb, transform, vectorAction, Speed);
        Navigation.MoveRotateMLAgent(_rb, transform, vectorAction, Speed);
        
        // punish for being inactive a predator and reward a pray for surviving
        if (playerType == PlayerType.Predator)
        {
            AddReward(-TinyReward);
        }
        else // as there are just two Enums
        {
            var gameManager = GameManager.Instance;
            if (gameManager.PreyCount < gameManager.nPreys / 1.5f)
            {
                AddReward(-TinyReward);
            }
            else
            {
                AddReward(TinyReward);
            }
        }
    }

    public override void Heuristic(float[] actionsOut)
    { 
        actionsOut[0] = Input.GetAxis("Debug Horizontal");
        actionsOut[1] = Input.GetAxis("Debug Vertical");
        // actionsOut[2] = Input.GetAxis("Debug Rotation");
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-TinyReward*2);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground")) { return; }
        if (collision.gameObject.CompareTag("Ground")) { return; }
        
        if (collision.gameObject.CompareTag("Wall"))
        {
            AddReward(-BigReward/2);
            return;
        }
        
        // Enemy tag is same as opposite tag or not same as mine
        var collidedMLPlayer = collision.gameObject.GetComponent<MLRaySharedBrain>();

        if (playerType == collidedMLPlayer.playerType)
        {
            AddReward(-BigReward/6);
        }
        else if (enemyType == collidedMLPlayer.playerType)
        {
            if (playerType == PlayerType.Predator)
            {
                AddReward(BigReward);
            }
            else
            {
                // Prey, because enum has two options
                // ToDo: make health system 
                GameManager.Instance.IncrementPreyCount();
                AddReward(-BigReward-BigReward); // double negative reward...
                gameObject.SetActive(false);
            }
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