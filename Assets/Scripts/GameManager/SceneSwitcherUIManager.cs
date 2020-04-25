using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public RectTransform panelButton;
    public RectTransform container;
    public GameObject ButtonPrefab;

    private List<SceneChangeButton> _buttons = new List<SceneChangeButton>();
    private Canvas _canvas = null;

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
        _canvas = GetComponentInParent<Canvas>();
        InitializeButtons();
        UpdateButtons();
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => UpdateButtons();
    }

    private void Update()
    {
        if (container.gameObject.activeSelf && IsClickOutside(container) && IsClickOutside(panelButton))
            container.gameObject.SetActive(false);
    }

    private void UpdateButtons()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        foreach (var item in _buttons) {
            if (item.SceneName == currentScene)
                item.gameObject.SetActive(false);
            else if (!item.gameObject.activeSelf)
                item.gameObject.SetActive(true);
        }
        container.gameObject.SetActive(false);
    }

     private bool IsClickOutside(RectTransform p_rectTransform)
     {
        if (Input.GetMouseButtonDown(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(p_rectTransform, Input.mousePosition, null))
                return (true);
        }
        return (false);
    }

    private void InitializeButtons()
    {
        foreach (var item in ScenesList)
        {
            GameObject obj = Instantiate(ButtonPrefab, container.transform);
            Image img = obj.GetComponent<Image>();
            Button btn = obj.GetComponent<Button>();
            SceneChangeButton changeSceneBtn = obj.GetComponent<SceneChangeButton>();

            img.preserveAspect = true;
            img.sprite = item.icon;
            btn.onClick.AddListener(UpdateButtons);
            changeSceneBtn.SceneName = item.name;
            obj.name = item.name + " button";
            _buttons.Add(changeSceneBtn);
        }
        container.gameObject.SetActive(false);
    }

}
