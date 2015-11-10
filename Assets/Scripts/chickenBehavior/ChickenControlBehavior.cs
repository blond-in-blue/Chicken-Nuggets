using UnityEngine;
using System.Collections;

/// <summary>
/// Chicken control behavior.
/// Every chicken should have this behavior which allows different things to 
/// control a chicken (Player or AI) by calling the methods of this class.
/// </summary>
public class ChickenControlBehavior : MonoBehaviour {

	public GameObject target = null;
	CharacterController controller;

	[SerializeField]
	float speed = 4.5f;

	[SerializeField]
	float jumpSpeed = 8.0F;

	[SerializeField]
	float gravity = 20.0F;

	[SerializeField]
	float power = 5f;

	[SerializeField]
	ChickenTeam team = ChickenTeam.Red;

    private static float maxHealth = 20F; // For use in ChickenAttackAIBehavior

    [SerializeField]
    float health = maxHealth;
	

	ChickenState currentState = ChickenState.Free;

	// Use this for initialization
	void Start () {

		controller = GetComponent<CharacterController> ();
	
	}


    /// <summary>
    /// Returns the current state the chicken is in
    /// </summary>
    /// <returns> The state that the chicken is in. </returns>
    public ChickenState getCurrentChickenState()
    {
        return currentState;
    }

    /// <summary>
    /// Returns the max health of the chicken
    /// </summary>
    /// <returns> The maximum health of the chicken. </returns>
    public float getMaxHealth()
    {
        return maxHealth;
    }

    /// <summary>
    /// Returns the current health of the chicken
    /// </summary>
    /// <returns> The current health of the chicken. </returns>
    public float getCurrentHealth()
    {
        return health;
    }
	
	// Update is called once per frame
	void Update () {

		lookAtTargetUpdate ();

		movementUpdate ();

		switch(currentState){

			case ChickenState.Dead:
				deadUpdate();
			break;

			case ChickenState.Dashing:
				dashUpdate ();
			break;

			case ChickenState.TakingDamage:
				takeDamageUpdate ();
			break;
			
		}

	}

	/// <summary>
	/// Gets what team the chicken is currentely fighting for
	/// </summary>
	/// <returns>The chickens team.</returns>
	public ChickenTeam getChickensTeam(){
		return team;
	}

	/// <summary>
	/// Changes what team the chicken is currentely on.
	/// </summary>
	/// <param name="team">Team.</param>
	public void setChickensTeam(ChickenTeam team){
		this.team = team;
	}
	
	/// <summary>
	/// Returns what the chicken is currentely focused on as it's target
	/// </summary>
	/// <returns>The target.</returns>
	public GameObject getTarget(){
		return target;
	}

	/// <summary>
	/// Set's the new target for the chicken to focus on.
	/// Setting the target to null turns off lock on.
	/// </summary>
	/// <param name="newTarget">New target.</param>
	public void setTarget(GameObject newTarget){
		this.target = newTarget;
	}

	/// <summary>
	/// Update of the chicken movement of anything outside of external control.
	/// This includes things like gravity.
	/// </summary>
	void movementUpdate(){

		//simulate gravity
		Vector3 moveDown = Vector3.zero;
		movementDirection.y -= gravity * Time.deltaTime;
		moveDown.y = movementDirection.y;
		controller.Move (moveDown*Time.deltaTime);

	}

	void OnCollisionEnter(Collision collision){
		if(collision.transform.tag == "BottomOfLevel"){
			enterDeadState();
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.transform.tag == "BottomOfLevel"){
			enterDeadState();
		}
	}

	/// <summary>
	/// This is called when one of our hitboxes collides with something while we're attacking.
	/// </summary>
	/// <param name="collision">Collision, the collision that took place</param>
	void hitBoxCollisionEnter(Collision collision){

		//If we hit ourself some how...
		if (collision.gameObject.transform == this.transform) {
			return;
		}


		if(currentState == ChickenState.Dashing && dashingDirection == Vector3.forward){

			ChickenControlBehavior otherChicken = collision.gameObject.GetComponent<ChickenControlBehavior>();

			if(otherChicken != null && (otherChicken.team != team || otherChicken.team == ChickenTeam.None) ){//added here disables friendly fire
			
				if( otherChicken.gameObject.GetComponent<NetworkChickenCharacter>() != null ){

					SpecialEffectsFactory.createEffect (otherChicken.gameObject.transform.position, SpecialEffectType.TakeDamage);
					otherChicken.gameObject.GetComponent<PhotonView>().RPC("takeDamage",otherChicken.gameObject.GetComponent<PhotonView>().owner,power);
					//otherChicken.gameObject.GetComponent<PhotonView>().RPC("takeDamage",PhotonTargets.All,power);

				} else {

					otherChicken.takeDamage(power);

				}

			} else if (collision.transform.tag == "Hitbox"){
				//play a ting noise like in smashbros.

			}

			stopDashing();
		}

	}
	
	void rotateLeft(){
		transform.Rotate(Vector3.up * Input.GetAxis("Mouse X"), Space.World);
			 
		// Maintain Z-axis rotation at zero.
		Quaternion chickenRotation = transform.rotation;
		chickenRotation.z = 0;
		transform.rotation = chickenRotation;
	}
	
	void rotateRight(float intensity){
		transform.Rotate(Vector3.up * Input.GetAxis("Mouse X"), Space.World);
			 
		// Maintain Z-axis rotation at zero.
		Quaternion chickenRotation = transform.rotation;
		chickenRotation.z = 0;
		transform.rotation = chickenRotation;
	}


	float damageStartTime = 0;



	/// <summary>
	/// Puts the chicken in a taking damage state and subtracts health from the chicken
	/// </summary>
	/// <param name="damageAmount">Damage amount.</param>
	[PunRPC]
	public void takeDamage(float damageAmount){

		if (currentState == ChickenState.Dead) {
			return;
		}

		SpecialEffectsFactory.createEffect (transform.position, SpecialEffectType.TakeDamage);

		// added check to see if player is dead
		if (health <= 0) {
			enterDeadState();
			return;
		}

		currentState = ChickenState.TakingDamage;
		damageStartTime = Time.time;
		transform.FindChild("graphics").GetComponent<MeshRenderer>().material.color = Color.red;
		health -= damageAmount;
		print ("health hit");
	}

	/// <summary>
	/// Update function called while the chicken is in the taking damage state.
	/// Acts as a stun and animates the chicken indicating damage was taken
	/// </summary>
	void takeDamageUpdate(){

		if (currentState != ChickenState.TakingDamage) {
			return;
		}

		//amount of time player is unable to move
		float stunTime = .2f;

		if (Time.time - damageStartTime > stunTime) {
			transform.FindChild("graphics").GetComponent<MeshRenderer>().material.color = Color.white;
			currentState = ChickenState.Free;
		}


	}

	/// <summary>
	/// Used to transition to the dead state.
	/// </summary>
	void enterDeadState(){

		currentState = ChickenState.Dead;

		GameState.getInstance ().removeCharacter (this);

		timeOfDeath = Time.time;

		SpecialEffectsFactory.createEffect (transform.position, SpecialEffectType.ChickenDeath);
		
		if (gameObject.GetComponent<PhotonView> () != null) {

			ChickenFactory.createNetworkPlayerChicken(new Vector3(0,1,0),ChickenTeam.None);
			PhotonNetwork.Destroy(gameObject);

		}

	}

	float timeOfDeath;

	/// <summary>
	/// The Update function that is called when the chicken is dead.
	/// </summary>
	void deadUpdate(){

		if (currentState != ChickenState.Dead) {
			return;
		}

		print ("You are dead!!!");
		speed = 0;
		jumpSpeed = 0;
		team = ChickenTeam.Dead;

		transform.FindChild ("graphics").GetComponent<MeshRenderer> ().material.color = Color.green;

		if(Time.time - timeOfDeath > 1f){
			Destroy(gameObject);
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
		currentState = ChickenState.Dashing;

		//Set the direction of the dash for the update function
		dashingDirection = direction;

		//give alittle hop
		movementDirection.y = 3;

	}

	Vector3 dashingDirection = Vector3.zero;
	float dashStartTime = 0f;

    //How  long it takes to dash
    [SerializeField]
    float dashingDuration = .25f;

    /// <summary>
    /// How much time the chicken will spend dashing, before it enters it's recovery period
    /// </summary>
    /// <returns></returns>
    public float getDashingDuration()
    {
        return dashingDuration;
    }

    //How long it takes to recover after dashing
    [SerializeField]
    float recoveryDuration = .15f;

    /// <summary>
    /// Returns the length it takes for the chicken to recover after it dashes
    /// During this recovery period the chicken can not move, but is still considered
    /// to be in it's dash state
    /// </summary>
    /// <returns></returns>
    public float getRecoveryDuration()
    {
        return recoveryDuration;
    }

    /// <summary>
    /// If the chicken is in it's dash state, then it will return the amount of time
    /// in seconds of how much time left the chicken will be in this state.
    /// </summary>
    /// <returns></returns>
    public float timeLeftInDash()
    {
        return (recoveryDuration + dashingDuration) - (Time.time - dashStartTime);
    }

    /// <summary>
    /// The update for dashing every frame incase the chicken is in the dashing state.
    /// </summary>
    void dashUpdate(){

		if (currentState != ChickenState.Dashing) {
			return;
		}

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

	/// <summary>
	/// If the chicken is currentely dashing then it leaves the dashing state
	/// and goes back into a free state open to control.
	/// </summary>
	void stopDashing(){

		if (currentState != ChickenState.Dashing) {
			return;
		}

		dashingDirection = Vector3.zero;
		currentState = ChickenState.Free;
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

		if (currentState == ChickenState.Free && controller.isGrounded) {
			return true;
		}

		return false;

	}


	/// <summary>
	/// Determines whether or not the chicken is allowed to dash in it's current state of affairs..
	/// </summary>
	/// <returns><c>true</c>, if chicken can dash, <c>false</c> otherwise.</returns>
	bool canDash(){
		if (currentState == ChickenState.Free && controller.isGrounded) {
			return true;
		}
		
		return false;
	}

}
