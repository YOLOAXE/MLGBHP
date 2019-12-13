using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class marche : MonoBehaviour
{
    public bool Marche;
	public float vitesse,positionY,coeffHeadUp;
	public float varLerp;
	private float vvLerp;
	public AudioSource m_AudioSource;
	public AudioClip[] bruitDePas;
	private Vector3 posObject;
	private bool lerpb;
	
	void Start()
	{
		positionY = gameObject.transform.position.y;
		//StartCoroutine(pas());
	}

    void LateUpdate()
    {
        if(Marche)
		{
			Vector3 posObject = gameObject.transform.position;
			varLerp = Mathf.SmoothStep(Mathf.PingPong(Time.time * vitesse, coeffHeadUp),varLerp,Time.deltaTime * vitesse);
			if(varLerp > vvLerp)
			{
				lerpb = true;
			}
			else
			{
				if(lerpb)
				{
					int rn = Random.Range(0,bruitDePas.Length);
					m_AudioSource.clip = bruitDePas[rn];
					m_AudioSource.PlayOneShot(m_AudioSource.clip);
					lerpb = false;
				}
			}
			vvLerp = varLerp;
			posObject = new Vector3(posObject.x,varLerp + positionY,posObject.z);
			gameObject.transform.position = posObject;
		}
    }

	IEnumerator pas()
	{
		while(true)
		{
			if(Marche)
			{
				int rn = Random.Range(0,bruitDePas.Length);
				m_AudioSource.clip = bruitDePas[rn];
				m_AudioSource.PlayOneShot(m_AudioSource.clip);
			}
			yield return new WaitForSeconds(vitesse);
		}
	}
}
