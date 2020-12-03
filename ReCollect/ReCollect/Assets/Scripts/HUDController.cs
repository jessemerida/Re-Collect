using DearVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class HUDController : MonoBehaviour
{
    Player player;
    GameObject rightHand;
    GameObject leftHand;

    public Sprite[] sprites;
    SpriteRenderer spriteRenderer;

    GunController gunController;
    ShieldController shieldController;

    [SerializeField] float max;
    [SerializeField] float level;
    bool changing;
    [SerializeField] bool reloading;
    [SerializeField] bool healing;

    GameObject weapon;

    public AudioSource activateSound;
    public AudioSource deactivateSound;
    [SerializeField] bool activated;
    [SerializeField] bool deactivated;
    public AudioSource switchSound;
    [SerializeField] bool weapon1switched;
    [SerializeField] bool weapon2switched;
    [SerializeField] bool switched;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        rightHand = GameObject.Find("RightHand");
        leftHand = GameObject.Find("LeftHand");
        spriteRenderer = GetComponent<SpriteRenderer>();

        activated = false;
        deactivated = false;
        weapon1switched = false;
        weapon2switched = false;
        switched = false;

        if (name == "ReloadHUD")
        {
            if (weapon != null)
            {
                gunController = weapon.GetComponent<GunController>();
                max = gunController.GetMaxAmmo();
                level = max;
            }

            transform.localScale = new Vector3(0, 1, 0);
            changing = false;
            reloading = false;
        }

        if (name == "ShieldHealthHUD")
        {
            shieldController = leftHand.GetComponent<ShieldController>();

            max = shieldController.GetMaxHealth();
            level = max;
            transform.localScale = new Vector3(0, 1, 0);
            //changing = false;
        }

        if (name == "DeveloperToolHUD")
        {
            max = player.DTMax;// 69;
            level = max;
            transform.localScale = new Vector3(0, 1, 0);
        }
    }

    public void SetSwitched(bool set)
    {
        switched = set;
    }

    // Update is called once per frame
    void Update()
    {
        #region positioning
        if (name.Equals("WeaponActivatorHUD") || name.Equals("SelectHUD") || name.Equals("ReloadHUD") || name.Equals("ReloadHUDEdge") || name.Equals("DeveloperToolHUD"))
        {
            if (rightHand.GetComponent<OVRHand>().IsTracked && rightHand.GetComponent<OVRHand>().GetTrackingConfidence() == OVRHand.TrackingConfidence.High)
            {
                GetComponent<ParentConstraint>().enabled = true;
            }
            else if ((rightHand.GetComponent<OVRHand>().GetTrackingConfidence() == OVRHand.TrackingConfidence.Low))
            {
                GetComponent<ParentConstraint>().enabled = false;
            }

            if (!rightHand.GetComponent<OVRHand>().IsTracked)
            {
                GetComponent<ParentConstraint>().enabled = false;
            }
        }
        else if (name.Equals("TeleportHUD") || name.Equals("ShieldSelectedHUD") || name.Equals("ShieldHealthHUD") || name.Equals("ShieldHealthEdge"))
        {
            if (leftHand.GetComponent<OVRHand>().IsTracked && leftHand.GetComponent<OVRHand>().GetTrackingConfidence() == OVRHand.TrackingConfidence.High)
            {
                GetComponent<ParentConstraint>().enabled = true;
            }
            else if ((leftHand.GetComponent<OVRHand>().GetTrackingConfidence() == OVRHand.TrackingConfidence.Low))
            {
                GetComponent<ParentConstraint>().enabled = false;
            }

            if (!leftHand.GetComponent<OVRHand>().IsTracked)
            {
                GetComponent<ParentConstraint>().enabled = false;
            }
        }
        #endregion

        switch (name)
        {
            case "WeaponActivatorHUD":
                {
                    if (!player.GetInCutscene())
                    {
                        if (rightHand.GetComponent<WeaponActivator>().weaponActivated && !activated)
                        {
                            spriteRenderer.sprite = sprites[1];
                            activated = true;
                            deactivated = false;
                            GetComponent<DearVRSource>().DearVRPlayOneShot(activateSound.clip);
                        }
                        else if (!rightHand.GetComponent<WeaponActivator>().weaponActivated && !deactivated)
                        {
                            spriteRenderer.sprite = sprites[0];
                            activated = false;
                            deactivated = true;
                            GetComponent<DearVRSource>().DearVRPlayOneShot(deactivateSound.clip);
                        }
                    }
                    else
                        spriteRenderer.sprite = sprites[2];
                    break;
                }

            case "TeleportHUD":
                {
                    if (!player.GetInCutscene())
                    {
                        if (leftHand.GetComponent<TeleportController>().teleporterActivated && !activated)
                        {
                            spriteRenderer.sprite = sprites[1];
                            activated = true;
                            deactivated = false;
                            GetComponent<DearVRSource>().DearVRPlayOneShot(activateSound.clip);
                        }
                        else if (!leftHand.GetComponent<TeleportController>().teleporterActivated && !deactivated)
                        {
                            spriteRenderer.sprite = sprites[0];
                            activated = false;
                            deactivated = true;
                            GetComponent<DearVRSource>().DearVRPlayOneShot(deactivateSound.clip);
                        }
                    }
                    else
                        spriteRenderer.sprite = sprites[2];
                    break;
                }

            case "SelectHUD":
                {
                    if (!player.GetInCutscene())
                    {
                        if (!rightHand.GetComponent<WeaponActivator>().weaponActivated)
                            spriteRenderer.sprite = sprites[1];
                        if (weapon != null)
                        {
                            if (weapon.name == "shooter(Clone)")
                                spriteRenderer.sprite = sprites[2];
                            if (weapon.name == "developertoolWL(Clone)")
                                spriteRenderer.sprite = sprites[3];
                        }
                    }
                    else
                        spriteRenderer.sprite = sprites[0];
                    break;
                }

            case "ReloadHUD":
                {
                    if (!player.GetInCutscene() && GameObject.Find("WeaponSelector").GetComponent<WeaponSelector>().selectedWeapon == 0)
                    {
                        spriteRenderer.enabled = true;
                        //print("sprite renderer enabled");
                        if (gunController != null)
                        {
                            if (!reloading && weapon != null)
                            {
                                level = gunController.GetClip();
                                transform.localScale = new Vector3(level / max, level / max, level / max);
                            }

                            if (gunController.GetReloadingState() && !changing)
                            {
                                if (!reloading)
                                    reloading = true;
                                StartCoroutine(Reload());
                            }
                            else if (!gunController.GetReloadingState())
                            {
                                reloading = false;
                                level = max;
                            }
                        }
                    }
                    else
                        spriteRenderer.enabled = false;
                    break;
                }

            case "ShieldSelectedHUD":
                {
                    if (!player.GetInCutscene())
                    {
                        if (!leftHand.GetComponent<ShieldController>().shieldActivated && !deactivated)
                        {
                            spriteRenderer.sprite = sprites[1];
                            activated = false;
                            deactivated = true;
                            GetComponent<DearVRSource>().DearVRPlayOneShot(deactivateSound.clip);
                        }
                        else if (leftHand.GetComponent<ShieldController>().shieldActivated && !activated)
                        {
                            spriteRenderer.sprite = sprites[2];
                            activated = true;
                            deactivated = false;
                            GetComponent<DearVRSource>().DearVRPlayOneShot(activateSound.clip);
                        }
                    }
                    else
                        spriteRenderer.sprite = sprites[0];
                    break;
                }

            case "ShieldHealthHUD":
                {
                    if (!player.GetInCutscene())
                    {
                        spriteRenderer.enabled = true;
                        //if (!healing)
                        //{
                        level = shieldController.GetHealth();
                        transform.localScale = new Vector3(level / max, level / max, level / max);
                        //}

                        /*if (shieldController.GetHealingState() && !changing)
                        {
                            if (!healing)
                                healing = true;
                            StartCoroutine(Heal());
                        }
                        else if (!shieldController.GetHealingState())
                        {
                            healing = false;
                            level = max;
                        }*/
                    }
                    else
                        spriteRenderer.enabled = false;
                    break;
                }

            case "DeveloperToolHUD":
                {
                    if (!player.GetInCutscene() && GameObject.Find("WeaponSelector").GetComponent<WeaponSelector>().selectedWeapon == 1)
                    {
                        spriteRenderer.enabled = true;
                        //if (!healing)
                        //{
                        level = player.GetDeveloperToolTime();
                        max = player.DTMax;
                        //if (level >= 0)
                            transform.localScale = new Vector3(level / max, level / max, level / max);
                        //}

                        /*if (shieldController.GetHealingState() && !changing)
                        {
                            if (!healing)
                                healing = true;
                            StartCoroutine(Heal());
                        }
                        else if (!shieldController.GetHealingState())
                        {
                            healing = false;
                            level = max;
                        }*/
                    }
                    else
                        spriteRenderer.enabled = false;
                    break;
                }
        }
    }

    IEnumerator Reload()
    {
        changing = true;
        //print("reloading ammo hud");
        yield return new WaitForSeconds(0.015f);
        transform.localScale = new Vector3(level / max, level / max, level / max);
        if (level + 0.31f <= max)
            level += 0.31f;
        changing = false;
    }

    /*IEnumerator Heal()
    {
        changing = true;
        print("reloading shield health hud");
        yield return new WaitForSeconds(0.01f);
        transform.localScale = new Vector3(level / max, level / max, level / max);
        if (level + 0.015f <= max)
            level += 0.015f;
        changing = false;
    }*/

    /*IEnumerator Fall()
    {
        changing = true;
        print("falling");
        yield return new WaitForSeconds(0.0001f);
        transform.localScale = new Vector3(level / max, level / max, level / max);
        level -= 0.01f;
        changing = false;
    }*/

    public void SetGunController(GunController set)
    {
        gunController = set;
        max = gunController.GetMaxAmmo();
        level = max;
    }

    public void SetWeapon(GameObject set)
    {
        weapon = set;
    }
}