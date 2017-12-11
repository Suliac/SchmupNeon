using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// BulletController Description
/// </summary>
public class BulletController : Projectile
{
    #region Attributes
    //[FoldoutGroup("Gameplay"), Tooltip("dommage sur les objets"), SerializeField]
    //private float bulletDamage = 50.0f;

    [FoldoutGroup("Gameplay"), Tooltip("speed de la bullet"), SerializeField]
    private float speedBullet = 3f;
    private float additionalSpeed = 0f; //additional speed added by the palyer at start

    #endregion

    #region Initialization
    

    private void OnEnable()
    {
        //SetUpBullet();
    }

    /// <summary>
    /// setup le bullet
    /// </summary>
    public override void SetUpBullet(PlayerController referencePlayer, float addSpeed)
    {
        PlayerController = referencePlayer;
        enabledBullet = true;           //active le bullet !
        isOnCamera.isOnScreen = true;
        isOnCamera.enabled = true;
        bodyBullet.velocity = Vector3.zero;
        additionalSpeed = addSpeed;
    }
    #endregion

    #region Core
    protected override void MoveProjectile()
    {
        bodyBullet.velocity = new Vector3((speedBullet + additionalSpeed) * Time.deltaTime, 0, 0);
    }
    #endregion

    #region Unity ending functions

    //private void OnTriggerEnter(Collider col)
    //{
    //    if (!enabledBullet) //si le bullet est désactivé, ne pas effectuer de test...
    //        return;

    //    LifeBehavior life = col.gameObject.GetComponent<LifeBehavior>();
    //    if (life)
    //    {
    //        enabledBullet = false;  //desactiver le bullet !
    //        Debug.Log("bang");
    //        //le life prend des dommages, si le life meurt... on s'ajoute du score !
    //        int score = life.TakeDamages(bulletDamage);
    //        if (score != 0)
    //        {
    //            PlayerController.ScorePlayer+= score;
    //        }
    //        Kill();
    //    }
    //}

    public override void Kill()
    {
        isOnCamera.enabled = false;
        //isOnCamera.isOnScreen = true;
        gameObject.SetActive(false);
    }

    #endregion
}
