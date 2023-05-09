using UnityEngine;
using UnityEngine.SceneManagement;

public class AboutButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("About");
    }
}
