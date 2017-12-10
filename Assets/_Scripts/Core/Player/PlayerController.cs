using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// PlayerController handle player movement
/// <summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IKillable
{
    #region Attributes
    [SerializeField]
    private float baseJumpForce = 5.0F;

    [SerializeField]
    private float lowJumpMultiplier = 1.025F;


    [FoldoutGroup("Gameplay"), Tooltip("Mouvement du joueur"), SerializeField]
    private float moveSpeed = 10.0F;

    [FoldoutGroup("Gameplay"), Tooltip("MaxMove"), SerializeField]
    private float maxMoveSpeed = 20.0F;

    [FoldoutGroup("Debug"), Tooltip("MaxMove"), SerializeField]
    private int idPlayer = 0;
    public int IdPlayer { set { idPlayer = value; } get { return idPlayer; } }

    // Components
    private Rigidbody playerBody;
    private Shoot shoot;

    [FoldoutGroup("Debug"), Tooltip("opti fps"), SerializeField]
    private FrequencyTimer updateTimer;

    private float horizMove;
    private float vertiMove;
    private bool isJumping;
    private bool hasMoved = false;
    private bool hasJumped = false;

    private bool isShooting;

    private float currentLife = 0.0f;
    #endregion

    #region Initialize

    private void Awake()
    {
        playerBody = GetComponent<Rigidbody>();
        shoot = GetComponent<Shoot>();
    }

    private void OnEnable()
    {
        gameObject.SetActive(true);
        playerBody.velocity = Vector3.zero;
        hasMoved = false;
    }

    #endregion

    #region Core

    private void InputPlayer()
    {
        horizMove = PlayerConnected.GetSingleton.getPlayer(idPlayer).GetAxis("Move Horizontal");
        vertiMove = PlayerConnected.GetSingleton.getPlayer(idPlayer).GetAxis("Move Vertical");
        isJumping = PlayerConnected.GetSingleton.getPlayer(idPlayer).GetButtonDown("FireA");

        if(PlayerConnected.GetSingleton.getPlayer(idPlayer).GetButtonDown("FireB"))
        {
            shoot.SetIsShooting(true);
        }
        else if(PlayerConnected.GetSingleton.getPlayer(idPlayer).GetButtonUp("FireB"))
        {
            shoot.SetIsShooting(false);
        }

        if (horizMove != 0 || vertiMove != 0)
            hasMoved = true;
        else
            hasMoved = false;

        if (isJumping)
            hasJumped = true;
    }

    private void MovePlayer()
    {
        if (hasMoved)
        {
            playerBody.velocity = new Vector3(horizMove * moveSpeed, playerBody.velocity.y, 0.0F);
        }
        if (hasJumped)
        {
            //SoundManager.GetSingularity.PlaySound(SoundManager.GetSingularity.Hurt);
            hasJumped = false;
        }
    }

    /////////////////////////////////////////////////////

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
        gameObject.SetActive(false);
    }
}