using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSalle : MonoBehaviour
{
	public bool Haut,Bas,Gauche,Droite;
	public bool BlockeHaut,BlockeBas,BlockeGauche,BlockeDroite;
	public float DistanceBlock = 6.5f;
	private bool DontDestroy = false;
	private SpawnSalle SS;
	void Start(){
		StartCoroutine(DD());
	}
	void OnTriggerStay (Collider other){
		if(other.tag == "SpawnPoint" && DontDestroy){
			SS = other.gameObject.GetComponent<SpawnSalle>();
			if(SS.Haut){Haut = true;}
			if(SS.Bas){Bas = true;}
			if(SS.Gauche){Gauche = true;}
			if(SS.Droite){Droite = true;}
			Destroy(other.gameObject);
		}
		if(other.tag == "RoomPoint"){
			Destroy(gameObject);
		}
	}
	IEnumerator DD(){
		yield return new WaitForSeconds(0.095f);
		DontDestroy = true;
	}
	void Update(){
		RaycastHit hitLeft;
		RaycastHit hitRight;
		RaycastHit hitForward;
		RaycastHit hitBackward;
		if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hitBackward)){BlockeBas = hitBackward.distance < DistanceBlock;}
		if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hitLeft)){BlockeGauche = hitLeft.distance < DistanceBlock;}
		if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hitRight)){BlockeDroite = hitRight.distance < DistanceBlock;}
		if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitForward)){BlockeHaut = hitForward.distance < DistanceBlock;}
	}
}
