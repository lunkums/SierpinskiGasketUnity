using UnityEngine;

public class QuitButton : MonoBehaviour
{
    private void Awake()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            gameObject.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
