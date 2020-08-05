using System.Collections.Generic;
using Core.Observer;
using UnityEngine;

public class Manager : MonoBehaviour, ISubject
{
    #region SingletonImplementation

    private static Manager _instance;
    public static Manager Instance { get { return _instance; } }

    private void InitSingleton()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region SubjectImplementation

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

    public GameObject controlledPrefab;
    public GameObject mlPrefab;
    
    private static float _timer;
    private const float TimeLimit = 3f;

    // ToDo: double-check is there a need to use these below
    // private List<ControlledPlayer> _controlledPlayers;
    // private List<MLPlayer> _mlPlayers;

    private void Awake()
    {
        InitSingleton();
        InitSubject();

        SpawnPlayers();
    }
    
    #region SpawnPlayers

    private void SpawnPlayers()
    {
        // ToDo: make it to be list of GameObject or list of lists of GO
        Instantiate(controlledPrefab, Vector3.zero, Quaternion.identity, Instance.transform);
        
        Instantiate(mlPrefab, Vector3.forward * 3, Quaternion.identity, Instance.transform);
    }

    #endregion

    private void Start()
    {
        Debug.Log("Observer Count: " + _observers.Count.ToString());
        
        NotifyObservers();
    }

    private void Update()
    {
        // if (_timer > TimeLimit)
        // {
        //     // ToDo: example to notify two types of agents every <_timer> seconds
        //     NotifyObservers();
        //     _timer = 0f;
        // }
        //
        // _timer += Time.deltaTime;
    }
}