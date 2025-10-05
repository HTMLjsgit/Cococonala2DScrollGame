using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager instance;

    [SerializeField]
    private CanvasGroup fadeCanvasGroup;
    [SerializeField]
    private float fadeDuration = 0.5f;

    // Awake, LoadScene, FadeIn, FadeOut メソッドは変更なし ...
    // (省略)
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            // 最初のシーン読み込み時にフェードアウトから開始
            fadeCanvasGroup.alpha = 1f;
            FadeOut();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        // フェードインが完了したら、非同期ロードを開始する
        FadeIn().OnComplete(() =>
        {
            StartCoroutine(LoadSceneAsyncRoutine(sceneName));
        });
    }
    
    // ▼▼▼ このメソッドを修正 ▼▼▼
    private IEnumerator LoadSceneAsyncRoutine(string sceneName)
    {
        // 非同期でシーンをロード開始
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        // シーンの読み込みが完了しても、自動で有効化しない
        asyncLoad.allowSceneActivation = false;

        // ロードが完了するまで待機
        while (asyncLoad.progress < 0.9f)
        {
            yield return null; // 1フレーム待つ
        }
        
        // ▼▼▼ ここが重要 ▼▼▼
        
        // 1. シーンの有効化を許可する（この時点ではまだ画面は暗いまま）
        asyncLoad.allowSceneActivation = true;

        // 2. シーンの有効化が完了するまで待つ
        yield return new WaitUntil(() => asyncLoad.isDone);

        // 3. 完全にシーンが切り替わった後で、フェードアウト（明転）を開始する
        FadeOut();
    }
    // ▲▲▲ 修正ここまで ▲▲▲

    private Tween FadeIn()
    {
        return fadeCanvasGroup.DOFade(1f, fadeDuration)
            .OnStart(() => fadeCanvasGroup.blocksRaycasts = true);
    }

    private Tween FadeOut()
    {
        return fadeCanvasGroup.DOFade(0f, fadeDuration)
            .OnComplete(() => fadeCanvasGroup.blocksRaycasts = false);
    }
}