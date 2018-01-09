using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Option_Menu : MonoBehaviour {

    public GameObject FullScreen;
    public GameObject Resolution1;
    public GameObject Resolution2;
    public GameObject Resolution3;
    public GameObject Quality1;
    public GameObject Quality2;
    public GameObject Quality3;
    public GameObject Volume;
    public GameObject Back;
    GameObject myEventSystem;

    public void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != FullScreen
            && EventSystem.current.currentSelectedGameObject != Resolution1
            && EventSystem.current.currentSelectedGameObject != Resolution2
            && EventSystem.current.currentSelectedGameObject != Resolution3
            && EventSystem.current.currentSelectedGameObject != Quality1
            && EventSystem.current.currentSelectedGameObject != Quality2
            && EventSystem.current.currentSelectedGameObject != Quality3
            && EventSystem.current.currentSelectedGameObject != Volume
            && EventSystem.current.currentSelectedGameObject != Back)
        {
            EventSystem.current.SetSelectedGameObject(Back);
        }
    }

    public void Awake()
    {
        myEventSystem = GameObject.Find("EventSystem");
    }
    
    public void OnEnable()
    {
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(FullScreen);
    }
}
