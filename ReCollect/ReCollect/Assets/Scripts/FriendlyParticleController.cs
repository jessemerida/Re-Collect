using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyParticleController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (transform.root.name.Contains("Friendly"))
        {
            if (other.tag == "EnemyAmmo")
            {
                print("friendly particle touched enemy ammo: " + other.tag);
                ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
                main.startColor = Color.red;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
        Color color;
        if (transform.root.name.Contains("Friendly"))
        {
            tag = "FriendlyHitboxParticle";
            switch (name)
            {
                case "BodyParticlesBody(Clone)":
                    {
                        string hex = "FFEA75";
                        if (ColorUtility.TryParseHtmlString("#" + hex, out color))
                            main.startColor = color;
                        break;
                    }
                case "BodyParticlesLegs(Clone)":
                    {
                        string hex = "FFEA75";
                        if (ColorUtility.TryParseHtmlString("#" + hex, out color))
                            main.startColor = color;
                        break;
                    }
                case "BodyParticlesForeArms(Clone)":
                    {
                        string hex = "FFEA75";
                        if (ColorUtility.TryParseHtmlString("#" + hex, out color))
                            main.startColor = color;
                        break;
                    }
                case "BodyParticlesArms(Clone)":
                    {
                        string hex = "FFEA75";
                        if (ColorUtility.TryParseHtmlString("#" + hex, out color))
                            main.startColor = color;
                        break;
                    }
                case "BodyParticlesHead(Clone)":
                    {
                        string hex = "4CFF76";
                        if (ColorUtility.TryParseHtmlString("#" + hex, out color))
                            main.startColor = color;
                        break;
                    }
                case "BodyParticlesFingers(Clone)":
                    {
                        string hex = "4CFF76";
                        if (ColorUtility.TryParseHtmlString("#" + hex, out color))
                            main.startColor = color;
                        break;
                    }
                case "BodyParticlesToes(Clone)":
                    {
                        string hex = "4CFF76";
                        if (ColorUtility.TryParseHtmlString("#" + hex, out color))
                            main.startColor = color;
                        break;
                    }
                case "BodyParticlesFeet(Clone)":
                    {
                        string hex = "FFEA75";
                        if (ColorUtility.TryParseHtmlString("#" + hex, out color))
                            main.startColor = color;
                        break;
                    }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
