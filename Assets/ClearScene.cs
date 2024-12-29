using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearScene : MonoBehaviour
{
    public void OnClickClearButton()
    {
        SceneManager.LoadScene("StartScene");
    }
}
