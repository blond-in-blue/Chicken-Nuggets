using UnityEngine;
using System.Collections;

[RequireComponent (typeof (ChickenControlBehavior))]
/// <summary>
/// Monitors player input and acts as an interface beween player controls and chicken functionality.
/// </summary>
public class PlayerBehavior : MonoBehaviour {

	ChickenControlBehavior chickenController;
	
	//The speed at which the player is rotated relative to mouse movement
	int mouseSensitivity = 5;



	// Use this for initialization
	void Start () {
		chickenController = gameObject.GetComponent<ChickenControlBehavior>();
	}

	/// <summary>
	/// Monitors input that the player will give
	/// </summary>
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
		
		// Jump
		if(Input.GetButton("Jump")){
			chickenController.attackForward();
		}
		
		// Right click
		// Target mode toggle
		if(Input.GetMouseButtonDown(1)) {
			targetModeToggle();
		} 
		
		// When the player is not currently targeting an object.
		if (chickenController.getTarget() == null) {
			 
			 // Rotate the chicken and camera concurrently on the X-axis with mouse movement.
			 if(Input.GetAxis("Mouse X") != 0){
				 transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity , Space.World);

			 }
			 
			 // Rotate the camera around the chicken on Y-axis with mouse movement.
			 float mouseYAxis = Input.GetAxis("Mouse Y");
			 if(mouseYAxis != 0){
				 transform.Find("Main Camera").transform.RotateAround(transform.position, transform.TransformDirection(-1,0,0), mouseYAxis * mouseSensitivity);
				 
				 if (mouseYAxis < 0) {
					  
				 }
			 }
			 
			 // Update the positions of the X and Y axes.

			 // Always look at the chicken.
			 //transform.Find("Main Camera").transform.LookAt(transform.position);
		} else if (chickenController.getTarget() != null) {
			 //transform.Find("Main Camera").transform.LookAt(chickenController.getTarget());
		}

	}
	
	// Update is called once per frame
	void Update () {

		inputUpdate ();


	}

	void targetModeToggle() {
		
			if (chickenController.getTarget() != null) {
				// Set the target to null.
				chickenController.setTarget(null);
				Debug.Log("Target has been set to null.");
				
				// Enable free-look camera.
				// There is no target, so allow the chicken to rotate on the x-axis using mouse movement.

			} else if (chickenController.getTarget() == null) {
				// Set the target.
				ChickenControlBehavior[] chickens = GameState.getInstance ().getAllCharactersNotOnTeam(chickenController.getChickensTeam());
		
				if (chickens == null || chickens.Length == 0) {
					return;
				}
		
				ChickenControlBehavior closestChicken = null;
				float closestDistance = float.PositiveInfinity;
				for(int i = 0; i < chickens.Length; i ++){
		
					float curDist = Vector3.Distance(chickens[i].transform.position, transform.position);
		
					if(curDist < closestDistance){
						closestChicken = chickens[i];
						closestDistance = curDist;
					}
		
				}
		
				chickenController.setTarget (closestChicken.gameObject);
				Debug.Log("Target has been set to closest chicken.");
			}

	}

}

