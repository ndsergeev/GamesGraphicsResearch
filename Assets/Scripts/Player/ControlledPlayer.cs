using Core.Observer;
using UnityEngine;

namespace Player
{
    public abstract class ControlledPlayer : MonoBehaviour, IObserver
    {
        protected PlayerType PlayerType;
        public virtual void AddToSubject()
        {
            GameManager.GameGameManagerInstance.AddObserver(this);
        }

        public abstract void HandleOnNotify();
    }
}