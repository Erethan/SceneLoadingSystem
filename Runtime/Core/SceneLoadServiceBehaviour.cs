using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Erethan.ScriptableServices;

namespace Erethan.ScreneTransition
{
    public class SceneLoadServiceBehaviour : ScriptableServiceBehaviour
    {
        public AssetReference LoadSceneAsset { get; set; }
        public AssetReference TransitionPrefab { 
            set
            {
                _hasTransition = value != null;
                if (!_hasTransition)
                {
                    Destroy(_screenTransitionGameObject);
                    _screenTransition = null;
                    _screenTransitionGameObject = null;
                    return;
                }

                value.InstantiateAsync(transform).Completed += OnTransitionInstantiated; //TODO: Move this to Initialize
            }
        }

        public bool Loading { get; private set; }


        private GameObject _screenTransitionGameObject;
        private IScreenTransition _screenTransition;
        private bool _hasTransition;


        public override void Initialize(){}

        private AsyncOperationHandle<SceneInstance> _loadOperation;
        public float Progress
        {
            get
            {
                if (!_loadOperation.IsValid())
                    return 1f;
                return _loadOperation.PercentComplete;
            }
        }


        private void OnTransitionInstantiated(AsyncOperationHandle<GameObject> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                _screenTransitionGameObject = obj.Result;
                _screenTransition = _screenTransitionGameObject.GetComponent<IScreenTransition>();
            }
        }


        public void LoadScene(AssetReference scene)
        {
            if (Loading == true)
            {
                Debug.LogWarning($"Cannot load scene {scene.SubObjectName} while another is currently loading.");
                return;
            }
            Loading = true;

            StartCoroutine(nameof(LoadRoutine), scene);
        }


        private IEnumerator LoadRoutine(AssetReference targetScene)
        {
            if (_hasTransition && _screenTransition == null)
            {
                yield return new WaitUntil(() => _screenTransition != null);
            }
            yield return SceneChange(LoadSceneAsset);
            yield return SceneChange(targetScene);
            Loading = false;
        }

        private IEnumerator SceneChange(AssetReference scene)
        {
            bool transitionLoaded = _screenTransition != null;

            if (transitionLoaded)
            {
                _screenTransition.FadeOut();
            }

            _loadOperation = Addressables.LoadSceneAsync(scene, LoadSceneMode.Single, activateOnLoad: false);

            yield return _loadOperation;
            if (transitionLoaded)
            {
                yield return new WaitUntil(() => _screenTransition.Faded);
            }

            yield return _loadOperation.Result.ActivateAsync();
            if (transitionLoaded)
            {
                _screenTransition.FadeIn();
                yield return new WaitUntil(() => !_screenTransition.Faded);
            }


        }
    }



}