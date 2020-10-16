using UnityEngine;

public class Door : MonoBehaviour {

	[SerializeField]
	private bool lockedOnStart = false;

	bool locked = false;
    private AudioSource audio = null;

	void Start(){
        audio = GetComponent<AudioSource>();
		if (!lockedOnStart) {
			transform.GetChild (0).GetComponent<BoxCollider> ().enabled = false;
			transform.GetChild (0).GetComponent<SpriteRenderer> ().enabled = false;
			GetComponent<SpriteRenderer> ().enabled = false;
		}
	}

	void OnTriggerEnter(Collider other){
		if(!locked && !lockedOnStart){
			locked = true;
			if(other.CompareTag("Player")){
                audio.PlayOneShot(audio.clip, 0.8f);
				transform.GetChild(0).GetComponent<BoxCollider> ().enabled = true;
				transform.GetChild(0).GetComponent<SpriteRenderer> ().enabled = true;
				GetComponent<SpriteRenderer> ().enabled = true;
			}
		}
	}

	public void Unlock(){
		Destroy (this.gameObject);
	}


}
