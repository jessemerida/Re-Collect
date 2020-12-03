using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class test5 : MonoBehaviour
{
    GameObject rH;

    // Start is called before the first frame update
    void Start()
    {
        rH = GameObject.Find("RightHand");

        ConstraintSource rightHand = new ConstraintSource();
        rightHand.sourceTransform = GameObject.Find("RightHand").transform;
        rightHand.weight = 0;
        GetComponent<ParentConstraint>().AddSource(rightHand);
        GetComponent<ParentConstraint>().translationAtRest = new Vector3(-0.24f, -0.024f, -0.019f);
        GetComponent<ParentConstraint>().rotationAtRest = new Vector3(24.274f, 6.538f, 108.256f);
        GetComponent<ParentConstraint>().locked = true;
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<ParentConstraint>().constraintActive = (rH.GetComponent<OVRHand>().IsTracked && rH.GetComponent<OVRHand>().GetTrackingConfidence() == OVRHand.TrackingConfidence.High);
    }
}