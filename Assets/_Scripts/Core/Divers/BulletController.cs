using UnityEngine;

/// <summary>
/// BulletController Description
/// </summary>
public class BulletController : MonoBehaviour, IKillable
{
    #region Attributes

	[Tooltip("opti fps"), SerializeField]
	private FrequencyTimer updateTimer;

    public float BulletDamage = 50.0f;

    #endregion

    #region Initialization

    private void Start()
    {
		// Start function
    }
    #endregion

    #region Core

    #endregion

    #region Unity ending functions

    private void Update()
    {
      //optimisation des fps
      if (updateTimer.Ready())
      {

      }
    }

    private void OnCollisionEnter(Collision col)
    {
        var life = col.gameObject.GetComponent<LifeBehavior>();
        if(life)
        {
            Debug.Log("bang");
            life.TakeDamages(BulletDamage);
            Kill();
        }
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }

    #endregion
}
