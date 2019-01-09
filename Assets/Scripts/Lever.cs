using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour {

	public GameObject enemy;
	public GameObject food;
	

	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag == "Player"){
			Vector3 pos = new Vector3( Random.Range(-10f, 10f), Random.Range(-10f, 10f));
			if(Random.Range(0f, 1f) < 0.75f){
				GameObject f = Instantiate(food, pos, Quaternion.identity);
				f.GetComponent<Food>().value = Random.Range(1, 4);
			}else {
				Instantiate(enemy, pos, Quaternion.identity);
			}
		}
	}
}
