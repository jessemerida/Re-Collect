using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DearVR;

public class DeveloperToolController : MonoBehaviour
{
    Player player;
    int time = 0;
    WeaponActivator weaponActivator;
    [SerializeField] bool pinched;
    [SerializeField] bool reloading;

    public AudioSource timeStopSound;
    public AudioSource notWorkingSound;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        weaponActivator = GameObject.Find("RightHand").GetComponent<WeaponActivator>();
        pinched = false;
    }

    // Update is called once per frame
    void Update()
    {
        time = player.GetDeveloperToolTime();
        reloading = player.GetDTReloading();

        if (weaponActivator.hand.GetFingerIsPinching(OVRHand.HandFinger.Middle) && weaponActivator.weaponActivated && !pinched && !reloading)
        {
            pinched = true;
            player.ZaWarudo();
        }

        if (!weaponActivator.hand.GetFingerIsPinching(OVRHand.HandFinger.Middle))
            pinched = false;

        #region testing
        if (!pinched && !reloading && Input.GetKeyDown(KeyCode.Z))
        {
            print("ZA WARUDOOO!");
            pinched = true;
            player.ZaWarudo();
        }

        if (Input.GetKeyUp(KeyCode.Z))
            pinched = false;
        #endregion
    }

    public bool GetReloadingState()
    {
        return reloading;
    }

    public void PlayStopTimeSound()
    {
        GetComponent<DearVRSource>().DearVRPlayOneShot(timeStopSound.clip);
    }
}