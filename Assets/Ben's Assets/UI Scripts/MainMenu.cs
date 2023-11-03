using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelection : MonoBehaviour
{
    public void AccessPlayGame ()
    {
        SceneManager.LoadScene("Ben's Scene 2 (Level Design)");
    }

    public void AccessMainMenu()
    {
        SceneManager.LoadScene("Ben's Scene");
    }
}