using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class EnemyController : MonoBehaviour
{
    int maxDamage; //if enough particles are turned red, the enemy will die. this is the threshold.
    int damageTaken; //amount of particles hit
    [SerializeField] int action; //0 = thinking, 1 = standing, 2 = walking/running/moving, 3 = attacking/shootings
    [SerializeField] bool currentlyDoingAnAction;
    public bool alive;
    bool dying;
    [SerializeField] bool isMoving;

    NavMeshAgent myAgent;
    Animator myAnim;

    public GameObject weapon;
    //public GameObject ammo;
    EnemyGunController enemyGunController;

    //GameObject[] particles;
    ArrayList particles = new ArrayList();
    Transform[] allChildren;

    GameObject player;
    GameObject friendly;
    int target;
    Vector3[] targets;
    [SerializeField] float particlesHit;
    public bool respawn;

    private void OnTriggerEnter(Collider other)
    {
        //when player ammo hits enemy, the number of particles that have been hit gets taken into account (each particle has a hit variable on their own script)
        //if enough particles are hit, then all particles turn red and alive = false
        //print("TRIGGER OTHER: " + other.name);
        if (other.tag == "Player" || other.tag == "Friendly")
        {
            if (myAgent != null)
                myAgent.ResetPath();
            //myAgent.isStopped = true;   
            //myAnim.SetBool("Move", false);
            //isMoving = false;
        }

        if (other.tag == "PlayerAmmo" && alive)
        {
            //print("enemy got hit by player ammo, calculating particlesHit");
            particlesHit = 0;
            foreach (GameObject particle in particles)
            {
                ParticleSystem.MainModule main = particle.GetComponent<ParticleSystem>().main;
                if (main.startColor.color == Color.red)
                    particlesHit++;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.tag == "Player" || other.tag == "Friendly") && isMoving)
        {
            myAgent.ResetPath();
            //myAgent.isStopped = true;
            //myAnim.SetBool("Move", false);
            //isMoving = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        maxDamage = 15;
        damageTaken = 0;
        action = 0;
        currentlyDoingAnAction = false;
        alive = true;
        dying = false;
        isMoving = false;
        myAgent = GetComponent<NavMeshAgent>();
        myAnim = GetComponent<Animator>();
        enemyGunController = GetComponent<EnemyGunController>();
        allChildren = GetComponentsInChildren<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");
        targets = new Vector3[2];
        respawn = false;

        foreach (Transform child in allChildren)
        {
            //print(child.name);
            if (child.name.Contains("BodyParticles") && !child.name.Contains("Finger") && !child.name.Contains("Toe"))
                particles.Add(child.gameObject);
        }
        //Test();
    }

    /*void Test()
    {
        print("particle count: " + particles.Count);
        foreach (GameObject particle in particles)
        {
            print(particle.transform.parent.name);
            if (particle.name.Contains("arm"))
            {
                ParticleSystem.MainModule main = particle.GetComponent<ParticleSystem>().main;
                main.startColor = Color.red;
            }
        }
    }*/

    // Update is called once per frame
    void FixedUpdate()
    {
        if (alive)
        {
            if (!currentlyDoingAnAction)
            {
                myAgent.isStopped = false;
                //print(name + " has selected an action");
                /*do //testing
                {
                    action = Random.Range(1, 4);
                } while (action != 3);*/
                action = Random.Range(1, 7);
                //print("action: " + action);
                currentlyDoingAnAction = true;
                //print(name + " has called DoAction()");
                target = 0;
                friendly = GameObject.Find("Friendly(Clone)");
                targets[0] = player.transform.position;
                if (friendly != null)
                {
                    target = Random.Range(0, 2); //0 = player, 1 = friendly
                    targets[1] = friendly.transform.position;
                }
                //target = 1;
                StartCoroutine(DoAction(action));
            }

            if (!myAgent.pathPending)
            {
                if (myAgent.remainingDistance <= myAgent.stoppingDistance)
                {
                    if (!myAgent.hasPath || myAgent.velocity.sqrMagnitude == 0f)
                    {
                        myAgent.isStopped = true;
                        myAnim.SetBool("Move", false);
                        isMoving = false;
                        //print(name + " has stopped moving animation");
                    }
                }
            }

            /*float test = particlesHit / particles.Count;
            print("particleHit: " + particlesHit + ", particles.Count: " + particles.Count + " " + (particlesHit / particles.Count) + ",   " + test);*/

            if ((particlesHit != 0 && particles.Count != 0) && ((particlesHit / particles.Count) > 0.9f))
            {
                //print("test");
                //StartCoroutine(Die());
                alive = false;
            }
        }
        else if (!dying)
            StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        dying = true;
        myAgent.isStopped = true;
        myAnim.SetBool("Die", true);
        //print(name + " is currently doing a death animation");
        foreach (GameObject particle in particles)
        {
            ParticleSystem.MainModule main = particle.GetComponent<ParticleSystem>().main;
            main.startColor = Color.red;
        }
        yield return new WaitForSeconds(2.367f);
        myAnim.SetBool("Die", false);
        //print(name + " has stopped death animation");
        //alive = false;
        respawn = true;
        gameObject.SetActive(false);
    }

    IEnumerator DoAction(int action)
    {
        //print("DoAction() has started");
        switch (action)
        {
            case 1: //standby
                {
                    myAnim.SetBool("Standby", true);
                    //print(name + " is currently doing a standby animation");
                    yield return new WaitForSeconds(3.117f);
                    myAnim.SetBool("Standby", false);
                    //print(name + " has stopped standby animation");
                    break;
                }
            case 2: //move
            case 3:
                {
                    //print("agent stopped: " + myAgent.isStopped);
                    isMoving = true;
                    myAnim.SetBool("Move", true);
                    //print(name + " is currently doing a moving animation");
                    myAgent.SetDestination(targets[target]);
                    //yield return new WaitForSeconds(3f);
                    yield return new WaitUntil(() => isMoving == false);
                    /*while (!myAgent.hasPath || myAgent.velocity.sqrMagnitude == 0f)
                        yield return null;*/
                    //print(name + " has stopped moving animation");
                    break;
                }
            case 4: //attack
            case 5:
            case 6:
                {
                    myAnim.SetBool("Attack", true);
                    //print(name + " is currently doing an attack animation");
                    transform.LookAt(targets[target]);
                    enemyGunController.SetShoot(true);
                    yield return new WaitForSeconds(3.17f); //one shot = 0.634 seconds, this is 5 shots
                    enemyGunController.SetShoot(false);
                    myAnim.SetBool("Attack", false);
                    //print(name + " has stopped attack animation");
                    break;
                }
        }
        StartCoroutine(DoThink());
    }

    IEnumerator DoThink()
    {
        action = 0;
        //myAnim.SetBool("Think", true);
        //print(name + " is currently doing a think animation");
        yield return new WaitForSeconds(2.65f); //5.3f = 1 anim
        //myAnim.SetBool("Think", false);
        //print(name + " has stopped think animation");
        currentlyDoingAnAction = false;
    }
}