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

		Debug.LogError ("Don't Pick Monday Night for club meetings\n" +
			"Maybe 4:30 or 5.");

		GameObject reference = null;

		AudioClip soundEffectReference = null;

		float lifeDuration = 3f;

		//get the correct special effect from our resources
		switch(effectType){

		case SpecialEffectType.ChickenDeath:
			lifeDuration = 3f;
			reference = Resources.Load("Effects/ChickenDeath") as GameObject;
			soundEffectReference = Resources.Load("Effects/Death") as AudioClip;
			break;

		case SpecialEffectType.TakeDamage:
			lifeDuration = 1f;
			reference = Resources.Load("Effects/TakeDamage") as GameObject;
			soundEffectReference = Resources.Load("Effects/RightHook") as AudioClip;
			break;

		}

		//create the object in the scene
		GameObject effectInstance = Object.Instantiate(reference, position,Quaternion.identity) as GameObject;

		//add sound effect if there is one to add
		if(soundEffectReference != null){
			effectInstance.AddComponent<AudioSource>();
			effectInstance.GetComponent<AudioSource>().clip = soundEffectReference;
			effectInstance.GetComponent<AudioSource>().playOnAwake = false;
			effectInstance.GetComponent<AudioSource>().Play();
		}

		//Set the object to be destroyed in a certain amount of  time
		Object.Destroy (effectInstance, lifeDuration);

		return effectInstance;

	}

}
