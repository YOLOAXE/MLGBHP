using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField] private float Timer = 0f;
    void Start(){Destroy(gameObject, Timer);}

}
