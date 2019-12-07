using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
	public float TimerDestroy,Vitesse;
	public float DistanceExplosion = 0.1f;
	public Transform SpawnExplosion;
	private Vector3 Target,hitForwardnormal,hitForwardpoint;
	public Rigidbody rb;
	private bool DontExplose = false;
	void Start(){
		rb = GetComponent<Rigidbody>();
		Destroy(gameObject,TimerDestroy);
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit)){
			Target = hit.point;
		}
		Vector3 targetPoint = Target - transform.position;
		float step = 800 * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, targetPoint, step, 0.0f);
		transform.rotation = Quaternion.LookRotation(newDir);
		rb.AddForce(transform.forward * Vitesse * 10);
	}
    void FixedUpdate()
    {
		RaycastHit hitForward;
		if(Physics.Raycast(gameObject.transform.position, gameObject.transform.TransformDirection(Vector3.forward), out hitForward)){
			if(hitForward.distance > DistanceExplosion){
				hitForwardpoint = hitForward.point;
				hitForwardnormal = hitForward.normal;
				DontExplose = false;
			}else{
				if(!DontExplose){
					Explosion();
				}
			}
		}
		if(DistanceExplosion < 10){DistanceExplosion += Time.deltaTime;}
		rb.AddForce(transform.forward * Vitesse);
    }
	void OnTriggerStay(Collider Other){
		if(Other.tag != "player"){
			Explosion();
		}else{
			DontExplose = true;
		}
	}
	void Explosion(){
		Instantiate(SpawnExplosion,hitForwardpoint,Quaternion.FromToRotation(Vector3.forward, hitForwardnormal));
		Destroy(gameObject);
	}
	
}
