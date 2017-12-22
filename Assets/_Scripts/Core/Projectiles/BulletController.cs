using UnityEngine;
using Sirenix.OdinInspector;

public enum BulletOrientation
{
    Right,
    Left,
    Top,
    Down
}

/// <summary>
/// BulletController Description
/// </summary>
public class BulletController : Projectile
{
    #region Attributes
    //[FoldoutGroup("Gameplay"), Tooltip("dommage sur les objets"), SerializeField]
    //private float bulletDamage = 50.0f;

    [FoldoutGroup("GamePlay"), Tooltip("speed de la bullet"), SerializeField]
    private float speedBullet = 3f;

    private float additionalSpeed = 0f; //additional speed added by the palyer at start
    private Vector3 orientation;
    #endregion

    #region Initialization


    private void OnEnable()
    {
        //SetUpBullet();
    }

    /// <summary>
    /// setup le bullet
    /// </summary>
    protected override void SetUpBullet(PlayerController referencePlayer, float addSpeed, Vector3 orientationWanted)
    {
        PlayerController = referencePlayer;
        enabledBullet = true;           //active le bullet !
        isOnCamera.isOnScreen = true;
        isOnCamera.enabled = true;
        bodyBullet.velocity = Vector3.zero;
        additionalSpeed = addSpeed;
        orientation = orientationWanted;
    }
    #endregion

    #region Core
    protected override void MoveProjectile()
    {
       bodyBullet.velocity = orientation * (speedBullet + additionalSpeed) * Time.deltaTime;
    }
    #endregion

    #region Unity ending functions

    public override void Kill()
    {
        if (isOnCamera.CheckOnCamera())
        {
            GameObject deathBullet = ObjectsPooler.GetSingleton.GetPooledObject(prefabsDeathTag, false);
            if (!deathBullet)
            {
                Debug.LogError("y'en a + que prévue, voir dans objectPool OU dans le tag du BulletCOntroller | "+ prefabsDeathTag);
                return;
            }
            deathBullet.transform.position = transform.position;
            deathBullet.transform.SetParent(GameManager.GetSingleton.ObjectDynamiclyCreated);
            deathBullet.SetActive(true);
        }
        

        isOnCamera.enabled = false;
        //isOnCamera.isOnScreen = true;
        gameObject.SetActive(false);
    }

    protected override void OnProjectileTooFar()
    {
        Kill(); // for a classic bullet, if it goes too far, just kill it 
    }

    #endregion
}
