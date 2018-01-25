using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

/// <summary>
/// PlayerController handle player movement
/// <summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : Pausable, IKillable
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

    [FoldoutGroup("GamePlay"), Tooltip("Picku^p Animator"), SerializeField]
    private GameObject animPick;
    public GameObject AnimPick { get { return animPick; } }

    [FoldoutGroup("GamePlay"), Tooltip("Objet Animator"), SerializeField]
    private Color colorPlayer = Color.yellow;
    public Color ColorPlayer { get { return colorPlayer; } }

    [FoldoutGroup("GamePlay"), Tooltip("tag de la prefab d'affichage de score"), SerializeField]
    private string prefabScoreTag = "Score";
    //[FoldoutGroup("Debug"), Tooltip("objets weapons"), SerializeField]
    //private Transform parentWeapons;
    [FoldoutGroup("GamePlay"), Tooltip("quand on meurt"), SerializeField]
    private Vibration dieVibration;

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

    //private bool immobilisePlayer = false;
    //public bool ImmobilisePlayer { get { return immobilisePlayer; } set { immobilisePlayer = value; } }
    private float inhibCoeff = 1.0f;
    public float InhibCoeff { get { return inhibCoeff; } set { inhibCoeff = value; } }

    private int scorePlayer;                //score du player...
    public int ScorePlayer
    {
        set
        {
            scorePlayer = value;
            if (scorePlayer < 0)
                scorePlayer = 0;
            GameManager.GetSingleton.ScoreManager.SetScore(idPlayer, scorePlayer);
        }
        get { return scorePlayer; }
    }

    private float horizMove;                //mouvement horizontal du joueur
    private float vertiMove;                //mouvement vertical du joueur

    private bool hasMoved = false;          //a-t-on bougé ?
    private bool isShooting = false;
    private bool lastFrameIsShooting = false;

    #endregion

    #region Initialize

    private void Awake()
    {
        if (!GameManager.GetSingleton)
        {
            Debug.LogError("Désactiver la trash !");
        }
        GameManager.GetSingleton.AddPlayerController(this, IdPlayer);
        playerBody = GetComponent<Rigidbody>();
        lifeBehavior = GetComponent<LifeBehavior>();
        weaponHandle = GetComponent<WeaponHandler>();
        pickupHandle = GetComponent<PickupHandler>();
        animPlayer.GetComponent<SpriteRenderer>().color = colorPlayer;

        for (int i = 0; i < 4; i++)
        {
            GameObject animator = transform.GetChild(1).GetChild(i).gameObject;
            if (i == idPlayer) // desactive tous les animatirs sauf celui du joueur actuel
                animPlayer = animator;
            else
                animator.SetActive(false);
        }
        //animPick = transform.GetChild(3).GetComponent<Animator>();
        //weapons = new List<Weapons>();
        //SetupListWeapons();                     //setup la lsit des weapons
    }

    private void OnEnable()
    {
        gameObject.SetActive(true);
        animPlayer.SetActive(true);
        if (animPick)
        {
            animPick.SetActive(false);
        }
        playerBody.velocity = Vector3.zero;
        hasMoved = false;
        enabledPlayer = true;
    }

    public void Init()
    {
        if (weaponHandle)
            weaponHandle.Init();

        if (pickupHandle)
            pickupHandle.Init();
    }

    #endregion

    #region Core

    private void InputPlayer()
    {
        //if (!immobilisePlayer)
        //{
        switch (StateManager.GetSingleton.State)
        {
            case StateManager.GameState.GameOver:
                horizMove = 0;
                vertiMove = 0;
                break;
            case StateManager.GameState.Victory: // lors de la victoire le player part vers la droite
                horizMove = 1;
                vertiMove = 0;
                break;
            default: // Par défaut le player peut bouger
                horizMove = PlayerConnected.GetSingleton.getPlayer(idPlayer).GetAxis("Move Horizontal");
                vertiMove = PlayerConnected.GetSingleton.getPlayer(idPlayer).GetAxis("Move Vertical");

                if (PlayerConnected.GetSingleton.getPlayer(idPlayer).GetButton("FireA"))
                {
                    isShooting = true;
                    weaponHandle.UseWeapon();
                }
                else
                {
                    isShooting = false;
                }

                if (lastFrameIsShooting != isShooting)
                {
                    if (isShooting)
                        SoundManager.GetSingularity.PlayProjectileSound(idPlayer);
                    else
                        SoundManager.GetSingularity.StopProjectileSound(idPlayer);
                }

                if (PlayerConnected.GetSingleton.getPlayer(IdPlayer).GetButton("FireX"))
                    pickupHandle.UseItem();

                lastFrameIsShooting = isShooting;
                //if (PlayerConnected.GetSingleton.getPlayer(IdPlayer).GetButton("FireX")) // NON FONCTIONNEL
                //    GameManager.GetSingleton.Pause();
                break;
        }


        if (horizMove != 0 || vertiMove != 0)
            hasMoved = true;
        else
            hasMoved = false;
        //}
        //else
        //{
        //    horizMove = 0f;
        //    vertiMove = 0f;
        //    hasMoved = false;
        //}
    }

    private void MovePlayer()
    {
        if (hasMoved)
        {
            Vector3 movement = new Vector3(horizMove * moveSpeed * Time.deltaTime * inhibCoeff, vertiMove * moveSpeed * Time.deltaTime * inhibCoeff, 0.0f);

            playerBody.velocity = Vector3.ClampMagnitude(movement, moveSpeed);
        }
        else
        {
            playerBody.velocity = Vector3.zero;
        }

    }

    private void CreateDeathObject()
    {
        GameObject deathPlayer = ObjectsPooler.GetSingleton.GetPooledObject(prefabsDeathTag, false);
        if (!deathPlayer)
        {
            Debug.LogError("y'en a + que prévue, voir dans objectPool OU dans le tag du player");
            return;
        }
        deathPlayer.transform.position = transform.position;
        deathPlayer.transform.SetParent(GameManager.GetSingleton.ObjectDynamiclyCreated);

        SpriteRenderer renderer = deathPlayer.GetComponent<SpriteRenderer>();
        if (renderer)
            renderer.color = ColorPlayer;

        deathPlayer.SetActive(true);
    }

    private void CreateDeathScoreObject()
    {
        //ScorePlayer -= lifeBehavior.ScoreToRemove;
        ScorePlayer -= ScorePlayer / 100 * lifeBehavior.ScoreToRemovePercent;

        GameObject scorePrefab = ObjectsPooler.GetSingleton.GetPooledObject(prefabScoreTag, false);
        if (!scorePrefab)
        {
            Debug.LogError("y'en a + que prévue, voir dans objectPool OU dans le tag du ScorePrefab | ");
            return;
        }
        scorePrefab.transform.position = transform.position;
        scorePrefab.transform.SetParent(GameManager.GetSingleton.ObjectDynamiclyCreated);

        scorePrefab.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        TextMesh text = scorePrefab.GetComponentInChildren<TextMesh>();
        if (text)
        {
            text.text = "-" + (ScorePlayer / 100 * lifeBehavior.ScoreToRemovePercent);//lifeBehavior.ScoreToRemove;
            text.color = ColorPlayer;
        }

        scorePrefab.SetActive(true);
    }

    /// <summary>
    /// pour respawn... juste désactive l'objet !
    /// </summary>
    private void RespawnIt()
    {
        gameObject.SetActive(false);
    }
    #endregion

    #region Unity ending

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

        InputPlayer();
    }

    private void FixedUpdate()
    {
        if (!enabledPlayer)
            return;

        MovePlayer();
    }

    [FoldoutGroup("Debug"), Button("Kill")]
    public void Kill()
    {
        if (!enabledPlayer)
            return;
        //Debug.Log("Dead");
        if (animPick)
        {
            animPick.SetActive(false);
            SpriteRenderer renderer = animPick.GetComponent<SpriteRenderer>();
            if (renderer)
            {
                renderer.sprite = null;
            }
        }

        if (StateManager.GetSingleton.State < StateManager.GameState.GameOver) // pas de déduction de score si gameover ou victoire
        {
            CreateDeathScoreObject();
        }
        dieVibration.play(idPlayer);

        weaponHandle.Init();
        pickupHandle.Init();

        CreateDeathObject();

        if (lifeBehavior.CurrentLife > 0)
            lifeBehavior.OnExternalKill();

        lifeBehavior.ForceStopInvincibility();

        SoundManager.GetSingularity.PlayDeadPlayerSound();

        ScreenShake ScreenShake = Camera.main.GetComponent<ScreenShake>();
        if (ScreenShake != null)
        {
            ScreenShake.Shake();
        }

        enabledPlayer = false;
        animPlayer.SetActive(false);
        Invoke("RespawnIt", timeBeforeRespawn);
    }

    public override void Pause()
    {
        inhibCoeff = 0.0f;
        Debug.Log("todo : stop anim");
    }

    public override void Resume()
    {
        inhibCoeff = 1.0f;
        Debug.Log("todo : restart anim");
    }
    #endregion
}