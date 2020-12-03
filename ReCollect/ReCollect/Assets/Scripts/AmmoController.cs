using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DearVR;

public class AmmoController : MonoBehaviour
{
    public AudioSource aiHitSound;
    public AudioSource playerHitSound;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shield" && gameObject.name == "EnemyAmmo(Clone)")
        {
            print("enemy ammo hit shield");
            Destroy(gameObject);
        }

        if (other.tag == "PlayerHitbox" && gameObject.name == "EnemyAmmo(Clone)")
        {
            print("enemy ammo hit player");
            GetComponent<DearVRSource>().DearVRPlayOneShot(playerHitSound.clip);
            //Destroy(gameObject);
        }

        if (other.tag == "FriendlyHitboxParticle" && gameObject.name == "EnemyAmmo(Clone)")
        {
            print("enemy ammo hit friendly");
            GetComponent<DearVRSource>().DearVRPlayOneShot(aiHitSound.clip);
            //Destroy(gameObject);
        }

        if (other.tag == "EnemyHitboxParticle" && gameObject.name == "FriendlyAmmo(Clone)")
        {
            print("friendly ammo hit enemy");
            GetComponent<DearVRSource>().DearVRPlayOneShot(aiHitSound.clip);
            //Destroy(gameObject);
        }

        if (other.tag == "EnemyHitboxParticle" && gameObject.tag == "PlayerAmmo")
        {
            GetComponent<DearVRSource>().DearVRPlayOneShot(aiHitSound.clip);
        }
    }
}
