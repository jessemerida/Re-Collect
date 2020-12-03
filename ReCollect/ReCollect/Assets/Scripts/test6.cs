using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test6 : MonoBehaviour
{
    [SerializeField] float max;
    [SerializeField] float level;
    [SerializeField] bool rise;
    bool changing;

    // Start is called before the first frame update
    void Start()
    {
        max = 1f;
        level = 0;
        transform.localScale = new Vector3(0, 0, 0);
        changing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (rise && level >= max)
            rise = false;
        else if (!rise && level <= 0)
            rise = true;

        if (!changing)
        {
            if (rise)
                StartCoroutine(Rise());
            else if (!rise)
                StartCoroutine(Fall());
        }
    }

    IEnumerator Rise()
    {
        changing = true;
        print("rising");
        yield return new WaitForSeconds(0.0001f);
        transform.localScale = new Vector3(level / max, level / max, level / max);
        level += 0.01f;
        changing = false;
    }

    IEnumerator Fall()
    {
        changing = true;
        print("falling");
        yield return new WaitForSeconds(0.0001f);
        transform.localScale = new Vector3(level / max, level / max, level / max);
        level -= 0.01f;
        changing = false;
    }
}
