using PlayerSystem;
using Service;
using UnityEngine;
using UnityEngine.SceneManagement;
using XenositeFramework.SceneSystem;

namespace XenositeFramework.SystemClass
{
    public class SystemClassInitializer : MonoBehaviour
    {
        private void Awake()
        {
            Destroy(gameObject);
        }
    }
}
