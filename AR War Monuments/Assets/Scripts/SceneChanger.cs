using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
   public void LoadScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void LoadHomeScene()
    {
        SceneManager.LoadScene("HomePage");
    }
}
