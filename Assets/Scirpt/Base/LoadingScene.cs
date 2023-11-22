using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    #region 변수
    [SerializeField] private Image ProgressBar;

    private static string NextScene;
    #endregion // 변수

    #region 함수
    /** 초기화 */
    private void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    /** 씬을 로드한다 */
    public static void LoadScene(string SceneName)
    {
        NextScene = SceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    /** 로딩중일때 Bar를 채운다 */
    private IEnumerator LoadSceneProcess()
    {
        AsyncOperation Oper = SceneManager.LoadSceneAsync(NextScene);
        Oper.allowSceneActivation = false;

        float Timer = 0.0f;
        while (!Oper.isDone)
        {
            yield return null;

            if (Oper.progress < 0.9f)
            {
                ProgressBar.fillAmount = Oper.progress;
            }
            else
            {
                Timer += Time.unscaledDeltaTime;
                ProgressBar.fillAmount = Mathf.Lerp(0.9f, 1f, Timer);
                if (ProgressBar.fillAmount >= 1f)
                {
                    Oper.allowSceneActivation = true;
                }
            }
        }
    }
    #endregion // 함수
}
