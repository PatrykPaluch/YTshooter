using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {

	public int value = 1;
	
	void OnTriggerEnter2D(Collider2D col){
		if(col.tag == "Player"){
			col.GetComponent<Player>().Heal(value);
			Destroy(this.gameObject);
		}
	}
}
