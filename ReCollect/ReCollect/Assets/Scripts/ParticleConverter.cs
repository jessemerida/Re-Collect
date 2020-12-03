using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleConverter : MonoBehaviour
{
    public ParticleSystem particles;
    public ParticleSystem particlesLegs;
    public ParticleSystem particlesForearms;
    public ParticleSystem particlesArms;
    public ParticleSystem particlesHead;
    public ParticleSystem particlesFingers;
    public ParticleSystem particlesToes;
    public ParticleSystem particlesFeet;
    List<GameObject> bodyParts;
    List<GameObject> bodyPartsLeg;
    List<GameObject> bodyPartsForearm;
    List<GameObject> bodyPartsArm;
    List<GameObject> bodyPartsHead;
    int fingerCount;
    List<GameObject> bodyPartsFinger;
    List<GameObject> bodyPartsToe;
    List<GameObject> bodyPartsFoot;

    // Start is called before the first frame update
    void Awake()
    {
        bodyParts = new List<GameObject>();
        bodyPartsLeg = new List<GameObject>();
        bodyPartsForearm = new List<GameObject>();
        bodyPartsArm = new List<GameObject>();
        bodyPartsHead = new List<GameObject>();
        fingerCount = 5;
        bodyPartsFinger = new List<GameObject>();
        bodyPartsToe = new List<GameObject>();
        bodyPartsFoot = new List<GameObject>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("BodyParts"))
        {
            if (obj.transform.root == transform.root)
                bodyParts.Add(obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("BodyPartsLeg"))
        {
            if (obj.transform.root == transform.root)
                bodyPartsLeg.Add(obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("BodyPartsForearm"))
        {
            if (obj.transform.root == transform.root)
                bodyPartsForearm.Add(obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("BodyPartsArm"))
        {
            if (obj.transform.root == transform.root)
                bodyPartsArm.Add(obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("BodyPartsHead"))
        {
            if (obj.transform.root == transform.root)
                bodyPartsHead.Add(obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("BodyPartsFinger"))
        {
            if (obj.transform.root == transform.root)
                bodyPartsFinger.Add(obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("BodyPartsToe"))
        {
            if (obj.transform.root == transform.root)
                bodyPartsToe.Add(obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("BodyPartsFoot"))
        {
            if (obj.transform.root == transform.root)
                bodyPartsFoot.Add(obj);
        }
        Convert();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Convert()
    {
        print("bodyPartsFinger size: " + bodyPartsFinger.Count);
        foreach (GameObject part in bodyParts)
        {
            Instantiate(particles, part.transform.position, part.transform.rotation, part.transform);
        }
        foreach (GameObject part in bodyPartsLeg)
        {
            Instantiate(particlesLegs, new Vector3(part.transform.position.x, part.transform.position.y - 0.15f, part.transform.position.z), new Quaternion(-1, bodyPartsHead[0].transform.rotation.y, bodyPartsHead[0].transform.rotation.z, bodyPartsHead[0].transform.rotation.w), part.transform);
        }
        foreach (GameObject part in bodyPartsForearm)
        {
            Instantiate(particlesForearms, new Vector3(part.transform.position.x, part.transform.position.y, part.transform.position.z), part.transform.rotation, part.transform);
        }
        foreach (GameObject part in bodyPartsArm)
        {
            Instantiate(particlesArms, new Vector3(part.transform.position.x, part.transform.position.y - 0.16f, part.transform.position.z), part.transform.rotation, part.transform);
        }
        Instantiate(particlesHead, new Vector3(bodyPartsHead[0].transform.position.x, bodyPartsHead[0].transform.position.y - 0.2f, bodyPartsHead[0].transform.position.z), new Quaternion(bodyPartsHead[0].transform.rotation.x, bodyPartsHead[0].transform.rotation.y, bodyPartsHead[0].transform.rotation.z, bodyPartsHead[0].transform.rotation.w), bodyPartsHead[0].transform);
        foreach (GameObject part in bodyPartsFinger)
        {
            if (fingerCount > 0)
                Instantiate(particlesFingers, new Vector3(part.transform.position.x, part.transform.position.y, part.transform.position.z), part.transform.rotation, part.transform);
            fingerCount--;
        }
        foreach (GameObject part in bodyPartsToe)
        {
            Instantiate(particlesToes, new Vector3(part.transform.position.x, part.transform.position.y, part.transform.position.z), part.transform.rotation, part.transform);
        }
        foreach (GameObject part in bodyPartsFoot)
        {
            Instantiate(particlesFeet, new Vector3(part.transform.position.x, part.transform.position.y, part.transform.position.z), part.transform.rotation, part.transform);
        }
    }
}
