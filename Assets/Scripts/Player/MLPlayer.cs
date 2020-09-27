using Core.Observer;
using Unity.MLAgents;
using UnityEngine;

namespace Player
{
    public abstract class MLPlayer : Agent, IObserver
    {
        public PlayerType playerType;
        public PlayerType enemyType;
        private Vector3 _resetPosition;
        protected float Speed = 1f;

        public void SetResetPosition(Vector3 resetPosition)
        {
            _resetPosition = resetPosition;
        }

        public void ResetPosition()
        {
            transform.position = _resetPosition;
        }

        public virtual void AddToSubject()
        {
            GameManager.Instance.AddObserver(this);
        }

        public abstract void HandleOnNotify();

        // ToDo: Implement 
        // protected abstract void Move();
    }
}