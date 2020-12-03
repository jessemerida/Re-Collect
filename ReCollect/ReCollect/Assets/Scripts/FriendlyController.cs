using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using System;

public class FriendlyController : MonoBehaviour
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
    FriendlyGunController enemyGunController;

    //GameObject[] particles;
    ArrayList particles = new ArrayList();
    Transform[] allChildren;

    GameObject player;
    GameObject[] enemies; //after every action, find closest enemy and do stuff (move/attack)
    Vector3 closestEnemy;
    [SerializeField] float particlesHit;
    public bool respawn;

    public bool set;

    private void OnTriggerEnter(Collider other)
    {
        //when player ammo hits enemy, the number of particles that have been hit gets taken into account (each particle has a hit variable on their own script)
        //if enough particles are hit, then all particles turn red and alive = false
        //print("TRIGGER OTHER: " + other.name);
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            myAgent.ResetPath();
            //myAgent.isStopped = true;   
            //myAnim.SetBool("Move", false);
            //isMoving = false;
        }

        if (other.tag == "EnemyAmmo" && alive)
        {
            print("friendly got hit by enemy ammo, calculating particlesHit");
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
        if ((other.tag == "Player" || other.tag == "Enemy") && isMoving)
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
        enemyGunController = GetComponent<FriendlyGunController>();
        allChildren = GetComponentsInChildren<Transform>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemies = new GameObject[3]; //3 is max enemy entities at a time on screen (4 is total entities, but 1 is a friendly)
        respawn = false;

        set = false;
        foreach (Transform child in allChildren)
        {
            //print(child.name);
            if (child.name.Contains("BodyParticles") && !child.name.Contains("Finger") && !child.name.Contains("Toe"))
                particles.Add(child.gameObject);
        }
        print("friendly particles count: " + particles.Count);
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
                print(name + " has selected an action");
                /*do //testing
                {
                    action = Random.Range(1, 4);
                } while (action != 3);*/
                action = UnityEngine.Random.Range(1, 7);
                print("action: " + action);
                currentlyDoingAnAction = true;
                print(name + " has called DoAction()");
                Array.Clear(enemies, 0, enemies.Length);
                enemies = GameObject.FindGameObjectsWithTag("Enemy");
                closestEnemy = transform.position;
                if (enemies != null && enemies.Length > 0)
                {
                    closestEnemy = enemies[0].transform.position;
                    foreach (GameObject enemy in enemies)
                    {
                        //print("FINDING ENEMY ROOT");
                        //print("TEST: " + Vector3.Distance(transform.position, enemy.transform.position));
                        //print("TEST: " + Vector3.Distance(transform.position, closestEnemy));
                        //print("TEST: " + enemy.transform.root.name + ", " + enemy.transform.root.GetChild(0).gameObject.GetComponent<EnemyController>().alive);

                        if (Vector3.Distance(transform.position, enemy.transform.position) <= Vector3.Distance(transform.position, closestEnemy) && enemy.transform.root.GetChild(0).gameObject.GetComponent<EnemyController>().alive)
                        {
                            if (set)
                                closestEnemy = enemy.transform.position;
                            else
                            {
                                GameObject[] bodyParticlesRoot = GameObject.FindGameObjectsWithTag("EnemyHitboxParticle");
                                foreach (GameObject particle in bodyParticlesRoot)
                                {
                                    if (particle.transform.root == enemy.transform.root && particle.name == "BodyParticlesBody(Clone)")
                                    {
                                        closestEnemy = particle.transform.position;
                                        print("FOUND ENEMY BODY PARTICLE");
                                    }
                                }
                            }
                        }
                    }
                }
                print("set: " + set + ", " + closestEnemy);
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

            if ((particlesHit != 0 && particles.Count != 0) && ((particlesHit / particles.Count) > 0.4f))
            {
                //print("test");
                StartCoroutine(Die());
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
        alive = false;
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
                    print("agent stopped: " + myAgent.isStopped);
                    if (closestEnemy != transform.position)
                    {
                        isMoving = true;
                        myAnim.SetBool("Move", true);
                        //print(name + " is currently doing a moving animation");
                        myAgent.SetDestination(closestEnemy); //SET TO ENEMY
                        //yield return new WaitForSeconds(3f);
                        yield return new WaitUntil(() => isMoving == false);
                        /*while (!myAgent.hasPath || myAgent.velocity.sqrMagnitude == 0f)
                            yield return null;*/
                    }
                    else
                        yield return new WaitForSeconds(0f);
                    myAnim.SetBool("Move", true);
                    //print(name + " has stopped moving animation");
                    break;
                }
            case 4: //attack
            case 5:
            case 6:
                {
                    myAnim.SetBool("Attack", true);
                    //print(name + " is currently doing an attack animation");
                    //transform.LookAt(player.transform.position); //SET TO ENEMY
                    if (closestEnemy != transform.position)
                    {
                        //print("shooting closest enemy");
                        transform.LookAt(closestEnemy);
                        //weapon.transform.LookAt(closestEnemy);
                        enemyGunController.SetShoot(true, closestEnemy);
                        yield return new WaitForSeconds(3.17f); //one shot = 0.634 seconds, this is 5 shots
                        enemyGunController.SetShoot(false, closestEnemy);
                        myAnim.SetBool("Attack", false);
                    }
                    else
                        yield return new WaitForSeconds(0f);
                    weapon.GetComponent<ParentConstraint>().constraintActive = true;
                    myAnim.SetBool("Attack", false);
                    //print("player is closer to friendly, no enemies nearby");
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