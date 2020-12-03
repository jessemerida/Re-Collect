using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DearVR;

public class QuickMenuController : MonoBehaviour
{
    string menu;

    GameObject quickMenu;

    GameObject logo;
    Button mainMenu;
    Button closeMenu;
    Button gameOptions;
    Button retryLevel;
    public Button selectLevel;
    Button newGame;
    Button inputName;
    GameObject nameSubmitHolder;
    Button level1;
    Button level2;
    Button backToMainMenu;

    GameObject namePanel;
    Button bNameSubmit;
    public GameObject inputPrefab;
    GameObject shiftBtn;
    bool shift;

    private void Update()
    {
        if (menu == "name")
        {
            quickMenu.GetComponent<GridLayoutGroup>().childAlignment = TextAnchor.LowerCenter;
            if (bNameSubmit.transform.GetChild(0).GetComponent<Text>().text.Length > 20)
            {
                bNameSubmit.transform.GetChild(0).GetComponent<Text>().text = bNameSubmit.transform.GetChild(0).GetComponent<Text>().text.Substring(0, 19);
            }
        }
        else
            quickMenu.GetComponent<GridLayoutGroup>().childAlignment = TextAnchor.MiddleCenter;
    }

    private void Start()
    {
        GetComponent<DearVRSource>().GetReverbSendList()[0].send = 1;

        quickMenu = GameObject.Find("qmMainMenu");

        logo = GameObject.Find("Logo");
        mainMenu = GameObject.Find("bMainMenu").GetComponent<Button>();
        closeMenu = GameObject.Find("bCloseMenu").GetComponent<Button>();
        gameOptions = GameObject.Find("bGameOptions").GetComponent<Button>();
        retryLevel = GameObject.Find("bRetryLevel").GetComponent<Button>();
        selectLevel = GameObject.Find("bSelectLevel").GetComponent<Button>();
        newGame = GameObject.Find("bNewGame").GetComponent<Button>();
        inputName = GameObject.Find("bName").GetComponent<Button>();
        nameSubmitHolder = GameObject.Find("NameSubmitHolder");
        level1 = GameObject.Find("bLevel1").GetComponent<Button>();
        level2 = GameObject.Find("bLevel2").GetComponent<Button>();
        backToMainMenu = GameObject.Find("bBackToMainMenu").GetComponent<Button>();
        namePanel = GameObject.Find("pNameInput");
        bNameSubmit = GameObject.Find("bNameSubmit").GetComponent<Button>();
        shift = false;

        SetupMenu();
    }

    void SetupMenu()
    {
        if (SceneManager.GetActiveScene().name == "Title")
        {
            menu = "main";
            mainMenu.gameObject.SetActive(false);
            closeMenu.gameObject.SetActive(false);
            retryLevel.gameObject.SetActive(false);
            backToMainMenu.gameObject.SetActive(false);
            level1.gameObject.SetActive(false);
            level2.gameObject.SetActive(false);
            namePanel.SetActive(false);
            nameSubmitHolder.SetActive(false);
            bNameSubmit.gameObject.SetActive(false);
        }
        else if (SceneManager.GetActiveScene().name != "Title")
        {
            logo.SetActive(false);
            selectLevel.gameObject.SetActive(false);
            newGame.gameObject.SetActive(false);
            inputName.gameObject.SetActive(false);
            level1.gameObject.SetActive(false);
            level2.gameObject.SetActive(false);
            backToMainMenu.gameObject.SetActive(false);
            namePanel.gameObject.SetActive(false);
            nameSubmitHolder.SetActive(false);
            bNameSubmit.gameObject.SetActive(false);
        }
    }

    public void ReloadScene()
    {
        GetComponent<DearVRSource>().DearVRPlayOneShot(GetComponent<AudioSource>().clip);
        print("scene reload");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNewGame()
    {
        GetComponent<DearVRSource>().DearVRPlayOneShot(GetComponent<AudioSource>().clip);
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        GetComponent<DearVRSource>().DearVRPlayOneShot(GetComponent<AudioSource>().clip);
        SceneManager.LoadScene(0);
    }

    public void OpenSelectLevel()
    {
        GetComponent<DearVRSource>().DearVRPlayOneShot(GetComponent<AudioSource>().clip);
        menu = "select";
        logo.SetActive(false);
        gameOptions.gameObject.SetActive(false);
        newGame.gameObject.SetActive(false);
        inputName.gameObject.SetActive(false);
        selectLevel.gameObject.SetActive(false);

        if (SceneManager.GetActiveScene().name == "Title")
            backToMainMenu.gameObject.SetActive(true);
        else
            closeMenu.gameObject.SetActive(true);

        level1.gameObject.SetActive(true);
        level2.gameObject.SetActive(true);
    }

    public void LoadLevel1()
    {
        GetComponent<DearVRSource>().DearVRPlayOneShot(GetComponent<AudioSource>().clip);
        SceneManager.LoadScene(1);
    }

    public void LoadLevel2()
    {
        GetComponent<DearVRSource>().DearVRPlayOneShot(GetComponent<AudioSource>().clip);
        SceneManager.LoadScene(2);
    }

    public void BackToMainMenu()
    {
        GetComponent<DearVRSource>().DearVRPlayOneShot(GetComponent<AudioSource>().clip);
        if (menu == "select")
        {
            backToMainMenu.gameObject.SetActive(false);
            level1.gameObject.SetActive(false);
            level2.gameObject.SetActive(false);
        }
        else if (menu == "name")
        {
            foreach (Transform input in namePanel.transform.GetChild(0).GetComponent<Transform>())
            {
                Destroy(input.gameObject);
            }
            bNameSubmit.transform.GetChild(0).GetComponent<Text>().text = "Famous Hacker";
            bNameSubmit.gameObject.SetActive(false);
            backToMainMenu.gameObject.SetActive(false);
            namePanel.SetActive(false);
            nameSubmitHolder.SetActive(false);
        }

        menu = "main";
        logo.SetActive(true);
        gameOptions.gameObject.SetActive(true);
        newGame.gameObject.SetActive(true);
        inputName.gameObject.SetActive(true);
        selectLevel.gameObject.SetActive(true);
    }

    public void OpenNameInput()
    {
        GetComponent<DearVRSource>().DearVRPlayOneShot(GetComponent<AudioSource>().clip);
        menu = "name";
        logo.SetActive(false);
        gameOptions.gameObject.SetActive(false);
        newGame.gameObject.SetActive(false);
        inputName.gameObject.SetActive(false);
        selectLevel.gameObject.SetActive(false);

        backToMainMenu.gameObject.SetActive(true);
        namePanel.SetActive(true);
        nameSubmitHolder.SetActive(true);
        bNameSubmit.gameObject.SetActive(true);
        for (int i = 97; i < 123; i++)
        {
            GameObject btn = Instantiate(inputPrefab, namePanel.transform.GetChild(0));
            btn.name = "" + System.Convert.ToChar(i);
            btn.transform.GetChild(0).GetComponent<Text>().text = "" + System.Convert.ToChar(i);
        }
        for (int i = 48; i < 58; i++)
        {
            GameObject btn = Instantiate(inputPrefab, namePanel.transform.GetChild(0));
            btn.name = "" + System.Convert.ToChar(i);
            btn.transform.GetChild(0).GetComponent<Text>().text = "" + System.Convert.ToChar(i);
        }
        shiftBtn = Instantiate(inputPrefab, namePanel.transform.GetChild(0));
        shiftBtn.name = "Shift";
        shiftBtn.transform.GetChild(0).GetComponent<Text>().text = "Shift";
        GameObject spaceBtn = Instantiate(inputPrefab, namePanel.transform.GetChild(0));
        spaceBtn.name = "Space";
        spaceBtn.transform.GetChild(0).GetComponent<Text>().text = "Space";
        GameObject dltBtn = Instantiate(inputPrefab, namePanel.transform.GetChild(0));
        dltBtn.name = "Delete";
        dltBtn.transform.GetChild(0).GetComponent<Text>().text = "Delete";
    }

    public void InsertStringInput(string txt)
    {
        GetComponent<DearVRSource>().DearVRPlayOneShot(GetComponent<AudioSource>().clip);
        print("insert string input: " + txt);
        if (!shift)
            bNameSubmit.transform.GetChild(0).GetComponent<Text>().text += txt;
        else
        {
            int shiftedTextAsInt = System.Convert.ToInt32(txt[0]);
            print("shiftedtextasint: " + shiftedTextAsInt);
            char shiftedTextAsString = System.Convert.ToChar(shiftedTextAsInt - 32);
            print("shiftedtextasstring: " + shiftedTextAsString);
            bNameSubmit.transform.GetChild(0).GetComponent<Text>().text += shiftedTextAsString;
            shiftBtn.GetComponent<Image>().color = Color.white;
            shift = false;
        }
    }

    public void InsertNumberInput(string txt)
    {
        GetComponent<DearVRSource>().DearVRPlayOneShot(GetComponent<AudioSource>().clip);
        bNameSubmit.transform.GetChild(0).GetComponent<Text>().text += txt;
    }

    public void InsertSpace()
    {
        GetComponent<DearVRSource>().DearVRPlayOneShot(GetComponent<AudioSource>().clip);
        bNameSubmit.transform.GetChild(0).GetComponent<Text>().text += " ";
    }

    public void DeleteCharacter()
    {
        GetComponent<DearVRSource>().DearVRPlayOneShot(GetComponent<AudioSource>().clip);
        if (bNameSubmit.transform.GetChild(0).GetComponent<Text>().text.Length > 0)
            bNameSubmit.transform.GetChild(0).GetComponent<Text>().text = bNameSubmit.transform.GetChild(0).GetComponent<Text>().text.Remove(bNameSubmit.transform.GetChild(0).GetComponent<Text>().text.Length - 1);
    }

    public void ShiftCharacter()
    {
        GetComponent<DearVRSource>().DearVRPlayOneShot(GetComponent<AudioSource>().clip);
        shift = true;
        shiftBtn.GetComponent<Image>().color = Color.blue;
    }
    
    public void SubmitName()
    {
        GetComponent<DearVRSource>().DearVRPlayOneShot(GetComponent<AudioSource>().clip);
        StartCoroutine(SendData(bNameSubmit.transform.GetChild(0).GetComponent<Text>().text));
        BackToMainMenu();
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

    public void HighlightBtn(string txt)
    {
        StartCoroutine(HB(txt));
    }

    IEnumerator HB(string txt)
    {
        GameObject btn = GameObject.Find(txt);
        btn.GetComponent<Image>().color = btn.GetComponent<Button>().colors.pressedColor;
        yield return new WaitForSeconds(0.1f);
        btn.GetComponent<Image>().color = Color.white;
    }
}