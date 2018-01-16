using UnityEngine;

/// <summary>
/// StateManager Description
/// </summary>
public class StateManager : MonoBehaviour
{
    public enum GameState
    {
        Menu,
        Tuto,
        Play,
        Credits,
        Pause,
        GameOver,
        Victory
    }
    #region Attributes

    private static StateManager instance;
    public static StateManager GetSingleton
    {
        get { return instance; }
    }

    #endregion

    #region Initialization
    public void SetSingleton()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    private void Awake()
    {
        SetSingleton();
    }

    private void Start()
    {

    }
    #endregion

    #region Core
    [SerializeField]
    private GameState state;
    public GameState State { get { return state; } set { state = value; } }
    #endregion
}
