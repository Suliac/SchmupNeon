using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// ExternalEntityActivation Description
/// </summary>
public class ExternalEntityActivation : MonoBehaviour
{
    #region Attributes

	[Tooltip("opti fps"), SerializeField]
	private FrequencyTimer updateTimer;

    [FoldoutGroup("Gameplay"), Tooltip("Ennemi à activer"), SerializeField]
    private BaseEnemy enemyToActivate;

    private IsOnCamera isOnCamera;
    #endregion

    #region Initialization

    private void Awake()
    {
        isOnCamera = GetComponent<IsOnCamera>();
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
            if (isOnCamera && enemyToActivate)
            {
                if (isOnCamera.isOnScreen)
                    enemyToActivate.WantToEnable = true;
            }
      }
    }

	#endregion
}
