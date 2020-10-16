using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

	private Transform[] patrolPoints = null;
	[SerializeField]
	private float patrolWaitTime = 2f;

	[SerializeField]
	private float observationRange = 5f;
	[SerializeField]
	private LayerMask observeMask = 0;
	[SerializeField]
	private float angleOfSight = 30f;
	[SerializeField]
	private Sprite[] statusSprites = null;


	[Space]
	[SerializeField]
	private bool alarmOnSpot = false;
	[SerializeField]
	private bool canHitPlayer = false;
	[SerializeField]
	private float playerStopDistance = .5f;
	[SerializeField]
	private float playerLookAtDistance = 2f;

	private SpriteRenderer statusSprite;
	protected Alarm alarm = null;
	private NavMeshAgent agent;
	private Transform playerTransform = null;

	private int currentPoint = 0;
	private float losePlayerTimer = 0f;

	private bool alarmed = false;
	private bool globalAlarmed = false;
	private bool lookAtPlayer = false;

	[HideInInspector]
	public List<Transform> visibleTargets = new List<Transform>();
	[SerializeField]
	private LayerMask moneyMask = 0;
	[SerializeField]
	private LayerMask obstacleMask = 0;

	Vector3 dir;

	void Start () {
		patrolPoints = new Transform[transform.parent.childCount - 1];
		for (int i = 1; i < transform.parent.childCount; i++) {
			patrolPoints [i - 1] = transform.parent.GetChild (i);
		}
		statusSprite = transform.GetChild (2).GetComponent<SpriteRenderer> ();
		playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
        alarm = Camera.main.GetComponent<Alarm>();
        agent = GetComponent<NavMeshAgent> ();
		StartCoroutine (Patrol ());
	}

	void Update(){
		SpotPlayer ();
		Alarmed ();
		if (!alarmed) {
			agent.autoBraking = true;
			SearchForMoney ();
		} else {
			agent.autoBraking = false;
		}
		if(lookAtPlayer)
			LookAtPlayer ();
	}

	void LookAtPlayer(){
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerTransform.position - transform.position), 10 * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other){
        var player = other.gameObject.GetComponent<Player>();
        if (player != null && canHitPlayer && alarmed) {
			if (!player.FreezeInput)
                player.Catched();

			alarmed = false;
			StopAllCoroutines ();
			StartCoroutine (Patrol ());
			lookAtPlayer = false;
		}
	}

	void SpotPlayer(){
		if (playerIsInSight()) {
			losePlayerTimer = 0;
			alarmed = true;
			if (alarmOnSpot) {
				alarm.SpotPlayer ();
			}
		} else if(alarmed) {
			losePlayerTimer += Time.deltaTime;
			if (losePlayerTimer > 5f) {
				losePlayerTimer = 0;
				alarmed = false;
			}
		}
	}

	void SearchForMoney(){
		visibleTargets.Clear ();
		Collider[] targetsInViewRadius = Physics.OverlapSphere (transform.position, observationRange, moneyMask);

		for (int i = 0; i < targetsInViewRadius.Length; i++) {
			Transform target = targetsInViewRadius [i].transform;
			Vector3 dirToTarget = (target.position - new Vector3(transform.position.x, 1, transform.position.z)).normalized;
			if (Vector3.Angle (transform.forward, dirToTarget) < angleOfSight / 2) {
				float dstToTarget = Vector3.Distance (transform.position, target.position);
				if (!Physics.Raycast (transform.position, dirToTarget, dstToTarget, obstacleMask)) {
					if (target.GetComponent<Money> () != null && target.GetComponent<Money> ().playerCreatedMoney) {
						visibleTargets.Add (target);
						target.GetComponent<Money> ().playerCreatedMoney = false;
					}
				}
			}
		}
		if (visibleTargets.Count > 0) {
			StopAllCoroutines ();
			agent.destination = transform.position;
			StartCoroutine (FindMoney ());
		}
	}

	IEnumerator FindMoney(){
		yield return null;
		statusSprite.sprite = statusSprites [0];
		agent.destination = transform.position;
		yield return new WaitForSeconds (2f);
		if (globalAlarmed) {
			StartCoroutine (CustomPatrol ());
		} else {
			statusSprite.sprite = null;
			StartCoroutine (Patrol ());
		}
	}


	protected bool playerIsInSight(){
		Vector3 playerDir = playerTransform.position - transform.position;
		RaycastHit hit;

		if (Physics.Raycast (transform.position, playerDir, out hit, observationRange, observeMask)) {
			if (hit.distance <= observationRange) { //if the player in observation range
				if(hit.transform.gameObject.CompareTag ("Player")){
					if (Vector3.Angle (transform.forward, playerDir) <= angleOfSight / 2) { //if the player in the angle of sight
						return true;
					}
				}
			}
		}
		return false;
	}

	void Alarmed(){
		if (!globalAlarmed) {
			if (alarm.globalPlayerPos != alarm.resetPos && !alarmed) {
				globalAlarmed = true;
				StopAllCoroutines ();
				StartCoroutine (CustomPatrol ());
				statusSprite.sprite = statusSprites [0];
			}
		} else {
			if (alarm.globalPlayerPos == alarm.resetPos && !alarmed) {
				globalAlarmed = false;
				StopAllCoroutines ();
				StartCoroutine (Patrol ());
			} else if (alarmed) {
				globalAlarmed = false;
				StopAllCoroutines ();
				StartCoroutine (ChasePlayer ());
			}
		}
	}

	IEnumerator CustomPatrol(){
		Vector3 customPatrolPoint = new Vector3 (alarm.globalPlayerPos.x + UnityEngine.Random.Range (-3f, 3f), transform.position.y, alarm.globalPlayerPos.z + UnityEngine.Random.Range (-3f, 3f));
		MoveTowards (customPatrolPoint);
		yield return new WaitForSeconds (1f);
		StartCoroutine (CustomPatrol ());
	}

	IEnumerator ChasePlayer(){
		statusSprite.sprite = statusSprites [1];
		dir = playerTransform.position - transform.position;
		if (dir.magnitude > playerStopDistance) {
			MoveTowards (playerTransform.position);
		} else {
			agent.destination = transform.position;
		}
		if (dir.magnitude > playerLookAtDistance) {
			lookAtPlayer = false;
		} else {
			lookAtPlayer = true;
		}

		yield return new WaitForSeconds (0.2f);
		if(alarmed)
			StartCoroutine (ChasePlayer ());
		else
			StartCoroutine (Patrol ());
	}

	void MoveTowards(Vector3 moveTarget){
		agent.SetDestination(moveTarget);
	}

	IEnumerator Patrol(){
		statusSprite.sprite = null;
		dir = patrolPoints [currentPoint].position - transform.position;
		if (dir.magnitude > 0.4f) {
			MoveTowards (patrolPoints [currentPoint].position);
			yield return new WaitForSeconds (patrolWaitTime);
			if(!alarmed)
				StartCoroutine (Patrol ());
			else
				StartCoroutine (ChasePlayer ());
		} else {
			if (currentPoint + 1 == patrolPoints.Length)
				currentPoint = 0;
			else
				currentPoint++;
			StartCoroutine (LookAtPatrolPoint ());
			yield return null;
		}
	}

	IEnumerator LookAtPatrolPoint(){
		Vector3 dirToLookTarget = ((patrolPoints [currentPoint].position) - transform.position).normalized;
		float targetAngle = 90 - Mathf.Atan2 (dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

		while (Mathf.DeltaAngle (transform.eulerAngles.y, targetAngle) > 0.05f) {
			float angle = Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetAngle, 130 * Time.deltaTime);
			transform.eulerAngles = Vector3.up * angle;
			yield return null;
		}
		yield return new WaitForSeconds (patrolWaitTime);
		if(!alarmed)
			StartCoroutine (Patrol ());
		else
			StartCoroutine (ChasePlayer ());
	}

}
