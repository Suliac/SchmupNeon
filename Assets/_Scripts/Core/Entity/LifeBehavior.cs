using Sirenix.OdinInspector;
using System.Collections;
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
    [FoldoutGroup("GamePlay"), Tooltip("Type de l'entitée"), SerializeField]
    private EntityType entityType;
    public EntityType EntityType { get { return entityType; } }

    private IKillable killable;

    [FoldoutGroup("GamePlay"), Tooltip("La vie initialise de l'objet"), SerializeField]
    private float StartLife = 100.0f;
    [FoldoutGroup("GamePlay"), Tooltip("Score à donner aux joueur quand cet objet meurt"), SerializeField]
    private int scoreToGiveToOther = 0;  //score à donner aux autre quand on meurt
    [FoldoutGroup("GamePlay"), Tooltip("Score à enlever au joueur quand on meurt"), SerializeField]
    private int scoreToRemove = 0;   //score à enlever à soit même quand on meurt (pour les joueurs)
    public int ScoreToRemove { get { return scoreToRemove; } }

    [FoldoutGroup("Debug"), Tooltip("vie courante de l'objet"), SerializeField]
    private float currentLife = 0.0f;
    public float CurrentLife { get { return currentLife; } }

    private bool coroutineStarted = false;

    [FoldoutGroup("GamePlay"), Tooltip("Invincibilité"), SerializeField]
    private bool invincible = false;

    [FoldoutGroup("GamePlay"), Tooltip("Renderer à faire blink lros de l'invincibilité"), SerializeField]
    private Renderer entityRenderer;
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
    /// prend des dommages, renvoi X point de score à rajouter si on meurt ! sinon 0
    /// (pour le scorring)
    /// </summary>
    /// <param name="damages"></param>
    /// <returns></returns>
    public int TakeDamages(float damages, bool oneShot)
    {
        if (!invincible)
        {
            if (oneShot)
                damages = currentLife;

            currentLife = Mathf.Max(0, currentLife - damages);

            if (currentLife <= 0)
            {
                if (killable != null)
                {
                    killable.Kill();
                    return (scoreToGiveToOther); //ici get le nombre de score que le gameObject donne en mourrant
                }
            } 
        }
        return (0);
    }

    /// <summary>
    /// itinialise la vie du joueur au début (ou au respawn)
    /// </summary>
    public void InitLife()
    {
        currentLife = StartLife;
    }

    public void Invicible(float time)
    {
        print("invincible");
        if (!coroutineStarted)
            StartCoroutine(InvicibleForSeconds(time));
    }

    IEnumerator InvicibleForSeconds(float time)
    {
        invincible = true;
        coroutineStarted = true;

        float currentTime = 0.0f;
        float currentPhaseTime = 0.0f;

        float phaseDuration = 0.1f;
        float alpha = 1.0f;

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            currentPhaseTime += Time.deltaTime;

            if (currentPhaseTime > phaseDuration)
            {
                alpha = alpha > 0 ? 0.0f : 1.0f;
                currentPhaseTime = 0.0f;
            }

            if (entityRenderer)
            {
                //print("alpha " + alpha);
                Color color = entityRenderer.material.color;
                color.a = alpha;
                entityRenderer.material.color = color;
            }

            yield return null;
        }
        Color colorAlphaOne = entityRenderer.material.color;
        colorAlphaOne.a = 1f;
        entityRenderer.material.color = colorAlphaOne;

        coroutineStarted = false;
        invincible = false;
        yield return null;
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
