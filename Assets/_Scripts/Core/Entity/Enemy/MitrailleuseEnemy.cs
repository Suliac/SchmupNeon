using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

/// <summary>
/// MitrailleuseEnemy Description
/// </summary>
public class MitrailleuseEnemy : ShootingEnemy
{
    #region Attributes
    [FoldoutGroup("Gameplay"), Tooltip("Rotation entre chaque tir"), SerializeField, Range(0, 360)]
    private float angleRotation = 25.0f;

    [FoldoutGroup("Gameplay"), Tooltip("Durée des phases de déplacement"), SerializeField]
    private float timeRotation = 1.0f;

    [FoldoutGroup("Gameplay"), Tooltip("Nombre de tir durant les phases de tir"), SerializeField]
    private int NumberOfShoots = 1;    

    private int currentNumberOfShoots = 0;

    private bool isMoving = true;
    private bool coroutineRunning = false;
    #endregion

    #region Core
    protected override void Move()
    {
        if (isMoving)
        {
            if (!coroutineRunning)
                StartCoroutine(SmoothRotation(timeRotation, angleRotation));
        }

    }

    protected override void Shoot()
    {
        if (!isMoving)
        {
            if (currentNumberOfShoots < NumberOfShoots)
            {
                if (weaponHandle.UseWeapon())
                    currentNumberOfShoots++;
            }
            else
            {
                currentNumberOfShoots = 0;
                isMoving = true;
            }
        }
    }
    #endregion

    IEnumerator SmoothRotation(float timeForRotation, float angle)
    {
        coroutineRunning = true;
        float myTime = 0.0f;
        Quaternion startRotation = transform.rotation;

        Quaternion rotationWanted = Quaternion.Euler(new Vector3(0, 0, angle));
        Quaternion endRotation = startRotation * rotationWanted;

        while (myTime < timeForRotation)
        {
            myTime += Time.deltaTime;
            float partOfTotalTime = myTime / timeForRotation;

            transform.rotation = Quaternion.Lerp(startRotation, endRotation, Mathf.Min(1, partOfTotalTime));
            yield return null;
        }
        isMoving = false;
        coroutineRunning = false;
    }

}
