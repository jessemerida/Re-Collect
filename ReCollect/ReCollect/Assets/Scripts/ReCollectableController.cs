using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReCollectableController : MonoBehaviour
{
    [SerializeField] bool recollectMode;
    //Vector3 enemy;
    EnemyController enemy;
    Transform lifeBand;
    bool healed;

    // Start is called before the first frame update
    void Start()
    {
        healed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (recollectMode)
        {
            transform.SetParent(enemy.transform);
            transform.localPosition = new Vector3(0, 2, 0);
        }

        if (enemy != null && !enemy.alive)
        {
            SetReCollectableMode(false);
            if (lifeBand != null)
            {
                transform.SetParent(lifeBand);
            }
            transform.localPosition = Vector3.zero;
            transform.localRotation = new Quaternion(0, 0, 0, 1);
            if (!healed)
            {
                healed = true;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddHealth(3);
            }
        }
    }

    public void SetReCollectableMode(bool set)
    {
        recollectMode = set;
        if (set)
            transform.localScale = new Vector3(4, 4, 4);
        else
            transform.localScale = Vector3.one;
    }

    public bool GetReCollectableMode()
    {
        return recollectMode;
    }

    /*public void SetFollowPosition(Vector3 position)
    {
        enemy = new Vector3(position.x, position.y + 2, position.z);
    }*/

    public void SetAliveReference(EnemyController set)
    {
        enemy = set;
    }

    public void SetLifeBandTransform(Transform set)
    {
        lifeBand = set;
    }
}
