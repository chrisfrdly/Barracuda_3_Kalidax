using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfDayResultsScreen : MonoBehaviour
{
    [SerializeField] private GameObject verticalLayoutGroupGO;

    private List<GameObject> rewardPanels = new List<GameObject>();
    public string sceneToTransitionTo;

    private void Awake()
    {
        //display screenshot of end of day

    }

    private void Start()
    {
        for(int i = 0; i < verticalLayoutGroupGO.transform.childCount; i++)
        {
            rewardPanels.Add(verticalLayoutGroupGO.transform.GetChild(i).gameObject);
        }
        
        StartCoroutine(ResultsAnimation());
    }

    IEnumerator ResultsAnimation()
    {
        //lop through vertical layout group children and set animation to middle of screen

        foreach(GameObject panel in rewardPanels) 
        {
            LeanTween.moveX(panel, verticalLayoutGroupGO.transform.position.x, 1).setEaseOutBack();
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene(sceneToTransitionTo);
    }
}
