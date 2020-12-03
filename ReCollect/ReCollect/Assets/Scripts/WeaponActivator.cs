using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WeaponActivator : OVRHand
{
    public bool weaponSelected;

    public OVRHand hand;
    public GameObject selectedWeapon;
    public GameObject weapon;

    public bool weaponActivated;
    GameObject weaponActivatedText;
    [SerializeField] bool pinched;

    Vector3 currentWeaponLocation;
    Transform tempParent;

    Vector3 meleePosOffset;
    Quaternion meleeRotOffset;

    Vector3 gunPosOffset;
    Quaternion gunRotOffset;

    Vector3 dtPosOffset;
    Quaternion dtRotOffset;

    public GameObject ReloadHUD;
    public GameObject SelectHUD;

    private void Awake()
    {
        //this.enabled = false;
        if (SceneManager.GetActiveScene().name == "Title")
            this.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        hand = GetComponent<OVRHand>();
        //weapon = GameObject.Find("weapon");
        weaponActivatedText = GameObject.Find("watext");

        weaponSelected = false;

        /*if (weaponSelected)
        {
            weapon = Instantiate(selectedWeapon, meleePosOffset, weaponRotOffset);

            weapon.SetActive(false);
            weaponActivated = false;
            meleePosOffset = new Vector3(-0.095f, -0.03500003f, -0.02900004f); //will probably need different offsets for different weapon types
            weaponRotOffset = new Quaternion(91.427f, -93.31799f, 49.89999f, 0f);
            weapon.transform.position = meleePosOffset;
            weapon.transform.localRotation = weaponRotOffset;
        }*/

        pinched = false;
        //tempParent = GameObject.Find("Floor").transform;
        tempParent = null;
    }

    // Update is called once per frame
    void Update() //mouse button code for testing w/o building to quest
    {
        if (weaponSelected)
        {
            //////////weapon type: sword
            if (weapon.GetComponent<Weapon>().type == "Melee")
            {
                //when index pinched, show weapon and stop pinched var from constantly being toggled, sets weapon parent to player
                if ((hand.GetFingerIsPinching(HandFinger.Index) == true || Input.GetMouseButtonDown(0)) && pinched == false)
                {
                    weaponActivated = !weaponActivated;
                    pinched = !pinched;
                    //weapon.transform.SetParent(gameObject.transform.parent, true);
                }

                //only toggle pinched var off when not pinching
                if (hand.GetFingerIsPinching(HandFinger.Index) == false || Input.GetMouseButtonUp(0))
                    pinched = false;

                //show weapon
                if (weaponActivated)
                {
                    weapon.SetActive(true);
                    weaponActivatedText.GetComponent<Text>().text = "weapon activated: " + weaponActivated /*+ "\nparent: " + weapon.transform.parent.name*/;
                }
                else //hide weapon
                {
                    weapon.SetActive(false);
                    weaponActivatedText.GetComponent<Text>().text = "weapon activated: " + weaponActivated /*+ "\nparent: " + weapon.transform.parent.name*/;
                }

                //weapon location only being tracked when hand = tracked and weapon = shown and moving weapon to hand when hand tracking confidence is high (hand model visible)
                if (weaponActivated && hand.IsTracked && hand.GetTrackingConfidence() == OVRHand.TrackingConfidence.High)
                {
                    currentWeaponLocation = weapon.transform.position;
                    weapon.transform.SetParent(gameObject.transform.parent, true);
                    //weapon.transform.localPosition = meleePosOffset;
                    //weapon.transform.localRotation = weaponRotOffset;
                    weapon.transform.localPosition = Vector3.Lerp(weapon.transform.localPosition, meleePosOffset, 15f * Time.deltaTime);
                    weapon.transform.localRotation = Quaternion.Lerp(weapon.transform.localRotation, meleeRotOffset, 15f * Time.deltaTime);
                }
                else if (hand.GetTrackingConfidence() == OVRHand.TrackingConfidence.Low) //hand model invisible
                {
                    weapon.transform.SetParent(tempParent, true);
                }

                //set weapon location to last hand tracked location when hand = not tracked and weapon = shown (weapon stays in place when hand derps out), sets parent of weapon to outside player
                if (!hand.IsTracked && weaponActivated/* Input.GetMouseButtonDown(1)*/)
                {
                    weapon.transform.position = currentWeaponLocation;
                    weapon.transform.SetParent(tempParent, true);
                }

                //when hand = (not) tracked and weapon = hidden, set weapon to hand
                if (weaponActivated == false && (!hand.IsTracked || hand.IsTracked))
                {
                    weapon.transform.localPosition = meleePosOffset;
                    weapon.transform.localRotation = meleeRotOffset;
                    //weapon.transform.SetParent(tempParent, true);
                }
            }

            //////////weapon type: range
            if (weapon.GetComponent<Weapon>().type == "Gun")
            {
                //when index pinched, show weapon and stop pinched var from constantly being toggled, sets weapon parent to player
                if ((hand.GetFingerIsPinching(HandFinger.Index) == true || Input.GetMouseButtonDown(0)) && pinched == false)
                {
                    weaponActivated = !weaponActivated;
                    pinched = !pinched;
                    //weapon.transform.SetParent(gameObject.transform.parent, true);
                }

                //only toggle pinched var off when not pinching
                if (hand.GetFingerIsPinching(HandFinger.Index) == false || Input.GetMouseButtonUp(0))
                    pinched = false;

                //show weapon
                if (weaponActivated)
                {
                    weapon.SetActive(true);
                    //weaponActivatedText.GetComponent<Text>().text = "weapon activated: " + weaponActivated /*+ "\nparent: " + weapon.transform.parent.name*/;
                }
                else //hide weapon
                {
                    weapon.SetActive(false);
                    //weaponActivatedText.GetComponent<Text>().text = "weapon activated: " + weaponActivated /*+ "\nparent: " + weapon.transform.parent.name*/;
                }

                //weapon location only being tracked when hand = tracked and weapon = shown and moving weapon to hand when hand tracking confidence is high (hand model visible)
                if (weaponActivated && hand.IsTracked && hand.GetTrackingConfidence() == OVRHand.TrackingConfidence.High)
                {
                    currentWeaponLocation = weapon.transform.position;
                    weapon.transform.SetParent(gameObject.transform.parent, true);
                    //weapon.transform.localPosition = meleePosOffset;
                    //weapon.transform.localRotation = weaponRotOffset;
                    weapon.transform.localPosition = Vector3.Lerp(weapon.transform.localPosition, gunPosOffset, 15f * Time.deltaTime);
                    //weapon.transform.localRotation = Quaternion.Lerp(weapon.transform.localRotation, gunRotOffset, 15f * Time.deltaTime);
                }
                else if (hand.GetTrackingConfidence() == OVRHand.TrackingConfidence.Low) //hand model invisible
                {
                    weapon.transform.SetParent(tempParent, true);
                }

                //set weapon location to last hand tracked location when hand = not tracked and weapon = shown (weapon stays in place when hand derps out), sets parent of weapon to outside player
                if (!hand.IsTracked && weaponActivated/* Input.GetMouseButtonDown(1)*/)
                {
                    weapon.transform.position = currentWeaponLocation;
                    weapon.transform.SetParent(tempParent, true);
                }

                //when hand = (not) tracked and weapon = hidden, set weapon to hand
                if (weaponActivated == false && (!hand.IsTracked || hand.IsTracked))
                {
                    weapon.transform.localPosition = gunPosOffset;
                    //weapon.transform.localRotation = gunRotOffset;
                    //weapon.transform.SetParent(tempParent, true);
                }
            }

            //////////weapon type: developer tool
            if (weapon.GetComponent<Weapon>().type == "DeveloperTool")
            {
                //when index pinched, show weapon and stop pinched var from constantly being toggled, sets weapon parent to player
                if ((hand.GetFingerIsPinching(HandFinger.Index) == true || Input.GetMouseButtonDown(0)) && pinched == false)
                {
                    weaponActivated = !weaponActivated;
                    pinched = !pinched;
                    //weapon.transform.SetParent(gameObject.transform.parent, true);
                }

                //only toggle pinched var off when not pinching
                if (hand.GetFingerIsPinching(HandFinger.Index) == false || Input.GetMouseButtonUp(0))
                    pinched = false;

                //show weapon
                if (weaponActivated)
                {
                    weapon.SetActive(true);
                    //weaponActivatedText.GetComponent<Text>().text = "weapon activated: " + weaponActivated /*+ "\nparent: " + weapon.transform.parent.name*/;
                }
                else //hide weapon
                {
                    weapon.SetActive(false);
                    //weaponActivatedText.GetComponent<Text>().text = "weapon activated: " + weaponActivated /*+ "\nparent: " + weapon.transform.parent.name*/;
                }

                //weapon location only being tracked when hand = tracked and weapon = shown and moving weapon to hand when hand tracking confidence is high (hand model visible)
                if (weaponActivated && hand.IsTracked && hand.GetTrackingConfidence() == OVRHand.TrackingConfidence.High)
                {
                    currentWeaponLocation = weapon.transform.position;
                    weapon.transform.SetParent(gameObject.transform.parent, true);
                    //weapon.transform.localPosition = meleePosOffset;
                    //weapon.transform.localRotation = weaponRotOffset;
                    weapon.transform.localPosition = Vector3.Lerp(weapon.transform.localPosition, dtPosOffset, 15f * Time.deltaTime);
                }
                else if (hand.GetTrackingConfidence() == OVRHand.TrackingConfidence.Low) //hand model invisible
                {
                    weapon.transform.SetParent(tempParent, true);
                }

                //set weapon location to last hand tracked location when hand = not tracked and weapon = shown (weapon stays in place when hand derps out), sets parent of weapon to outside player
                if (!hand.IsTracked && weaponActivated/* Input.GetMouseButtonDown(1)*/)
                {
                    weapon.transform.position = currentWeaponLocation;
                    weapon.transform.SetParent(tempParent, true);
                }

                //when hand = (not) tracked and weapon = hidden, set weapon to hand
                if (weaponActivated == false && (!hand.IsTracked || hand.IsTracked))
                {
                    weapon.transform.localPosition = dtPosOffset;
                    //weapon.transform.SetParent(tempParent, true);
                }
            }
        }
        //else
          //  weaponActivatedText.GetComponent<Text>().text = "weapon not selected";
    }

    public void SetPinched(bool set)
    {
        pinched = set;
    }

    //since weapon scripts are in update, this prevents errors when no weapon is selected (cutscenes, testing, etc.)
    public void SetWeaponSelected(bool set)
    {
        weaponSelected = set;
    }

    //instantiates selected weapon, weapon selector will have to call a method to destroy current weapon
    public void PrepareWeapon(string type)
    {
        if (SceneManager.GetActiveScene().name != "Title")
        {
            if (weapon != null)
                Destroy(weapon);
            if (type == "Melee")
            {
                weapon = Instantiate(selectedWeapon, meleePosOffset, meleeRotOffset);
                weapon.SetActive(false);
                weaponActivated = false;
                weapon.transform.SetParent(gameObject.transform.parent, true);
                meleePosOffset = new Vector3(-0.095f, -0.03500003f, -0.02900004f);
                meleeRotOffset = new Quaternion(91.427f, -93.31799f, 49.89999f, 1f);
                weapon.transform.position = meleePosOffset;
                weapon.transform.localRotation = meleeRotOffset;
            }
            else if (type == "Gun")
            {
                weapon = Instantiate(selectedWeapon, gunPosOffset, gunRotOffset);
                ReloadHUD.GetComponent<HUDController>().SetGunController(weapon.GetComponent<GunController>());
                ReloadHUD.GetComponent<HUDController>().SetWeapon(weapon);
                SelectHUD.GetComponent<HUDController>().SetWeapon(weapon);
                weapon.SetActive(false);
                weaponActivated = false;
                weapon.transform.SetParent(gameObject.transform.parent, true);
                gunPosOffset = new Vector3(-0.095f, -0.03500003f, -0.02900004f);
                gunRotOffset = new Quaternion(0f, 0f, 0f, 1f);
                weapon.transform.position = gunPosOffset;
                weapon.transform.localRotation = gunRotOffset;
            }
            else if (type == "DeveloperTool")
            {
                weapon = Instantiate(selectedWeapon, dtPosOffset, new Quaternion(0, 0, 0, 0));
                SelectHUD.GetComponent<HUDController>().SetWeapon(weapon);
                weapon.SetActive(false);
                weaponActivated = false;
                weapon.transform.SetParent(gameObject.transform.parent, true);
                dtPosOffset = new Vector3(-0.095f, -0.03500003f, -0.02900004f);
                weapon.transform.position = dtPosOffset;
            }
        }
    }
}