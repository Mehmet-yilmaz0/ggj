using UnityEngine;
using UnityEngine.SceneManagement;

public class Menumanager : MonoBehaviour
{
    public void startfonks(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
