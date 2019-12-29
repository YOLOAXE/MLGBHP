using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmeVisualCoerance : MonoBehaviour
{
	public float Munition = 99;
	public GameObject LootRenderer;
	public ArmeLoot_S ScriptVar;
	
    void Start()
    {
		ScriptVar = GetComponent<ArmeLoot_S>();   
		Munition = ScriptVar.Munition;
		LootRenderer.SetActive(Munition > 0); 
    }

}
