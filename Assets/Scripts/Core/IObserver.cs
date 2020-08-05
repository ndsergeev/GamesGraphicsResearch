using UnityEngine;

namespace Core
{
    namespace Observer
    {
        public interface ISubject
        {
            void AddObserver(IObserver observer);
            void RemoveObserver(IObserver observer);
            void NotifyObservers();
        }
        
        public interface IObserver
        {
            void AddToSubject();
            void HandleOnNotify();
        }
        
        public interface IObserverEvent
        {
            void OnEvent(GameObject observer);
        }
    }
}