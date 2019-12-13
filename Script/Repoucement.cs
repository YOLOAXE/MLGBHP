using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repoucement : MonoBehaviour
{
    enum VectorDirection { back, forward, left, right };

    [SerializeField]
    private float force = 0f;
    [SerializeField]
    private VectorDirection type_Direction = VectorDirection.forward;

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Rigidbody>())
        {
            if (type_Direction == VectorDirection.forward)
            {
                other.GetComponent<Rigidbody>().AddForce(Vector3.forward * force);
            }
            if (type_Direction == VectorDirection.back)
            {
                other.GetComponent<Rigidbody>().AddForce(Vector3.back * force);
            }
            if (type_Direction == VectorDirection.left)
            {
                other.GetComponent<Rigidbody>().AddForce(Vector3.left * force);
            }
            if (type_Direction == VectorDirection.right)
            {
                other.GetComponent<Rigidbody>().AddForce(Vector3.right * force);
            }
        }
    }
}
