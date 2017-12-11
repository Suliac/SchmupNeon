using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// BulletController Description
/// </summary>
public class BulletController : MonoBehaviour, IKillable
{
    #region Attributes
    [FoldoutGroup("Gameplay"), Tooltip("dommage sur les objets"), SerializeField]
    private float bulletDamage = 50.0f;

    [FoldoutGroup("Gameplay"), Tooltip("speed de la bullet"), SerializeField]
    private float speedBullet = 3f;



    [FoldoutGroup("Debug"), Tooltip("opti fps"), SerializeField]
	private FrequencyTimer updateTimer;

    private Rigidbody bodyBullet;   //ref du rigidbody
    private IsOnCamera isOnCamera;  //ref du IsOnCamera pour savoir si le bullet est hors champs
    #endregion

    #region Initialization
    private void Awake()
    {
        bodyBullet = GetComponent<Rigidbody>();
        isOnCamera = GetComponent<IsOnCamera>();
    }

    private void Start()
    {
        SetUpBullet();
    }

    /// <summary>
    /// setup le bullet
    /// </summary>
    public void SetUpBullet()
    {
        isOnCamera.isOnScreen = true;
        isOnCamera.enabled = true;
        bodyBullet.velocity = Vector3.zero;
        //bodyBullet.AddForce(transform.right * (speedBullet), ForceMode.Impulse);
    }
    #endregion

    #region Core

    #endregion

    #region Unity ending functions

    private void Update()
    {
        bodyBullet.velocity = new Vector3(speedBullet, 0, 0);
        //optimisation des fps
        if (updateTimer.Ready())
        {
            if (!isOnCamera.enabled)
                isOnCamera.enabled = true;
            if (!isOnCamera.isOnScreen)
                Kill();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        LifeBehavior life = col.gameObject.GetComponent<LifeBehavior>();
        if (life)
        {
            Debug.Log("bang");
            life.TakeDamages(bulletDamage);
            Kill();
        }
    }

    public void Kill()
    {
        isOnCamera.enabled = false;
        isOnCamera.isOnScreen = true;
        gameObject.SetActive(false);
    }

    #endregion
}
