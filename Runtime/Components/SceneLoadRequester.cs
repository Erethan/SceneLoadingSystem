using UnityEngine;
using UnityEngine.AddressableAssets;
using Erethan.ScriptableServices;

namespace Erethan.ScreneTransition.Components
{
    public class SceneLoadRequester : MonoBehaviour
    {
        [SerializeField] private SceneLoadService _loadSystem;
        [SerializeField] private AssetReference _sceneToLoad;


        public void Request()
        {
            _loadSystem.LoadScene(_sceneToLoad);
        }
    }
}