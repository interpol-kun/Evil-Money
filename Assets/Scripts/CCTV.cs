using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : MonoBehaviour {

	[SerializeField]
	private float angleOfRotation = 50f;
	[SerializeField]
	private float rotSpeed = 30f;
	[SerializeField]
	private float observationRange = 5f;
	[SerializeField]
	private float angleOfSight = 30f;

	private Transform playerTransform = null;
	private Alarm alarm = null;

	private float startRot;
	private float dir = 1f;
	private bool wait;
	private float cdTimer = 0f;
	private float rotTimer = 0f;

	void Start () {
		startRot = transform.eulerAngles.y;
		playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
		alarm = Camera.main.GetComponent<Alarm>();
	}

	void Update(){
		Rotate ();
		SpotPlayer ();
	}

	void SpotPlayer(){
		Vector3 playerDir = playerTransform.position - transform.position;
		RaycastHit hit;

		if (Physics.Raycast (transform.position, playerDir, out hit)) {
			if (hit.transform.gameObject.CompareTag ("Player") && hit.distance <= observationRange) { //if the player in observation range
				if (Vector3.Angle (transform.forward, playerDir) <= angleOfSight / 2) { //if the player in the angle of sight
					alarm.SpotPlayer();
					Debug.Log ("Alarm Rised!");
				}
			}
		}
	}

	void Rotate(){
		
		if (!wait) {
			if (rotTimer > .3f) {
				if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, startRot)) > angleOfRotation) {
					if (dir == 1) {
						dir = -1;
					} else {
						dir = 1;
					}
					wait = true;
				}
			}
			rotTimer += Time.deltaTime;
			transform.Rotate (new Vector3 (0, dir * rotSpeed * Time.deltaTime, 0));
		} else {
			if (cdTimer < 1f) {
				cdTimer += Time.deltaTime;
			} else {
				wait = false;
				cdTimer = 0;
				rotTimer = 0;
			}
		}
	}
}
