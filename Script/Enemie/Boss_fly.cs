using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_fly : MonoBehaviour
{
    [SerializeField] private float bossCurrentLife = 1500f, bossMaxLife = 1500f, vitesseRotation = 0.4f, VitesseDeplacement = 35f/*, vitesseRotatCou = 2f*/;
    [SerializeField] private GameObject[] travelPoint = null;
    [SerializeField] private GameObject VuePoint = null;
    /*[SerializeField] private Transform attaque = null;*/
    [SerializeField] private RectTransform BarDeVie = null;
    [SerializeField] private bool Move = false, SeePlayer = false, isDead = false;
    [SerializeField] private Animator m_AnimatorDragon = null;
    private Vector3 PointerPosition = Vector3.zero, BossPosdepart = Vector3.zero;
    private GameObject Player = null;
    public float CoeffVitesseAceleration = 1, CoeffRotation = 1, CoeffDistance;

    void Start()
    {
        Player = GameObject.FindWithTag("player");
        bossCurrentLife = bossMaxLife;
        BarDeVie.sizeDelta = new Vector2((bossCurrentLife / bossMaxLife) * 100, BarDeVie.sizeDelta.y);
        Move = true;
        PointerPosition = travelPoint[1].transform.position;
        StartCoroutine(Attaque());
        BossPosdepart = transform.position;
    }

    void Update()
    {
        if (isDead) { return; }
        VuePoint.transform.LookAt(Player.transform);
        transform.Translate(Vector3.forward * Time.deltaTime * VitesseDeplacement * CoeffVitesseAceleration);
        Vector3 targetPoint = PointerPosition - transform.position;
        float step = vitesseRotation * Time.deltaTime * CoeffRotation;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetPoint, step, 0.0f);

        if (Move)
        {
            CoeffDistance += Time.deltaTime * 0.5f;
            if (CoeffVitesseAceleration < 1) { CoeffVitesseAceleration += Time.deltaTime * 0.5f; }
            if (CoeffRotation < 2) { CoeffRotation += Time.deltaTime * (1.5f - (Vector3.Distance(PointerPosition, transform.position) / Vector3.Distance(PointerPosition, transform.position))); }
            Move = Vector3.Distance(PointerPosition, transform.position) > (10 * CoeffDistance);
            if (!Move)
            {
                if (Random.Range(0, 10) == 0)
                {
                    if (!SeePlayer)
                    {
                        StartCoroutine(ReMove(Random.Range(5, 8)));
                    }
                    else
                    {
                        StartCoroutine(ReMove(Random.Range(10, 20)));
                    }
                }
                else
                {
                    Move = true;
                    CoeffDistance = 0;
                    PointerPosition = travelPoint[Random.Range(0, travelPoint.Length)].transform.position;
                }
            }
            transform.rotation = Quaternion.LookRotation(newDir);
        }
        else
        {
            if (CoeffVitesseAceleration > 0) { CoeffVitesseAceleration -= Time.deltaTime * 0.5f; }
            if (CoeffRotation < 0.5f) { CoeffRotation -= Time.deltaTime * 2; }
            PointerPosition = Player.transform.position;
            transform.rotation = Quaternion.LookRotation(new Vector3(newDir.x, Mathf.Lerp(newDir.y, 0f, Time.deltaTime * 3), newDir.z));
            CoeffDistance = 0;
        }
    }
    public void ReceiveDamagePlayer(float Damage)
    {
        if (!isDead)
        {
            Damage -= bossCurrentLife;
            BarDeVie.sizeDelta = new Vector2((bossCurrentLife / bossMaxLife) * 100, BarDeVie.sizeDelta.y);
            if (bossCurrentLife <= 0)
            {
                Mort();
                isDead = true;
            }
        }
    }

    void Mort()
    {

    }

    IEnumerator ReMove(float Time)
    {
        m_AnimatorDragon.SetFloat("Speed", 1.5f);
        yield return new WaitForSeconds(Time);
        Move = true;
        BossPosdepart = transform.position;
        PointerPosition = travelPoint[Random.Range(0, travelPoint.Length)].transform.position;
        yield return new WaitForSeconds(1f);
        m_AnimatorDragon.SetFloat("Speed", 1f);
    }
    IEnumerator Attaque()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3, 10));
            Debug.Log("attaque");
        }
    }
}
