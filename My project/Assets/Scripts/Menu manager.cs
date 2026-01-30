using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menumanager : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void startfonks(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
