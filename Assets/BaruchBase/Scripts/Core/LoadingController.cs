using System;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Baruch.UI;

namespace Baruch
{
    [AddComponentMenu("Baruch/Scene Management/Loading Controller"), DisallowMultipleComponent]
    public class LoadingController : MonoBehaviour
    {
        [SerializeField] private SlicedFilledImage loadingBarFill = default;
        [SerializeField] private LoadingMode loadingMode = default;
        private AsyncOperation loadingOperation = default;

        private void Start()
        {
            if (loadingMode is LoadingMode.PlaceHolder)
            {
                loadingBarFill.fillAmount = 0f;
                DOTween.To(() => loadingBarFill.fillAmount, (x) => loadingBarFill.fillAmount = x, 1f, 1f);
                Invoke(nameof(LoadNextScene), 1.25f);
            }
            else LoadNextScene();
        }

        private void LoadNextScene()
        {
#if UNITY_EDITOR
            if ((EditorBuildSettings.scenes.Length - 1) < SceneManager.GetActiveScene().buildIndex + 1)
            {
                throw new ArgumentOutOfRangeException("",
                    $"There is no scene with the specified build index: {SceneManager.GetActiveScene().buildIndex + 1}. Please check your editor build settings!");
            }
#endif
            LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
        private void LoadScene(int sceneIndex)
        {
            loadingOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
            loadingOperation.completed += OnTargetSceneLoaded;
        }

        private void Update()
        {
            if (loadingMode is LoadingMode.PlaceHolder || loadingOperation is null) return;
            loadingBarFill.fillAmount = loadingOperation.progress;
            if (loadingOperation.isDone) loadingBarFill.fillAmount = 1f;
        }

        private void OnTargetSceneLoaded(AsyncOperation operation)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        }

        private enum LoadingMode
        {
            PlaceHolder,
            Realtime
        }
    }
}