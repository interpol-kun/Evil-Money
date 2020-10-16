using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

	[SerializeField]
	private Door[] door = null;

	void OnTriggerEnter(Collider other){
		if(other.CompareTag("Player")){
			for (int i = 0; i < door.Length; i++) {
				door[i].Unlock ();	
			}
			Destroy (this.gameObject);
		}
	}
}
