using UnityEngine;
using System.Collections;

/// <summary>
/// Special effects factory meant to encapsulate creation logic
/// </summary>
public class SpecialEffectsFactory {

	/// <summary>
	/// Creates a special effect based on the type at a specified position
	/// </summary>
	/// <returns>The effect created in the scene</returns>
	/// <param name="position">Position of where the effect is to be created.</param>
	public static GameObject createEffect(Vector3 position, SpecialEffectType effectType){

		GameObject reference = null;

		float lifeDuration = 3f;

		//get the correct special effect from our resources
		switch(effectType){

		case SpecialEffectType.ChickenDeath:
			lifeDuration = 3f;
			reference = Resources.Load("Effects/ChickenDeath") as GameObject;
			break;

		case SpecialEffectType.TakeDamage:
			lifeDuration = 1f;
			reference = Resources.Load("Effects/TakeDamage") as GameObject;
			break;

		}

		//create the object in the scene
		GameObject effectInstance = Object.Instantiate(reference, position,Quaternion.identity) as GameObject;

		//Set the object to be destroyed in a certain amount of  time
		Object.Destroy (effectInstance, lifeDuration);

		return effectInstance;

	}

}
