using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DearVR;

public class TeleportController : OVRHand
{
    public LayerMask mask;
    LineRenderer lR;

    GameObject rayInfoText;
    public GameObject hitOrigin;
    Vector3 hitOriginPos;
    Quaternion hitOriginRot;

    OVRHand hand;
    bool pinched;
    Vector3 currentTeleporterPosition;
    Transform tempParent;
    public bool teleporterActivated;
    Vector3 teleportPoint;
    bool teleported;
    bool firstPinch;
    bool lastPinch;

    public AudioSource teleportSound;

    private void Awake()
    {
        //this.enabled = false;
        if (SceneManager.GetActiveScene().name == "Title")
            this.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        lR = GetComponent<LineRenderer>();
        lR.SetWidth(0.1f, 0.1f);
        lR.material.color = Color.magenta;
        lR.enabled = false;

        rayInfoText = GameObject.Find("raytext");
        hitOriginPos = new Vector3(0.1f, 0.05f, 0);
        hitOriginRot = new Quaternion(-14.28f, -15, -20, 1);

        hand = GetComponent<OVRHand>();
        pinched = false;
        tempParent = null;
        teleported = false;
        firstPinch = false;
        lastPinch = false;
    }

    // Update is called once per frame
    void Update()
    {
        #region old reference stuff
        /*Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.forward;
        Vector3 endpoint = origin + direction * 100000;

        RaycastHit hit;
        lR.SetPosition(0, origin);
        if (Physics.Raycast(origin, fwd, out hit, Mathf.Infinity))
        {
            lR.SetPosition(1, hit.point);
        }
        else
        {
            lR.SetPosition(1, origin);
        }*/

        /*RaycastHit hit;
        lR.SetPosition(0, hitOrigin.transform.position);
        if (Physics.Raycast(hitOrigin.transform.position, hitOrigin.transform.forward, out hit, Mathf.Infinity))
        {
            //lR.SetPosition(1, hit.point);
            rayInfoText.GetComponent<Text>().text = "raycast info: \n" + "name: " + hit.transform.name;
        }
        else
            rayInfoText.GetComponent<Text>().text = "raycast info: nothing hit";*/

        ///////////////
        /*if (hand.GetTrackingConfidence() == OVRHand.TrackingConfidence.High)
        {
            Ray ray = new Ray(hitOrigin.transform.position, hitOrigin.transform.forward);
            Vector3 endPosition = hitOrigin.transform.position + (10000 * hitOrigin.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, mask))
            {
                endPosition = raycastHit.point;
            }
            lR.SetPosition(0, hitOrigin.transform.position);
            lR.SetPosition(1, endPosition);
        }*/
        ///////////////
        #endregion
        //when index pinched, show weapon and stop pinched var from constantly being toggled, sets weapon parent to player
        if (hand.GetFingerIsPinching(HandFinger.Index) == true && pinched == false && teleported == false && firstPinch == false && !lastPinch)
        {
            teleporterActivated = true;
            pinched = true;
            firstPinch = true;
        }

        if (hand.GetFingerIsPinching(HandFinger.Index) == true && pinched == true && firstPinch == false)
        {
            //teleport code
            if (teleportPoint != null && !teleported && lR.GetPosition(1) != lR.GetPosition(0))
            {
                GameObject.Find("OVRCameraRig").transform.position = new Vector3(teleportPoint.x, GameObject.Find("OVRCameraRig").transform.position.y, teleportPoint.z);
                teleported = true;
                lR.SetPosition(1, lR.GetPosition(0));
                hitOrigin.GetComponent<DearVRSource>().DearVRPlayOneShot(hitOrigin.GetComponent<AudioSource>().clip);
            }
            pinched = false;
            teleporterActivated = false;
            lastPinch = true;
        }

        //only toggle teleported var off when not pinching
        if (hand.GetFingerIsPinching(HandFinger.Index) == false)
        {
            teleported = false;
        }

        if (hand.GetFingerIsPinching(HandFinger.Index) == false)
        {
            if (teleporterActivated)
                firstPinch = false;
            else
                lastPinch = false;
        }

        if (teleporterActivated)
        {
            hitOrigin.SetActive(true);
            lR.enabled = true;
        }
        else //hide weapon
        {
            hitOrigin.SetActive(false);
            lR.enabled = false;
        }

        //weapon location only being tracked when hand = tracked and weapon = shown and moving weapon to hand when hand tracking confidence is high (hand model visible)
        if (teleporterActivated && hand.IsTracked && hand.GetTrackingConfidence() == OVRHand.TrackingConfidence.High && pinched)
        {
            lR.enabled = true;
            currentTeleporterPosition = hitOrigin.transform.position;
            hitOrigin.transform.SetParent(gameObject.transform.parent, true);
            //weapon.transform.localPosition = meleePosOffset;
            //weapon.transform.localRotation = weaponRotOffset;
            hitOrigin.transform.localPosition = Vector3.Lerp(hitOrigin.transform.localPosition, hitOriginPos, 15f * Time.deltaTime);
            hitOrigin.transform.localRotation = Quaternion.Lerp(hitOrigin.transform.localRotation, hitOriginRot, 15f * Time.deltaTime);

            Vector3 originPosition = hitOrigin.transform.position;
            Ray ray = new Ray(originPosition, hitOrigin.transform.forward);
            Vector3 endPosition = originPosition + (10000 * hitOrigin.transform.forward);
            lR.SetPosition(0, originPosition);
            lR.SetPosition(1, originPosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 6, mask))
            {
                float lrLength = Vector3.Distance(originPosition, raycastHit.point);
                /*if (lrLength < 2)
                    lR.material.color = new Color(255, 255, 255);
                else if (lrLength < 4)
                    lR.material.color = new Color(0, 255, 255);
                else
                    lR.material.color = new Color(0, 0, 255);*/
                endPosition = raycastHit.point;
                //rayInfoText.GetComponent<Text>().text = "raycast info: \n" + "name: " + raycastHit.transform.name;
                //lR.SetPosition(1, endPosition/2);
                //lR.SetPosition(1, new Vector3(lR.GetPosition(1).x, lR.GetPosition(1).y + 0.5f, lR.GetPosition(1).z));
                lR.SetPosition(1, endPosition);
                teleportPoint = raycastHit.point;
            }
            else
            {
                //rayInfoText.GetComponent<Text>().text = "raycast info: nothing hit";
                lR.SetPosition(1, originPosition);
            }
            
        }
        else if (hand.GetTrackingConfidence() == OVRHand.TrackingConfidence.Low) //hand model invisible
        {
            hitOrigin.transform.SetParent(tempParent, true);
            lR.enabled = false;
        }

        //set weapon location to last hand tracked location when hand = not tracked and weapon = shown (weapon stays in place when hand derps out), sets parent of weapon to outside player
        if (!hand.IsTracked && teleporterActivated/* Input.GetMouseButtonDown(1)*/)
        {
            hitOrigin.transform.position = currentTeleporterPosition;
            hitOrigin.transform.SetParent(tempParent, true);
            lR.enabled = false;
        }

        //when hand = (not) tracked and weapon = hidden, set weapon to hand
        if (teleporterActivated == false && (!hand.IsTracked || hand.IsTracked))
        {
            hitOrigin.transform.localPosition = hitOriginPos;
            hitOrigin.transform.localRotation = hitOriginRot;
            //weapon.transform.SetParent(tempParent, true);
        }
/*
        if (lR.GetPosition(1) != lR.GetPosition(0))
            rayInfoText.GetComponent<Text>().text = "tp line showing";
        else
            rayInfoText.GetComponent<Text>().text = "tp line not showing";*/
    }
}