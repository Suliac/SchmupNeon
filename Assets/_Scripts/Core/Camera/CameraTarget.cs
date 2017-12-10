using UnityEngine;

/// <summary>
/// Add target to camera
/// <summary>
public class CameraTarget : MonoBehaviour
{
    #region Attributes


	[SerializeField]
	private bool onEnableAdd = true;

	[SerializeField]
	private bool onDisableRemove = true;

    private CameraController cameraController;

    #endregion

    #region Initialisation

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    private void OnEnable()
    {
        if (onEnableAdd)
        {
            AddTarget();
        }
    }
    #endregion

    #region Core
    /// <summary>
    /// Add objet to the list of target's camera
    /// </summary>
    public void AddTarget()
	{
        if (cameraController)
            cameraController.AddTarget (this);
	}

    /// <summary>
    /// remove objet to the list of target's camera
    /// </summary>
	public void RemoveTarget()
	{
        if (cameraController)
		{
            cameraController.RemoveTarget (this);
		}
	}

    #endregion

    #region Unity ending function
    // Unity functions


    private void OnDisable()
    {
		if (onDisableRemove)
		{
			RemoveTarget ();
		}
    }

	private void OnDestroy()
	{
		RemoveTarget ();
	}

    #endregion
}
