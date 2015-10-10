using UnityEngine;
using System.Collections;

public class PlayerBehavior : MonoBehaviour {

	public GameObject target = null;
	CharacterController controller;

	public float speed = 4.5f;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;

	enum playerStatePrototype {free, attacking, strafing, juicing, takingDamage}
	playerStatePrototype currentState = playerStatePrototype.free;



	// Use this for initialization
	void Start () {
	
		controller = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {

		lookAtUpdate ();

		CharacterController controller = GetComponent<CharacterController>();
		if (controller.isGrounded) {
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			if (Input.GetButtonDown("Jump")) {
				if (Input.GetAxis ("Horizontal") != 0) {
					moveDirection.x = jumpSpeed * 15 * Input.GetAxis("Horizontal");
				}
				else if (Input.GetAxis ("Horizontal") == 0 && Input.GetAxis ("Vertical") == 0) {
					moveDirection.z = -jumpSpeed * 15;
				}
			}
			
		}
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);

	}

	void movementUpdate () {


	}


	void lookAtUpdate () {

		transform.LookAt(target.transform);
	}

}
