using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class ShieldController : OVRHand
{
    public GameObject shield;
    public ParentConstraint shieldPlacementHolder;
    public ParentConstraint shieldPlacementHolder2;
    public bool shieldActivated;
    OVRHand hand;
    bool spawnPinch;
    bool pinched;
    [SerializeField] bool stopTracking; //has shield been placed yet?
    [SerializeField] bool placed;

    Vector3 currentShieldLocation;
    Transform tempParent;

    Vector3 shieldPosOffset;
    Quaternion shieldRotOffset;

    int maxHealth;
    [SerializeField] int health;
    GameObject[] brokenShieldPieces;
    [SerializeField] int healthLost;
    [SerializeField] int partBroken; //0, first index
    [SerializeField] bool healing;
    [SerializeField] bool firstHeal;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Title")
            this.enabled = false;

        shield = GameObject.Find("shield");
        print("awake shield assigned");

        maxHealth = 15;
        health = maxHealth;
        brokenShieldPieces = GameObject.FindGameObjectsWithTag("BrokenShieldParts");
        healthLost = 0;
        partBroken = 0;
        healing = false;
        firstHeal = false;

        foreach (GameObject part in brokenShieldPieces)
            part.GetComponent<MeshRenderer>().enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        shield = GameObject.Find("shield");
        shield.SetActive(false);
        hand = GetComponent<OVRHand>();
        spawnPinch = false;
        pinched = false;
        tempParent = null;
        stopTracking = false;
        placed = false;
    }

    // Update is called once per frame
    void Update()
    {
        #region placement
        //when index pinched, show shield and stop pinched var from constantly being toggled, sets shield parent to player
        if ((hand.GetFingerIsPinching(HandFinger.Middle) == true && !spawnPinch && !pinched))
        {
            shieldActivated = true;
            spawnPinch = !spawnPinch;
            pinched = !pinched;
            firstHeal = false;
            //shield.transform.SetParent(gameObject.transform.parent, true);
        }

        //only toggle pinched var off when not pinching
        if (hand.GetFingerIsPinching(HandFinger.Middle) == false)
            pinched = false;

        if (stopTracking && shield != null)
            shield.GetComponentInParent<ParentConstraint>().enabled = false;

        //show shield
        if (shieldActivated && !stopTracking && shield != null)
        {
            shield.GetComponentInParent<ParentConstraint>().enabled = true;
            shield.SetActive(true);
            //stopTracking = true;
        }
        else if (!stopTracking)
        {
            shield.SetActive(false);
            //pinched = false;
            spawnPinch = false;
            placed = false;
        }

        if (shieldActivated)
        {
            healing = false;
            firstHeal = false;
        }

        //shield location only being tracked when hand = tracked and shield = shown and moving shield to hand when hand tracking confidence is high (hand model visible)
        if (shieldActivated && hand.IsTracked && hand.GetTrackingConfidence() == OVRHand.TrackingConfidence.High && stopTracking && !placed)
        {
            //shield.GetComponent<ParentConstraint>().enabled = true;
            stopTracking = false;
            //currentShieldLocation = shield.transform.position;
            //shield.transform.SetParent(gameObject.transform.parent, true);
            //shield.transform.localPosition = meleePosOffset;
            //shield.transform.localRotation = shieldRotOffset;
            //shield.transform.localPosition = Vector3.Lerp(shield.transform.localPosition, shieldPosOffset, 15f * Time.deltaTime);
            //shield.transform.localRotation = Quaternion.Lerp(shield.transform.localRotation, shieldRotOffset, 15f * Time.deltaTime);
        }
        else if (shieldActivated && hand.GetTrackingConfidence() == OVRHand.TrackingConfidence.Low) //hand model invisible
        {
            stopTracking = true;
        }

        //set shield location to last hand tracked location when hand = not tracked and shield = shown (shield stays in place when hand derps out), sets parent of shield to outside player
        if (!hand.IsTracked && shieldActivated && !stopTracking/* Input.GetMouseButtonDown(1)*/)
        {
            //shield.transform.position = currentShieldLocation;
            //shield.transform.SetParent(tempParent, true);
            stopTracking = true;
        }

        //when hand = (not) tracked and shield = hidden, set shield to hand
        /*if (shieldActivated == false && (!hand.IsTracked || hand.IsTracked))
        {
            shield.transform.localPosition = shieldPosOffset;
            shield.transform.localRotation = shieldRotOffset;
            //shield.transform.SetParent(tempParent, true);
        }*/

        if (shieldActivated && hand.GetFingerIsPinching(HandFinger.Middle) && !pinched && !stopTracking && !placed && hand.IsTracked && hand.GetTrackingConfidence() == OVRHand.TrackingConfidence.High)
        {
            stopTracking = true;
            placed = true;
            pinched = true;
        }

        if (shieldActivated && stopTracking && hand.GetFingerIsPinching(HandFinger.Middle) && !pinched && placed && hand.IsTracked && hand.GetTrackingConfidence() == OVRHand.TrackingConfidence.High)
        {
            shieldActivated = false;
            stopTracking = false;
            pinched = true;
        }

        if (placed)
        {
            shieldPlacementHolder.enabled = false;
            shieldPlacementHolder2.enabled = false;
        }
        else
        {
            shieldPlacementHolder.enabled = true;
            shieldPlacementHolder2.enabled = true;
        }
        #endregion

        //shield healing
        if (!shieldActivated && !healing && health < maxHealth)
        {
            healing = true;
            StartCoroutine(IncreaseHealth());
        }
    }

    private void OnDisable()
    {
        if (shield != null)
            shield.GetComponentInParent<ParentConstraint>().enabled = false;
    }

    private void OnEnable()
    {
        if (stopTracking == false && shield != null)
            shield.GetComponentInParent<ParentConstraint>().enabled = true;
    }

    IEnumerator IncreaseHealth()
    {
        print("shield health increased");
        if (!firstHeal)
        {
            yield return new WaitForSeconds(1.5f);
            firstHeal = true;
        }
        else if ((health + 1) < maxHealth)
        {
            health++;
            healthLost--;
        }
        else
        {
            health = maxHealth;
            healthLost = 0;
        }
        yield return new WaitForSeconds(0.2f);
        /*if ((health + 5) < maxHealth)
        {
            health += 5;
            healthLost -= 5;
        }
        else
        {
            health = maxHealth;
            healthLost -= 0;
        }
        yield return new WaitForSeconds(5f);*/
        CheckHealth(true);
        /*if (health < 50)
            StartCoroutine(IncreaseHealth());*/
        healing = false;
    }

    public void DecreaseHealth()
    {
        print("shield health decreased");
        if ((health - 1) > 0)
        {
            health-=1;
            healthLost+=1;
        }
        else
        {
            health = 0;
            healthLost = maxHealth;
        }
        CheckHealth(false);
    }

    void CheckHealth(bool healed) //healing = true, not healing = false
    {   
        if (healed)
        {
            if (healthLost % 3 == 0)
            {
                brokenShieldPieces[partBroken].GetComponent<MeshRenderer>().enabled = false;
                if (partBroken > 0)
                    partBroken--;
            }

            int partsBroken = 0;
            bool allPartsBroken = false;
            foreach (GameObject part in brokenShieldPieces)
            {
                if (part.GetComponent<MeshRenderer>().enabled)
                    partsBroken++;
            }
            if (partsBroken == 5)
                allPartsBroken = true;

            if (!allPartsBroken)
            {
                shield.GetComponent<MeshRenderer>().enabled = true;
                shield.GetComponent<MeshCollider>().enabled = true;
            }
        }
        else
        {
            if (healthLost % 3 == 0)
            {
                brokenShieldPieces[partBroken].GetComponent<MeshRenderer>().enabled = true;
                shield.GetComponent<ShieldHitboxController>().PieceBroken();
                if (partBroken < 4)
                    partBroken++;
            }

            bool allPartsBroken = true;
            foreach (GameObject part in brokenShieldPieces)
            {
                if (!part.GetComponent<MeshRenderer>().enabled)
                    allPartsBroken = false;
            }

            if (allPartsBroken)
            {
                shield.GetComponent<MeshRenderer>().enabled = false;
                shield.GetComponent<MeshCollider>().enabled = false;
                shield.GetComponent<ShieldHitboxController>().AllBroken();
            }
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public bool GetHealingState()
    {
        return healing;
    }
}