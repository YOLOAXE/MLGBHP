using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmeLoot_S : MonoBehaviour
{
    public int ID = 0;
	public int Munition,Chargeur,MunitionMax;
    public bool canTake = false;
    [SerializeField] private float TempsTake = 0.8f;

    void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        canTake = false;
        yield return new WaitForSeconds(TempsTake);
        canTake = true;
    }
}