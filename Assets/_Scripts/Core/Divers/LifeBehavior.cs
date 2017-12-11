using Sirenix.OdinInspector;
using UnityEngine;

public enum EntityType
{
    Player,
    Projectile,
    Ennemy
}

/// <summary>
/// LifeBehavior Description
/// </summary>
public class LifeBehavior : MonoBehaviour
{
    #region Attributes
    //[Tooltip("opti fps"), SerializeField]
    //private FrequencyTimer updateTimer;

    [FoldoutGroup("GamePlay"), Tooltip("Vie de départ de l'entitée"), SerializeField]
    private float startLife = 100.0f;
    [FoldoutGroup("GamePlay"), Tooltip("Vie de actuelle de l'entitée"), SerializeField]
    private float currentLife = 0.0f;
    [FoldoutGroup("GamePlay"), Tooltip("Type de l'entitée"), SerializeField]
    private EntityType entityType;

    public EntityType GetEntityType()
    {
        return entityType;
    }

    private IKillable killable;

    #endregion

    #region Initialization

    private void Awake()
    {
        killable = gameObject.GetComponent<IKillable>();
    }

    private void Start()
    {
        // Start function
        InitLife();
    }
    #endregion

    #region Core

    /// <summary>
    /// Deal damage to target
    /// </summary>
    /// <param name="damages">Number of damages</param>
    /// <returns>True if target's life reachs 0 & false otherwise</returns>
    public bool TakeDamages(float damages)
    {
        currentLife = Mathf.Max(0, currentLife - damages);

        if (currentLife <= 0)
        {                       
            if (killable != null)
                killable.Kill(); // Si on a plus de vie, on essaie de kill l'entitée actuelle

            return true;
        }

        return false;
    }

    public void OneShot()
    {
        currentLife = 0;
        if (killable != null)
            killable.Kill();
    }

    public void InitLife()
    {
        currentLife = startLife;
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
