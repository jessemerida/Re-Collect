using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test4 : MonoBehaviour
{
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ZaWarudo()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            print("time has stopped");
            GetComponent<Button>().interactable = false;
            StartCoroutine(TokiWoTomare());
        }
        else
        {
            Time.timeScale = 1;
            print("time has started");
            text.text = "Za Warudo";
        }
    }

    IEnumerator TokiWoTomare()
    {
        text.text = "TOKI WO TOMAREEE!!!";
        yield return new WaitForSecondsRealtime(3f);
        GetComponent<Button>().interactable = true;
        text.text = "un Za Warudo";
    }
}
