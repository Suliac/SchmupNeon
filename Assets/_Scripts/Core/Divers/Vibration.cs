using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Vibration Description
/// </summary>
public class Vibration : MonoBehaviour
{
    #region Attributes

    [FoldoutGroup("GamePlay"), Tooltip("set le rotor à 0 ou 1 pour un rotor spacifique, ou 2 pour utiliser les deux"), SerializeField]
    private int rotorUsed = 0;

    [FoldoutGroup("GamePlay"), Tooltip("intensité rotor 0"), SerializeField]
    public float inensityLevel0 = 0;
    [FoldoutGroup("GamePlay"), Tooltip("intensité rotor 1"), SerializeField]
    public float inensityLevel1 = 0;
    [FoldoutGroup("GamePlay"), Tooltip("durée rotor 0"), SerializeField]
    public float duration0 = 0;
    [FoldoutGroup("GamePlay"), Tooltip("durée rotor 0"), SerializeField]
    public float duration1 = 0;

    #endregion

    #region Initialization

    #endregion

    #region Core
    public void play(int idPlayer)
    {
        if (rotorUsed == 2)
        {
            PlayerConnected.GetSingleton.getPlayer(idPlayer).SetVibration(0, inensityLevel0, duration0);
            PlayerConnected.GetSingleton.getPlayer(idPlayer).SetVibration(1, inensityLevel1, duration1);
        }
        else
        {
            Debug.Log("here");
            float intensity = (rotorUsed == 0) ? inensityLevel0 : inensityLevel1;
            float duration = (rotorUsed == 0) ? duration0 : duration1;
            PlayerConnected.GetSingleton.getPlayer(idPlayer).SetVibration(rotorUsed, intensity, duration);
        }
    }
    #endregion

    #region Unity ending functions

	#endregion
}
