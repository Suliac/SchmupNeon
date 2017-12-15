using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// BaseEnemy Description
/// </summary>
public abstract class BaseEnemy : MonoBehaviour, IKillable
{
    #region Attributes

    [Tooltip("opti fps"), SerializeField]
    private FrequencyTimer updateTimer;

    #endregion

    #region Initialization

    #endregion

    #region Core
    abstract protected void Behavior();

    [FoldoutGroup("Debug"), Button("Kill")]
    public void Kill()
    {
        Debug.Log("Dead");
        Destroy(gameObject);
    }
    #endregion

    #region Unity ending functions

    protected void Update()
    {
        //optimisation des fps
        if (updateTimer.Ready())
        {
            Behavior();
        }
    }

    #endregion
}
