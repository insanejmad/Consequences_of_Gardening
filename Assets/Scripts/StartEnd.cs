using UnityEngine;
using UnityEngine.SceneManagement;

public class StartEnd : MonoBehaviour
{
    public bool IsEnding = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsEnding)
                SceneManager.LoadScene("Room");
            else
                Application.Quit();
        }
    }
}
