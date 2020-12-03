using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DearVR;

public class GunController : MonoBehaviour
{
    public GameObject ammo;
    int clipMax;
    [SerializeField] int clip;
    float power;
    GameObject spawnPoint;
    [SerializeField] bool shoot;
    GameObject bullet;
    [SerializeField] bool shot;
    bool reloading;
    WeaponActivator weaponActivator;
    [SerializeField] bool inTrigger;
    bool pinched;

    public AudioSource PlayerShootSound;

    // Start is called before the first frame update
    void Awake()
    {
        clipMax = 10;
        clip = clipMax;
        print("on awake, clip size: " + clip);
        power = 15f;
        spawnPoint = transform.GetChild(0).gameObject;
        shoot = false;
        shot = false;
        reloading = false;
        weaponActivator = GameObject.Find("RightHand").GetComponent<WeaponActivator>();
        inTrigger = false;
        pinched = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponActivator.hand.GetFingerIsPinching(OVRHand.HandFinger.Middle) && weaponActivator.weaponActivated && !pinched && !reloading)
        {
            pinched = true;
            StartCoroutine(ReloadClip());
        }

        if (!weaponActivator.hand.GetFingerIsPinching(OVRHand.HandFinger.Middle))
            pinched = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            shoot = false;
            inTrigger = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("this tag is being hit: " + other.tag);
        //if collides with enemy, fire ammo that with collide with particles
        if (other.tag == "Enemy"/* && !shooting*/) //might switch to particle tag on individual particles later
        {
            inTrigger = true;
            print("gun collider hit enemy, will shoot");
            shoot = true;
            if (!shot && !reloading)
                StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        print("shooting");
        print("clip size before shoot(): " + clip);
        while (shoot && !shot && clip > 0)
        {
            shot = true;
            GetComponent<DearVRSource>().DearVRPlayOneShot(PlayerShootSound.clip);
            bullet = Instantiate(ammo, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
            Rigidbody rBody = bullet.GetComponent<Rigidbody>();
            rBody.AddForce(spawnPoint.transform.up.normalized * power, ForceMode.Impulse);
            print("1 player bullet shot");
            clip--;
            yield return new WaitForSeconds(0.2f);
            shot = false;
        }
    }

    private void OnDisable()
    {
        shot = false;
    }

    public IEnumerator ReloadClip()
    {
        reloading = true;
        shoot = false;
        shot = false;
        yield return new WaitForSeconds(1.5f);
        clip = clipMax;
        if (inTrigger)
        {
            print("gun collider in trigger after reload, start shooting again");
            shoot = true;
            StartCoroutine(Shoot());
        }
        reloading = false;
    }

    public bool GetReloadingState()
    {
        return reloading;
    }

    public bool GetShotState()
    {
        return shot;
    }

    public int GetMaxAmmo()
    {
        return clipMax;
    }

    public int GetClip()
    {
        return clip;
    }
}
