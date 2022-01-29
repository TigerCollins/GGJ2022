using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class MainMenuHandler : MenuScript
{
    [Header("Main Menu Specific")]
    [SerializeField] MMFeedbacks hideFeedback;
    [SerializeField] LevelLoader gameLevelLoader;
    public void Init()
    {
     //base.   
    }

    public void OpenLevel()
    {
        gameLevelLoader.LoadScene();
    }

    public void CloseWholeMenuFunction()
    {
        hideFeedback.PlayFeedbacks();
    }
}
