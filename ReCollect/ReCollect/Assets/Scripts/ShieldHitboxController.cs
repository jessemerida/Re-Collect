using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DearVR;

public class ShieldHitboxController : MonoBehaviour
{
    [SerializeField] bool shot;
    ShieldController shield;

    public AudioSource hit;
    public AudioSource pieceBroken;
    public AudioSource allBroken;

    // Start is called before the first frame update
    void Start()
    {
        shot = false;
        shield = GameObject.Find("LeftHand").GetComponent<ShieldController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyAmmo"/* && !shot*/)
        {
            shot = true;
            shield.DecreaseHealth();
            GetComponent<DearVRSource>().DearVRPlayOneShot(hit.clip);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "EnemyAmmo")
            shot = false;
    }

    public void PieceBroken()
    {
        GetComponent<DearVRSource>().DearVRPlayOneShot(pieceBroken.clip);
    }

    public void AllBroken()
    {
        GetComponent<DearVRSource>().DearVRPlayOneShot(allBroken.clip);
    }
}
