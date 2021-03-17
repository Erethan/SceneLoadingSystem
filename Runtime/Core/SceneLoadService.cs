using UnityEngine;
using UnityEngine.AddressableAssets;

using Erethan.ScriptableServices;

namespace Erethan.ScreneTransition
{

    [CreateAssetMenu(fileName = "Scene Loading Service", menuName = "Erethan/Scene Management/Scene Loading Service")]
    public class SceneLoadService : ScriptableService<SceneLoadServiceBehaviour> 
    {
        [SerializeField] private AssetReference _loadingScene;
        [SerializeField] private AssetReference _transitionPrefab;

        public float Progress => ControllerBehaviour.Progress;
        public void LoadScene(AssetReference scene) => ControllerBehaviour.LoadScene(scene);

        public AssetReference LoadingScene => _loadingScene;

        protected override void ConfigureBehaviour()
        {
            ControllerBehaviour.LoadSceneAsset = _loadingScene;
            ControllerBehaviour.TransitionPrefab = _transitionPrefab;
        }
    }
}