using Player;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class MLRaySharedBrain : MLPlayer
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
        _rb = GetComponent<Rigidbody>();
        
        AddToSubject();
        
        playerType = CompareTag("Predator") ? PlayerType.Predator : PlayerType.Prey;
        enemyType = playerType == PlayerType.Prey ? PlayerType.Predator : PlayerType.Prey;
        Speed = playerType == PlayerType.Prey ? 5.0f : 5.5f;
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
        Navigation.MoveMLAgent(_rb, transform, vectorAction, Speed);
        
        // punish for being inactive a predator and reward a pray for surviving
        if (playerType == PlayerType.Predator)
        {
            AddReward(-TinyReward);
        }
        else // as there are just two Enums
        {
            AddReward(TinyReward);
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground")) { return; }
        
        // Enemy tag is same as opposite tag or not same as mine
        var mlPlayer = collision.gameObject.GetComponent<MLRaySharedBrain>();

        if (enemyType == mlPlayer.playerType)
        {
            if (playerType != PlayerType.Prey) return;
            
            // ToDo: make health system 
            collision.gameObject.SetActive(false);
            GameManager.GameManagerInstance.IncrementPreyCount();
            AddReward(-BigReward);
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