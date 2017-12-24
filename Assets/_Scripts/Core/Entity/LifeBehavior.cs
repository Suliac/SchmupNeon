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

    [FoldoutGroup("GamePlay"), Tooltip("Score à donner aux joueur quand cet objet est touché"), SerializeField]
    private int scoreToGiveToOtherOnHit = 0;  //score à donner aux autre quand on meurt

    [FoldoutGroup("GamePlay"), Tooltip("Score à enlever au joueur quand on meurt"), SerializeField]
    private int scoreToRemove = 0;   //score à enlever à soit même quand on meurt (pour les joueurs)
    public int ScoreToRemove { get { return scoreToRemove; } }

    [FoldoutGroup("Debug"), Tooltip("vie courante de l'objet"), SerializeField]
    private float currentLife = 0.0f;
    public float CurrentLife { get { return currentLife; } }

    private bool coroutineStarted = false;
    private bool coroutineOnTakeDmgStarted = false;

    [FoldoutGroup("GamePlay"), Tooltip("Invincibilité"), SerializeField]
    private bool invincible = false;

    [FoldoutGroup("GamePlay"), Tooltip("Renderer à faire blink lors de l'invincibilité"), SerializeField]
    private Renderer entityRenderer;

    [FoldoutGroup("GamePlay"), Tooltip("Couleur du blink lors de l'invincibilité"), SerializeField]
    private Color invincibilityColorBlink;
    [FoldoutGroup("GamePlay"), Tooltip("Couleur lorsque l'entité se prend des dégats"), SerializeField]
    private Color takeDamageColor;
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
        lock (this)
        {
            if (!invincible && currentLife > 0)
            {
                if (!coroutineOnTakeDmgStarted && entityType == EntityType.Ennemy)
                    StartCoroutine(OnTakeDamage(0.1f));

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
                else
                {
                    //si on est pas mort, ajouter quand même le score au joueur ! 
                    return (scoreToGiveToOtherOnHit);
                }


            }
            return (0);
        }
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
        //print("invincible");
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
        bool isGrey = true;

        Color oldColor = entityRenderer ? entityRenderer.material.color : Color.white;

        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            currentPhaseTime += Time.deltaTime;

            if (entityRenderer)
            {
                if (currentPhaseTime > phaseDuration)
                {
                    isGrey = !isGrey;
                    currentPhaseTime = 0.0f;
                }

                if (isGrey)
                    entityRenderer.material.color = invincibilityColorBlink;
                else
                    entityRenderer.material.color = oldColor;
            }

            yield return null;
        }
        entityRenderer.material.color = oldColor;

        coroutineStarted = false;
        invincible = false;
        yield return null;
    }

    IEnumerator OnTakeDamage(float time)
    {
        coroutineOnTakeDmgStarted = true;
        Color oldColor = entityRenderer ? entityRenderer.material.color : Color.white;

        if (entityRenderer)
            entityRenderer.material.color = takeDamageColor;

        yield return new WaitForSeconds(time);
        entityRenderer.material.color = oldColor;
        coroutineOnTakeDmgStarted = false;
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
