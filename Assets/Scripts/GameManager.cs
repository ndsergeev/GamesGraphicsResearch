using System.Collections.Generic;
using System.Linq;
using Core.Observer;
using Player;
using Unity.MLAgents;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour, ISubject
{
    #region Singleton Implementation

    public static GameManager Instance { get; private set; }

    private void InitSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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
        
        // Helps to manually reset the environment 
        // Academy.Instance.AutomaticSteppingEnabled = false;
        // Academy.Instance.EnvironmentParameters
    }
    
    private void Update()
    {
        ResetPickups();
        ResetEnvironment();

        // NotifyObservers();
    }

    #region ML Agent

    public float timeToResetEnv = 128;
    private float _resetEnvironmentTimer;
    
    private void ResetEnvironment()
    {
        _resetEnvironmentTimer -= Time.deltaTime;
        if (_resetEnvironmentTimer > 0 && PreyCount < nPreys) return;
        
        foreach (var player in MLPlayers)
        {
            player.gameObject.SetActive(true);
            player.EndEpisode();
        }
            
        Academy.Instance.EnvironmentStep();
        ResetPreyCount();
        _resetEnvironmentTimer = timeToResetEnv;
    }

    #endregion

    #region Prey Counter

    public int PreyCount { get; private set; }

    private void ResetPreyCount()
    {
        PreyCount = 0;
    }
    
    public void IncrementPreyCount()
    {
        PreyCount++;
    }
    
    #endregion
    
    #region Spawn MLPlayers
    
    public int nPredators;
    public int nPreys;
    public float xOffset;
    public float zOffset;

    public List<GameObject> prefabs;
    public List<GameObject> predatorCharacters;
    public List<GameObject> preyCharacters;
    public readonly List<MLPlayer> MLPlayers = new List<MLPlayer>();
    
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
        if (nPlayers < 1) return;
        
        var spawnRow =  CalculateFirstNegativeOffsetXPosition(xOffset, 0f, atZOffset, nPlayers);
        for (var i = 0; i < nPlayers; ++i)
        {
            var prefab = Instantiate(ofPrefab,
                spawnRow,
                Quaternion.Euler(atAngle),
                Instance.transform);
            // player is unused because it is a child of prefab
            // var player =
            Instantiate(fromCharacterArray[i],
                spawnRow,
                Quaternion.Euler(atAngle),
                prefab.transform);
            
            var component = prefab.GetComponent<MLPlayer>();
            if (component)
            {
                component.SetResetPosition(spawnRow);
                MLPlayers.Add(component);   
            }
            spawnRow.x += xOffset;
        }
    }

    private static Vector3 CalculateFirstNegativeOffsetXPosition(float xOffset, float yOffset, float zOffset, int n)
    {
        var newXOffset = -xOffset * (n - 1) / 2;
        return new Vector3(newXOffset, yOffset, zOffset);
    }

    #endregion

    #region Pickups

    public List<Pickup> pickups = new List<Pickup>();
    
    public float timeToResetPickups = 8f;
    private float _resetPickupsTimer = 0f;
    
    private void ResetPickups()
    {
        _resetPickupsTimer -= Time.deltaTime;
        if (_resetPickupsTimer > 0) return;
        
        // guarantees that all ara hidden
        foreach (var pickup in pickups)
        {
            pickup.Hide();
        }
            
        foreach (var pickup in pickups.Where(pickup => Random.Range(0f, 1f) < 0.5f))
        {
            pickup.Expose();
        }

        _resetPickupsTimer = timeToResetPickups;
    }
    
    #endregion
}