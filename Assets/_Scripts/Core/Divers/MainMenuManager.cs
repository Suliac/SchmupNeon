using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MainMenuManager Description
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    #region Attributes

    [Tooltip("GamePlay"), SerializeField]
    private GameObject menuOption;

    [Tooltip("GamePlay"), SerializeField]
    private GameObject areYouSure;

    [Tooltip("Debug"), SerializeField]
    private List<CustomButton> buttonsMainMenu;

    [Tooltip("Debug"), SerializeField]
    private List<CustomButton> buttonsOption;

    [Tooltip("Debug"), SerializeField]
    private List<CustomButton> buttonsAreYouSure;


    [Tooltip("opti fps"), SerializeField]
    private FrequencyTimer updateTimer;

    private int modeMenu = 0;           //0: normal, 1: areYouSure, 2: quit
    private bool optionDisplay = false;
    private bool areYouSureDisplay = false;

    #endregion

    #region Initialization

    private void Start()
    {
        Cursor.visible = false;
        buttonsMainMenu[0].Select();
        displayOptionGame(false);
        menuOption.SetActive(false);
    }
    #endregion

    #region Core
    private void activeList(List<CustomButton> butt, bool active)
    {
        for (int i = 0; i < butt.Count; i++)
        {
            butt[0].interactable = active;
        }
    }

    /// <summary>
    /// affiche ou cache le menu option ?
    /// </summary>
    /// <param name="active"></param>
    public void displayOptionGame(bool active)
    {
        modeMenu = (active) ? 2 : 0;
        optionDisplay = active;
        menuOption.SetActive(active);
        if (!optionDisplay)
        {
            //activeList(buttonsMainMenu, true);
            //activeList(buttonsOption, false);
            buttonsMainMenu[0].Select();
        }            
        else
        {
            //activeList(buttonsMainMenu, false);
            //activeList(buttonsOption, true);
            buttonsOption[0].Select();
        }
            
    }

    /// <summary>
    /// affiche ou cache le menu etes vous sure ?
    /// </summary>
    /// <param name="active"></param>
    public void displayMenuAreYouSure(bool active)
    {
        modeMenu = (active) ? 1 : 0;
        areYouSureDisplay = active;
        areYouSure.SetActive(active);
        if (!areYouSureDisplay)
        {
            //activeList(buttonsMainMenu, true);
            //activeList(buttonsAreYouSure, false);
            buttonsMainMenu[0].Select();
        }            
        else
        {
            //activeList(buttonsMainMenu, false);
            //activeList(buttonsAreYouSure, true);
            buttonsAreYouSure[0].Select();
        }
    }

    /// <summary>
    /// from button
    /// </summary>

    public void StartGame()
    {
        SceneChangeManager.GetSingleton.JumpToSceneWithFade("2_Game");
    }

    public void Quit()
    {
        SceneChangeManager.GetSingleton.Quit();
    }

    /// <summary>
    /// input des menus
    /// </summary>
    private void InputMenu()
    {
        switch (modeMenu)
        {
            case 0: //menu principal
                if (PlayerConnected.GetSingleton.getPlayer(-1).GetButtonDown("Escape")
                    || PlayerConnected.GetSingleton.getPlayer(0).GetButtonDown("FireB")
                    || PlayerConnected.GetSingleton.getPlayer(0).GetButtonDown("Start"))
                {
                    Debug.Log("ici ????");
                    displayMenuAreYouSure(true);
                }
                break;
            case 1: //menu areYouSure
                if (PlayerConnected.GetSingleton.getPlayer(-1).GetButtonDown("Escape")
                    || PlayerConnected.GetSingleton.getPlayer(0).GetButtonDown("FireB")
                    || PlayerConnected.GetSingleton.getPlayer(0).GetButtonDown("Start"))
                {
                    displayMenuAreYouSure(false);
                }
                break;
            case 2: //menu areYouSure
                if (PlayerConnected.GetSingleton.getPlayer(-1).GetButtonDown("Escape")
                    || PlayerConnected.GetSingleton.getPlayer(0).GetButtonDown("FireB")
                    || PlayerConnected.GetSingleton.getPlayer(0).GetButtonDown("Start"))
                {
                    displayOptionGame(false);
                }
                break;
        }
        
    }

    #endregion

    #region Unity ending functions

    private void Update()
    {
        InputMenu();
      //optimisation des fps
      if (updateTimer.Ready())
      {

      }
    }

	#endregion
}
