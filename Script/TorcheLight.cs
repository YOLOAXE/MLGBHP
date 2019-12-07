using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class TorcheLight : MonoBehaviour
{
	private Light torche;
	public float MinIntensity,MaxIntensity,TempsChange;

    void Start()
    {
        torche = GetComponent<Light>();
		StartCoroutine(Intensitymodifier());
    }


   IEnumerator Intensitymodifier()
   {
	   while(true)
	   {
			torche.intensity = Random.Range(MinIntensity,MaxIntensity);
			yield return new WaitForSeconds(TempsChange);
	   }
   }
}
