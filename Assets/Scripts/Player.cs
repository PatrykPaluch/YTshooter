using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour {

	public float speed = 2;
	[Header("Run & stamina")]
	public float runSpeedModifier = 2;
	public float maxStamina = 2;
	public float staminaRegeneraton = 1;
	public float runDelay = 0.5f;
	private float currRunDelay = 0;
	private float currStamina;

	[Header("HP")]
	public int maxHp = 10;
	public int hp { get; private set; }


	public Slider hpSlider;
	private Rigidbody2D rb;
	private Camera cam;

	[Header("Shooting")]
	public int shootDmg = 1;
	public float shootDistance = 5;
	public LayerMask shootMask;
	public LineRenderer shootLine;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		cam = Camera.main;
		hpSlider.maxValue = maxHp;
		hp = maxHp;

		hpSlider.value = hp;
		currStamina = maxStamina;
	}
	
	public void Hit(int dmg){
		if(dmg<=0) return;
		hp -= dmg;
		hpSlider.value = hp;

		if(hp<0) Kill();
	}

	public void Heal(int val){
		if(val<=0) return;
		hp+=val;
		hpSlider.value = hp;

		if(hp>maxHp) hp = maxHp;
	}

	void Kill(){
		
		Debug.Log("Game over");
		//Go to menu
	}

	void Update(){
		Vector3 mouseInWorld = cam.ScreenToWorldPoint( Input.mousePosition );
		Vector3 diff = mouseInWorld - transform.position;
		float angle = Mathf.Atan2(diff.x, diff.y) * Mathf.Rad2Deg;
		angle -= 90;
		transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);

		if(Input.GetButtonDown("Fire1")){
			Shoot();
		}
	}

	void Shoot(){
		
		//pos, dir, [dis], [mas]
		RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, shootDistance, shootMask);
		if(hit){
			GameObject enemy = hit.transform.gameObject;
			if(enemy.tag == "Enemy"){
				enemy.GetComponent<Enemy>().Hit(shootDmg);
			}

			shootLine.SetPosition(1, enemy.transform.position);
		}else {
			shootLine.SetPosition(1, transform.position + transform.right * shootDistance);
		}

		shootLine.SetPosition(0, transform.position);

		shootLine.gameObject.SetActive(true);
		StartCoroutine( HideShootLine() );
	}

	IEnumerator HideShootLine(){

		yield return new WaitForSeconds(0.25f);
		
		shootLine.gameObject.SetActive(false);
	}

	void FixedUpdate () {
		//GetAxis -> -1, 0, 1
		Vector2 vel = new Vector2( Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical") );

		//wektor ma długośc 1
		vel.Normalize();



		//vel = vel * speed;
		vel *= speed;

		if(currRunDelay<=0 && Input.GetAxisRaw("Run")!=0){
			vel *= runSpeedModifier;
			currStamina -= Time.deltaTime;
			if(currStamina<=0){
				currRunDelay = runDelay;
			}
		}else {
			currRunDelay -= Time.deltaTime;
			currStamina += staminaRegeneraton * Time.deltaTime;
			if(currStamina>maxStamina){
				currStamina = maxStamina;	
			}
		}
		

		rb.velocity  = vel;

	}


	void OnDrawGizmosSelected(){
		Gizmos.DrawWireSphere(transform.position, shootDistance);
	}
}