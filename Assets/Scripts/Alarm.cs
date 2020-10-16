using UnityEngine;

public class Alarm : MonoBehaviour {
	
	public Vector3 globalPlayerPos;
	public Vector3 resetPos = new Vector3(100, 100, 100);
	public Transform playerTransform;

	[SerializeField]
	private PlayerInformation _playerInfo = null;
	private GameObject[] tiers = new GameObject[5];
	private GameObject lights;
	private float resetTimer = 0f;

	void Start(){
		lights = GameObject.FindGameObjectWithTag ("Light");
		globalPlayerPos = resetPos;
		playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
		if (GameObject.FindGameObjectWithTag ("Tiers") != null) {
			for (int i = 0; i < GameObject.FindGameObjectWithTag ("Tiers").transform.childCount; i++) {
				tiers [i] = GameObject.FindGameObjectWithTag ("Tiers").transform.GetChild (i).gameObject;
			}

			if (tiers.Length >= 5) {
				if (PlayerInformation.HasFlag (_playerInfo.tier, PlayerInformation.Tier.First)) {
					tiers [1].SetActive (true);
				}
				if (PlayerInformation.HasFlag (_playerInfo.tier, PlayerInformation.Tier.Second)) {
					tiers [2].SetActive (true);
				}
				if (PlayerInformation.HasFlag (_playerInfo.tier, PlayerInformation.Tier.Third)) {
					tiers [3].SetActive (true);
				}
				if (PlayerInformation.HasFlag (_playerInfo.tier, PlayerInformation.Tier.DoorTier)) {
					tiers [4].SetActive (true);
				}
			}
		}
	}

	void Update(){
		if (globalPlayerPos != resetPos && resetTimer < 15f) {
			resetTimer += Time.deltaTime;
		} else if (resetTimer >= 15f){
			globalPlayerPos = resetPos;
			resetTimer = 0f;
			Debug.Log ("Alarm Reseted");
			AlarmLightsOff ();
		}
	}

	public void SpotPlayer(){
		globalPlayerPos = playerTransform.position;
		AlarmLightsOn ();
		ResetAlarm ();
	}

	void ResetAlarm(){
		resetTimer = 0f;
	}

	void AlarmLightsOn(){
		if(lights != null){
			for (int i = 0; i < lights.transform.childCount; i++) {
				lights.transform.GetChild (i).GetComponent<Light> ().color = Color.red;
			}
		}
	}

	void AlarmLightsOff(){
		if(lights != null){
			for (int i = 0; i < lights.transform.childCount; i++) {
				lights.transform.GetChild (i).GetComponent<Light> ().color = Color.white;
			}
		}
	}

}