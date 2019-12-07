using UnityEngine;

[System.Serializable]
public class SoundFootstep
{
	public string Name;
	public AudioClip[] m_FootstepSounds;
}

public class StrafeMovement : MonoBehaviour
{
    [SerializeField] private float accel = 200f;         
    [SerializeField] private float airAccel = 200f;    
    [SerializeField] private float maxSpeed = 10f;     
	[SerializeField] private float maxSpeedSprint = 15f; 
	private float maxSpeedSprintVar;
	private float maxSpeedVar;
    [SerializeField] private float maxAirSpeed = 1f;   
    [SerializeField] private float friction = 8f;       
    [SerializeField] private float jumpForce = 5.5f;       
    [SerializeField] private LayerMask groundLayers;
	[SerializeField] private SoundFootstep[] sfs_FootstepSounds;
    [SerializeField] private AudioClip m_JumpSound;           
    [SerializeField] private AudioClip m_LandSound;   
	[SerializeField] private bool m_IsWalking,m_IsCrouch;
	[SerializeField] private float m_StepInterval;
	[Range(0f, 1f)] [SerializeField] private float m_RunstepLenghten;
    [SerializeField] private GameObject camObj;

    private float lastJumpPress = -1f;
    private float jumpPressDuration = 0.1f;
	private bool onGround = false;
    private float SpeedGround = 10f; 
	private float m_StepCycle;
    private float m_NextStep;
	private float VelociteMagnitudePlayer;
	private float HelpJumpSound;
	private int Id_FootstepSounds;
	private bool OtherFootstepGround = false;
	[SerializeField] private float DistanceOnground;
	[SerializeField] private float TempsDeChute;
	[SerializeField] private bool Escalier = false;
	[SerializeField] private Animator m_AnimatorCharacter;
	[SerializeField] private GameObject CharacterePlayer;
	private bool animeSaut;
	private bool Crouchanim;
	private float TransitionAcroupie;
	[SerializeField] private bool AsRifle,AsMelee,AsHandGun;
	[SerializeField] private float fps = 0;
	[SerializeField] private bool OnDialog;
	private AudioSource m_AudioSource;
	
	private void Start()
	{
		m_AudioSource = GetComponent<AudioSource>();	
		maxSpeedVar = maxSpeed;
		maxSpeedSprintVar = maxSpeedSprint;
	}

	
	private void Update()
	{
		if(!OnDialog)
		{
			if(Input.GetButton("Acroupie"))
			{
				m_IsCrouch = true;
				transform.GetComponent<CapsuleCollider>().height = Mathf.Lerp(transform.GetComponent<CapsuleCollider>().height,0.5f,Time.deltaTime * 10);
				camObj.transform.localPosition = new Vector3(0f,Mathf.Lerp(camObj.transform.localPosition.y,0.3f,Time.deltaTime * 10),0f);
			}
			else
			{
				if(m_IsCrouch)
				{
					Ray rayCrouch = new Ray(transform.position, Vector3.up);
					RaycastHit hitCouch;
					if(Physics.Raycast(rayCrouch,out hitCouch))
					{
						m_IsCrouch = hitCouch.distance < 1.5f;
					}
				}
				else
				{
					transform.GetComponent<CapsuleCollider>().height = Mathf.Lerp(transform.GetComponent<CapsuleCollider>().height,2f,Time.deltaTime * 20);
					camObj.transform.localPosition = new Vector3(0f,Mathf.Lerp(camObj.transform.localPosition.y,0.75f,Time.deltaTime * 10),0f);
				}
			}
			if(m_IsCrouch || Input.GetButton("Fire2"))
			{
				maxSpeed = maxSpeedVar/2;
				maxSpeedSprint = maxSpeedSprintVar/2;
				m_AnimatorCharacter.SetFloat("FB",Input.GetAxis("Vertical") * (5 + (Input.GetAxis("Sprint") * 3)),0.5f,Time.deltaTime * 5);
				m_AnimatorCharacter.SetFloat("LR",Input.GetAxis("Horizontal") * (5 + (Input.GetAxis("Sprint") * 3)),0.5f,Time.deltaTime * 5);
			}
			else
			{
				maxSpeed = maxSpeedVar;
				maxSpeedSprint = maxSpeedSprintVar;	
				m_AnimatorCharacter.SetFloat("FB",Input.GetAxis("Vertical") * (7 + (Input.GetAxis("Sprint") * 3)),0.5f,Time.deltaTime * 5);
				m_AnimatorCharacter.SetFloat("LR",Input.GetAxis("Horizontal") * (7 + (Input.GetAxis("Sprint") * 3)),0.5f,Time.deltaTime * 5);
			}
			
			m_AnimatorCharacter.SetBool("OnJump",!onGround);
			m_AnimatorCharacter.SetBool("Crouch",m_IsCrouch);
			m_AnimatorCharacter.SetBool("Weapon",AsRifle || AsHandGun);
			m_AnimatorCharacter.SetBool("Melee",AsMelee);
			
			if(m_IsCrouch)
			{
				if(TransitionAcroupie <= 0.49f){TransitionAcroupie += Time.deltaTime * 3f;}else{TransitionAcroupie = 0.5f;}
				if(Crouchanim){if(onGround){m_AnimatorCharacter.Play("MovCrouch", 0, 0.25f);}}
				Crouchanim = false;
			}
			else
			{
				if(!Crouchanim)
				{
					if(AsRifle){m_AnimatorCharacter.Play("MowRifle", 0, 0.25f);}
					else if(AsMelee){m_AnimatorCharacter.Play("MoveMelee", 0, 0.25f);}
					else{m_AnimatorCharacter.Play("Mouvement", 0, 0.25f);}
				}
				Crouchanim = true;	
				if(TransitionAcroupie >= 0.05f){TransitionAcroupie -= Time.deltaTime * 3f;}else{TransitionAcroupie = 0f;}
			}	
			CharacterePlayer.transform.position = new Vector3(0,TransitionAcroupie,0) + transform.position;	
			
			if(!onGround && animeSaut != onGround && !m_IsCrouch){
				if(AsRifle)
				{
					m_AnimatorCharacter.Play("JumpUpRifle", 0, 0.25f);
				}
				else
				{
					m_AnimatorCharacter.Play("Jumping Up", 0, 0.25f);
				}
			}
			if(onGround && animeSaut != onGround && !m_IsCrouch){
				if(AsRifle)
				{
					m_AnimatorCharacter.Play("JumpDownRifle", 0, 0.25f);	
				}
				else
				{
					m_AnimatorCharacter.Play("Jumping Down", 0, 0.25f);	
				}
			}
			animeSaut = onGround;
			if (Input.GetButton("Jump"))
			{
				lastJumpPress = Time.time;
			}
		
			if (Input.GetButton("Sprint")){
				SpeedGround = maxSpeedSprint;
			}
			else
			{
				SpeedGround = maxSpeed;
			}
			ProgressStepCycle(SpeedGround);
		
			if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0){m_IsWalking = true;}else{m_IsWalking = false;}
		
			if(HelpJumpSound > 0){HelpJumpSound -= Time.deltaTime;}
		
			if(!onGround)
			{
				TempsDeChute += Time.deltaTime;
			}
			else
			{
				if(TempsDeChute > 1f)
				{
					m_AudioSource.clip = m_LandSound;
					m_AudioSource.PlayOneShot(m_AudioSource.clip);	
				}
				TempsDeChute = 0;
			}
		}
		else
		{
			m_AnimatorCharacter.SetFloat("FB",0,0.5f,Time.deltaTime * 5);
			m_AnimatorCharacter.SetFloat("LR",0,0.5f,Time.deltaTime * 5);
			m_AnimatorCharacter.SetBool("OnJump",false);
		}	
	
	}

	private void FixedUpdate()
	{
	if(!OnDialog){
		Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 playerVelocity = GetComponent<Rigidbody>().velocity;

        playerVelocity = CalculateFriction(playerVelocity);

        playerVelocity += CalculateMovement(input, playerVelocity);

		GetComponent<Rigidbody>().velocity = playerVelocity;
	}else{
		Vector2 input2 = new Vector2(0, 0);

        Vector3 playerVelocity = GetComponent<Rigidbody>().velocity;

        playerVelocity = CalculateFriction(playerVelocity);

        playerVelocity += CalculateMovement(input2, playerVelocity);

		GetComponent<Rigidbody>().velocity = playerVelocity;
	}
	}

	private Vector3 CalculateFriction(Vector3 currentVelocity)
	{
        onGround = CheckGround();
		float speed = currentVelocity.magnitude;

        if (!onGround || Input.GetButton("Jump") || speed == 0f)
            return currentVelocity;

        float drop = speed * friction * Time.deltaTime;
        return currentVelocity * (Mathf.Max(speed - drop, 0f) / speed);
    }
    
	private Vector3 CalculateMovement(Vector2 input, Vector3 velocity)
	{
        onGround = CheckGround();

        float curAccel = accel;
        if (!onGround)
            curAccel = airAccel;

        float curMaxSpeed = SpeedGround;

        if (!onGround)
            curMaxSpeed = maxAirSpeed;
        
        Vector3 camRotation = new Vector3(0f, camObj.transform.rotation.eulerAngles.y, 0f);
        Vector3 inputVelocity = Quaternion.Euler(camRotation) *
                                new Vector3(input.x * curAccel, 0f, input.y * curAccel);

        Vector3 alignedInputVelocity = new Vector3(inputVelocity.x, 0f, inputVelocity.z) * Time.deltaTime;

        Vector3 currentVelocity = new Vector3(velocity.x, 0f, velocity.z);

        float max = Mathf.Max(0f, 1 - (currentVelocity.magnitude / curMaxSpeed));

        float velocityDot = Vector3.Dot(currentVelocity, alignedInputVelocity);

        Vector3 modifiedVelocity = alignedInputVelocity * max;

        Vector3 correctVelocity = Vector3.Lerp(alignedInputVelocity, modifiedVelocity, velocityDot);

        correctVelocity += GetJumpVelocity(velocity.y);

        return correctVelocity;
    }

	private Vector3 GetJumpVelocity(float yVelocity)
	{
		Vector3 jumpVelocity = Vector3.zero;

		if(Time.time < lastJumpPress + jumpPressDuration && yVelocity < jumpForce && CheckGround())
		{
			if(HelpJumpSound <= 0){
			m_AudioSource.clip = m_JumpSound;
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
			HelpJumpSound = 0.5f;
			}
			TempsDeChute = 0;
			lastJumpPress = -1f;
			jumpVelocity = new Vector3(0f, jumpForce - yVelocity, 0f);
		}

		return jumpVelocity;
	}
	
	private bool CheckGround()
	{
        Ray ray = new Ray(transform.position, Vector3.down);
		RaycastHit hit;
		OtherFootstepGround = false;
		if(Physics.Raycast(ray, out hit)){
			if(hit.transform.tag == "Untagged"){Id_FootstepSounds = 0;OtherFootstepGround = true;}
			
			if(hit.transform.tag == "Metal"){Id_FootstepSounds = 1;OtherFootstepGround = true;}
			
			if(hit.transform.tag == "Terre"){Id_FootstepSounds = 2;OtherFootstepGround = true;}
			
			if(hit.transform.tag == "Neige"){Id_FootstepSounds = 3;OtherFootstepGround = true;}
			
			if(hit.transform.tag == "Bois"){Id_FootstepSounds = 4;OtherFootstepGround = true;}
			
			if(hit.transform.tag == "Sable"){Id_FootstepSounds = 5;OtherFootstepGround = true;}
			
			if(hit.transform.tag == "Beton"){Id_FootstepSounds = 6;OtherFootstepGround = true;}
			Escalier = false;
			if(hit.transform.tag == "Escalier"){Escalier = true;}
			
			if(!OtherFootstepGround){Id_FootstepSounds = 0;}
		}
		if(Escalier){
		DistanceOnground = 0.5f;
		}else{
		DistanceOnground = 0.1f;
		}
        bool result = Physics.Raycast(ray, GetComponent<Collider>().bounds.extents.y + DistanceOnground, groundLayers);
		
        return result;
	}
	        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
			Vector2 m_Input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			VelociteMagnitudePlayer = new Vector3(GetComponent<Rigidbody>().velocity.x, 0f, GetComponent<Rigidbody>().velocity.z).magnitude;
            if ((VelociteMagnitudePlayer > 0) && (m_Input.x != 0 || m_Input.y != 0) && (Input.GetButton("Jump") == false))
            {
                m_StepCycle += ( VelociteMagnitudePlayer + (speed*(m_IsWalking ? 1f : m_RunstepLenghten))) * Time.deltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!onGround){return;}

			int n = Random.Range(1, sfs_FootstepSounds[Id_FootstepSounds].m_FootstepSounds.Length);
			m_AudioSource.clip = sfs_FootstepSounds[Id_FootstepSounds].m_FootstepSounds[n];
			m_AudioSource.PlayOneShot(m_AudioSource.clip);
			sfs_FootstepSounds[Id_FootstepSounds].m_FootstepSounds[n] = sfs_FootstepSounds[Id_FootstepSounds].m_FootstepSounds[0];
			sfs_FootstepSounds[Id_FootstepSounds].m_FootstepSounds[0] = m_AudioSource.clip;
        }
		public void AUneArmeDeTypeRifle(bool asRifle){AsRifle = asRifle;}
		public void AUneArmeDeTypeMelee(bool asMelee){AsMelee = asMelee;}
		public void AUneArmeDeTypeHandGun(bool asHandGun){AsHandGun = asHandGun;}
		public void ReceiveDialogState(bool Dialog){OnDialog = Dialog;}
		
		void OnGUI()
		{
			fps = (9.0f * fps + 1.0f / Time.deltaTime) / 10.0f;
			GUI.Label(new Rect(0, 0, 100, 50), "FPS: " + (int)fps);
		}
}
