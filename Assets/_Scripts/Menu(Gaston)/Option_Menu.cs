using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Option_Menu : MonoBehaviour {

    public GameObject selectButton;
    public GameObject Back;
    GameObject myEventSystem;

    public void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != selectButton && EventSystem.current.currentSelectedGameObject != Back)
        {
            EventSystem.current.SetSelectedGameObject(selectButton);
        }
    }

    public void Awake()
    {
        myEventSystem = GameObject.Find("EventSystem");
    }

    public void OnEnable()
    {
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(selectButton);
    }
}
