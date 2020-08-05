using System;
using Core.Observer;
using Unity.MLAgents;
using UnityEngine;

namespace Player
{
    public abstract class MLPlayer : Agent, IObserver
    {
        protected static readonly Vector3 Forward = Vector3.forward;
        protected static readonly Vector3 Back = -Vector3.forward;
        protected static readonly Vector3 Right = Vector3.right;
        protected static readonly Vector3 Left = -Vector3.right;
        
        public virtual void AddToSubject()
        {
            Manager.Instance.AddObserver(this);
        }

        public abstract void HandleOnNotify();

        // ToDo: Implement 
        // protected abstract void Move();
    }
}