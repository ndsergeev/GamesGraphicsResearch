using Core.Observer;
using UnityEngine;

namespace Player
{
    public abstract class ControlledPlayer : MonoBehaviour, IObserver
    {
        public virtual void AddToSubject()
        {
            GameManager.Instance.AddObserver(this);
        }

        public abstract void HandleOnNotify();
    }
}