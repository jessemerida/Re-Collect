using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitboxController : MonoBehaviour
{
    bool shot;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        shot = false;
        player = transform.parent.GetComponent<Player>();
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
            player.DecreaseHealth();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "EnemyAmmo")
            shot = false;
    }
}
