using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneChangeButton : MonoBehaviour
{
    Button button;

    public string SceneName;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Execute);
    }

    public delegate void SceneChangeEvent(string name);
    public static SceneChangeEvent OnSceneChange;

    private void Execute()
    {
        GameManager.instance.ChangeScene(SceneName);
    }
}
