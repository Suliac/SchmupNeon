using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// WeaponHandler Description
/// </summary>
public class WeaponHandler : MonoBehaviour
{
    #region Attributes

	[Tooltip("opti fps"), SerializeField]
	private FrequencyTimer updateTimer;

    [FoldoutGroup("Debug"), Tooltip("objets weapons"), SerializeField]
    private Transform parentWeapons;

    //id weapons de l'entitée (joueur ou ennemi)
    private int idWeapon = 0;
    public int IdWeapon { set { idWeapon = value; } get { return idWeapon; } }

    private List<Weapons> weapons;          //list des weapons de l'entitée (joueur ou ennemi)

    private PlayerController playerController; // Lien vers le playercontroller s'il y en a un pour gérer les points
    #endregion

    #region Initialization
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        weapons = new List<Weapons>();
        SetupListWeapons();                     //setup la list des weapons
    }

    /// <summary>
    /// replie la list des weapoins du joueurs
    /// </summary>
    private void SetupListWeapons()
    {
        weapons.Clear();
        for (int i = 0; i < parentWeapons.childCount; i++)
        {
            Weapons childWeapon = parentWeapons.GetChild(i).GetComponent<Weapons>();
            childWeapon.PlayerController = playerController;
            weapons.Add(childWeapon);
        }
    }
    #endregion

    #region Core
    public void UseWeapon()
    {
        weapons[idWeapon].TryShoot();
    }
    #endregion

    #region Unity ending functions
    
	#endregion
}
