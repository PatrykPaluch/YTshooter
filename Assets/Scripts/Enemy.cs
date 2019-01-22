using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour {

	public float speed = 1;	
	
	public float viewDistance = 5;

	public float attackRange = 1;
	public int dmg = 1;
	public float attackSpeed = 0.5f;
	
	public int hp = 4;

	[Header("Random move")]
	public float moveMaxDistance = 5;
	public float moveDelay = 2;
	private float currMoveDelay;
	private Vector2 targetPosition;

	private float currAttackTime = 0;
	private Rigidbody2D rb;
	private Player player;

	public void Hit(int dmg){
		if(dmg<=0) return;
		
		hp-= dmg;
		if(hp<0){
			Destroy(this.gameObject);
		}
	}

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		player = GameObject.FindObjectOfType<Player>();
		targetPosition = transform.position;
		currMoveDelay = Random.Range(-5, moveDelay);
	}
	
	void FixedUpdate () {
		float distance = Vector2.Distance(toVec2(transform), toVec2(player.transform));
		if(distance < viewDistance){

			MoveToPosition(player.transform.position);
			currMoveDelay = 0;

		}else if(currMoveDelay > moveDelay ) {
			
			
			MoveToPosition(targetPosition);

			if( Vector2.Distance(toVec2(transform), targetPosition) < 0.1 ){
				currMoveDelay = 0;
				targetPosition = Utils.RandomVec3(-moveMaxDistance, moveMaxDistance);
				targetPosition += (Vector2)transform.position;
			}
			
		}else {
			currMoveDelay += Time.deltaTime;
		}

		currAttackTime += Time.deltaTime;
		if(distance < attackRange && currAttackTime >= attackSpeed){
			player.Hit(dmg);
			currAttackTime = 0;
		}
	}

	void MoveToPosition(Vector2 target){
		LookAt(target);
		rb.velocity = transform.right * speed;
	}

	void LookAt(Vector3 target){
		Vector3 diff = target - transform.position;
		float angle = Mathf.Atan2(diff.x, diff.y) * Mathf.Rad2Deg;
		angle -= 90;
		transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
	}

	Vector2 toVec2(Transform t){
		return new Vector2(t.position.x, t.position.y);
	}

	void OnDrawGizmosSelected(){
		Gizmos.color = new Color(0.2f, 0.2f, 1f);
		Gizmos.DrawWireSphere(transform.position, viewDistance);
		Gizmos.color = new Color(1f, 0,0, 0.25f);
		Gizmos.DrawSphere(transform.position, attackRange);
		// if(currMoveDelay>=moveDelay)
		// 	Gizmos.color = Color.green;
		// else 
		// 	Gizmos.color = Color.red;
		// Gizmos.DrawRay(transform.position, targetPosition);
	}
}
