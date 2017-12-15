using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// InitialiseButton Description
/// </summary>
public class InitialiseButton : MonoBehaviour
{
    GameObject lastselect;
    void Start()
    {
        lastselect = new GameObject();
    }
    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastselect);
        }
        else
        {
            lastselect = EventSystem.current.currentSelectedGameObject;
        }
    }
}