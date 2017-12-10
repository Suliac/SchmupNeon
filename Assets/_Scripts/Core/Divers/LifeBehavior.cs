using UnityEngine;

/// <summary>
/// LifeBehavior Description
/// </summary>
public class LifeBehavior : MonoBehaviour
{
    #region Attributes

    //[Tooltip("opti fps"), SerializeField]
    //private FrequencyTimer updateTimer;

    public float currentLife = 0.0f;
    public float StartLife = 100.0f;
    #endregion

    #region Initialization

    private void Start()
    {
        // Start function
        InitLife();
    }
    #endregion

    #region Core

    public void TakeDamages(float damages)
    {
        currentLife = Mathf.Max(0, currentLife - damages);

        if (currentLife <= 0)
        {
            // Si on a plus de vie, on essaie de kill l'entitée actuelle
            IKillable killable = gameObject.GetComponent<IKillable>();
            if (killable != null)
            {
                killable.Kill();
            }
        }
    }

    public void InitLife()
    {
        currentLife = StartLife;
    }
    #endregion

    #region Unity ending functions

    private void Update()
    {
        //optimisation des fps
        //if (updateTimer.Ready())
        //{

        //}
    }

    #endregion
}
