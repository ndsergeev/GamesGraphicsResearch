using Player;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class MLSRayPredator : MLPlayer
{
    // ToDo: Call function based on the action space
    // private BehaviorParameters _behaviorParameters;

    #region ML Agent
    
    public override void Initialize()
    {
        // ToDo: Call function based on the action space 
        // _behaviorParameters = GetComponent<BehaviorParameters>();
        Rb = GetComponent<Rigidbody>();
        
        AddToSubject();
        
        playerType = PlayerType.Predator;
        enemyType = PlayerType.Prey;
        Speed = playerType == PlayerType.Prey ? 0.6f : 0.65f;
    }

    public override void OnEpisodeBegin()
    {
        ResetPosition();
    }

    public override void CollectObservations(VectorSensor sensor)
    { }

    public override void OnActionReceived(float[] vectorAction)
    {
        // Navigation.MoveMLAgent(_rb, transform, vectorAction, Speed);
        Navigation.MoveRotateMLAgent(Rb, transform, vectorAction, Speed);
        
        // punish for being inactive a predator and reward a pray for surviving
        AddReward(-TinyReward);
    }

    #endregion

    #region Collision
    
    private void OnCollisionEnter(Collision collision)
    {
        // if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground")) { return; }
        if (collision.gameObject.CompareTag("Ground")) { return; }
        
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
        AddReward(HugeReward);
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