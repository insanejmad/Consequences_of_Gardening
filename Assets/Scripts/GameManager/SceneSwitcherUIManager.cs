using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SceneItem
{
    public string name;
    public Sprite icon;
}

public class SceneSwitcherUIManager : MonoBehaviour
{

    public static SceneSwitcherUIManager instance = null;

    public List<SceneItem> ScenesList;

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
        foreach (var item in ScenesList)
        {
	    Image img = null;
            GameObject obj = Instantiate(ButtonPrefab) as GameObject;
	    img = obj.GetComponent<Image>();
	    img.preserveAspect = true;
	    img.sprite = item.icon;
            obj.GetComponent<SceneChangeButton>().SceneName = item.name;
            obj.transform.SetParent(container.transform, false);
        }
        container.SetActive(false);
    }

}
