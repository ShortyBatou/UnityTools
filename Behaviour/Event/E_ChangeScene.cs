using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class E_ChangeScene : MonoBehaviour, I_Event
{
    public string scene;
    public void Activate()
    {
        SceneManager.LoadScene(scene);
    }
}
