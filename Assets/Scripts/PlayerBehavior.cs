using UnityEngine;
using System.Collections;

[RequireComponent (typeof (ChickenControlBehavior))]
public class PlayerBehavior : MonoBehaviour {

	ChickenControlBehavior chickenController;

	// Use this for initialization
	void Start () {
		chickenController = gameObject.GetComponent<ChickenControlBehavior>();
	}

	//Monitors input that the player will give
	void inputUpdate ()
	{
		if(Input.GetAxisRaw("Horizontal") > 0){

			if(Input.GetButton("Jump")){
				chickenController.dashRight();
			}

			chickenController.strafeRight();

		} else if(Input.GetAxisRaw("Horizontal") < 0){

			if(Input.GetButton("Jump")){
				chickenController.dashLeft();
			}

			chickenController.strafeLeft();
		}

		if(Input.GetAxisRaw("Vertical") > 0){

			if(Input.GetButton("Jump")){
				chickenController.attackForward();
			}

			chickenController.moveForward();

		} else if(Input.GetAxisRaw("Vertical") < 0){

			if(Input.GetButton("Jump")){
				chickenController.dashBack();
			}

			chickenController.moveBackward();
		}

		if(Input.GetButton("Jump")){
			chickenController.attackForward();
		}

	}
	
	// Update is called once per frame
	void Update () {

		inputUpdate ();

	}

}
