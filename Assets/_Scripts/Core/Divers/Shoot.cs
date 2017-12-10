using UnityEngine;

/// <summary>
/// Shoot Description
/// </summary>
public class Shoot : MonoBehaviour
{
    #region Attributes

    [Tooltip("opti fps"), SerializeField]
    private FrequencyTimer fireRate;

    private bool isShooting = false;

    public Transform Bullet;
    public float BulletSpeed = 20.0f;
    #endregion

    #region Initialization

    private void Start()
    {
        // Start function
    }
    #endregion

    #region Core
    public void SetIsShooting(bool value)
    {
        isShooting = value;
    }
    #endregion

    #region Unity ending functions

    private void Update()
    {
        //optimisation des fps
        if (fireRate.Ready())
        {
            if (isShooting)
            {
                var bullet = Instantiate(Bullet, transform.position + transform.right, Quaternion.identity);
                bullet.GetComponent<Rigidbody>().AddForce(transform.right * BulletSpeed, ForceMode.Impulse);
            }
        }
    }

    #endregion
}
