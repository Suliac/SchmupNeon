using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Shoot Description
/// </summary>
public class BulletHell : Weapons
{
    #region Attributes


    [FoldoutGroup("Gameplay"), Tooltip("Tag du bullet"), SerializeField]
    private string tagBullet;
    [FoldoutGroup("Gameplay"), Tooltip("Cree le bullet un peu devant le joueur [0 = sur le joueur, inf]"), SerializeField]
    private float forwardBullet = 1f;
    [FoldoutGroup("Gameplay"), Tooltip("AdditionalSpeed when player is moving"), SerializeField]
    private float AdditionalSpeed = 50f;

    [FoldoutGroup("Debug"), Tooltip("opti fps"), SerializeField]
    private FrequencyTimer updateTimer;

    private GameObject bullet = null;
    #endregion

    #region Initialization

    #endregion

    #region Core

    protected override void Shoot()
    {
        bullet = ObjectsPooler.GetSingleton.GetPooledObject(tagBullet, false);                  //cree un bullet
        if (!bullet)
        {
            Debug.LogError("y'en a + que prévue, voir dans objectPool pour ajouter des bullet");
            return;
        }

        bullet.transform.SetParent(GameManager.GetSingleton.ObjectDynamiclyCreated);            //set le parent
        bullet.transform.rotation = transform.rotation;                                         //set la position

        BulletOrientation orientation = PlayerController ? BulletOrientation.Right : BulletOrientation.Left;
        Vector3 projectileOrientation = PlayerController ? transform.right : -transform.right;  // si bullet hell sur un ennemi -> il tire vers la gauche
        bullet.transform.position = transform.position + projectileOrientation * forwardBullet;       //set la rotation

        //get le projectile du bullet, et le setup (qui peut être de plusieurs types !)
        Projectile projectile = bullet.GetComponent<Projectile>();

        //initialise
        float initSpeed = (isMovingRight) ? PlayerController.PlayerBody.velocity.magnitude * AdditionalSpeed : 0f;

        projectile.SetUpBullet(PlayerController, initSpeed, orientation);

        GiveDamage giveDamage = bullet.GetComponent<GiveDamage>();
        giveDamage.LinkToPlayer(PlayerController);

        //enfin, l'activer
        bullet.SetActive(true);                                                                 //active l'objet
    }

    #endregion

    #region Unity ending functions

    private void Update()
    {
        //optimisation des fps
        if (updateTimer.Ready())
        {

        }
    }

    #endregion
}
