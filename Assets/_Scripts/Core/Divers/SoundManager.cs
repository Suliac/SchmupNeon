using UnityEngine;
using Sirenix.OdinInspector;
/// <summary>
/// description
/// <summary>

public class SoundManager : MonoBehaviour
{
    #region Attributes
    private static SoundManager instance;
    public static SoundManager GetSingularity
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
    #endregion

    #region Core
    
    public void PlaySound()
    {

    }
    #endregion
}
