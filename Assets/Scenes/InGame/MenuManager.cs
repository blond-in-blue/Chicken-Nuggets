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

public class MenuManager : MonoBehaviour {

    public Menu CurrentMenu;

    public void Start()
    {
        ShowMenu(CurrentMenu);
    }

    public void ShowMenu(Menu menu)
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

}
