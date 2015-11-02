using UnityEngine;
using System.Collections;

public class PauseMenuManager : MonoBehaviour {

    public PauseMenuBehavior CurrentMenu;
    public AudioSource VolumeControl;
    private bool MenuIsOpen = false;

    public void Start()
    {
        //ShowMenu(CurrentMenu);
    }

    public void ShowMenu(PauseMenuBehavior menu)
    {
        if (CurrentMenu != null)
        {
            CurrentMenu.IsOpen = false;
        }
        CurrentMenu = menu;
        CurrentMenu.IsOpen = true;

    }

    public void CloseMenu(PauseMenuBehavior menu)
    {
        CurrentMenu = menu;
        CurrentMenu.IsOpen = false;
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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Application.LoadLevel("NetworkingLobby");
        }

        if (Input.GetKeyDown(KeyCode.Escape) && CurrentMenu.IsOpen == false )
        {
            ShowMenu(CurrentMenu);
        

        }


        //if (Input.GetKeyDown(KeyCode.Escape) && MenuIsOpen == true)
        //{
        //    CurrentMenu.IsOpen = false;
        //    MenuIsOpen = false;
        //}


    }

}
