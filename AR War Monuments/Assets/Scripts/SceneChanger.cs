using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class SceneChanger : MonoBehaviour
{
    public void LoadScene()
    {
        LoaderUtility.Initialize();
        SceneManager.LoadScene("MainScene");
    }   
    public void LoadSceneWithBackground()
    {
        LoaderUtility.Initialize();
        SceneManager.LoadScene("MainScene With Background");
    }

    public void LoadHomeScene()
    {
        SceneManager.LoadScene("HomePage");
        LoaderUtility.Deinitialize();
    }
}
