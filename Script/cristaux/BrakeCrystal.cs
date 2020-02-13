using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeCrystal : MonoBehaviour
{
    [SerializeField] private AudioClip SondBrake = null;
    [SerializeField] private GameObject[] crystaux = null;
    [SerializeField] private float Vie = 100f;
    private AudioSource m_AudioSource = null;

    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void ReceiveDamage(float Damage)
    {
        if (Vie <= 0)
        {
            for (int i = 0; i < crystaux.Length; i++)
            {
                crystaux[i].GetComponent<Rigidbody>().isKinematic = false;
                crystaux[i].transform.SetParent(null);
                crystaux[i].SendMessage("detach");
            }
            Destroy(gameObject);
        }
        else
        {
            Vie -= Damage;
            m_AudioSource.clip = SondBrake;
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
        }
    }
}
