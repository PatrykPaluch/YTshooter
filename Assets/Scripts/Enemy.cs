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
	}
	
	void FixedUpdate () {
		float distance = Vector2.Distance(toVec2(transform), toVec2(player.transform));
		if(distance < viewDistance){
			Vector3 diff = player.transform.position - transform.position;
			float angle = Mathf.Atan2(diff.x, diff.y) * Mathf.Rad2Deg;
			angle -= 90;
			transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);

			rb.velocity = transform.right * speed;
		}

		currAttackTime += Time.deltaTime;
		if(distance < attackRange && currAttackTime >= attackSpeed){
			player.Hit(dmg);
			currAttackTime = 0;
		}
	}

	Vector2 toVec2(Transform t){
		return new Vector2(t.position.x, t.position.y);
	}

	void OnDrawGizmosSelected(){
		Gizmos.color = new Color(0.2f, 0.2f, 1f);
		Gizmos.DrawWireSphere(transform.position, viewDistance);
		Gizmos.color = new Color(1f, 0,0, 0.25f);
		Gizmos.DrawSphere(transform.position, attackRange);
	}
}
