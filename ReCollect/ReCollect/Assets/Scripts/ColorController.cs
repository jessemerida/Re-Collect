using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ColorController : OVRHand 
{
    OVRHand hand;
    Hand handType;
    GameObject floor;
    bool isPinkyFingerPinching;
    bool isRingFingerPinching;
    bool isMiddleFingerPinching;
    bool isIndexFingerPinching;
    bool isPinkyFingerPinchingLeft;
    bool isRingFingerPinchingLeft;
    bool isMiddleFingerPinchingLeft;
    bool isIndexFingerPinchingLeft;
    Text isPinching;
    Text whichHandType;
    Text systempinch;
    Text isPinchingLeft;
    Text whichHandTypeLeft;
    Text systempinchleft;

    Text popText;
    //Text mt;

    private void Awake()
    {
        //this.enabled = false;
        if (SceneManager.GetActiveScene().name != "TestScene")
            this.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        hand = GetComponent<OVRHand>();
        handType = hand.GetHandType();
        floor = GameObject.Find("Floor");
        isPinching = GameObject.Find("isPinching").GetComponent<Text>();
        whichHandType = GameObject.Find("whichHandType").GetComponent<Text>();
        systempinch = GameObject.Find("systempinch").GetComponent<Text>();
        isPinchingLeft = GameObject.Find("isPinchingLeft").GetComponent<Text>();
        whichHandTypeLeft = GameObject.Find("whichHandTypeLeft").GetComponent<Text>();
        systempinchleft = GameObject.Find("systempinchleft").GetComponent<Text>();
        popText = GameObject.Find("poptext").GetComponent<Text>();
        //mt = GameObject.Find("mt").GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        isPinkyFingerPinching = hand.GetFingerIsPinching(HandFinger.Pinky);
        isRingFingerPinching = hand.GetFingerIsPinching(HandFinger.Ring);
        isMiddleFingerPinching = hand.GetFingerIsPinching(HandFinger.Middle);
        isIndexFingerPinching = hand.GetFingerIsPinching(HandFinger.Index);

        isPinkyFingerPinchingLeft = hand.GetFingerIsPinching(HandFinger.Pinky);
        isRingFingerPinchingLeft = hand.GetFingerIsPinching(HandFinger.Ring);
        isMiddleFingerPinchingLeft = hand.GetFingerIsPinching(HandFinger.Middle);
        isIndexFingerPinchingLeft = hand.GetFingerIsPinching(HandFinger.Index);

        switch (handType)
        {
            case Hand.HandRight:
                {
                    whichHandType.text = "right hand tracked: " + hand.IsTracked + "\ntracking confidence: " + hand.GetTrackingConfidence();

                    if (isPinkyFingerPinching)
                    {
                        isPinching.text = "pinky: " + isPinkyFingerPinching;
                        //floor.GetComponent<MeshRenderer>().material.color = new Color(255, 0, 0, 10);
                    }
                    else if (isRingFingerPinching)
                    {
                        isPinching.text = "ring: " + isRingFingerPinching;
                        //floor.GetComponent<MeshRenderer>().material.color = new Color(0, 255, 0, 10);
                    }
                    else if (isMiddleFingerPinching)
                    {
                        isPinching.text = "middle: " + isMiddleFingerPinching;
                        //floor.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 255, 10);
                    }
                    else if (isIndexFingerPinching)
                    {
                        isPinching.text = "index: " + isIndexFingerPinching;
                        //floor.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 10);
                    }
                    else
                        isPinching.text = "nothing pinching";

                    if (hand.IsSystemGestureInProgress)
                    {
                        systempinch.text = "system gesture on";
                        transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
                    }
                    else
                    {
                        systempinch.text = "system gesture off";
                        transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Pause();
                        transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Clear();
                    }

                    popText.text = "right hand:\nIsPointerPoseValid" + hand.IsPointerPoseValid + ", PointerPose" + hand.PointerPose.localPosition;
                    break;
                }
            case Hand.HandLeft:
                {
                    whichHandTypeLeft.text = "left hand tracked: " + hand.IsTracked + "\ntracking confidence: " + hand.GetTrackingConfidence();

                    if (isPinkyFingerPinchingLeft)
                    {
                        isPinchingLeft.text = "pinky: " + isPinkyFingerPinching;
                    }
                    else if (isRingFingerPinchingLeft)
                    {
                        isPinchingLeft.text = "ring: " + isRingFingerPinching;
                    }
                    else if (isMiddleFingerPinchingLeft)
                    {
                        isPinchingLeft.text = "middle: " + isMiddleFingerPinching;
                    }
                    else if (isIndexFingerPinchingLeft)
                    {
                        isPinchingLeft.text = "index: " + isIndexFingerPinching;
                    }
                    else
                        isPinchingLeft.text = "nothing pinching";

                    if (hand.IsSystemGestureInProgress)
                    {
                        systempinchleft.text = "system gesture on";
                        transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Play();
                    }
                    else
                    {
                        systempinchleft.text = "system gesture off";
                        transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Pause();
                        transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().Clear();
                    }
                    break;
                }
        }

        
    }
}
