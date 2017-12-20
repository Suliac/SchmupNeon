using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// GiveDamage Description
/// </summary>
public class GiveDamage : MonoBehaviour
{
    #region Attributes

    [Tooltip("opti fps"), SerializeField]
    private FrequencyTimer updateTimer;

    [FoldoutGroup("Debug")] [Tooltip("ref sur playerController")] private PlayerController playerController;
    public PlayerController PlayerController { set { playerController = value; } get { return playerController; } }

    [FoldoutGroup("Gameplay"), Tooltip("Dommages sur les objets"), SerializeField]
    private float damages = 50.0f;


    [FoldoutGroup("Gameplay"), Tooltip("Nombre de collision avant de supprimmer la source de dégâts"), SerializeField]
    private int collisionBeforeKilling = 1; // nb : -1 = source de damages jamais killée (= le joueur ?) (peut être useless)

    [FoldoutGroup("Gameplay"), Tooltip("Detecte la collision à l'entrée"), SerializeField]
    private bool collideOnEnter = true;

    [FoldoutGroup("Gameplay"), Tooltip("Detecte la collision à la sortie"), SerializeField]
    private bool collideOnExit = false;

    [FoldoutGroup("Gameplay"), Tooltip("Applique les dégâts aux joueurs"), SerializeField]
    private bool damagePlayer = true;
    [FoldoutGroup("Gameplay"), Tooltip("Applique les dégâts aux ennemis"), SerializeField]
    private bool damageEnnemy = true;
    [FoldoutGroup("Gameplay"), Tooltip("Applique les dégâts aux projectiles"), SerializeField]
    private bool damageProjectile = true;

    [FoldoutGroup("Gameplay"), Tooltip("Les dégats OneShot les cibles"), SerializeField]
    private bool oneShot = false;

    private bool isSourceEnabled = true;
    private int counterCollision = 0;
    private bool currentlyIntoTrigger = false;
    #endregion

    #region Initialization

    private void OnEnable()
    {
        Init();
    }
    #endregion

    #region Core
    public void LinkToPlayer(PlayerController sourcePlayerController)
    {
        playerController = sourcePlayerController;
    }

    private void Init()
    {
        isSourceEnabled = true;
        counterCollision = 0;
        currentlyIntoTrigger = false;
    }

    /// <summary>
    /// test si le type de l'objet est damageable
    /// </summary>
    private bool isDamagingThis(EntityType playerEntity)
    {
        switch (playerEntity)
        {
            case (EntityType.Player):
                if (damagePlayer)
                    return (true);
                break;
            case (EntityType.Projectile):
                if (damageProjectile)
                    return (true);
                break;
            case (EntityType.Ennemy):
                if (damageEnnemy)
                    return (true);
                break;
        }
        return (false);
    }

    /// <summary>
    /// test la collision avec l'objet
    /// </summary>
    private void CollisionBehavior(Collider col)
    {
        LifeBehavior life = col.gameObject.GetComponent<LifeBehavior>();
        if (life && !currentlyIntoTrigger)
        {
            bool damageable = isDamagingThis(life.EntityType);
            if (damageable)
            {
                counterCollision++;
                int score = life.TakeDamages(damages, oneShot);
                if (score != 0)
                {
                    if (PlayerController) // NB si un ennemi fait des degats, il ne gagne pas de points et ne devrait pas avoir de playerController
                        PlayerController.ScorePlayer += score;
                }
                else
                {
                    //si on est pas mort, ajouter quand même le score au joueur ! (le score des dommages infligé only)
                    if (PlayerController)
                        PlayerController.ScorePlayer += (int)damages;

                    // Si on kill la target pas besoin de désactiver la source de dommage 
                    // Important car TriggerExit jamais appelé donc currentlyIntoTrigger reste à true
                    currentlyIntoTrigger = true;
                }

                if (collisionBeforeKilling >= 0 && counterCollision >= collisionBeforeKilling)
                {
                    isSourceEnabled = false;  //desactiver l'origine des dégât si souhaité

                    IKillable killable = gameObject.GetComponent<IKillable>();
                    if (killable != null)
                        killable.Kill();
                }
            }
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

    private void OnTriggerEnter(Collider col)
    {
        if (!isSourceEnabled || !collideOnEnter)
            return;

        CollisionBehavior(col);
    }

    private void OnTriggerExit(Collider col)
    {
        currentlyIntoTrigger = false;
        if (!isSourceEnabled || !collideOnExit)
            return;

        CollisionBehavior(col);
    }
    #endregion
}
