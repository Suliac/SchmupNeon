using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// class abstraite des weapons,
/// contient un cooldown, et une reference au player
/// </summary>
public abstract class Weapons : MonoBehaviour
{
    #region Attributes
    [FoldoutGroup("Debug")] [Tooltip("ref sur playerController")] private PlayerController playerController;
    public PlayerController PlayerController { set { playerController = value; } get { return playerController; } }

    [FoldoutGroup("Gameplay")] [Tooltip("ref sur playerController"), SerializeField] protected float cooldown;

	private float nextShoot;    //gère le lancé du next shoot
    protected bool isMovingRight = false;
    #endregion

    #region Initialization

    #endregion

    #region Core
    /// <summary>
    /// Essaie de shoot
    /// </summary>
    /// <returns><see cref="true"/> si on a pu tirer ou <see cref="false"/> si on a apas pu tirer</returns>
    public bool TryShoot()
	{
		if (nextShoot <= 0)
		{
			nextShoot = cooldown;
            UpdateDirectionVelocity();
            Shoot ();
            return true;
		}

        return false;
	}

    /// <summary>
    /// cet fonction update la variable isMovingRight selon les déplacements du player
    /// </summary>
    private void UpdateDirectionVelocity()
    {
        if (PlayerController != null && PlayerController.PlayerBody.velocity.x > 0)
        {
            isMovingRight = true;
        }
        else
            isMovingRight = false;

    }

    abstract protected void Shoot ();
    #endregion

    #region Unity ending functions
    private void LateUpdate()
    {
        if (nextShoot > 0)
        {
            nextShoot -= Time.deltaTime;
        }
    }
    #endregion


}
