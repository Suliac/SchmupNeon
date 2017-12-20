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

    [FoldoutGroup("GamePlay"), Tooltip("Vitesse de déplacement du joueur"), SerializeField]
    private float moveSpeed = 10.0f;

    [FoldoutGroup("GamePlay"), Tooltip("ref du prefabs de la particule de mort"), SerializeField]
    private string prefabsDeathTag;

    [FoldoutGroup("GamePlay"), Tooltip("Temps avant le respawn du joueur"), SerializeField]
    private float timeBeforeRespawn = 0.5f;

    [FoldoutGroup("GamePlay"), Tooltip("Objet Animator"), SerializeField]
    private GameObject animPlayer;

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

    private bool immobilisePlayer = false;
    public bool ImmobilisePlayer { get { return immobilisePlayer; } set { immobilisePlayer = value; } }

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
        animPlayer.SetActive(true);
        playerBody.velocity = Vector3.zero;
        hasMoved = false;
        enabledPlayer = true;
    }

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
        if (!enabledPlayer)
            return;

        if (updateTimer.Ready())
        {

        }

        if (!immobilisePlayer)
            InputPlayer(); 
    }

    private void FixedUpdate()
    {
        if (!enabledPlayer)
            return;

        MovePlayer();
    }

    private void CreateDeathObject()
    {
        GameObject deathBullet = ObjectsPooler.GetSingleton.GetPooledObject(prefabsDeathTag, false);
        if (!deathBullet)
        {
            Debug.LogError("y'en a + que prévue, voir dans objectPool OU dans le tag du player");
            return;
        }
        deathBullet.transform.position = transform.position;
        deathBullet.transform.SetParent(GameManager.GetSingleton.ObjectDynamiclyCreated);
        deathBullet.SetActive(true);
    }

    /// <summary>
    /// pour respawn... juste désactive l'objet !
    /// </summary>
    private void RespawnIt()
    {
        gameObject.SetActive(false);
    }
    #endregion

    [FoldoutGroup("Debug"), Button("Kill")]
    public void Kill()
    {
        if (!enabledPlayer)
            return;
        Debug.Log("Dead");
        
        ScorePlayer -= lifeBehavior.ScoreToRemove;


        CreateDeathObject();

        enabledPlayer = false;
        animPlayer.SetActive(false);
        Invoke("RespawnIt", timeBeforeRespawn);        
    }
}