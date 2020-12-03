using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : MonoBehaviour
{
    OVRHand hand;

    private void Awake()
    {
        this.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        hand = GameObject.Find("RightHand").GetComponent<OVRHand>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = hand.PointerPose.rotation;
    }
}
