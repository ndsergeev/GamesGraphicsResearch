using Player;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class MLRayPredator : MLPlayer
{
    private Rigidbody _rb;
    
    [SerializeField]
    private const float TinyReward = 0.002f;
    // [SerializeField]
    // private const float MidReward = 0.4f;
    [SerializeField]
    private const float BigReward = 1f;

    #region ML Agent
    
    public override void Initialize()
    {
        playerType = PlayerType.Predator;
        enemyType = playerType == PlayerType.Prey ? PlayerType.Predator : PlayerType.Prey;
        Speed = playerType == PlayerType.Prey ? 5.0f : 5.5f;
        
        _rb = GetComponent<Rigidbody>();
        
        AddToSubject();
    }

    public override void OnEpisodeBegin()
    {
        ResetPosition();
    }

    public override void CollectObservations(VectorSensor sensor)
    { }

    public override void OnActionReceived(float[] vectorAction)
    {
        Navigation.MoveMLAgent(_rb, transform, vectorAction);
        
        // punish for being inactive
        AddReward(-TinyReward);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground")) { return; }
        
        // Predator
        var mlPlayer = collision.gameObject.GetComponent<MLPlayer>();
        if (enemyType == mlPlayer.playerType)
        {
            AddReward(BigReward);
        }
        else if (playerType == mlPlayer.playerType)
        {
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