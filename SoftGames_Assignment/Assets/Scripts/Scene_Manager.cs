using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    public void LoadScene(int i)
    {
        SceneManager.LoadScene(i);
    }
}
