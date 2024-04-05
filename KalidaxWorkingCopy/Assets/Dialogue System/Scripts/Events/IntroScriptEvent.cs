using DS_Events;

using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScriptEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DS_GameEvents.Instance.m_IntroScriptAction += ChangeScene;
    }
    private void OnDisable()
    {
        DS_GameEvents.Instance.m_SellAlienAction -= ChangeScene;
    }
    private void ChangeScene()
    {
        Debug.Log("WORKED");
        SceneManager.LoadScene("SebasScene");
    }
}
