using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class FriendlyGunController : MonoBehaviour
{
    public GameObject ammo;
    float power;
    public GameObject spawnPoint;
    [SerializeField] bool shoot;
    GameObject bullet;
    [SerializeField] bool shot;
    [SerializeField] bool shootPause;
    Vector3 lookAt;

    // Start is called before the first frame update
    void Start()
    {
        power = 15f;
        shot = false;
        shootPause = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (shoot && !shot)
        {
            if (shootPause)
                StartCoroutine(ShootPause());
            else
                StartCoroutine(Shoot());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.tag == "Enemy")
        //    shoot = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        /*print("this tag is being hit: " + other.tag);
        //if collides with enemy, fire ammo that with collide with particles
        if (other.tag == "Enemy"/* && !shooting) //might switch to particle tag on individual particles later
        {
            print("gun collider hit enemy, will shoot");
            shoot = true;
            if (!shot)
                StartCoroutine(Shoot());
        }*/
    }

    IEnumerator Shoot()
    {
        while (shoot)
        {
            shot = true;
            //GetComponent<FriendlyController>().weapon.GetComponent<ParentConstraint>().constraintActive = false;
            //GetComponent<FriendlyController>().weapon.transform.LookAt(lookAt, new Vector3(100000, -10000, -1));
            bullet = Instantiate(ammo, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
            Rigidbody rBody = bullet.GetComponent<Rigidbody>();
            rBody.AddForce(spawnPoint.transform.forward.normalized * power, ForceMode.Impulse);
            print("1 bullet shot");
            yield return new WaitForSeconds(0.634f);
            shot = false;
        }
    }

    IEnumerator ShootPause()
    {
        yield return new WaitForSeconds(0.5f);
        shootPause = false;
    }

    private void OnDisable()
    {
        shot = false;
    }

    public void SetShoot(bool set, Vector3 set2)
    {
        shoot = set;
        lookAt = set2;
        if (shoot == false)
            shootPause = true;
    }
}
