using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootCristaux : MonoBehaviour
{
    [SerializeField] private GameObject RootDegat = null;
    public bool isDetach = false;

    public void ReceiveDamagePlayer(float Damage)
    {
        if (!isDetach)
        {
            RootDegat.SendMessage("ReceiveDamage", Damage);
        }
    }

    public void detach()
    {
        isDetach = true;
        gameObject.tag = "ObjectLoot";
    }
}
