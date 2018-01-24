using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// MovePlatform Description
/// </summary>
public class MovePlatform : MonoBehaviour
{
    #region Attributes

    [FoldoutGroup("Gameplay"), Tooltip("Vitesse du scrolling"), SerializeField]
    private float speedScrolling = 10.0f;
    /*[SerializeField]
	private FrequencyTimer updateTimer; Disable for frame control */
    private bool isScrollingAcrtive = false;
    public bool IsScrollingAcrtive { get { return isScrollingAcrtive; } set { isScrollingAcrtive = value; } }

    #endregion

    #region Initialization

    private void Start()
    {
		// Start function
    }
    #endregion

    #region Core
    /// <summary>
    /// bouge la platform où se trouve les players, spawn & boundary
    /// </summary>
    private void Scroll()
    {
        float translation = speedScrolling * Time.deltaTime;
        transform.Translate(translation, 0, 0);
    }
    #endregion

    #region Unity ending functions

    private void Update()
    {
        if (isScrollingAcrtive)
            Scroll();
    }

	#endregion
}
