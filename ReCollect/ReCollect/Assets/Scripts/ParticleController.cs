using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (transform.root.GetChild(0).name == "RiflemanParticles(Clone)")
        {
            //print("particle touched: " + other.tag);
            //print("ENEMY: " + name + " touched: " + other.tag);
            if (other.tag == "PlayerAmmo" || other.tag == "FriendlyAmmo")
            {
                ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
                main.startColor = Color.red;
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        ParticleSystem.MainModule main = GetComponent<ParticleSystem>().main;
        Color color;
        if (transform.root.GetChild(0).name == "RiflemanParticles(Clone)")
        {
            tag = "EnemyHitboxParticle";
            switch (name)
            {
                case "BodyParticlesBody(Clone)":
                    {
                        string hex = "462DFF";
                        if (ColorUtility.TryParseHtmlString("#" + hex, out color))
                            main.startColor = color;
                        break;
                    }
                case "BodyParticlesLegs(Clone)":
                    {
                        string hex = "462DFF";
                        if (ColorUtility.TryParseHtmlString("#" + hex, out color))
                            main.startColor = color;
                        break;
                    }
                case "BodyParticlesForeArms(Clone)":
                    {
                        string hex = "462DFF";
                        if (ColorUtility.TryParseHtmlString("#" + hex, out color))
                            main.startColor = color;
                        break;
                    }
                case "BodyParticlesArms(Clone)":
                    {
                        string hex = "462DFF";
                        if (ColorUtility.TryParseHtmlString("#" + hex, out color))
                            main.startColor = color;
                        break;
                    }
                case "BodyParticlesHead(Clone)":
                    {
                        string hex = "000000";
                        if (ColorUtility.TryParseHtmlString("#" + hex, out color))
                            main.startColor = color;
                        break;
                    }
                case "BodyParticlesFingers(Clone)":
                    {
                        string hex = "000000";
                        if (ColorUtility.TryParseHtmlString("#" + hex, out color))
                            main.startColor = color;
                        break;
                    }
                case "BodyParticlesToes(Clone)":
                    {
                        string hex = "000000";
                        if (ColorUtility.TryParseHtmlString("#" + hex, out color))
                            main.startColor = color;
                        break;
                    }
                case "BodyParticlesFeet(Clone)":
                    {
                        string hex = "462DFF";
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
