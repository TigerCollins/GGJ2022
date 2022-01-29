using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScriptHelper : MonoBehaviour
{
    [SerializeField] MenuScript currentMenu;
    [SerializeField] MenuScript.OtherMenus targetMenu;

    public void ChangeMenu()
    {
        currentMenu.GoToDifferentMenu(targetMenu);
    }

    public void ChangeSceneMainMenu()
    {
        MainMenuHandler mainMenuHandler = currentMenu as MainMenuHandler;
        if(mainMenuHandler != null)
        {
            mainMenuHandler.OpenLevel();
        }
    }
}
