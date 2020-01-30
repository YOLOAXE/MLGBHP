using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionShake : MonoBehaviour
{
	public IEnumerator Shake (float duration, float magnitude)
	{	
		Vector3 originalPos = new Vector3(0.0f,0.75f,0.0f);
		
		float elapsed = 0.0f;
		
		while(elapsed < duration)
		{
			float x = Random.Range(-1f,1f) * magnitude;
			float y = Random.Range(-1f,1f) * magnitude;
			
			transform.localPosition = new Vector3(originalPos.x + x,originalPos.y + y,originalPos.z);
			
			elapsed += Time.deltaTime;
			
			yield return null;
		}
		
		transform.localPosition = originalPos;
	}

    public IEnumerator ShakeTire(float magnitude)
    {
        Vector3 originalRot = transform.localEulerAngles;
        float x = Random.Range(-1.5f, 1.5f) * magnitude;
        float y = Random.Range(-0.5f, 0.5f) * magnitude;

        transform.localEulerAngles = new Vector3(originalRot.x + x, originalRot.y + y, originalRot.z);
        yield return null;
        transform.localEulerAngles = originalRot;

    }
}
