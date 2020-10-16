using UnityEngine;

public class CameraFollowing : MonoBehaviour {

    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private float distance = 10f;

	void Start () {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
	
	void LateUpdate () {
        transform.position = new Vector3(playerTransform.position.x, distance, playerTransform.position.z);
	}
}
