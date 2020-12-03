using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacePlayer : MonoBehaviour
{
    Vector3 playerPos;
    // Start is called before the first frame update
    void Start()
    {
        ActivateGetData();
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, 0, GameObject.FindGameObjectWithTag("Player").transform.position.z);
        transform.LookAt(2 * transform.position - playerPos);
        //transform.LookAt(2 * transform.position - GameObject.FindGameObjectWithTag("Player").transform.position);
    }

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
            transform.GetChild(0).GetComponent<Text>().text = www.text;
        }
        else
            print("not nice");
    }
    #endregion
}
