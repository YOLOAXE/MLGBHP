using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootCristaux : MonoBehaviour
{
	public GameObject RootDegat;
	public bool isDetach = false;

	public void ReceiveDamagePlayer(float Damage){if(!isDetach){RootDegat.SendMessage("ReceiveDamage",Damage);}}
	public void detach(){isDetach = true;gameObject.tag = "ObjectLoot";}
}
