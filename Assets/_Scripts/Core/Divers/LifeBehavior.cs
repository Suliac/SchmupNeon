using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// LifeBehavior Description
/// </summary>
public class LifeBehavior : MonoBehaviour
{
    #region Attributes

    [FoldoutGroup("Gameplay"), Tooltip("La vie initialise de l'objet"), SerializeField]
    private float StartLife = 100.0f;
    [FoldoutGroup("Gameplay"), Tooltip("Score à donner aux joueur quand cet objet meurt"), SerializeField]
    private int scoreToGiveToOther = 0;  //score à donner aux autre quand on meurt
    [FoldoutGroup("Gameplay"), Tooltip("Score à enlever au joueur quand on meurt"), SerializeField]
    private int scoreToRemove = 0;   //score à enlever à soit même quand on meurt (pour les joueurs)
    public int ScoreToRemove { get { return scoreToRemove; } }

    [FoldoutGroup("Debug"), Tooltip("vie courante de l'objet"), SerializeField]
    private float currentLife = 0.0f;

    #endregion

    #region Initialization

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
    public int TakeDamages(float damages)
    {
        currentLife = Mathf.Max(0, currentLife - damages);

        if (currentLife <= 0)
        {
            // Si on a plus de vie, on essaie de kill l'entitée actuelle
            IKillable killable = gameObject.GetComponent<IKillable>();
            if (killable != null)
            {
                killable.Kill();
                return (scoreToGiveToOther); //ici get le nombre de score que le gameObject donne en mourrant
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
