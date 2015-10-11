using UnityEngine;
using System.Collections;

public class HitboxBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision){
		SendMessageUpwards ("hitBoxCollisionEnter",collision);
	}
}
