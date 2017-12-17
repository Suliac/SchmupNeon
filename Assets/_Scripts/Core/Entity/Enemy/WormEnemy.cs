﻿using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// WormEnemy Description
/// </summary>
public class WormEnemy : BaseEnemy
{
    #region Attributes
    private ShootOnDeathEnemy[] wormParts;

    private Vector3 topPosition;
    private Vector3 botPosition;

    private bool goingTop = true;
    #endregion

    #region Initialization
    protected void Awake()
    {
        base.Awake();
        botPosition = transform.GetChild(2).GetChild(1).position; // crado !
        topPosition = transform.GetChild(2).GetChild(0).position; // crado !

        wormParts = transform.GetComponentsInChildren<ShootOnDeathEnemy>();
    }


    private void Start()
    {
        // Start function
    }
    #endregion

    #region Core
    protected override void Move()
    {
        Vector3 nextCheckPoint = goingTop ? topPosition : botPosition;

        Vector3 dir = nextCheckPoint - transform.position;
        body.velocity = dir.normalized * speed;

        if ((goingTop && transform.position.y >= topPosition.y)
                || (!goingTop && transform.position.y <= botPosition.y))
            goingTop = !goingTop;

    }

    protected override void Shoot()
    {

    }

    protected override void OnBeforeKill()
    {
        foreach (var wormPart in wormParts)
        {
            wormPart.Kill();
        }
    }
    #endregion

    #region Unity ending functions

    #endregion
}
