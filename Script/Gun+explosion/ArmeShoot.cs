using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ImpactTire
{
    public string tagName;
    public Transform ImpactSpawn;
    public bool sticky;
    public bool SendDegat;
}

[System.Serializable]
public class SON
{
    public AudioClip Sond;
    public float VolumeSond;
    public float pitch;
}

[System.Serializable]
public class AudioArme
{
    public AudioSource m_AudioSource;
    public SON SondTire;
    public SON SondRecharge;
    public SON SondRechargeOutofAmmo;
    public SON SondNoAmmoTire;
}

public class ArmeShoot : MonoBehaviour
{
    [Header("Arme")]
    public int munitionMax = 0;
    public int munition = 0;
    public int munitionChargeur = 0;
    public float ConsumeMana = 0f;
    public float cadence = 0.1f;
    public float damage = 0f;
    public bool useMana = false;
    public bool NoAmmo = false;
    [SerializeField] private float cadenceVar = 0;
    [SerializeField] private int BallePareTire = 1;
    [Header("RayCast")]
    [SerializeField] private LayerMask layerAuthoriser = 0;
    [SerializeField] private float hitForceTire = 0;
    [SerializeField] private bool RayCast = true;
    [SerializeField] private bool CoupParCoup = false;
    [SerializeField] private float Z = 10;
    [SerializeField] private float scale = 0.1f;
    [SerializeField] private bool ShotParticule = false;
    [SerializeField] private GameObject SpawnBalleRay = null;
    [SerializeField] private float SpeedBulletRay = 1f;
    private RaycastHit hit;
    private Ray ray;
    [Header("Spawn")]
    [SerializeField] private Transform SpawnProjectile = null;
    [SerializeField] private GameObject pointDapparitionProjectile = null;
    [SerializeField] private float tempsSpawn = 1.5f;
    [Header("Effet")]
    [SerializeField] private AudioArme AA = new AudioArme();
    [SerializeField] private ImpactTire[] IT = new ImpactTire[11];
    [SerializeField] private Vector2 TailleImpactMinMax = new Vector2(1, 2);
    [SerializeField] private ParticleSystem TireParticle = null;
    private Transform Reasigneparent;
    [Header("Rechargement")]
    [SerializeField] private float tempsDeRechargement = 0.5f;
    [SerializeField] private bool RechargementState = false;
    [Header("Autre")]
    [SerializeField] private Transform castingBullet = null;
    [SerializeField] private GameObject pointDapparitionBullet = null;
    [SerializeField] private float forceCasting = 200f;
    [SerializeField] private float TailleBullet = 1f;
    [SerializeField] private PlayerStat statPlayer = null;
    [SerializeField] private Animator Arm_Animator = null;
    [SerializeField] private float ForceShake = 2f;
    [SerializeField] private float DivideShake = 4f;
    private Transform casting = null;
    public bool OnAction = false;

    public bool testAim = false;
    private int i = 0,bt = 0;
    private bool ParticleLoop = false;
    private GameObject MainCamera = null;

    void Start()
    {
        if (!CoupParCoup)
        {
            RHP(TireParticle.transform.gameObject);
        }
        MainCamera = GameObject.FindWithTag("MainCamera");
    }

    void OnEnable()
    {
        StartCoroutine(WaitApparition());
    }

    void Update()
    {
        if (!OnAction)
        {
            if (Input.GetButton("Fire1") && !RechargementState && cadenceVar < 0)
            {
                if ((munition > 0 || useMana) && (statPlayer.ManaJoueur >= ConsumeMana || !useMana))
                {
                    if (Input.GetButtonDown("Fire1") || !CoupParCoup) /*CoupParCoup*/
                    {
                        for (bt = 0; bt < BallePareTire; bt++)
                        {
                            if (RayCast)
                            {
                                Vector3 direction = Random.insideUnitCircle * scale;
                                direction.z = Z;
                                direction = MainCamera.transform.TransformDirection(direction.normalized);
                                ray = new Ray(MainCamera.transform.position, direction);
                                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerAuthoriser))
                                {
                                    Debug.DrawLine(transform.position, hit.point);
                                    if (SpawnBalleRay != null)
                                    {
                                        if (!ShotParticule)
                                        {
                                            StartCoroutine(Trajectoire(Instantiate(SpawnBalleRay, pointDapparitionProjectile.transform.position, Quaternion.identity), hit.point));
                                        }
                                        else
                                        {
                                            Instantiate(SpawnBalleRay, pointDapparitionProjectile.transform.position, Quaternion.Euler(MainCamera.transform.eulerAngles)).transform.LookAt(hit.point);
                                        }
                                    }
                                    StartCoroutine(MainCamera.GetComponent<MouseLook>().Shake(ForceShake,DivideShake));
                                    if (hit.rigidbody != null)
                                    {
                                        hit.rigidbody.AddForce(ray.direction * hitForceTire);
                                    }
                                    for (i = 0; i < IT.Length; i++)
                                    {
                                        if (hit.transform.tag == IT[i].tagName)
                                        {
                                            Reasigneparent = Instantiate(IT[i].ImpactSpawn, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                                            Reasigneparent.localScale = new Vector3(1f, 1f, 1f) * Random.Range(TailleImpactMinMax.x, TailleImpactMinMax.y);
                                            if (IT[i].sticky)
                                            {
                                                Reasigneparent.SetParent(hit.transform);
                                            }
                                            if (IT[i].SendDegat)
                                            {
                                                hit.collider.gameObject.SendMessage("ReceiveDamagePlayer", damage);
                                            }
                                            i = IT.Length;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Instantiate(SpawnProjectile, pointDapparitionProjectile.transform.position, Quaternion.identity);
                            }
                        }
                        if (castingBullet != null)
                        {
                            casting = Instantiate(castingBullet, pointDapparitionBullet.transform.position, Quaternion.Euler(pointDapparitionBullet.transform.eulerAngles));
                            casting.localScale = new Vector3(1f,1f,1f) * TailleBullet;
                            if (casting.GetComponent<Rigidbody>())
                            {
                                casting.GetComponent<Rigidbody>().AddForce(-pointDapparitionBullet.transform.forward * forceCasting);
                            }
                        }

                        if (!useMana) { munition--; } else { statPlayer.ReceiveTakeMana(ConsumeMana); }
                        if (TireParticle != null && !ParticleLoop) { TireParticle.Play(); ParticleLoop = true; }

                        PlaySound(AA.SondTire);
                        Arm_Animator.SetBool("Shoot", true);
                        cadenceVar = cadence;
                    }
                }
                else
                {
                    if (Input.GetButtonDown("Fire1")) { PlaySound(AA.SondNoAmmoTire); }
                    if (cadenceVar < 0) { Arm_Animator.SetBool("Shoot", false); }
                    if (TireParticle != null && ParticleLoop) { TireParticle.Stop(); ParticleLoop = false; }
                }
            }
            else
            {
                if (cadenceVar < 0 && cadenceVar >-9)
                {
                    Arm_Animator.SetBool("Shoot", false);
                    if (TireParticle != null && ParticleLoop) { TireParticle.Stop(); ParticleLoop = false; }
                    cadenceVar = -10;
                } 
            }

            if (cadenceVar >= 0) { cadenceVar -= Time.deltaTime; if (CoupParCoup && cadenceVar <= cadence*0.8f) { Arm_Animator.SetBool("Shoot", false); } }

            if (Input.GetButtonDown("Recharger") && !useMana && munitionChargeur > 0)
            {
                if (munition < munitionMax && !RechargementState)
                {
                    StartCoroutine(Rechargement());
                }
            }

            // Animation
            Arm_Animator.SetBool("Aim", Input.GetButton("Fire2") || testAim);
            Arm_Animator.SetBool("Walk", !Input.GetButton("Sprint") && (Input.GetButton("Horizontal") || Input.GetButton("Vertical")));
            Arm_Animator.SetBool("Run", Input.GetButton("Sprint") && (Input.GetButton("Horizontal") || Input.GetButton("Vertical")));
        }
    }

    IEnumerator Trajectoire(GameObject SRB,Vector3 destination)
    {
        while (SRB.transform.position != destination)
        {
             SRB.transform.position = Vector3.MoveTowards(SRB.transform.position, destination, Time.deltaTime * SpeedBulletRay);
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(SRB,5);
    }

    IEnumerator Rechargement()
    {
        RechargementState = true;
        if (munition > 0)
        {
            PlaySound(AA.SondRecharge);
            Arm_Animator.SetBool("Rechargement", true);
        }
        else
        {
            PlaySound(AA.SondRechargeOutofAmmo);
            Arm_Animator.SetBool("RechargementOutOfAmmo", true);
        }
        yield return new WaitForSeconds(tempsDeRechargement);
        if (munitionChargeur >= Mathf.Abs(munition - munitionMax))
        {
            munitionChargeur += (munition - munitionMax);
            munition = munitionMax;

        }
        else
        {
            munition = munition + munitionChargeur;
            munitionChargeur = 0;
        }
        RechargementState = false;
    }

    IEnumerator WaitApparition()
    {
        RechargementState = true;
        yield return new WaitForSeconds(tempsSpawn);
        RechargementState = false;
    }

    void PlaySound(SON sound)
    {
        if (sound.Sond != null)
        {
            AA.m_AudioSource.clip = sound.Sond;
            AA.m_AudioSource.volume = sound.VolumeSond;
            AA.m_AudioSource.pitch = sound.pitch;
            AA.m_AudioSource.PlayOneShot(AA.m_AudioSource.clip);
        }
    }

    void RHP(GameObject part)
    {
        if(part != null)
        {
            if (part.GetComponent<ParticleSystem>())
            {
                ParticleSystem.MainModule main = part.transform.GetChild(i).GetComponent<ParticleSystem>().main;
                main.loop = true;
            }
            Transform[] allChildren = part.GetComponentsInChildren<Transform>();
            foreach (Transform child in allChildren)
            {
                if (child.GetComponent<ParticleSystem>())
                {
                    ParticleSystem.MainModule main = child.GetComponent<ParticleSystem>().main;
                    main.loop = true;
                }
            }
        }
    }
}
