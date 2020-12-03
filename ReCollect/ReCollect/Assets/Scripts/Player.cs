using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using DearVR;

public class Player : OVRHand
{
    [SerializeField] int health;
    ConstraintSource rightHandSource;
    public GameObject lifeBand;
    GameObject lifeBandClone;
    GameObject lifeBandNode1;
    GameObject lifeBandNode2;
    GameObject lifeBandNode3;
    GameObject lifeBandNode4;
    [SerializeField] GameObject[] nodes;
    [SerializeField] bool inCutscene; //disables scripts that aren't menu-related if in cutscene
    GameObject rightHand;
    GameObject leftHand;
    [SerializeField] bool pausedState;

    bool startRecollect;
    public bool recollecting;
    [SerializeField] bool allNodesActive;
    [SerializeField] List<GameObject> recollectableEnemies;

    [SerializeField] int developerToolTime;
    [SerializeField] bool DTreloading;
    [SerializeField] float stoptime;
    public bool timeStopped;
    public int DTMax;
    bool leftIndexPinched;
    bool leftMiddlePinched;
    bool rightIndexPinched;
    bool rightMiddlePinched;

    private void Awake()
    {
        //this.enabled = false;
        //GetComponent<DearVRSource>().DearVRPlayOneShot(GetComponent<AudioSource>().clip);
        RenderSettings.skybox.SetFloat("_Exposure", 8);
        StartCoroutine(LoadLevel(8));
    }

    public bool GetPausedState()
    {
        return pausedState;
    }

    // Start is called before the first frame update
    void Start()
    {
        health = 40;
        inCutscene = false;
        rightHand = GameObject.Find("RightHand");
        //rightHand.GetComponent<DearVRSource>().GetReverbSendList()[0].send = 1;
        leftHand = GameObject.Find("LeftHand");
        //leftHand.GetComponent<DearVRSource>().GetReverbSendList()[0].send = 1;
        pausedState = false;

        SetUpLifeBand();
        startRecollect = false;
        recollecting = false;
        allNodesActive = false;

        DTMax = 7;
        developerToolTime = DTMax;//69;
        DTreloading = false;
        stoptime = 1;
        timeStopped = false;

        leftIndexPinched = false;
        leftMiddlePinched = false;
        rightIndexPinched = false;
        rightMiddlePinched = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pausedState&& (!leftIndexPinched && !leftMiddlePinched) && leftHand.GetComponent<OVRHand>().IsTracked && leftHand.GetComponent<OVRHand>().GetTrackingConfidence() == OVRHand.TrackingConfidence.High && (leftHand.GetComponent<OVRHand>().GetFingerIsPinching(HandFinger.Index) || leftHand.GetComponent<OVRHand>().GetFingerIsPinching(HandFinger.Middle)))
        {
            leftIndexPinched = true;
            leftMiddlePinched = true;
            leftHand.GetComponent<DearVRSource>().DearVRPlayOneShot(leftHand.GetComponent<AudioSource>().clip);
        }

        if (!leftHand.GetComponent<OVRHand>().GetFingerIsPinching(HandFinger.Index))
            leftIndexPinched = false;
        if (!leftHand.GetComponent<OVRHand>().GetFingerIsPinching(HandFinger.Middle))
            leftMiddlePinched = false;

        if (!pausedState && (!rightIndexPinched && !rightMiddlePinched) && rightHand.GetComponent<OVRHand>().IsTracked && rightHand.GetComponent<OVRHand>().GetTrackingConfidence() == OVRHand.TrackingConfidence.High && (rightHand.GetComponent<OVRHand>().GetFingerIsPinching(HandFinger.Index) || rightHand.GetComponent<OVRHand>().GetFingerIsPinching(HandFinger.Middle)))
        {
            rightIndexPinched = true;
            rightMiddlePinched = true;
            rightHand.GetComponent<DearVRSource>().DearVRPlayOneShot(rightHand.GetComponent<AudioSource>().clip);
            if (rightHand.GetComponent<OVRHand>().GetFingerIsPinching(HandFinger.Middle) && !rightHand.GetComponent<WeaponActivator>().weaponActivated)
            {
                GameObject.Find("SelectHUD").GetComponent<DearVRSource>().DearVRPlayOneShot(GameObject.Find("SelectHUD").GetComponent<HUDController>().switchSound.clip);
            }
            if (rightHand.GetComponent<OVRHand>().GetFingerIsPinching(HandFinger.Middle) && rightHand.GetComponent<WeaponActivator>().weapon != null && rightHand.GetComponent<WeaponActivator>().weapon.name == "developertoolWL(Clone)" && rightHand.GetComponent<WeaponActivator>().weapon.GetComponent<DeveloperToolController>().GetReloadingState())
            {
                print("can't za warudo");
                GameObject.Find("developertoolWL(Clone)").GetComponent<DearVRSource>().DearVRPlayOneShot(GameObject.Find("developertoolWL(Clone)").GetComponent<DeveloperToolController>().notWorkingSound.clip);
            }
        }

        if (!rightHand.GetComponent<OVRHand>().GetFingerIsPinching(HandFinger.Index))
            rightIndexPinched = false;
        if (!rightHand.GetComponent<OVRHand>().GetFingerIsPinching(HandFinger.Middle))
            rightMiddlePinched = false;

        if (inCutscene) //if true, disables most player functionality. used for (pausing) menus as well
        {
            if (SceneManager.GetActiveScene().name != "Title")
            {
                rightHand.GetComponent<WeaponActivator>().weaponActivated = false;
                if (rightHand.GetComponent<WeaponActivator>().weapon != null)
                    rightHand.GetComponent<WeaponActivator>().weapon.SetActive(false);
                rightHand.GetComponent<WeaponActivator>().enabled = false;
                rightHand.GetComponentInChildren<WeaponSelector>().enabled = false;
                leftHand.GetComponent<TeleportController>().teleporterActivated = false;
                leftHand.GetComponent<TeleportController>().hitOrigin.SetActive(false);
                leftHand.GetComponent<TeleportController>().enabled = false;
                leftHand.GetComponent<ShieldController>().enabled = false;
                foreach (GameObject node in nodes)
                {
                    node.GetComponent<MeshRenderer>().enabled = false;
                }
            }
            //when menu open, pausedState = true
            if (pausedState && Time.timeScale > 0.5f)
                StartCoroutine(PausedState());
        }
        else
        {
            foreach (GameObject node in nodes)
            {
                node.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        
        if (health > 0 && !recollecting && !inCutscene)
        {
            //gameplay stuff
            if (SceneManager.GetActiveScene().name != "Title")
            {
                rightHand.GetComponent<WeaponActivator>().enabled = true;
                rightHand.GetComponentInChildren<WeaponSelector>().enabled = true;
                leftHand.GetComponent<TeleportController>().enabled = true;
                leftHand.GetComponent<ShieldController>().enabled = true;
            }
        }
        else //initiate recollect
        {
            if (startRecollect)
            {
                health = 30;
                startRecollect = false;
                recollecting = true;
                allNodesActive = true;
                //menucontroller.cs pauses game when player finally dies (in first if statement in update)
                recollectableEnemies = new List<GameObject>();
                //GameObject.FindGameObjectsWithTag("ReCollectable"); //remember to check for null enemies if there aren't any (player defeated last enemy as they "died")
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("ReCollectable"))
                {
                    if (enemy.GetComponent<EnemyController>().alive) //node will follow above enemy until they die. might have to deal with re-parenting
                        recollectableEnemies.Add(enemy);
                }

                int enemyCount = 0;
                if (recollectableEnemies.Count > 0)
                {
                    foreach (GameObject node in nodes)
                    {
                        node.GetComponent<ReCollectableController>().SetLifeBandTransform(lifeBandClone.transform);
                        node.GetComponent<ReCollectableController>().SetAliveReference(recollectableEnemies[enemyCount].GetComponent<EnemyController>());
                        node.GetComponent<ReCollectableController>().SetReCollectableMode(true);
                        if (recollectableEnemies.Count >= 4 && enemyCount < 4)
                        {
                            enemyCount++;
                        }
                        if (recollectableEnemies.Count < 4)
                        {
                            enemyCount++;
                            if (enemyCount == recollectableEnemies.Count)
                                enemyCount = 0;
                        }
                    }
                }
                print("in startRecollect loop");
            }
            //print("in else, player health < 0");
            if (recollecting)
            {
                if (allNodesActive)
                {
                    print("checking if all nodes are active");
                    allNodesActive = false;
                    foreach (GameObject node in nodes)
                    {
                        if (node.GetComponent<ReCollectableController>().GetReCollectableMode())
                        {
                            allNodesActive = true;
                            print("allnodesactive set to true");
                        }
                    }
                    if (!allNodesActive)
                        print("all nodes active = false");
                }
                else
                {
                    print("player healed");
                    recollecting = false;
                    health = 40;
                    CheckLifeBand();
                }
            }
        }

        lifeBandClone.GetComponent<ParentConstraint>().constraintActive = (/*rightHand.GetComponent<OVRHand>().IsTracked && rightHand.GetComponent<OVRHand>().GetTrackingConfidence() == OVRHand.TrackingConfidence.High*/true);
        if (lifeBandClone != null)
            lifeBandClone.GetComponent<ParentConstraint>().SetSource(0, rightHandSource);

        if (rightHand.GetComponent<OVRHand>().IsTracked && rightHand.GetComponent<OVRHand>().GetTrackingConfidence() == OVRHand.TrackingConfidence.High)
        {
            lifeBandClone.GetComponent<ParentConstraint>().enabled = true;
        }
        else if ((rightHand.GetComponent<OVRHand>().GetTrackingConfidence() == OVRHand.TrackingConfidence.Low))
        {
            lifeBandClone.GetComponent<ParentConstraint>().enabled = false;
        }

        if (!rightHand.GetComponent<OVRHand>().IsTracked)
        {
            lifeBandClone.GetComponent<ParentConstraint>().enabled = false;
        }
    }

    IEnumerator PausedState()
    {
        Time.timeScale = 0.01f;
        print("TIME: " + Time.timeScale);
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 0.0f;
        print("TIME: " + Time.timeScale);
    }

    public bool GetInCutscene()
    {
        return inCutscene;
    }

    public void SetInCutscene(bool set)
    {
        inCutscene = set;
    }

    public void SetPausedState(bool set)
    {
        pausedState = set;
    }

    public void DecreaseHealth()
    {
        health-=1;
        if (!allNodesActive)
            CheckLifeBand();
    }

    public int GetHealth()
    {
        return health;
    }

    public void AddHealth(int set)
    {
        health += set;
    }

    void SetUpLifeBand()
    {
        lifeBandClone = Instantiate(lifeBand);
        lifeBandClone.SetActive(true);
        rightHandSource = new ConstraintSource
        {
            sourceTransform = rightHand.transform,
            weight = 1
        };
        lifeBand.GetComponent<ParentConstraint>().SetSource(0, rightHandSource);
        lifeBand.GetComponent<ParentConstraint>().translationAtRest = new Vector3(-15.74f, 0f, -33.97f);
        lifeBand.GetComponent<ParentConstraint>().rotationAtRest = new Vector3(0f, 0f, 0f);
        lifeBand.GetComponent<ParentConstraint>().SetTranslationOffset(0, new Vector3(-0.02f, -0.03f, 0.02f));
        lifeBand.GetComponent<ParentConstraint>().SetRotationOffset(0, new Vector3(-20, 90, 0f));
        lifeBand.GetComponent<ParentConstraint>().locked = true;

        lifeBandNode1 = lifeBandClone.transform.GetChild(0).gameObject;
        lifeBandNode2 = lifeBandClone.transform.GetChild(1).gameObject;
        lifeBandNode3 = lifeBandClone.transform.GetChild(2).gameObject;
        lifeBandNode4 = lifeBandClone.transform.GetChild(3).gameObject;

        nodes = new GameObject[] {lifeBandNode1, lifeBandNode2, lifeBandNode3, lifeBandNode4};
    }

    public void CheckLifeBand()
    {
        if (health >= 40)
        {
            foreach (GameObject node in nodes)
            {
                node.GetComponent<MeshRenderer>().material.color = Color.cyan;
            }
        }
        if (health <= 30)
            nodes[0].GetComponent<MeshRenderer>().material.color = Color.red;
        if (health <= 20)
            nodes[1].GetComponent<MeshRenderer>().material.color = Color.red;
        if (health <= 10)
            nodes[2].GetComponent<MeshRenderer>().material.color = Color.red;
        if (health <= 0)
        {
            nodes[3].GetComponent<MeshRenderer>().material.color = Color.red;
            startRecollect = true;
        }
    }

    public int GetDeveloperToolTime()
    {
        return developerToolTime;
    }

    public bool GetDTReloading()
    {
        return DTreloading;
    }

    public void ZaWarudo()
    {
        GameObject.Find("developertoolWL(Clone)").GetComponent<DeveloperToolController>().PlayStopTimeSound();
        timeStopped = true;
        DTreloading = true;
        StartCoroutine(TokiWoTomare());
        DTMax = 7;
        developerToolTime = DTMax;
        StartCoroutine(EmptyDT());
    }
    
    IEnumerator TokiWoTomare()
    {
        if (!inCutscene)
        {
            stoptime -= 0.1f;
            Time.timeScale = stoptime;
            print("timescale: " + Time.timeScale);
            yield return new WaitForSecondsRealtime(0.2f);
            if (stoptime < 0.1f)
            {
                Time.timeScale = 0.01f;
                rightHand.GetComponent<MenuController>().ZaWarudoHelper();
                print("time has stopped, timescale: " + Time.timeScale);
                yield return new WaitForSecondsRealtime(5f);
                StartCoroutine(StartTime());
            }
            else
                StartCoroutine(TokiWoTomare());
        }
        else
        {
            yield return new WaitForSecondsRealtime(1f);
            StartCoroutine(TokiWoTomare());
        }
    }

    IEnumerator EmptyDT()
    {
        if (!inCutscene)
        {
            print("emptying dt");
            developerToolTime--;
            yield return new WaitForSecondsRealtime(1f);
            if (developerToolTime <= 0)
            {
                developerToolTime = 0;//69;
                DTMax = 30;
                StartCoroutine(ReloadDT());
            }
            else
                StartCoroutine(EmptyDT());
        }
        else
        {
            yield return new WaitForSecondsRealtime(1f);
            StartCoroutine(EmptyDT());
        }
    }

    IEnumerator ReloadDT()
    {
        if (!inCutscene)
        {
            print("reloading dt");
            developerToolTime++;
            yield return new WaitForSecondsRealtime(1f);
            if (developerToolTime > (DTMax - 1))
            {
                developerToolTime = DTMax;//69;
                DTreloading = false;
            }
            else
                StartCoroutine(ReloadDT());
        }
        else
        {
            yield return new WaitForSecondsRealtime(1f);
            StartCoroutine(ReloadDT());
        }
    }

    IEnumerator StartTime()
    {
        if (!inCutscene)
        {
            stoptime += 0.1f;
            Time.timeScale = stoptime;
            print("timescale: " + Time.timeScale);
            yield return new WaitForSecondsRealtime(0.05f);
            if (stoptime > 0.8f)
            {
                timeStopped = false;
                Time.timeScale = 1;
                stoptime = 1;
                print("time has started, timescale: " + Time.timeScale);
            }
            else
                StartCoroutine(StartTime());
        }
        else
        {
            yield return new WaitForSecondsRealtime(1f);
            StartCoroutine(StartTime());
        }
    }

    IEnumerator LoadLevel(float exposure)
    {
        exposure-=0.08f;
        RenderSettings.skybox.SetFloat("_Exposure", exposure);
        yield return new WaitForSecondsRealtime(0.01f);
        if (exposure > 1)
            StartCoroutine(LoadLevel(exposure));
        else
            RenderSettings.skybox.SetFloat("_Exposure", 1);
    }

    public IEnumerator UnLoadLevel(float exposure)
    {
        exposure+=0.08f;
        RenderSettings.skybox.SetFloat("_Exposure", exposure);
        yield return new WaitForSecondsRealtime(0.01f);
        if (exposure < 8)
            StartCoroutine(UnLoadLevel(exposure));
        else
            RenderSettings.skybox.SetFloat("_Exposure", 8);
    }
}
