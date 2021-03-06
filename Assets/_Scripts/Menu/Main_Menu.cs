﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Main_Menu : MonoBehaviour
{
    public GameObject PlayButton;
    public GameObject OptionButton;
    public GameObject Controls;
    public GameObject Leaderboard;
    public GameObject CreditsButton;
    public GameObject QuitButton;

    GameObject myEventSystem;

    public void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != PlayButton &&
            EventSystem.current.currentSelectedGameObject != OptionButton &&
            EventSystem.current.currentSelectedGameObject != Controls &&
            EventSystem.current.currentSelectedGameObject != Leaderboard &&
            EventSystem.current.currentSelectedGameObject != CreditsButton &&
            EventSystem.current.currentSelectedGameObject != QuitButton)
        {
            EventSystem.current.SetSelectedGameObject(PlayButton);
        }
    }

    public void Awake()
    {
        myEventSystem = GameObject.Find("EventSystem");
    }

    public void OnEnable()
    {
        myEventSystem.GetComponent<EventSystem>().SetSelectedGameObject(PlayButton);
    }
}
