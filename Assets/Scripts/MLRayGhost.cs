using Player;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class MLRayGhost : MLPlayer
{
    private Rigidbody _rb;
    
    private const float TinyReward = 0.002f;
    private const float BigReward = 1f;

    public override void Initialize()
    {
        PlayerType = PlayerType.Predator;
        _rb = GetComponent<Rigidbody>();
        
        AddToSubject();
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
        if (!collision.gameObject.CompareTag(Tag)) return;
        /* ToDo:
             * Add enum to Notify() so switch-case expression can be used
             */
        AddReward(BigReward);
        collision.gameObject.GetComponent<MLPlayer>().AddReward(-BigReward);
    }
}