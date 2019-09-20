using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadNextScreen()
    {
       // UnityEngine.SceneManagement.SceneManager.LoadScene("ResultsWin");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Rules");
    }
}
