using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	public bool invertY = false;
	
	public GameObject ChracterePlyer,BrasMainTete;
	
	float rotationY = 0F;
    float rotationX = 0F;
    float ySens = 0F;

    void Update ()
	{ 
		ySens = sensitivityY;
		if(invertY) { ySens *= -1f; }

		if (axes == RotationAxes.MouseXAndY)
		{
			rotationX = transform.localEulerAngles.y + GetMouseX() * sensitivityX;
			
			rotationY += GetMouseY() * ySens;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, GetMouseX() * sensitivityX, 0);
		}
		else
		{
			rotationY += GetMouseY() * ySens;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}
		ChracterePlyer.transform.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y, 0);
		BrasMainTete.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,transform.rotation.eulerAngles.y, 0);
	}
	
	void Start ()
	{
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
	}

    float GetMouseX()
    {
        return Input.GetAxis("Mouse X");
    }

    float GetMouseY()
    {
        return Input.GetAxis("Mouse Y");
    }

    public IEnumerator Shake(float magnitude,float Divide)
    {
        float x = Random.Range(-0.2f, 0.2f) * magnitude;
        float y = Random.Range(0.5f, 1f) * magnitude;
        float Temps = 0f;

        rotationX += x;
        rotationY += y;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
        //transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);

        while (Temps < 1/Divide)
        {
            Temps += Time.deltaTime;
            rotationX = transform.localEulerAngles.y + GetMouseX() * (sensitivityX/2) - (x * Time.deltaTime * Divide);
            rotationY += GetMouseY() * (sensitivityY/2) - (y * Time.deltaTime * Divide);
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
            //transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
            yield return null;
        }
    }
}