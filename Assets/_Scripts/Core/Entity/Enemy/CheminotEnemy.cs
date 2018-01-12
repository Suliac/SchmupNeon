using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

/// <summary>
/// CheminotEnemy Description
/// </summary>
public class CheminotEnemy : ShootingEnemy
{
    #region Attributes
    [Header("Attributs cheminot")]
    [FoldoutGroup("GamePlay"), Tooltip("Rotation entre chaque tir"), SerializeField, Range(0, 360)]
    private float angleRotation = 25.0f;

    [FoldoutGroup("GamePlay"), Tooltip("Durée des phases de déplacement"), SerializeField]
    private float timeRotation = 1.0f;

    [FoldoutGroup("GamePlay"), Tooltip("Tir toutes les X secondes"), SerializeField]
    private FrequencyTimer shootFrequency;

    private bool rotateRight = true;
    private bool coroutineRunning = false;
    #endregion

    #region Core
    protected override void Move()
    {
        if (!coroutineRunning)
            StartCoroutine(SmoothRotation(timeRotation, angleRotation));
    }

    protected override void Shoot()
    {
        if (shootFrequency.Ready())
        {
            if (weaponHandle)
                weaponHandle.UseWeapon(); 
        }
    }

    protected override void OnBeforeKill()
    {
        // Nothing to do
    }

    /// <summary>
    /// lors de l'activation de l'objet quand il entre dans la caméra
    /// </summary>
    protected override void OnEnableInCamera()
    {
        transform.SetParent(GameManager.GetSingleton.EnemyGroup);   //set l'ennemy dans la moving platform
        if (timeBeforeStop != -1)
            StartCoroutine(stopAfterSomeTime());
    }

    IEnumerator stopAfterSomeTime()
    {
        yield return new WaitForSeconds(timeBeforeStop);
        
        transform.SetParent(GameManager.GetSingleton.ObjectDynamiclyCreated);   //set l'ennemy en static mouvement
    }

    IEnumerator SmoothRotation(float timeForRotation, float angle)
    {
        angle = rotateRight ? angle : angle * -1;
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
        rotateRight = !rotateRight;
        coroutineRunning = false;
    }
    #endregion

}
