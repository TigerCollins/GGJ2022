using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    public MenuScripts menuScripts;
    public List<CreditsDetails> credits;

    private void Awake()
    {
        instance = this;
       
    }

    private void Start()
    {
        Init();
    }

    void Init()
    {
        if(menuScripts.GetMenuScriptsAsList().Count != 0)
        {
            menuScripts.defaultMenu.GoToDifferentMenu(menuScripts.defaultMenu.menuGroup);
            menuScripts.currentMenu = menuScripts.defaultMenu;
            menuScripts.ForceCloseOtherMenuScripts();
            menuScripts.InitSwitch();
        }
      
    }


    public void Quit()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void ResumeGame()
    {
     //   GameController.instance.gamePhase = GameController.instance.resumeGamePhase;
    }

   
}

[System.Serializable]
public class MenuScripts
{
    public MenuScript defaultMenu;
    public MenuScript currentMenu;

    [Space(15)]

    public MenuScript mainMenuScript;
    public MenuScript creditsScript;
    public MenuScript optionMenuScript;
    public MenuScript otherScript;
    public MenuScript splashMenuScript;
    public MenuScript pauseMenuScript;
    public MenuScript generalGUIScript;

    public MenuScript CurrentMenuScript
    {
        set
        {
            currentMenu = value;
        }

        get
        {
            return currentMenu;
        }
    }

    public List<MenuScript> GetMenuScriptsAsList()
    {
        List<MenuScript> list = new List<MenuScript>();
        if (mainMenuScript != null)
        {
            list.Add(mainMenuScript);
        }
        
        if (creditsScript != null)
        {
            list.Add(creditsScript);
        }
        
        if (optionMenuScript != null)
        {
            list.Add(optionMenuScript);
        }

       /* if (saveMenuScript != null)
        {
            list.Add(saveMenuScript);
        }
       */
        if (splashMenuScript != null)
        {
            list.Add(splashMenuScript);
        }
        if (pauseMenuScript != null)
        {
            list.Add(pauseMenuScript);
        }
        if (generalGUIScript != null)
        {
            list.Add(generalGUIScript);
        }
        return list;
    }

    public MainMenuHandler MainMenuScript()
    {
        return mainMenuScript as MainMenuHandler;
    }

    public CreditsMenuhandler CreditsMenuHandlerScript()
    {
        return creditsScript as CreditsMenuhandler;
    }
    /*
    public GeneralMenuGameplayHandler GeneralGUIScript()
    {
        return generalGUIScript as GeneralMenuGameplayHandler;
    }
   */

    public void ForceCloseOtherMenuScripts()
    {
        foreach (MenuScript item in GetMenuScriptsAsList())
        {
            if(item != currentMenu)
            {
                if(item == mainMenuScript)
                {
                    MainMenuScript().CloseWholeMenuFunction();
                }
               if(item == creditsScript)
                {
                    CreditsMenuHandlerScript().CloseWholeMenuFunction();
                }
                
            }

            
        }
    }

    public void InitSwitch()
    {
        switch (currentMenu.menuGroup)
        {
            case MenuScript.OtherMenus.Main:
                UIManager.instance.menuScripts.MainMenuScript().Init();
                break;
            case MenuScript.OtherMenus.Options:
              //  UIManager.instance.menuScripts.LobbyHandlerScript().Init();
                break;
            case MenuScript.OtherMenus.Credits:
                break;
            case MenuScript.OtherMenus.Other:
                break;
            default:
                break;
        }
    }
}

[System.Serializable]
public class CreditsDetails
{
    public string name;
    public string job1;
    public string job2;
    public Sprite icon;
    public LinksInfo webLink;
}

[System.Serializable]
public class LinksInfo
{
    public string displayName;
    public string link;
}
