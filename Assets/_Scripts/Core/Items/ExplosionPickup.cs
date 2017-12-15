using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ExplosionPickup Description
/// </summary>
public class ExplosionPickup : HandablePickup
{
    #region Attributes

	[Tooltip("opti fps"), SerializeField]
	private FrequencyTimer updateTimer;


    [FoldoutGroup("Gameplay"), Tooltip("Rayon de l'explosion"), SerializeField]
    private float radius = 5.0f;
    [FoldoutGroup("Gameplay"), Tooltip("Force de poussée"), SerializeField]
    private float speedPush = 5.0f;

    [FoldoutGroup("Debug"), Tooltip("Est ce que l'item a été utilisé")]
    private bool launched = false;

    private List<PlayerController> playersToPush = new List<PlayerController>();
    #endregion

    #region Initialization
    
    #endregion

    #region Core

    protected override void Use()
    {
        //print("yo");
        if (!launched)
        {
            launched = true;
            PlayerController currentPlayer = currentHandler.GetComponent<PlayerController>();

            Collider[] hits = Physics.OverlapSphere(currentHandler.transform.position, radius, 1 << 8);
            foreach (var hit in hits)
            {
                PlayerController otherPlayer = hit.GetComponent<PlayerController>();
                // Attention, comme 2 collider, on detecte 2 fois un meme objet
                if (otherPlayer && otherPlayer.GetInstanceID() != currentPlayer.GetInstanceID() && !playersToPush.Contains(otherPlayer))
                {
                    //print(otherPlayer.gameObject.name+" "+otherPlayer.gameObject.layer);
                    otherPlayer.ImmobilisePlayer = true;
                    playersToPush.Add(otherPlayer);
                }
            }

            print(playersToPush.Count);
        }

    }
    #endregion

    #region Unity ending functions

    public void Update()
    {
        if (launched)
        {
            //optimisation des fps
            if (updateTimer.Ready())
            {
                if (playersToPush.Count > 0)
                {
                    List<PlayerController> tmpPlayers = new List<PlayerController>(playersToPush);
                    foreach (var playerToPush in tmpPlayers)
                    {
                        Vector3 diff = (playerToPush.transform.position - currentHandler.transform.position);
                        if (diff.magnitude > radius) // si entité + loins que radius, plus besoin de la bouger
                        {
                            playerToPush.PlayerBody.AddForce(diff, ForceMode.Impulse);
                            playerToPush.ImmobilisePlayer = false;
                            playersToPush.Remove(playerToPush);
                            break;
                        }

                        diff.Normalize();
                        playerToPush.transform.Translate(diff * speedPush * Time.deltaTime);
                    }
                }
                else
                {
                    print("Stop this shit");
                    Stop();
                }
            } 
        }
    }

	#endregion
}
