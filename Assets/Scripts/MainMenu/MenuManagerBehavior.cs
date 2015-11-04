//I do not clame the rights to this code.
//All concepts came from the video:
//https://www.youtube.com/watch?v=QxRAIjXdfFU

//Things to do:
//  -When mouse hovers over button make a sound.
//  -When button is clicked make a sound.
//  -Menu transitions.
//  -Scene transition (fade in fade out).
//  -Custom menu sprites.
//  -Menue Scenery(chicken coop?)
//  -Particle system that emits fethers.


using UnityEngine;
using System.Collections;

public class MenuManagerBehavior : MonoBehaviour {

    public MenuBehavior CurrentMenu;
    public MenuBehavior PauseMenu;
    public MenuBehavior PlayersOnServerMenu;
    public MenuBehavior TeamAndPlayerInfoMenu;
    private bool IsOpenCurrentMenu = true;
    private bool IsOpenPauseMenu = true;
    private bool IsOpenPlayersOnServerMenu = true;
    private bool IsOpenTeamAndPlayerInfoMenu = true;

    public void Start()
    {
        if(Application.loadedLevelName == "MainMenuRework")
        {
            ShowMenu(CurrentMenu);
        }
        
        if(Application.loadedLevelName == "NetworkingLobby")
        {
            TeamAndPlayerInfoMenu.IsOpen = IsOpenTeamAndPlayerInfoMenu;
            IsOpenTeamAndPlayerInfoMenu = !IsOpenTeamAndPlayerInfoMenu;
        }
    }

    public void ShowMenu(MenuBehavior menu)
    {
        if (CurrentMenu != null)
        {
            CurrentMenu.IsOpen = false;
        }
        CurrentMenu = menu;
        CurrentMenu.IsOpen = true;

    }

    public void StartALevel(string level)
    {
        Application.LoadLevel(level);
    }

    public void Quit()
    {
        Debug.Break();
        Application.Quit();
    }

    public void ShowPause()
    {
        PauseMenu.IsOpen = IsOpenPauseMenu;
        IsOpenPauseMenu = !IsOpenPauseMenu;
    }

	public void Update(){

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PlayersOnServerMenu.IsOpen = IsOpenPlayersOnServerMenu;
            IsOpenPlayersOnServerMenu = !IsOpenPlayersOnServerMenu;
        }

        if(Input.GetKeyDown(KeyCode.F1))
        {
            TeamAndPlayerInfoMenu.IsOpen = IsOpenTeamAndPlayerInfoMenu;
            IsOpenTeamAndPlayerInfoMenu = !IsOpenTeamAndPlayerInfoMenu;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.IsOpen = IsOpenPauseMenu;
            IsOpenPauseMenu = !IsOpenPauseMenu;
        }

	}


}
