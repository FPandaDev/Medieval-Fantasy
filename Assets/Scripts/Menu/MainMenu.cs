using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private float time;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private IEnumerator ChangeSceneAnim(string newScene)
    {
        anim.SetTrigger("End");

        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(newScene);
    }

    public void ChangeScene(string newScene)
    {
        StartCoroutine(ChangeSceneAnim(newScene));
    }

    public void ExitGame()
    {
        Debug.Log("Saliste");
        Application.Quit();
    }
}
