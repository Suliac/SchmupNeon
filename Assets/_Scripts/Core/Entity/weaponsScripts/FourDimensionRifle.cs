using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourDimensionRifle : Weapons
{
    [FoldoutGroup("Gameplay"), Tooltip("Tag du bullet"), SerializeField]
    private string tagBullet;

    [FoldoutGroup("Gameplay"), Tooltip("Cree le bullet un peu devant le joueur [0 = sur le joueur, inf]"), SerializeField]
    private float forwardBullet = 1f;

    private GameObject bullet = null;

    protected override void Shoot()
    {
        for (int i = 0; i < 4; i++)
        {
            bullet = ObjectsPooler.GetSingleton.GetPooledObject(tagBullet, false);                  //cree un bullet
            if (!bullet)
            {
                Debug.LogError("y'en a + que prévue, voir dans objectPool pour ajouter des bullet");
                return;
            }

            bullet.transform.SetParent(GameManager.GetSingleton.ObjectNotMovingDynamiclyCreated);            //set le parent
            bullet.transform.rotation = transform.rotation;                                         //set la position
            
            Vector3 projectileOrientation = Vector3.zero;

            switch ((BulletOrientation)i)
            {
                case BulletOrientation.Right:
                    projectileOrientation = transform.right;
                    break;
                case BulletOrientation.Left:
                    projectileOrientation = -transform.right;
                    break;
                case BulletOrientation.Top:
                    projectileOrientation = transform.up;
                    break;
                case BulletOrientation.Down:
                    projectileOrientation = -transform.up;
                    break;
                default:
                    break;
            }

            bullet.transform.position = transform.position + projectileOrientation * forwardBullet;       //set la rotation 

            //get le projectile du bullet, et le setup (qui peut être de plusieurs types !)
            Projectile projectile = bullet.GetComponent<Projectile>();
            
            projectile.SetUpBullet(PlayerController, 0.0f, (BulletOrientation)i);

            GiveDamage giveDamage = bullet.GetComponent<GiveDamage>();
            giveDamage.LinkToPlayer(PlayerController);

            //enfin, l'activer
            bullet.SetActive(true);
        }
    }
}
