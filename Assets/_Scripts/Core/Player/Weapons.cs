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
    #endregion

    #region Initialization

    #endregion

    #region Core
    public void TryShoot()
	{
		if (nextShoot <= 0)
		{
			nextShoot = cooldown;
			Shoot ();
		}
	}

    /*
    //abstract public void Dir(float horizMove, float vertiMove);
	//public virtual void OnShootRelease(){}

	public virtual float WeaponPercent()
	{
		return Mathf.Clamp((cooldown - nextShoot) / cooldown, 0.0F, 1.0F);
	}
    */

    abstract protected void Shoot ();
    #endregion

    #region Unity ending functions
    void LateUpdate()
    {
        if (nextShoot > 0)
        {
            nextShoot -= Time.deltaTime;
        }
    }
    #endregion


}
