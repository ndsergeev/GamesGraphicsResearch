using System.Collections.Generic;
using Core.Observer;
using UnityEngine;

public class GameManager : MonoBehaviour, ISubject
{
    #region Singleton Implementation

    private static GameManager _gameGameManagerInstance;
    public static GameManager GameGameManagerInstance { get { return _gameGameManagerInstance; } }

    private void InitSingleton()
    {
        if (_gameGameManagerInstance != null && _gameGameManagerInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        _gameGameManagerInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region Subject Implementation

    private List<IObserver> _observers;

    private void InitSubject()
    {
        _observers = new List<IObserver>();
    }
    
    public void AddObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (var player in _observers)
        {
            // drop here condition if required
            player.HandleOnNotify();
        }
    }

    #endregion

    public Camera mainCamera;
    public List<GameObject> prefabs;

    // ToDo: double-check is there a need to use these below
    // private List<ControlledPlayer> _controlledPlayers;
    // private List<MLPlayer> _mlPlayers;

    private void Awake()
    {
        InitSingleton();
        InitSubject();

        SpawnPlayers();
    }
    
    #region Spawn Players

    private void SpawnPlayers()
    {
        if (prefabs.Count < 1) return;
        
        foreach (var prefab in prefabs)
        {
            Instantiate(prefab, Vector3.zero, Quaternion.identity, GameGameManagerInstance.transform);    
        }
    }

    #endregion

    private void Start()
    {
        // Double check all observers initialised properly, REMOVE when confident
        // Debug.Log("Observer Count: " + _observers.Count.ToString());
        
        NotifyObservers();
    }
}