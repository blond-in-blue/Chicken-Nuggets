using UnityEngine;
using System.Collections;

/// <summary>
/// Tests functionality in the Prototype scene
/// </summary>
public class PrototypeGameInit : MonoBehaviour {

	// Use this for initialization
	void Start () {

		//spawn players
		GameObject enemy = ChickenFactory.createChicken (new Vector3(-5,2,0), ChickenTeam.Red, 0f);
		GameObject player = ChickenFactory.createPlayerChicken( new Vector3(5,2,0), ChickenTeam.Blue);

		ChickenControlBehavior playerChickenBehavior = player.GetComponent<ChickenControlBehavior> ();
		playerChickenBehavior.setTarget (enemy);

		enemy.GetComponent<ChickenControlBehavior> ().setTarget (player);
		enemy.GetComponent<ChickenAttackAIBehavior> ().setTarget (playerChickenBehavior);

	}
	

}
