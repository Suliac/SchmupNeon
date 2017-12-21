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

    private bool spawnedOnce = false;          //reference du playerController
    public bool SpawnedOnce { get { return spawnedOnce; } }

    private LifeBehavior playerLife;    //reference a playerLife
    #endregion

    #region Initialization

    private void Start()
    {
        //init le player
        if (player && animSpawn)    //si tout est ok dans les references
        {
            spawnedOnce = false;
            playerController = player.GetComponent<PlayerController>(); //get la reference du playerCOntroller
            playerController.IdPlayer = transform.GetSiblingIndex();    //set son id selon la position du spawnPlayer dans la hiérarchie
            playerController.ScorePlayer = GameManager.GetSingleton.ScoreManager.Data.scorePlayer[playerController.IdPlayer];   //set son score
            playerLife = player.GetComponent<LifeBehavior>();   //get reference de la vie
            player.transform.SetParent(GameManager.GetSingleton.ObjectDynamiclyCreated);    //set son emplacement dans la hiérarchie
            player.transform.position = transform.position; //change sa position

            animSpawn.SpawnPlayer = this;   //set la reference à l'animation
            animSpawn.gameObject.SetActive(false);  //desactive l'animation si elle est activé (on est en initialisation, on active pas encore...
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

        if (StateManager.Get.State != StateManager.GameState.GameOver)
        {
            isSpawning = false;
            animSpawn.gameObject.SetActive(false);
            player.SetActive(true);
            spawnedOnce = true;
            playerLife.InitLife(); // reinit life ici ? 
            playerController.Init();
        }
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
