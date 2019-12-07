using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeudeCampDegat : MonoBehaviour
{
	
	public float degats = 1,TimerInterval;
	public bool degatFeu;
	private float TimerVar;

	void OnTriggerEnter(Collider other)
	{
		EnterCollider(other);
	}
	void OnTriggerStay(Collider other)
	{
		if(TimerVar < 0)
		{	
			EnterCollider(other);
		}
		if(TimerVar > 0){TimerVar -= Time.deltaTime;}
	}
	private void EnterCollider(Collider other)
	{
		if(other.tag == "player")
		{
			if(degatFeu)
			{
				other.SendMessage("ReceiveDamageFire",degats);
			}
			else
			{
				other.SendMessage("ReceiveDamage",degats);
			}
			TimerVar = TimerInterval;
		}
	}
}
