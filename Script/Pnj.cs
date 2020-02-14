using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pnj : MonoBehaviour
{
    public NavMeshAgent Agent;
    public float TimerChangePosition;
    public float TimerChangePositionVar;
    public int PositionPoint;
    public float m_ForwardAmount;
    public float SyncroVitesse = 0.17f;
    public float SyncroAnnimation = 0.1f;
    public bool ModeRandom = false;
    public bool RandomTime = false;
    public float AddRandomTimeMax = 5f;
    public float DistanceRestante;
    public bool OnMaison;
    private float MaisonfadeTransition;
    public GameObject ModelOpaque, ModelFade;
    public Transform ObstacleArret;
    public Transform ObstacleAsigner;
    private bool asigneObstacle = false;
    Animator m_Animator;
    public GameObject[] PointDeplacement;
    Color fadeColor;
    public GameObject Maison;
    public float tempsJourNuit;
    private bool PhasesMaison = false;
    public bool OnDialog;
    private GameObject player;
    public Vector3 targetPointDialog;

    void Start()
    {
        player = GameObject.FindWithTag("player");
        OnDialog = false;
        tempsJourNuit = 0.5f;
        m_Animator = GetComponent<Animator>();
        if (RandomTime)
        {
            TimerChangePositionVar = TimerChangePosition + Random.Range(0f, AddRandomTimeMax);
        }
        else
        {
            TimerChangePositionVar = TimerChangePosition;
        }
        if (ModeRandom)
        {
            PositionPoint = Random.Range(0, PointDeplacement.Length);
        }
        if (PointDeplacement.Length > 0)
        {
            Agent.SetDestination(PointDeplacement[PositionPoint].transform.position);
            if (ModeRandom)
            {
                PositionPoint = Random.Range(0, PointDeplacement.Length);
            }
            else
            {
                PositionPoint++;
            }
        }
        fadeColor = ModelFade.GetComponent<Renderer>().material.color;
    }

    void Update()
    {
        Agent.isStopped = OnDialog;
        if (!OnDialog)
        {
            m_ForwardAmount = Agent.desiredVelocity.magnitude;
            m_Animator.SetFloat("Forward", m_ForwardAmount * SyncroVitesse, SyncroAnnimation, Time.deltaTime);
            if (OnMaison)
            {
                ModelOpaque.SetActive(false);
                ModelFade.SetActive(true);
                fadeColor.a = MaisonfadeTransition;
                ModelFade.GetComponent<Renderer>().material.color = fadeColor;
                if (MaisonfadeTransition > 0.05f)
                {
                    MaisonfadeTransition -= Time.deltaTime * 0.7f;
                }
                else
                {
                    MaisonfadeTransition = 0f;
                    gameObject.GetComponent<CapsuleCollider>().enabled = false;
                    Agent.SetDestination(Maison.transform.position);
                }

            }
            else
            {
                fadeColor.a = MaisonfadeTransition;
                ModelFade.GetComponent<Renderer>().material.color = fadeColor;
                if (MaisonfadeTransition < 0.99f)
                {
                    MaisonfadeTransition += Time.deltaTime;
                }
                else
                {
                    gameObject.GetComponent<CapsuleCollider>().enabled = true;
                    MaisonfadeTransition = 1f;
                    ModelOpaque.SetActive(true);
                    ModelFade.SetActive(false);
                }
            }
            if (tempsJourNuit >= 0.25f && tempsJourNuit <= 0.75f)
            {
                if (tempsJourNuit <= 0.255f)
                {
                    TimerChangePositionVar = 0.5f;
                }
                PhasesMaison = false;
                OnMaison = false;
                TimerChangePositionVar -= Time.deltaTime;
                if (TimerChangePositionVar <= 0f)
                {
                    if (asigneObstacle)
                    {
                        Destroy(ObstacleAsigner.gameObject);
                        asigneObstacle = false;
                    }
                    if (PointDeplacement.Length > 0)
                    {
                        Agent.SetDestination(PointDeplacement[PositionPoint].transform.position);
                        if (ModeRandom) { PositionPoint = Random.Range(0, PointDeplacement.Length); } else { PositionPoint++; }
                        if (PointDeplacement.Length < PositionPoint) { PositionPoint = 0; }
                    }
                    TimerChangePositionVar = TimerChangePosition;
                }

                if (m_ForwardAmount <= 0f && asigneObstacle == false && TimerChangePositionVar <= TimerChangePosition * 0.8f && Agent.remainingDistance < 3)
                {
                    ObstacleAsigner = Instantiate(ObstacleArret, transform.position, Quaternion.identity);
                    ObstacleAsigner.SetParent(transform);
                    asigneObstacle = true;
                    //Agent.Stop();
                }
                DistanceRestante = Agent.remainingDistance;
                if (Agent.remainingDistance >= 3)
                {
                    if (asigneObstacle)
                    {
                        Destroy(ObstacleAsigner.gameObject);
                        asigneObstacle = false;
                    }
                }
            }
            else
            {
                if (!PhasesMaison)
                {
                    Agent.SetDestination(Maison.transform.position);
                    PhasesMaison = true;
                }
                if (!OnMaison)
                {
                    if (PhasesMaison && Vector3.Distance(Maison.transform.position, transform.position) <= 4)
                    {
                        OnMaison = true;
                    }
                }
            }
        }
        else
        {
            Agent.SetDestination(transform.position);
            m_Animator.SetFloat("Forward", 0, SyncroAnnimation, Time.deltaTime);
            TimerChangePositionVar = 1;
            float step = 3 * Time.deltaTime;
            targetPointDialog = player.transform.position - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetPointDialog, step, 0.0f);
            Vector3 Correction = new Vector3(newDir.x, 0, newDir.z);
            transform.rotation = Quaternion.LookRotation(Correction);
        }
    }
    public void ReceiveCycle(float cycleJourNuit)
    {
        tempsJourNuit = cycleJourNuit;
    }

    public void ReceiveDialogState(bool Dialog)
    {
        OnDialog = Dialog;
        if (!OnDialog && PhasesMaison)
        {
            Agent.SetDestination(Maison.transform.position);
        }
        if (OnDialog)
        {
            if (GetComponent<DialogueTrigger>() != null)
            {
                GetComponent<DialogueTrigger>().TriggerDialogue();
            }
        }
        else
        {
            GetComponent<DialogueTrigger>().ConditionDialogue();
        }
    }
}
