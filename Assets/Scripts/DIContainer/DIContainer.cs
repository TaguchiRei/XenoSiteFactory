using UnityEngine;
using UnityEngine.SceneManagement;

namespace DIContainer
{
    public class DiContainer : MonoBehaviour
    {
        [SerializeField] private Scene _managerScene;

        public static DiContainer Instance;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Register<T>(T instance)
        {
            
        }

        public void Unregister<T>(T instance)
        {
            
        }

        public bool TryGet<T>(out T instance)
        {
            
        }

        private void Start()
        {
            SceneManager.LoadScene(_managerScene.buildIndex);
        }
    }
}
