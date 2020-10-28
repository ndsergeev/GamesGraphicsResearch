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
        protected bool IsStunned = false;

        protected const float TinyReward = 0.002f;
        protected const float MidReward = 0.5f;
        protected const float BigReward = 1f;
        protected const float HugeReward = 2f;
        
        protected Rigidbody Rb;

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
        
        public override void Heuristic(float[] actionsOut)
        {
            /*
             * ToDo: adapt new unity input system
             */
            actionsOut[0] = Input.GetAxis("Debug Horizontal");
            actionsOut[1] = Input.GetAxis("Debug Vertical");
            // actionsOut[2] = Input.GetAxis("Debug Rotation");
        }
    }
}