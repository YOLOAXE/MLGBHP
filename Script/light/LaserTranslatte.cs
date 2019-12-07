using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTranslatte : MonoBehaviour
{
	public float TimerDestroy,Vitesse;
	private Vector3 Target;
	
	void Start(){
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
	}
    void Update()
    {
		transform.Translate(Vector3.forward * Time.deltaTime * Vitesse);
    }
}
