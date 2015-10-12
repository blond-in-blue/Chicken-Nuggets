using UnityEngine;
using System.Collections;

public class ChickenControlBehavior : MonoBehaviour {

	public GameObject target = null;
	CharacterController controller;
	
	public float speed = 4.5f;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	public float power = 5f;
	
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

		takeDamageUpdate ();
	}

	void movementUpdate(){

		//simulate gravity
		Vector3 moveDown = Vector3.zero;
		movementDirection.y -= gravity * Time.deltaTime;
		moveDown.y = movementDirection.y;
		controller.Move (moveDown*Time.deltaTime);

	}

	void hitBoxCollisionEnter(Collision collision){


		if (collision.gameObject.transform == this.transform) {
			return;
		}


		if(currentState == ChickenState.dashing && dashingDirection == Vector3.forward){

			ChickenControlBehavior otherChicken = collision.gameObject.GetComponent<ChickenControlBehavior>();

			if(otherChicken != null){

				otherChicken.takeDamage(power);

			} else if (collision.transform.tag == "Hitbox"){
				//play a ting noise like in smashbros.

			}

			stopDashing();
		}

	}


	float damageStartTime = 0;

	public void takeDamage(float damageAmount){
		currentState = ChickenState.takingDamage;
		damageStartTime = Time.time;
		transform.FindChild("graphics").GetComponent<MeshRenderer>().material.color = Color.red;
	}

	void takeDamageUpdate(){

		if (currentState != ChickenState.takingDamage) {
			return;
		}

		//amount of time player is unable to move
		float stunTime = .2f;

		if (Time.time - damageStartTime > stunTime) {
			transform.FindChild("graphics").GetComponent<MeshRenderer>().material.color = Color.white;
			currentState = ChickenState.free;
		}

	}

	/// <summary>
	/// Has the chicken dash forward quickly and hurt things infront of it
	/// </summary>
	public void attackForward(){
		beginDash (Vector3.forward);
	}

	/// <summary>
	/// Has the chicken dash towards it's left quickly
	/// </summary>
	public void dashLeft(){
		beginDash (Vector3.left);
	}

	/// <summary>
	/// Has the chicken dash towards it's right quickly
	/// </summary>
	public void dashRight(){
		beginDash (Vector3.right);
	}

	/// <summary>
	/// Has the chicken dash backwards quickly
	/// </summary>
	public void dashBack(){
		beginDash (Vector3.back);
	}

	/// <summary>
	/// Begins the dash "animation".
	/// Dashing forward will count as an attack.
	/// Will dashing the player has no control.
	/// Dashing has a small "recovery" when the player can still not move
	/// </summary>
	/// <param name="direction">Direction.</param>
	void beginDash(Vector3 direction){

		//if the dash is happening in no direction then we can't dash
		if (direction.x == 0 && direction.z == 0) {
			return;
		}

		if (!canDash ()) {
			return;
		}

		if (direction == Vector3.forward) {
			transform.FindChild("hitbox").GetComponent<Rigidbody>().isKinematic = false;
			transform.FindChild("hitbox").GetComponent<SphereCollider>().enabled = true;
		}

		//Mark where we begin so we know when to end
		dashStartTime = Time.time;

		//Change state to dashing
		currentState = ChickenState.dashing;

		//Set the direction of the dash for the update function
		dashingDirection = direction;

		//give alittle hop
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

		//Variables for dashing and recovery
		float dashingDuration = .25f;
		float recoveryDuration = .15f;

		//if we're still dashing
		if (Time.time - dashStartTime < dashingDuration) {

			//Move at an accelerated speed
			move(dashingDirection, speed*3);

		} 
		//if we're done dashing and have completely recovered.
		else if(Time.time - dashStartTime > recoveryDuration+dashingDuration){

			//exit the dashing state
			stopDashing();

		}

	}

	void stopDashing(){
		dashingDirection = Vector3.zero;
		currentState = ChickenState.free;
		transform.FindChild("hitbox").GetComponent<Rigidbody>().isKinematic = true;
		transform.FindChild("hitbox").GetComponent<SphereCollider>().enabled = false;
	}

	/// <summary>
	/// Makes sure the player is always looking at the target if there is one.
	/// </summary>
	void lookAtTargetUpdate () {

		//If we don't have a target we can't look at it
		if (target == null) {
			return;
		}

		//A smooth rotation
		transform.rotation  = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position), Time.deltaTime*10);
	}

	/// <summary>
	/// Moves the chicken counter clockwise around it's target if it has one
	/// </summary>
	public void strafeRight ()
	{
		if (!canMove ()) {
			return;
		}
		move(Vector3.right, speed);
	}

	/// <summary>
	/// Moves the chicken clockwise around it's target if it has one
	/// </summary>
	public void strafeLeft ()
	{
		if (!canMove ()) {
			return;
		}
		move(Vector3.left, speed);
	}

	/// <summary>
	/// Moves the chicken backwards.
	/// </summary>
	public void moveBackward()
	{
		if (!canMove ()) {
			return;
		}
		move(Vector3.back, speed);
	}

	/// <summary>
	/// Moves the chicken forward.
	/// </summary>
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


	/// <summary>
	/// Determines whether or not the chicken is allowed to dash in it's current state of affairs..
	/// </summary>
	/// <returns><c>true</c>, if chicken can dash, <c>false</c> otherwise.</returns>
	bool canDash(){
		if (currentState == ChickenState.free && controller.isGrounded) {
			return true;
		}
		
		return false;
	}

}
