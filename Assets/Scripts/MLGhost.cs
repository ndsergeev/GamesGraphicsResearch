using System;
using System.Collections.Generic;
using Player;
using UnityEngine;
using Unity.MLAgents.Sensors;

public class MLGhost : MLPlayer
{
    private Rigidbody _rb;
    
    private const string Tag = "Player";
    private const float TinyNegativeReward = -0.002f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public override void Initialize()
    {
        AddToSubject();
    }

    public override void OnEpisodeBegin()
    { }

    public override void CollectObservations(VectorSensor sensor)
    { }

    public override void OnActionReceived(float[] vectorAction)
    {
        Move(vectorAction);
        
        // punish for being inactive
        AddReward(TinyNegativeReward);
    }
    
    #region Observed

    public override void HandleOnNotify()
    {
        Debug.Log("Object name " + gameObject.name);
    }

    #endregion

#if !CONTINUOUS_ACTION_SPACE
    private void Move(IReadOnlyList<float> act)
    {
        // Discrete Action Space Move:
        
        var action = Mathf.FloorToInt(act[0]);

        switch (action)
        {
            case 1:
                Navigation.Move(_rb, transform, Forward);
                break;
            case 2:
                Navigation.Move(_rb, transform, Back);
                break;
            case 3:
                Navigation.Move(_rb, transform, Right);
                break;
            case 4:
                Navigation.Move(_rb, transform, Left);
                break;
        }
    }
#else
    private void Move(IReadOnlyList<float> act)
    {
        // Continuous Action Space Move:
        // act [0] back - forward, -1 -> 1
        // act [1] left - right  , -1 -> 1
        
        var direction3 = new Vector3(act[0], 0, act[1]);
        
        Navigation.Move(_rb, transform, direction3);
    }
#endif

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