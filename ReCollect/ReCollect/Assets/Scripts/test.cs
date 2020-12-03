using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    GameObject obj;

    private void Awake()
    {
        //this.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.Find("CenterEyeAnchor");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = new Quaternion(0, obj.transform.rotation.y, 0, obj.transform.rotation.w);
    }
}
