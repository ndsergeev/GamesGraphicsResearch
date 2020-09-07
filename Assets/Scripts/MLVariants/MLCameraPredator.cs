using Player;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class MLCameraPredator : MLPlayer
{
    private Rigidbody _rb;
    // private CameraSensorComponent _cameraSensor;
    
    [SerializeField]
    private const float TinyReward = 0.002f;
    // [SerializeField]
    // private const float MidReward = 0.4f;
    [SerializeField]
    private const float BigReward = 1f;

    public override void Initialize()
    {
        _rb = GetComponent<Rigidbody>();
        
        AddToSubject();
        
        playerType = PlayerType.Predator;
        enemyType = playerType == PlayerType.Prey ? PlayerType.Predator : PlayerType.Prey;
        Speed = playerType == PlayerType.Prey ? 5.0f : 5.5f;
        
        // it is the most beautiful part of ML Agent library, 
        // CameraSensor isn't a component you can use, 
        // but to get a COMPONENT you need to use CameraSensorComponent
        GetComponent<CameraSensorComponent>().Camera = GameManager.GameManagerInstance.mainCamera;
    }

    public override void OnEpisodeBegin()
    { }

    public override void CollectObservations(VectorSensor sensor)
    { }

    public override void OnActionReceived(float[] vectorAction)
    {
        Navigation.MoveMLAgent(_rb, transform, vectorAction);
        
        // punish for being inactive
        AddReward(-TinyReward);
    }
    
    #region Observed

    public override void HandleOnNotify()
    {
        Debug.Log("Object name " + gameObject.name);
    }

    #endregion

    private void OnCollisionEnter(Collision collision)
    {
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
}