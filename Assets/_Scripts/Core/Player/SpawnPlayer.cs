using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// SpawnPlayer Description
/// </summary>
public class SpawnPlayer : MonoBehaviour
{
    #region Attributes

    [FoldoutGroup("Debug"), Tooltip("opti fps"), SerializeField]
    private FrequencyTimer updateTimer;

    [FoldoutGroup("Debug"), Tooltip("link du player"), SerializeField]
    private GameObject player;                           //player objet

    [FoldoutGroup("Debug"), Tooltip("objet spawn animé"), SerializeField]
    private AnimSpawn animSpawn;                        //affichage du spawn

    private PlayerController playerController;          //reference du playerController
    public PlayerController PlayerController { get { return playerController; } }
    private bool isSpawning = false;                    //le joueur est-il en train d'être spawn ?

    private LifeBehavior playerLife;
    #endregion

    #region Initialization

    private void Start()
    {
        if (player && animSpawn)
        {
            playerController = player.GetComponent<PlayerController>();
            playerController.IdPlayer = transform.GetSiblingIndex();
            playerLife = player.GetComponent<LifeBehavior>();
            player.transform.SetParent(GameManager.GetSingleton.ObjectDynamiclyCreated);
            player.transform.position = transform.position;
            animSpawn.SpawnPlayer = this;
            animSpawn.gameObject.SetActive(false);
        }
    }
    #endregion

    #region Core
    /// <summary>
    /// commence à spawner le joueur
    /// </summary>
    public void prepareSpawning()
    {
        isSpawning = true;
        animSpawn.gameObject.SetActive(true);

        player.transform.position = transform.position;
        //Invoke();
    }

    /// <summary>
    /// l'animation de spawn est terminé, on spawn le joueur !
    /// </summary>
    public void spawnIt()
    {
        //ici on test si la manette du joueur est connecté
        //si elle ne l'ai pas, ne rien faire (l'animation va boucler,
        //  et appeler cette fonction à chaque tour de boucle)
        if (!PlayerConnected.GetSingleton.playerArrayConnected[playerController.IdPlayer])
            return;

        if (!isSpawning)
        {
            //si on n'est pas en train de spawner... un problème dans l'animation ?
            Debug.LogError("on n'est pas censé être ici...");
            return;
        }
        isSpawning = false;
        animSpawn.gameObject.SetActive(false);
        player.SetActive(true);
        playerLife.InitLife(); // reinit life ici ?
    }


    #endregion

    #region Unity ending functions
    /// <summary>
    /// effectué à chaque frame
    /// </summary>
    private void Update()
    {
        //optimisation des fps
        if (updateTimer.Ready())
        {
            if (player && !isSpawning && !player.activeInHierarchy)
            {
                prepareSpawning();
            }
                
        }
    }

	#endregion
}
