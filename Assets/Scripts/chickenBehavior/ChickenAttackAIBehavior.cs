using UnityEngine;
using System.Collections;

[RequireComponent (typeof (ChickenControlBehavior))]
/// <summary>
/// A more reactionary based agent meant for fighting whatever
/// </summary>
public class ChickenAttackAIBehavior : MonoBehaviour {


	/// <summary>
	/// Definition of a percept for this agent,
	/// A percept is a structure meant to represent the state of the environment at a given time.
	/// Given a percept sequence (History of percepts) we can more accurately make desicions.
	/// </summary>
	private struct Percept {

		public int curHealth;
        public int numOfTeamates;
        public float[] teamatesHealths;
        public float[] teamatesDistancesFromUs;
        public float[] teamatesDistancesFromTarget;

        public int targetHealth;
        public int numOfTargetAllies;
        public float[] targetsAlliesHealths;
        public float[] targetsAlliesDistancesFromTarget;
        public float[] targetsAlliesDistancesFromUs;
        public ChickenState targetState;

	}

	//The target we want to kill
	[SerializeField]
	ChickenControlBehavior target = null;

	//reference to our own control
	ChickenControlBehavior control = null;

	// Use this for initialization
	void Start () {
		control = gameObject.GetComponent<ChickenControlBehavior> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (target == null) {
			findTarget();
		} else {
			AIUpdate();
		}

	}

	/// <summary>
	/// Changes whatever the chickens target is.
	/// </summary>
	/// <param name="chickenControl">Chicken control.</param>
	public void setTarget(ChickenControlBehavior target){
		if (target == null) {
			return;
		}

		this.target = target;
	}

	/// <summary>
	/// The update called to run the agents thinking and allows it to make it's moves.
	/// </summary>
	void AIUpdate(){

		Percept currentPercept = getPerceptAtThisInstance ();

		float attackingAppeal = appealOfAttacking (currentPercept);

		float evadingAppeal = appealOfEvading (currentPercept);

		float invadingAppeal = appealOfInvading (currentPercept);

		if (invadingAppeal >= evadingAppeal && invadingAppeal >= attackingAppeal) {
			invadeUpdate();
		} else  if (attackingAppeal >= evadingAppeal && attackingAppeal >= invadingAppeal) {
			attackUpdate();
		} else if (evadingAppeal >= attackingAppeal && evadingAppeal >= invadingAppeal) {
			evadeUpdate();
		} 

	}

	/// <summary>
	/// Update function for attacking the target
	/// If the AI has made the desicion that it wants to attack, 
	/// it will start calling this function
	/// </summary>
	void attackUpdate(){
		control.attackForward();
	}

	/// <summary>
	/// Update function for evading the target
	/// If the AI has made the desicion that it wants to run away, 
	/// it will start calling this function
	/// </summary>
	void evadeUpdate(){
		control.moveBackward ();
	}

	/// <summary>
	/// Update function for getting closer to the target
	/// If the AI has made the desicion that it wants to invade, 
	/// it will start calling this function
	/// </summary>
	void invadeUpdate(){
		control.moveForward ();
	}

	/// <summary>
	/// What the apeal is to the agent for executing an attack on the target.
	/// The higher the rating the higher the appeal value, and the more likely 
	/// the agent is to attack.
	/// </summary>
	/// <returns>The appeal of attacking the target</returns>
	/// <param name="percept">Percept, environment we're evaluating</param>
	float appealOfAttacking(Percept percept){
        // If the enemy is not dashing and we are at med-high health, we want to attack
        if (target.getCurrentChickenState() != ChickenState.Dashing)
        {
            // If enemy is within attack range (roughly 3.4 units)
            if (Vector3.Distance(target.transform.position, control.transform.position) <= 3.5)
            {    
                // If we have moderate HP
                if (percept.curHealth / control.getMaxHealth() >= .45F)
                {
                    return 1;
                }

                // If we have subpar HP
                if (percept.curHealth / control.getMaxHealth() < .45F)
                {
                    // If we have teammates nearby
                    if (percept.numOfTeamates > 1)
                    {
                        return 1;
                    }
                    return 0.5F; // Attack if we must
                }
            }
        }

        // If enemy is dashing
        if (target.getCurrentChickenState() == ChickenState.Dashing)
        {
            if (percept.curHealth / control.getMaxHealth() >= .45F)
            {
                return 0.5F; // If we have moderate HP, might as well attack
            }
            return 0.3F; // Only attack if we really need to
        }
		return 0;
	}

	/// <summary>
	/// What the apeal is to the agent for getting away from it's target
	/// The higher the rating the higher the appeal value, and the more likely 
	/// the agent is for running away.
	/// </summary>
	/// <returns>The appeal of invading the target</returns>
	/// <param name="percept">Percept, environment we're evaluating</param>
	float appealOfEvading(Percept percept){
        // If the player is dashing and enemy is low health, enemy wants to evade
        if (target.getCurrentChickenState() == ChickenState.Dashing){
            return 1 - (percept.curHealth / control.getMaxHealth());
        }
		return 0;
	}

	/// <summary>
	/// What the apeal is to the agent for getting closer to it's target.
	/// The higher the rating the higher the appeal value, and the more likely 
	/// the agent is for invading.
	/// </summary>
	/// <returns>The appeal of invading the target</returns>
	/// <param name="percept">Percept, environment we're evaluating</param>
	float appealOfInvading(Percept percept){
		return 0;
	}

	/// <summary>
	/// Generates a percept for the given instance of time that is meant to represent
	/// all data relevant to the agent for making a desicion.
	/// </summary>
	/// <returns>The percept at this instance.</returns>
	Percept getPerceptAtThisInstance(){

		//The distance we are away from our target.
		//float distanceFromTarget = Vector3.Distance (this.transform.position, target.transform.position);
		
		//The state our target is in.
		//ChickenState targetCurrentState = target.getCurrentChickenState ();

		return new Percept();
	}


	/// <summary>
	/// Attempt's to find a target for the AI to attack
	/// </summary>
	void findTarget(){

	}

}
