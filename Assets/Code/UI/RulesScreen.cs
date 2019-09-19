using UnityEngine;
using System.Collections;

public class RulesScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadNextScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Build");
    }
}
