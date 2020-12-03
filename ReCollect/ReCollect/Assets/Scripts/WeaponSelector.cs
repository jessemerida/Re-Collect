using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelector : OVRHand
{
    [SerializeField] public int selectedWeapon;
    GameObject equippedWeapon;
    public GameObject[] weaponsList;
    public int weaponsListLength;
    GameObject rightHand;
    OVRHand hand;
    [SerializeField] bool pinched;
    public GameObject weaponsListHolder;
    GameObject holder;
    bool rotate;

    GameObject FindWeaponList;
    GameObject CenterEyeText;

    private void Awake()
    {
        //this.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        rightHand = GameObject.Find("RightHand");
        hand = rightHand.GetComponent<OVRHand>();
        pinched = false;
        selectedWeapon = 0;
        weaponsListLength = weaponsList.Length;
        rotate = true;

        //eventually goes in a method when a weapon is selected by the WeaponSelector
        /*equippedWeapon = weaponsList[selectedWeapon];
        rightHand.GetComponent<WeaponActivator>().PrepareWeapon(weaponsList[selectedWeapon].GetComponent<Weapon>().type);
        rightHand.GetComponent<WeaponActivator>().SetWeaponSelected(true);*/

        //created = false;

        FindWeaponList = GameObject.Find("fwltext");
        CenterEyeText = GameObject.Find("ceamtext");
    }

    // Update is called once per frame
    void Update()
    {
        if (rightHand.GetComponent<WeaponActivator>().weaponActivated == false && hand.GetFingerIsPinching(HandFinger.Middle) == true/* || Input.GetMouseButton(2))*/ && pinched == false)
        {
            pinched = true;
            if (weaponsList.Length > 0)
            {
                if (selectedWeapon + 1 == weaponsList.Length)
                    selectedWeapon = 0;
                else
                    selectedWeapon++;
                SelectWeapon();
            }
        }

        if (hand.IsTracked && hand.GetFingerIsPinching(HandFinger.Middle) == false/* || !Input.GetMouseButton(2)*/)
            pinched = false;

        #region sadness
        /*if (GameObject.Find("CenterEyeAnchor") != null)
        {
            CenterEyeText.GetComponent<Text>().text = "centereyeanchor local rot: " + GameObject.Find("CenterEyeAnchor").transform.localRotation;
        }
        else
            CenterEyeText.GetComponent<Text>().text = "unable to find CenterEyeAnchor";

        if (hand.GetFingerIsPinching(HandFinger.Middle) == true || Input.GetMouseButton(2))
        {
            if (pinched == false)
            {
                pinched = true;
                if (weaponsList.Length != 0)
                    CreateWeaponsList3D();
            }
        }

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (!Input.GetMouseButton(2)) //hand needs to be commented out to use mouse buttons
            {
                if (pinched)
                    DestroyWeaponsList3D();
                pinched = false;
            }
        }
        else //running on Quest
        {
            if (hand.GetFingerIsPinching(HandFinger.Middle) == false) //hand needs to be commented out to use mouse buttons
            {
                if (pinched)
                    DestroyWeaponsList3D();
                pinched = false;
            }
        }

        if (holder != null)
        {
            FindWeaponList.GetComponent<Text>().text = "WeaponListHolder Coords: " + holder.transform.position + "\nlocal rot: " + holder.transform.localRotation;
            if (rotate)
            {
                //holder.transform.rotation = transform.rotation;
                //rotate = false;
            }
        }
        else
            FindWeaponList.GetComponent<Text>().text = "WeaponListHolder not spawned";
        */
        #endregion
    }

    void SelectWeapon()
    {
        if (rightHand.GetComponent<WeaponActivator>().selectedWeapon != null)
            UnequipWeapon();
        rightHand.GetComponent<WeaponActivator>().selectedWeapon = weaponsList[selectedWeapon];
        rightHand.GetComponent<WeaponActivator>().PrepareWeapon(weaponsList[selectedWeapon].GetComponent<Weapon>().type);
        rightHand.GetComponent<WeaponActivator>().SetWeaponSelected(true);
    }

    void UnequipWeapon()
    {
        rightHand.GetComponent<WeaponActivator>().SetWeaponSelected(false);
        rightHand.GetComponent<WeaponActivator>().selectedWeapon = null;
    }

    #region sadness pt2
    void CreateWeaponsList3D()
    {
        holder = Instantiate(weaponsListHolder); //keep weaponSelector's transform in mind (in scene)
        holder.transform.SetParent(transform);
        transform.SetParent(GameObject.Find("CenterEyeAnchor").transform);
        transform.localPosition = Vector3.forward * 0.2f;
        float weaponSpacing = 0.25f;
        float startingValue = (weaponsList.Length / 2 + 0.5f - 1) * weaponSpacing * -1; //the number of weapons / 2 + 0.5 says which weapon will be in the middle, minus 1 shows how many weapons will be to the left of the middle one, and times -1 gives the left most weapon its x value. exceptions for 0 and 1
        if (weaponsList.Length == 1)
        {
            Instantiate(weaponsList[0], new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z), new Quaternion(0, 0, 0, 1), holder.transform);
        }
        else
        {
            foreach (GameObject weapon in weaponsList)
            {
                Instantiate(weapon, new Vector3(gameObject.transform.position.x + startingValue, gameObject.transform.position.y, gameObject.transform.position.z), new Quaternion(0, 0, 0, 1), holder.transform);

                startingValue += weaponSpacing;
            }
        }
    }

    void DestroyWeaponsList3D()
    {
        Destroy(/*transform.GetChild(0).gameObject*/holder);
        rotate = true;
    }
    #endregion
}