using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameObjectActivationButton : MonoBehaviour
{
    Button button;

    public GameObject goToModify;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Execute);
    }

    private void Execute()
    {
        goToModify.SetActive(!goToModify.activeInHierarchy);
    }
}
