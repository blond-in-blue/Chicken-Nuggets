using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuControl : MonoBehaviour {


	public Canvas CreditsMenu;
	public Canvas QuitMenu;
	public Button QuitGameButton;
	public Button CreditsButton;
	public Button TrainingButton;
	public Button CockFightButton;
	public Button ChickenWafflesButton;
	public Button ChickenSkewerButton;
	
	
	// Use this for initialization
	void Start () {
		
		QuitMenu = QuitMenu.GetComponent<Canvas> ();
		QuitMenu.enabled = false;
		QuitGameButton = QuitGameButton.GetComponent<Button> ();
		CreditsButton = CreditsButton.GetComponent<Button> ();
		TrainingButton = TrainingButton.GetComponent<Button> ();
		CockFightButton = CockFightButton.GetComponent<Button> ();
		ChickenWafflesButton = ChickenWafflesButton.GetComponent<Button> ();
		ChickenSkewerButton = ChickenSkewerButton.GetComponent<Button> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	// Disables all main menu buttons while the quit menu is opened.
	public void OpenQuitMenu() {
		
		QuitMenu.enabled = true;
		QuitGameButton.enabled = false;
		CreditsButton.enabled = false;
		TrainingButton.enabled = false;
		CockFightButton.enabled = false;
		ChickenWafflesButton.enabled = false;
		ChickenSkewerButton.enabled = false;
	}
	
	// Re-enables all main menu buttons when the quit menu is closed. 
	public void QuitMenuCancel() {
		
		QuitMenu.enabled = false;
		QuitGameButton.enabled = true;
		CreditsButton.enabled = true;
		TrainingButton.enabled = true;
		CockFightButton.enabled = true;
		ChickenWafflesButton.enabled = true;
		ChickenSkewerButton.enabled = true;
	}
	
	public void QuitGame() {
		
		Application.Quit();
	}
	
	public void OpenCredits() {
		
		//Application.OpenCreditsMenu();
	}
	
	public void OpenTraining() {
		
		//Application.LoadLevel(Training1);
	}
	
	public void OpenCockFight() {
		
		Application.LoadLevel("Prototype");
	}
	
	public void OpenChickenWaffles() {
		
	}
	
	public void OpenChickenSkewer() {
		
	}
}