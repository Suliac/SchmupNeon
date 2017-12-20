using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Rifle Description
/// </summary>
public class Rifle : Weapons
{
    #region Attributes    
    [FoldoutGroup("Gameplay"), Tooltip("Tag du bullet"), SerializeField]
    private string tagBullet;
    [FoldoutGroup("Gameplay"), Tooltip("Cree le bullet un peu devant le joueur [0 = sur le joueur, inf]"), SerializeField]
    private float forwardBullet = 1f;
    [FoldoutGroup("Gameplay"), Tooltip("AdditionalSpeed when player is moving"), SerializeField]
    private float AdditionalSpeed = 50f;

    [FoldoutGroup("Gameplay"), Tooltip("Nombre de balles tirées lors d'un tir"), SerializeField]
    private int NbrBulletPerShot = 1;
    [FoldoutGroup("Gameplay"), Tooltip("Angle de tir de la première balle"), SerializeField, Range(0, 360)]
    private float FirstAngleShot = 180.0f;
    [FoldoutGroup("Gameplay"), Tooltip("Angle entre chacune des balles"), SerializeField, Range(0, 360)]
    private float angleBetweenEachShoot = 0.0f;

    [FoldoutGroup("Debug"), Tooltip("opti fps"), SerializeField]
    private FrequencyTimer updateTimer;

    private GameObject bullet = null;

    #endregion

    #region Initialization

    #endregion

    #region Core

    protected override void Shoot()
    {
        float currentShotAngle = FirstAngleShot;
        for (int i = 0; i < NbrBulletPerShot; i++)
        {
            bullet = ObjectsPooler.GetSingleton.GetPooledObject(tagBullet, false);                  //cree un bullet
            if (!bullet)
            {
                Debug.LogError("y'en a + que prévue, voir dans objectPool pour ajouter des bullet");
                return;
            }

            bullet.transform.SetParent(GameManager.GetSingleton.ObjectDynamiclyCreated);            //set le parent
            bullet.transform.rotation = transform.rotation;                                         //set la position

            //Vector3 projectileOrientation = new Vector3(Mathf.Cos(Mathf.Deg2Rad * currentShotAngle), Mathf.Sin(Mathf.Deg2Rad * currentShotAngle), 0);
            Quaternion rotationForTheShoot = Quaternion.Euler(new Vector3(0, 0, currentShotAngle));
            Vector3 projectileOrientation = rotationForTheShoot * transform.right;

            bullet.transform.position = transform.position + projectileOrientation * forwardBullet;       //set la rotation 

            //get le projectile du bullet, et le setup (qui peut être de plusieurs types !)
            Projectile projectile = bullet.GetComponent<Projectile>();

            float initSpeed = 0.0f;
            if (PlayerController)
                initSpeed = (isMovingRight) ? PlayerController.PlayerBody.velocity.magnitude * AdditionalSpeed : 0f;

            projectile.Setup(PlayerController, initSpeed, projectileOrientation);

            if (PlayerController)
            {
                GiveDamage giveDamage = bullet.GetComponent<GiveDamage>();
                giveDamage.LinkToPlayer(PlayerController);
                projectile.ColorBullet = PlayerController.ColorPlayer;
            }

            //enfin, l'activer
            bullet.SetActive(true);
            currentShotAngle += angleBetweenEachShoot;
        }
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
