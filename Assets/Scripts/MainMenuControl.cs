using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuControl : MonoBehaviour {

	//public Menu CreditsMenu;
	public Button CreditsButton;
	public Button TrainingButton;
	public Button CockFightButton;
	public Button ChickenWafflesButton;
	public Button ChickenSkewerButton;
	
	
	// Use this for initialization
	void Start () {
		
		CreditsButton = CreditsButton.GetComponent<Button> ();
		TrainingButton = TrainingButton.GetComponent<Button> ();
		CockFightButton = CockFightButton.GetComponent<Button> ();
		ChickenWafflesButton = ChickenWafflesButton.GetComponent<Button> ();
		ChickenSkewerButton = ChickenSkewerButton.GetComponent<Button> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OpenCredits() {
		//Application.OpenCreditsMenu();
	}
	
	void OpenTraining() {
		//Application.LoadLevel(Training1);
	}
	
	public void OpenCockFight() {
		//Application.LoadLevel(CockFight1);
		Application.LoadLevel ("Prototype");
	}
	
	void OpenChickenWaffles() {
		
	}
	
	void OpenChickenSkewer() {
		
	}
}
