using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : OVRHand
{
    public LayerMask mask;
    LineRenderer lR;
    LineRenderer lRRaycast;
    public GameObject hitOrigin;
    Vector3 hitOriginPos;
    Quaternion hitOriginRot;

    OVRHand hand;
    [SerializeField] bool pinched;
    Vector3 currentPointerPosition;
    Transform tempParent;
    public bool pointerActivated;
    Vector3 teleportPoint;
    bool teleported;
    bool firstPinch;
    bool lastPinch;

    Player player;
    GameObject quickmc;
    Text mt;
    Vector3 selectedHitPointButtonPos;
    Vector3 selectedHitPointHandPos;
    string selectedButton;
    bool pinching;
    [SerializeField]
    bool stopTracking;
    bool loadTitle;
    bool menuFix;

    private void Awake()
    {
        //this.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        lR = GetComponent<LineRenderer>();
        lR.SetWidth(0.01f, 0.01f);
        lR.material.color = Color.white;
        lR.enabled = false;

        lRRaycast = transform.GetChild(3).GetComponent<LineRenderer>();
        lRRaycast.SetWidth(0.03f, 0.03f);
        lRRaycast.material.color = Color.blue;
        lRRaycast.enabled = false;

        hitOriginPos = new Vector3(-0.22f, -0.06f, 0);
        hitOriginRot = new Quaternion(0, -1f, 1f, 1);

        hand = GetComponent<OVRHand>();
        pinched = false;
        tempParent = null;
        teleported = false;
        firstPinch = false;
        lastPinch = false;

        player = GameObject.Find("OVRCameraRig").GetComponent<Player>();
        quickmc = GameObject.Find("QuickMenu");
        quickmc.gameObject.SetActive(false);
        if (SceneManager.GetActiveScene().name == "TestScene")
            mt = GameObject.Find("mt").GetComponent<Text>();
        selectedHitPointHandPos = hitOriginPos;
        pinching = false;
        stopTracking = false;
        loadTitle = false;
        menuFix = false;
    }

    public void ZaWarudoHelper()
    {
        //pinched = true;
        //stopTracking = true;
        //player.SetInCutscene(true);
        //player.SetPausedState(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Title" && !loadTitle)
        {
            loadTitle = true;
            StartCoroutine(OpenTitleMenu());
            //player.SetPausedState(true);
        }

        IEnumerator OpenTitleMenu()
        {
            yield return new WaitForSeconds(0.01f);
            pointerActivated = true;
            pinched = true;
            player.SetInCutscene(true);
        }

        //when index pinched, show weapon and stop pinched var from constantly being toggled, sets weapon parent to player
        if ((hand.GetFingerIsPinching(HandFinger.Middle) == true && hand.IsSystemGestureInProgress && !pinched /*&& teleported == false && firstPinch == false && !lastPinch*/) || (player.GetHealth() <= 0 && player.recollecting)) //shouldn't be at 0 and recollecting, otherwise i've died twice (game over)
        {
            selectedButton = "bRetryLevel";
            menuFix = false;
            pointerActivated = true;
            pinched = true;
            player.SetInCutscene(true);
            player.SetPausedState(true);
            //firstPinch = true;
        }

        /*if (hand.GetFingerIsPinching(HandFinger.Index) == true /*&& pinched == true && firstPinch == false)
        {
            //teleport code
            if (teleportPoint != null && !teleported && lR.GetPosition(1) != lR.GetPosition(0))
            {
                GameObject.Find("OVRCameraRig").transform.position = new Vector3(teleportPoint.x, GameObject.Find("OVRCameraRig").transform.position.y, teleportPoint.z);
                teleported = true;
                lR.SetPosition(1, lR.GetPosition(0));
            }
            pinched = false;
            pointerActivated = false;
            lastPinch = true;
        }*/

        //only toggle teleported var off when not pinching
        if (hand.GetFingerIsPinching(HandFinger.Index) == false)
        {
            pinching = false;
        }

        /*if (hand.GetFingerIsPinching(HandFinger.Index) == false)
        {
            if (pointerActivated)
                firstPinch = false;
            else
                lastPinch = false;
        }*/

        if (stopTracking && SceneManager.GetActiveScene().name != "Title")
            quickmc.GetComponent<ParentConstraint>().enabled = false;

        if (pointerActivated && !stopTracking)
        {
            hitOrigin.SetActive(true);
            lR.enabled = true;
            lRRaycast.enabled = true;
            quickmc.GetComponent<ParentConstraint>().enabled = true;
            quickmc.SetActive(true);
            stopTracking = true;
        }
        else if (!stopTracking && player.GetHealth() > 0 && !player.timeStopped/* && !player.GetPausedState()*/)
        {
            Time.timeScale = 1f;

            hitOrigin.SetActive(false);
            lR.SetPosition(1, lR.GetPosition(0));
            lR.enabled = false;
            lRRaycast.enabled = false;
            quickmc.SetActive(false);
        }
        else if (!stopTracking && player.GetHealth() > 0)
        {
            hitOrigin.SetActive(false);
            lR.SetPosition(1, lR.GetPosition(0));
            lR.enabled = false;
            lRRaycast.enabled = false;
            quickmc.SetActive(false);
        }

        //print("point act: " + pointerActivated + "\nhand tracked: " + hand.IsTracked + "\npinched: " + pinched);

        //weapon location only being tracked when hand = tracked and weapon = shown and moving weapon to hand when hand tracking confidence is high (hand model visible)
        if (pointerActivated && hand.IsTracked && hand.GetTrackingConfidence() == OVRHand.TrackingConfidence.High && pinched)
        {
            lR.enabled = true;
            lRRaycast.enabled = true;
            currentPointerPosition = hitOrigin.transform.position;
            hitOrigin.transform.SetParent(gameObject.transform.parent, true);
            //weapon.transform.localPosition = meleePosOffset;
            //weapon.transform.localRotation = weaponRotOffset;
            hitOrigin.transform.localPosition = Vector3.Lerp(hitOrigin.transform.localPosition, hitOriginPos, 15f * Time.unscaledDeltaTime);
            hitOrigin.transform.localRotation = Quaternion.Lerp(hitOrigin.transform.localRotation, hitOriginRot, 15f * Time.unscaledDeltaTime);

            Vector3 originPosition = hitOrigin.transform.position;
            Ray ray = new Ray(originPosition, hitOrigin.transform.forward);
            Vector3 endPosition = originPosition + (10000 * hitOrigin.transform.forward);
            //endPosition = GameObject.Find("bCloseMenu").transform.position;
            lR.SetPosition(0, originPosition);

            if (Vector3.Distance(currentPointerPosition, selectedHitPointHandPos) < 1f)
            {
                lR.SetPosition(1, selectedHitPointButtonPos);
            }
            else
                lR.SetPosition(1, endPosition);

            lRRaycast.SetPosition(0, originPosition);
            lRRaycast.SetPosition(1, endPosition);

            RaycastHit raycastHit;
            if (!menuFix)
                selectedButton = "bCloseMenu";
            if (Physics.Raycast(ray, out raycastHit, 6, mask))
            {
                selectedHitPointButtonPos = raycastHit.transform.position;
                if (Vector3.Distance(currentPointerPosition, selectedHitPointHandPos) > 1f)
                    selectedHitPointHandPos = hitOrigin.transform.position;
                float lrLength = Vector3.Distance(originPosition, raycastHit.point);
                endPosition = raycastHit.collider.transform.position;
                //lR.SetPosition(1, endPosition/2);
                //lR.SetPosition(1, new Vector3(lR.GetPosition(1).x, lR.GetPosition(1).y + 0.5f, lR.GetPosition(1).z));
                lR.SetPosition(1, endPosition);
                if (SceneManager.GetActiveScene().name == "TestScene")
                    mt.text = "" + selectedButton;
                selectedButton = raycastHit.transform.name;
                menuFix = true;
                //quickmc.GetComponent<QuickMenuController>().selectLevel.transform.GetChild(0).GetComponent<Text>().text = "" + selectedButton;
            }
            /*else
            {
                //lR.SetPosition(1, originPosition);
                mt.text = "nothing hit";
            }*/

            //quick menu options
            if (hand.GetFingerIsPinching(HandFinger.Index) && !pinching)
            {
                print("selectedButton: " + selectedButton);
                pinching = true;
                if (SceneManager.GetActiveScene().name == "Title")
                {
                    switch (selectedButton)
                    {
                        case "bNewGame":
                            {
                                StartCoroutine(player.UnLoadLevel(1));
                                quickmc.GetComponent<QuickMenuController>().LoadNewGame();
                                selectedButton = "";
                                break;
                            }
                        case "bSelectLevel":
                            {
                                quickmc.GetComponent<QuickMenuController>().OpenSelectLevel();
                                selectedButton = "";
                                break;
                            }
                        case "bLevel1":
                            {
                                StartCoroutine(player.UnLoadLevel(1));
                                quickmc.GetComponent<QuickMenuController>().LoadLevel1();
                                selectedButton = "";
                                break;
                            }
                        case "bLevel2":
                            {
                                StartCoroutine(player.UnLoadLevel(1));
                                quickmc.GetComponent<QuickMenuController>().LoadLevel2();
                                selectedButton = "";
                                selectedButton = "";
                                break;
                            }
                        case "bBackToMainMenu":
                            {
                                quickmc.GetComponent<QuickMenuController>().BackToMainMenu();
                                selectedButton = "";
                                break;
                            }
                        case "bGameOptions":
                            {
                                //quickmc.GetComponent<QuickMenuController>().OpenGameOptions();
                                selectedButton = "";
                                break;
                            }
                        case "bName":
                            {
                                quickmc.GetComponent<QuickMenuController>().OpenNameInput();
                                selectedButton = "";
                                break;
                            }
                        case "a":
                        case "b":
                        case "c":
                        case "d":
                        case "e":
                        case "f":
                        case "g":
                        case "h":
                        case "i":
                        case "j":
                        case "k":
                        case "l":
                        case "m":
                        case "n":
                        case "o":
                        case "p":
                        case "q":
                        case "r":
                        case "s":
                        case "t":
                        case "u":
                        case "v":
                        case "w":
                        case "x":
                        case "y":
                        case "z":
                            {
                                quickmc.GetComponent<QuickMenuController>().InsertStringInput(selectedButton);
                                quickmc.GetComponent<QuickMenuController>().HighlightBtn(selectedButton);
                                selectedButton = "";
                                break;
                            }
                        case "0":
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                        case "9":
                            {
                                quickmc.GetComponent<QuickMenuController>().InsertNumberInput(selectedButton);
                                quickmc.GetComponent<QuickMenuController>().HighlightBtn(selectedButton);
                                selectedButton = "";
                                break;
                            }
                        case "Space":
                            {
                                quickmc.GetComponent<QuickMenuController>().InsertSpace();
                                quickmc.GetComponent<QuickMenuController>().HighlightBtn(selectedButton);
                                selectedButton = "";
                                break;
                            }
                        case "Delete":
                            {
                                quickmc.GetComponent<QuickMenuController>().DeleteCharacter();
                                quickmc.GetComponent<QuickMenuController>().HighlightBtn(selectedButton);
                                selectedButton = "";
                                break;
                            }
                        case "Shift":
                            {
                                quickmc.GetComponent<QuickMenuController>().ShiftCharacter();
                                //quickmc.GetComponent<QuickMenuController>().HighlightBtn(selectedButton);
                                selectedButton = "";
                                break;
                            }
                        case "bNameSubmit":
                            {
                                quickmc.GetComponent<QuickMenuController>().SubmitName();
                                //quickmc.GetComponent<QuickMenuController>().HighlightBtn(selectedButton);
                                selectedButton = "";
                                break;
                            }
                    }
                }
                else if (SceneManager.GetActiveScene().name != "Title")
                {
                    switch (selectedButton)
                    {
                        case "bRetryLevel":
                            {
                                StartCoroutine(player.UnLoadLevel(1));
                                quickmc.GetComponent<QuickMenuController>().ReloadScene();
                                selectedButton = "";
                                break;
                            }
                        case "bCloseMenu":
                            {
                                if (player.GetHealth() > 0)
                                {
                                    pointerActivated = false;
                                    pinched = false;
                                    stopTracking = false;
                                    player.SetInCutscene(false);
                                    gameObject.GetComponent<WeaponActivator>().SetPinched(true);
                                    player.SetPausedState(false);
                                }
                                selectedButton = "";
                                break;
                            }
                        case "bMainMenu":
                            {
                                quickmc.GetComponent<QuickMenuController>().LoadMainMenu();
                                selectedButton = "";
                                break;
                            }
                        case "bGameOptions":
                            {
                                //quickmc.GetComponent<QuickMenuController>().OpenGameOptions();
                                selectedButton = "";
                                break;
                            }
                    }
                }
            }
        }
        else if (hand.GetTrackingConfidence() == OVRHand.TrackingConfidence.Low) //hand model invisible
        {
            hitOrigin.transform.SetParent(tempParent, true);
            lR.enabled = false;
            lRRaycast.enabled = false;
        }

        //set weapon location to last hand tracked location when hand = not tracked and weapon = shown (weapon stays in place when hand derps out), sets parent of weapon to outside player
        if (!hand.IsTracked && pointerActivated/* Input.GetMouseButtonDown(1)*/)
        {
            hitOrigin.transform.position = currentPointerPosition;
            hitOrigin.transform.SetParent(tempParent, true);
            lR.enabled = false;
            lRRaycast.enabled = false;
        }

        //when hand = (not) tracked and weapon = hidden, set weapon to hand
        if (pointerActivated == false && (!hand.IsTracked || hand.IsTracked))
        {
            hitOrigin.transform.localPosition = hitOriginPos;
            hitOrigin.transform.localRotation = hitOriginRot;
            //weapon.transform.SetParent(tempParent, true);
        }
    }
}
