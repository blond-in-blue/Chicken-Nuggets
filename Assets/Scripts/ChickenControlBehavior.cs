using UnityEngine;
using System.Collections;

public class ChickenControlBehavior : MonoBehaviour {

	public GameObject target = null;
	CharacterController controller;
	
	public float speed = 4.5f;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	
	enum ChickenState {free, dashing, strafing, juicing, takingDamage}
	ChickenState currentState = ChickenState.free;

	// Use this for initialization
	void Start () {
		controller = GetComponent<CharacterController> ();
	
	}
	
	// Update is called once per frame
	void Update () {

		lookAtTargetUpdate ();

		dashUpdate ();

		movementUpdate ();
	}

	void movementUpdate(){
		//simulate gravity
		Vector3 moveDown = Vector3.zero;
		movementDirection.y -= gravity * Time.deltaTime;
		moveDown.y = movementDirection.y;
		controller.Move (moveDown*Time.deltaTime);
	}

	public void attackForward(){
		beginDash (Vector3.forward);
	}

	public void dashLeft(){
		beginDash (Vector3.left);
	}

	public void dashRight(){
		beginDash (Vector3.right);
	}

	public void dashBack(){
		beginDash (Vector3.back);
	}

	void beginDash(Vector3 direction){

		if (!canDash ()) {
			return;
		}

		dashStartTime = Time.time;
		currentState = ChickenState.dashing;
		direction = 
		dashingDirection = direction;
		movementDirection.y = 3;

	}

	Vector3 dashingDirection = Vector3.zero;
	float dashStartTime = 0f;

	/// <summary>
	/// The update for dashing every frame incase the chicken is in the dashing state.
	/// </summary>
	void dashUpdate(){

		if (currentState != ChickenState.dashing) {
			return;
		}

		if (Time.time - dashStartTime < .25f) {
			move(dashingDirection, speed*3);
		} else {
			dashingDirection = Vector3.zero;
			currentState = ChickenState.free;
		}

	}
	
	void lookAtTargetUpdate () {

		if (target == null) {
			return;
		}

		transform.LookAt(target.transform);
	}

	public void strafeRight ()
	{
		if (!canMove ()) {
			return;
		}
		move(Vector3.right, speed);
	}

	public void strafeLeft ()
	{
		if (!canMove ()) {
			return;
		}
		move(Vector3.left, speed);
	}

	public void moveBackward()
	{
		if (!canMove ()) {
			return;
		}
		move(Vector3.back, speed);
	}

	public void moveForward()
	{
		if (!canMove ()) {
			return;
		}
		move(Vector3.forward, speed);
	}


	Vector3 movementDirection = Vector3.zero;

	/// <summary>
	/// Moves by the specified direction relative 
	/// to the direction the chicken is facing.
	/// 
	/// Ignores the y direction.
	/// Ignores state of chicken.
	/// </summary>
	/// <param name="direction">Direction.</param>
	void move(Vector3 direction, float movementSpeed){

		//Change the direction to be relative to the direction the chicken is facing
		//i.e. Vector3.left is chickens left instead of global left
		direction = transform.TransformDirection(direction);

		//move along x and z appropriately
		movementDirection.z = direction.z * movementSpeed;
		movementDirection.x = direction.x * movementSpeed;

		//Move chicken
		controller.Move(movementDirection * Time.deltaTime);

	}

	/// <summary>
	/// Determines whether or not the chicken is allowed to move.
	/// </summary>
	/// <returns><c>true</c>, if can move, <c>false</c> otherwise.</returns>
	bool canMove (){

		if (currentState == ChickenState.free && controller.isGrounded) {
			return true;
		}

		return false;

	}

	bool canDash(){
		if (currentState == ChickenState.free && controller.isGrounded) {
			return true;
		}
		
		return false;
	}

}
