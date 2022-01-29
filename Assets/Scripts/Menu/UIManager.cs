using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    public MenuScripts menuScripts;
    [SerializeField] GameVersion gameVersion;
    [SerializeField] QuitPrompt quitPrompt;

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
            menuScripts.currentMenu = menuScripts.defaultMenu;
            menuScripts.defaultMenu.ShowMenu();
            menuScripts.ForceCloseOtherMenuScripts();
            menuScripts.InitSwitch();
            gameVersion.Init();
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
    public MenuScript lobbyMenuScript;
    public MenuScript optionMenuScript;
    public MenuScript saveMenuScript;
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

        if (lobbyMenuScript != null)
        {
            list.Add(lobbyMenuScript);
        }

        if (optionMenuScript != null)
        {
            list.Add(optionMenuScript);
        }

        if (saveMenuScript != null)
        {
            list.Add(saveMenuScript);
        }
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

   /* public LobbyHandler LobbyHandlerScript()
    {
        return lobbyMenuScript as LobbyHandler;
    }

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
                   // MainMenuScript().CloseWholeMenuFunction();
                }
               if(item == lobbyMenuScript)
                {
                  //  LobbyHandlerScript().CloseWholeMenuFunction();
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
            case MenuScript.OtherMenus.Lobby:
              //  UIManager.instance.menuScripts.LobbyHandlerScript().Init();
                break;
            case MenuScript.OtherMenus.Options:
                break;
            case MenuScript.OtherMenus.Saves:
                break;
            default:
                break;
        }
    }
}

[System.Serializable]
public class GameVersion
{
    [Header("Game Version")]
    public TextMeshProUGUI versionText;
    public string gameVersion;

    public void Init()
    {
        if (gameVersion == null)
        {
            gameVersion = Application.version;
        }
        versionText.text = gameVersion;
    }
}


[System.Serializable]
public class QuitPrompt
{
    [Header("Quit Prompt Dialogue")]
    [TextArea]    public  string quitWarning;
    public string confirmText;
    public string declineText;

    public void Cancel()
    {
        //
    }
}