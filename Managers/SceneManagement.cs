using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    void Awake()
    {
        if(SceneManager.GetActiveScene().name=="Menu")
        {
            Cursor.visible = true;
        }
    }
    public void LoadScene(string scene)=>SceneManager.LoadSceneAsync(scene);
    public void ReloadScene()=>SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    public void ExitGame()=>Application.Quit();
    public void TestButton()=>Debug.Log("Working");
}
