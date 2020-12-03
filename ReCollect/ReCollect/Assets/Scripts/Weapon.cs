using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string type;
    public string weaponName;
    public float cooldown; //cooldowns might have to be tied to weapons list instead of weapons themselves, as it would reset when weapon gets changed. might not be implemented either, since hand tracking is derpy and i don't want the player to have two "handicaps"

    private void Awake()
    {
        //this.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
