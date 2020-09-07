using System;
using System.Collections.Generic;
using Core.Observer;
using Player;
using UnityEngine;

public class GameManager : MonoBehaviour, ISubject
{
    #region Singleton Implementation

    private static GameManager _gameManagerInstance;
    public static GameManager GameManagerInstance { get { return _gameManagerInstance; } }

    private void InitSingleton()
    {
        if (_gameManagerInstance != null && _gameManagerInstance != this)
        {
            Destroy(gameObject);
            return;
        }

        _gameManagerInstance = this;
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

    // ToDo: double-check is there a need to use these below
    // private List<ControlledPlayer> _controlledPlayers;
    // private List<MLPlayer> _mlPlayers;

    private void Awake()
    {
        InitSingleton();
        InitSubject();

        SpawnAllPlayers();
    }

    private void Update()
    {
        if (_preyCount < nPreys) { return; }
        // NotifyObservers();
        foreach (var pred in _mlPlayers)
        {
            pred.gameObject.SetActive(true);
            pred.EndEpisode();
        }
        ResetPreyCount();
    }

    #region Prey Counter
    
    private int _preyCount = 0;
    public void ResetPreyCount()
    {
        _preyCount = 0;
    }
    public void IncrementPreyCount()
    {
        _preyCount++;
    }
    
    #endregion
    
    #region Spawn Players
    
    public int nPredators;
    public int nPreys;
    public float xOffset;
    public float zOffset;

    public List<GameObject> prefabs;
    public List<GameObject> predatorCharacters;
    public List<GameObject> preyCharacters;
    private readonly List<MLPlayer> _mlPlayers = new List<MLPlayer>();
    
    private void SpawnAllPlayers()
    {
        if (prefabs.Count < 1) return;
        
        // init Predators
        SpawnPlayer(prefabs[0], predatorCharacters, nPredators, zOffset, new Vector3(0,180,0));

        // init Preys
        SpawnPlayer(prefabs[1], preyCharacters, nPreys, -zOffset, new Vector3(0,0,0));
    }

    private void SpawnPlayer(GameObject ofPrefab, IReadOnlyList<GameObject> fromCharacterArray, int nPlayers, float atZOffset, Vector3 atAngle)
    {
        var spawnRow =  CalculateFirstNegativeOffsetXPosition(xOffset, 0f, atZOffset, nPlayers);
        for (var i = 0; i < nPlayers; ++i)
        {
            var prefab = Instantiate(ofPrefab,
                spawnRow,
                Quaternion.Euler(atAngle),
                GameManagerInstance.transform);
            var player = Instantiate(fromCharacterArray[i],
                spawnRow,
                Quaternion.Euler(atAngle),
                prefab.transform);
            var component = prefab.GetComponent<MLPlayer>();
            component.SetResetPosition(spawnRow);
            _mlPlayers.Add(component);
            spawnRow.x += xOffset;
        }
    }

    private static Vector3 CalculateFirstNegativeOffsetXPosition(float xOffset, float yOffset, float zOffset, int n)
    {
        var newXOffset = -xOffset * (n - 1) / 2;
        return new Vector3(newXOffset, yOffset, zOffset);
    }

    #endregion
}