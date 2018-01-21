﻿using Sirenix.OdinInspector;
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

    [FoldoutGroup("GamePlay"), Tooltip("Durée maximale de l'effet"), SerializeField]
    private float maxDuration = 5.0f;
    private float currentDuration;

    [FoldoutGroup("GamePlay"), Tooltip("Rayon de l'explosion"), SerializeField]
    private float radius = 5.0f;
    [FoldoutGroup("GamePlay"), Tooltip("Force de poussée"), SerializeField]
    private float speedPush = 5.0f;

    [FoldoutGroup("Debug"), Tooltip("Est ce que l'item a été utilisé")]
    private bool launched = false;

    private List<PlayerController> playersToPush = new List<PlayerController>();

    private Vector3 usePosition;
    #endregion

    #region Initialization

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    #endregion

    #region Core

    protected override void Use()
    {
        //print("yo");
        if (!launched)
        {
            launched = true;
            PlayerController currentPlayer = currentHandler.GetComponent<PlayerController>();
            SoundManager.GetSingularity.PlayExplosionSound();

            Collider[] hits = Physics.OverlapSphere(currentHandler.transform.position, radius, 1 << 8);
            foreach (var hit in hits)
            {
                PlayerController otherPlayer = hit.GetComponent<PlayerController>();
                // Attention, comme 2 collider, on detecte 2 fois un meme objet
                if (otherPlayer && otherPlayer.GetInstanceID() != currentPlayer.GetInstanceID() && !playersToPush.Contains(otherPlayer))
                {
                    //print(otherPlayer.gameObject.name+" "+otherPlayer.gameObject.layer);
                    otherPlayer.InhibCoeff = 0.0f;
                    playersToPush.Add(otherPlayer);
                }
            }
            usePosition = currentPlayer.transform.position;
            CreateTakeObject(currentPlayer.gameObject.transform, prefabsUsePickup, currentHandler); //utilise 
            UtilityFunctions.createWave(currentPlayer.transform.position, 0);
            //print(playersToPush.Count);
        }
    }
    #endregion

    #region Unity ending functions

    public void Update()
    {
        if (launched)
        {
            currentDuration += Time.deltaTime;

            //optimisation des fps
            if (updateTimer.Ready())
            {
                if (playersToPush.Count > 0 && currentDuration < maxDuration)
                {
                    List<PlayerController> tmpPlayers = new List<PlayerController>(playersToPush);
                    foreach (var playerToPush in tmpPlayers)
                    {
                        Vector3 diff = (playerToPush.transform.position - usePosition);
                        float dtDistanceBetweenSourceAndTarget = diff.magnitude / radius;
                        if (diff.magnitude > radius) // si entité + loins que radius, plus besoin de la bouger
                        {
                            playerToPush.PlayerBody.AddForce(diff, ForceMode.Impulse);
                            playerToPush.InhibCoeff = 1.0f;
                            playersToPush.Remove(playerToPush);
                            break;
                        }

                        //playerToPush.InhibCoeff = 0.0f;
                        var lessSpeedOverDistance = 1-dtDistanceBetweenSourceAndTarget;
                        diff.Normalize(); // On veut une distance normalisée
                        playerToPush.transform.Translate(diff * speedPush * Time.deltaTime * lessSpeedOverDistance);
                    }
                }
                else
                {
                    if (playersToPush.Count > 0)
                    {
                        //print("Yo");
                        foreach (var playerToPush in playersToPush)
                            playerToPush.InhibCoeff = 1.0f;

                        playersToPush.Clear();
                    }
                    //print("Stop this shit");
                    Stop();
                }
            }
        }
    }

    #endregion
}
