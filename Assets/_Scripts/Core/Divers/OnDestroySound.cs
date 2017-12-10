using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// OnDestroySound Description
/// </summary>
[RequireComponent(typeof(AudioClip))]
public class OnDestroySound : MonoBehaviour, IKillable
{
    private AudioSource aClip;

    #region Initialization

    private void Awake()
    {
        aClip = gameObject.GetComponent<AudioSource>();
    }

    #endregion

    #region Core
    /// <summary>
    /// désactive l'objet
    /// </summary>
    private void DestroyThis()
    {
        gameObject.SetActive(false);
    }
    #endregion

    [FoldoutGroup("Debug"), Button("Kill")]
    public void Kill()
    {
        if (aClip.clip)
            Invoke("DestroyThis", aClip.clip.length);
        else
            DestroyThis();
    }
}
