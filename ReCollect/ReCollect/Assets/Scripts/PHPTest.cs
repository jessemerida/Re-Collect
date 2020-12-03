using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHPTest : MonoBehaviour
{
    OVRHand hand;
    bool pinched;
    string data;

    // Start is called before the first frame update
    void Start()
    {
        hand = GetComponent<OVRHand>();
        data = "questy";
        //ActivateSendData();
        //ActivateGetData();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (hand.GetFingerIsPinching(OVRHand.HandFinger.Pinky) && !pinched)
        {
            pinched = true;
            ActivateSendData();
        }*/
    }

    #region send data
    void ActivateSendData()
    {
        StartCoroutine(SendData(data));
    }

    IEnumerator SendData(string data)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", data);

        WWW www = new WWW("https://jessemeridasgreatwebsite.altervista.org/AddPlayerName.php", form);
        yield return www;
        if (www.text != null && www.text != "")
        {
            print("nice: " + www.text);
        }
        else
            print("not nice");
    }
    #endregion

    #region get data
    void ActivateGetData()
    {
        StartCoroutine(GetData());
    }

    IEnumerator GetData()
    {
        WWW www = new WWW("https://jessemeridasgreatwebsite.altervista.org/GetPlayerName.php");
        yield return www;
        if (www.text != null && www.text != "")
        {
            print("nice: " + www.text);
        }
        else
            print("not nice");
    }
    #endregion
}
