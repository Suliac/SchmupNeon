using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PauseMenu : MonoBehaviour {

    public GameObject Resume;
    public GameObject Options;
    public GameObject Restart;
    public GameObject MainMenu;

    GameObject myEventSystem;

    public void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != Resume && EventSystem.current.currentSelectedGameObject != Options &&
            EventSystem.current.currentSelectedGameObject != Restart && EventSystem.current.currentSelectedGameObject != MainMenu)
        {
            EventSystem.current.SetSelectedGameObject(Resume);
        }
    }

    public void Awake()
    {
        myEventSystem = GameObject.Find("EventSystem");
    }

    public void OnEnable()
    {
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(Resume);
    }
}
