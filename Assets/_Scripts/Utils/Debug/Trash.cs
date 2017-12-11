using UnityEngine;

/// <summary>
/// Trash détruit l'objet courant instantanément
/// </summary>
public class Trash : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject);
    }
}
