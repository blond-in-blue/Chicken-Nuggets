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
    public AudioSource VolumeControl;

    public void Start()
    {
        ShowMenu(CurrentMenu);
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

	public void Update(){
		if(Input.GetKeyDown(KeyCode.M)){
			Application.LoadLevel("NetworkingLobby");
		}
	}

}
