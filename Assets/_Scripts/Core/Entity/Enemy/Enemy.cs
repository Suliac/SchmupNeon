using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Ennemy Description
/// </summary>
public class Enemy : MonoBehaviour, IKillable
{
    #region Attributes

	[Tooltip("opti fps"), SerializeField]
	private FrequencyTimer updateTimer;

    #endregion

    #region Initialization

    private void Start()
    {
		// Start function
    }
    #endregion

    #region Core

    #endregion

    #region Unity ending functions

    private void Update()
    {
        //optimisation des fps
        if (updateTimer.Ready())
        {

        }
    }

    [FoldoutGroup("Debug"), Button("Kill")]
    public void Kill()
    {
        Debug.Log("Dead");
        Destroy(gameObject);
    }

    #endregion
}
