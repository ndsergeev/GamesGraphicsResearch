using Player;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class MLCameraGhost : MLPlayer
{
    private Rigidbody _rb;
    // private CameraSensorComponent _cameraSensor;
    
    private const float TinyNegativeReward = -0.002f;

    public override void Initialize()
    {
        PlayerType = PlayerType.Predator;
        
        // it is the most beautiful part of ML Agent library, 
        // CameraSensor isn't a component you can use, 
        // but to get a COMPONENT you need to use CameraSensorComponent
        GetComponent<CameraSensorComponent>().Camera = GameManager.GameGameManagerInstance.mainCamera;
        
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
        AddReward(TinyNegativeReward);
    }
    
    #region Observed

    public override void HandleOnNotify()
    {
        Debug.Log("Object name " + gameObject.name);
    }

    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Tag))
        {
            /* ToDo:
             * Add enum to Notify() so switch-case expression can be used
             */
        } 
    }
}