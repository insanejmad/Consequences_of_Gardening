using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneSwitcherUIManager : MonoBehaviour
{

    public static SceneSwitcherUIManager instance = null;

    public List<string> ScenesList;

    public GameObject container;

    public GameObject ButtonPrefab;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeButtons();

    }

    public void InitializeButtons()
    {
        foreach (string name in ScenesList)
        {
            GameObject obj = Instantiate(ButtonPrefab) as GameObject;
            obj.GetComponent<SceneChangeButton>().SceneName = name;
            obj.transform.SetParent(container.transform, false);
            obj.transform.GetChild(0).gameObject.GetComponent<Text>().text = name;
            }
        container.SetActive(false);
    }

}
