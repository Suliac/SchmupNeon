using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Sure_Menu : MonoBehaviour {

    public GameObject Yes;
    public GameObject No;
    GameObject myEventSystem;

    public void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != Yes
            && EventSystem.current.currentSelectedGameObject != No)
        {
            EventSystem.current.SetSelectedGameObject(Yes);
        }
    }

    public void Awake()
    {
        myEventSystem = GameObject.Find("EventSystem");
    }

    public void OnEnable()
    {
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(No);
    }
}
