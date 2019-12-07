using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileOrb : MonoBehaviour
{
	private GameObject Player;
	public float VitesseProjectile,Degat;
	public Transform Impact;
	private Transform Object;
	
    void Start()
    {
		Player = GameObject.FindWithTag("player");
		Vector3 targetPoint = Player.transform.position - transform.position;
        float step = 1000 * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetPoint, step, 0.0f);
		transform.rotation = Quaternion.LookRotation(newDir);
    }

    void Update()
    {
    transform.Translate(Vector3.forward * Time.deltaTime * VitesseProjectile);
    }
	void OnTriggerStay(Collider other){
		if(other.tag == "player"){
			Player.SendMessage("ReceiveDamage",Degat);
		}
		if(!(other.tag == "Enemie" || other.tag == "EnemiePierre" || other.tag == "EnemieBleu" || other.tag == "EnemieVert" || other.tag == "NoImpact")){
		Object = Instantiate(Impact,transform.position,Quaternion.identity);
		Destroy(gameObject);	
		}

	}
	//public void 
}
