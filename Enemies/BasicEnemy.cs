using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour {
    public Transform Player, goal;
    public Transform[] PatrolPoint;
    private UnityEngine.AI.NavMeshAgent myAgent;
    public Vector3 lastPosiiton;
    Vector3 StartPoint;

    public bool JumpToPlayerHeight = true;
    public bool GoesOnPatrol = true;

    public GameObject PatrolPointPrefab;
    public Transform[] PatrolPointInstatiated = new Transform[2];

    public GameObject MyModel;
    public Animator myAnim;

    public int PatrolIndex = 0;

    public float ChaseSpeed = 3, PatrolSpeed = 1;

    public float MaxDetectionRange = 5f, MaxChaseRange = 10f;

    public bool PlayerSpotted = false;

    public float YHeight;

    // Use this for initialization
    void Start () {
        Player = GameObject.Find("Player").transform;
        if (!GoesOnPatrol)
        {
            GameObject NewPatrolPoint = GameObject.Instantiate(PatrolPointPrefab, transform.position, transform.rotation);
            GameObject NewPatrolPoint2 = GameObject.Instantiate(PatrolPointPrefab, transform.position, transform.rotation);
            PatrolPointInstatiated[0] = NewPatrolPoint.transform;
            PatrolPointInstatiated[1] = NewPatrolPoint2.transform;
            PatrolPoint = PatrolPointInstatiated;
            
        }
        goal = PatrolPoint[0];
        myAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //goal.position = transform.position;
        lastPosiiton = goal.position;
        myAgent.destination = goal.position;
        MyModel = transform.Find("Model").gameObject;
        myAnim = MyModel.GetComponent<Animator>();
        YHeight = transform.position.y;
        StartPoint = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (JumpToPlayerHeight)
        {
            if (!CameraMaster.Instance.TopDown)
            {

                YHeight = transform.position.y;
            }
            else if (CameraMaster.Instance.TopDown)
            {
                Vector3 toPlayerY = transform.position;
                toPlayerY.y = Player.position.y;
                transform.position = toPlayerY;
                if (Input.GetButtonDown("Camera"))
                {
                    toPlayerY.y = YHeight;
                    transform.position = toPlayerY;
                }
            }
        }

        
            if (PatrolIndex > PatrolPoint.Length - 1)
            {
                PatrolIndex = 0;
            }


            if (Vector3.Distance(transform.position, Player.position) < MaxDetectionRange && !PlayerSpotted)
            {
                goal = Player;
                PlayerSpotted = true;
                myAgent.speed = ChaseSpeed;
            }
            else if (PlayerSpotted && Vector3.Distance(transform.position, Player.position) < MaxDetectionRange * 2)
            {
                goal = Player;
                myAgent.speed = ChaseSpeed;
            }
            else if (PlayerSpotted && Vector3.Distance(transform.position, Player.position) >= MaxDetectionRange * 2)
            {
                goal = PatrolPoint[PatrolIndex];
                myAgent.speed = PatrolSpeed;
                PlayerSpotted = false;
            }
            else
            {
                goal = PatrolPoint[PatrolIndex];
                myAgent.speed = PatrolSpeed;
            }

            if (goal.position != lastPosiiton)
            {
                myAgent.destination = goal.position;
                lastPosiiton = goal.position;
            }
        

        //Animate
        AnimateEnemy();
    }

    private void OnTriggerEnter(Collider myColl)
    {
        if (myColl.tag == "PatrolPoint")
        {
            Debug.Log("Point reached");
            PatrolIndex++;
        }
    }
    private void OnCollisionEnter(Collision myCollision)
    {
        if(myCollision.gameObject.tag == "Player")
        {
            PlayerController player = myCollision.gameObject.GetComponent<PlayerController>();
            player.DeathGo();
        }
    }

    void AnimateEnemy()
    {
        if (!PlayerSpotted)
        {
            myAnim.SetBool("Chase", false);
        }
        else
        {
            myAnim.SetBool("Chase", true);
        }
    }
}
