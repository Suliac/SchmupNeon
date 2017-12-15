using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

/// <summary>
/// PlayerController handle player movement
/// <summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IKillable
{
    #region Attributes

    [FoldoutGroup("Gameplay"), Tooltip("Vitesse de déplacement du joueur"), SerializeField]
    private float moveSpeed = 10.0f;


    //[FoldoutGroup("Debug"), Tooltip("objets weapons"), SerializeField]
    //private Transform parentWeapons;

    [FoldoutGroup("Debug"), Tooltip("opti fps"), SerializeField]
    private FrequencyTimer updateTimer;


    ////id weapons du joueur
    //private int idWeapon = 0;
    //public int IdWeapon { set { idWeapon = value; } get { return idWeapon; } }

    //id du joueur (relatif à sa manette)
    private int idPlayer = 0;
    public int IdPlayer { set { idPlayer = value; } get { return idPlayer; } }

    //private List<Weapons> weapons;          //list des weapons du joueur

    private Rigidbody playerBody;           //rigidbody du joueur
    public Rigidbody PlayerBody { get { return playerBody; } }

    private bool enabledPlayer = false;
    public bool EnabledPlayer { get { return enabledPlayer; } }

    private LifeBehavior lifeBehavior;      //liens vers la vie du joueur
    private WeaponHandler weaponHandle;
    private PickupHandler pickupHandle;

    private int scorePlayer;                //score du player...
    public int ScorePlayer { set
        {
            scorePlayer = value;
            if (scorePlayer < 0)
                scorePlayer = 0;
            GameManager.GetSingleton.ScoreManager.setScore(idPlayer, scorePlayer);
        } get { return scorePlayer; } }

    private float horizMove;                //mouvement horizontal du joueur
    private float vertiMove;                //mouvement vertical du joueur
    

    private bool hasMoved = false;          //a-t-on bougé ?
    

    #endregion

    #region Initialize

    private void Awake()
    {
        playerBody = GetComponent<Rigidbody>();
        lifeBehavior = GetComponent<LifeBehavior>();
        weaponHandle = GetComponent<WeaponHandler>();
        pickupHandle = GetComponent<PickupHandler>();
        //weapons = new List<Weapons>();
        //SetupListWeapons();                     //setup la lsit des weapons
    }

    private void OnEnable()
    {
        gameObject.SetActive(true);
        playerBody.velocity = Vector3.zero;
        hasMoved = false;
        enabledPlayer = true;
    }

    /// <summary>
    /// replie la list des weapoins du joueurs
    /// </summary>
    //private void SetupListWeapons()
    //{
    //    weapons.Clear();
    //    for (int i = 0; i < parentWeapons.childCount; i++)
    //    {
    //        Weapons childWeapon = parentWeapons.GetChild(i).GetComponent<Weapons>();
    //        childWeapon.PlayerController = this;
    //        weapons.Add(childWeapon);
    //    }
    //}

    #endregion

    #region Core

    private void InputPlayer()
    {
        horizMove = PlayerConnected.GetSingleton.getPlayer(idPlayer).GetAxis("Move Horizontal");
        vertiMove = PlayerConnected.GetSingleton.getPlayer(idPlayer).GetAxis("Move Vertical");

        if(PlayerConnected.GetSingleton.getPlayer(idPlayer).GetButton("FireA"))
        {
            //weapons[idWeapon].TryShoot();
            weaponHandle.UseWeapon();
        }

        if(PlayerConnected.GetSingleton.getPlayer(IdPlayer).GetButton("FireB"))
        {
            pickupHandle.UseItem();
        }

        if (horizMove != 0 || vertiMove != 0)
            hasMoved = true;
        else
            hasMoved = false;
    }

    private void MovePlayer()
    {
        if (hasMoved)
        {
            Vector3 movement = new Vector3(horizMove * moveSpeed * Time.deltaTime, vertiMove * moveSpeed * Time.deltaTime, 0.0f);

            playerBody.velocity = Vector3.ClampMagnitude(movement, moveSpeed);
        }
        else
        {
            playerBody.velocity = Vector3.zero;
        }

    }

    /////////////////////////////////////////////////////

    private void OnDisable()
    {
        enabledPlayer = false;
    }

    private void Update()
    {
        if (updateTimer.Ready())
        {

        }
        InputPlayer();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    #endregion

    [FoldoutGroup("Debug"), Button("Kill")]
    public void Kill()
    {
        Debug.Log("Dead");
        ScorePlayer -= lifeBehavior.ScoreToRemove;
        gameObject.SetActive(false);
    }
}