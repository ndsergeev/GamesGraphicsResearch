using Core.Observer;
using Unity.MLAgents;

namespace Player
{
    public abstract class MLPlayer : Agent, IObserver
    {
        protected PlayerType PlayerType;
        protected const string Tag = "Predator";
        public virtual void AddToSubject()
        {
            GameManager.GameGameManagerInstance.AddObserver(this);
        }

        public abstract void HandleOnNotify();

        // ToDo: Implement 
        // protected abstract void Move();
    }
}