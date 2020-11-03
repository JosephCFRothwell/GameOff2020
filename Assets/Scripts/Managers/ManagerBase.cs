
using UnityEngine;

namespace Rothwell.Managers
{
    public class ManagerBase : MonoBehaviour
    {
        private static GameObject _managerBaseObject;
        private static ManagerBase _managerBaseInstance;

        public static ManagerBase MBI
        {
            get
            {

                if (_managerBaseInstance != null) return _managerBaseInstance;
                _managerBaseInstance = FindObjectOfType<ManagerBase>();
                if (_managerBaseInstance != null) return _managerBaseInstance;
                _managerBaseInstance = new GameObject("ManagerBase", typeof(ManagerBase)).GetComponent<ManagerBase>();

                return _managerBaseInstance;
            }

            set => _managerBaseInstance = value;
        }
    }
}