using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapinExplosion : MonoBehaviour
{
	public string[] touche;
	public float timeReset,timer;
	public int i;
	public Transform Spawn; 
	
    void Update()
    {
		if(timer <= 0){timer = timeReset;i=0;}
		if(timer > 0){timer -= Time.deltaTime;}
		if(Input.GetKeyDown(touche[i])){i++;timer = timeReset;}
		if(i >= touche.Length)
		{
			Instantiate(Spawn,transform.position,Quaternion.identity);
			i = 0;
		}
    }
}
