using UnityEngine;
using System.Collections;

/// <summary>
/// Factory meant to ease the insertion of Chickens into your scene by hiding creation logic
/// Static so it can be called anywhere at any time.
/// </summary>
public class ChickenFactory {

	/// <summary>
	/// Creates a chicken in the scene.
	/// </summary>
	/// <returns>The chicken.</returns>
	/// <param name="position">Location where you want the chicken to spawn.</param>
	/// <param name="team">Team.</param>
	/// <param name="skillLevel">How skilled the chicken is on a scale from 0 to 1</param>
	public static GameObject createChicken(Vector3 position, ChickenTeam team, float skillLevel){

		skillLevel = Mathf.Clamp (skillLevel, 0f, 1f);

		GameObject chickenGameObject = Resources.Load("Chicken/Standard Chicken") as GameObject;
		chickenGameObject = (GameObject)Object.Instantiate (chickenGameObject, position, Quaternion.identity);
		chickenGameObject.GetComponent<ChickenControlBehavior> ().setChickensTeam (team);

		GameState.getInstance ().addCharacter (chickenGameObject.GetComponent<ChickenControlBehavior> ());

		return chickenGameObject;

	}

	/// <summary>
	/// Creates the player chicken.
	/// </summary>
	/// <returns>The player chicken.</returns>
	/// <param name="position">Location where you want the chicken to spawn.</param>
	/// <param name="team">The team the player will be fighting for.</param>
	public static GameObject createPlayerChicken(Vector3 position, ChickenTeam team){

		GameObject chickenGameObject = Resources.Load("Chicken/Player") as GameObject;
		chickenGameObject = (GameObject)Object.Instantiate (chickenGameObject, position, Quaternion.identity);
		chickenGameObject.GetComponent<ChickenControlBehavior> ().setChickensTeam (team);

		GameState.getInstance ().addCharacter (chickenGameObject.GetComponent<ChickenControlBehavior> ());

		return chickenGameObject;
	}

}
