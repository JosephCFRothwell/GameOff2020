#region Snippet Information and Use

/* Creator Information
 *
 * Script Name: ManagerGameState
 * Author: Joseph CF Rothwell
*/

/* Steps for use
 *
 * 1) Do...
 */

#endregion


using System.Diagnostics.CodeAnalysis;
using Rothwell.State;
using UnityEngine;

namespace Rothwell.Managers
{
    public class ManagerGameState : MonoBehaviour
    {
        public string currentStateName;
        public bool creditsHaveStarted;
        public bool creditsHaveFinished;
        
        private static GameObject _gameStateManagerObject;
        private static ManagerGameState _gameStateManagerInstance;

        [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
        [SuppressMessage("ReSharper", "HeapView.ObjectAllocation")]
        public static ManagerGameState GSMI
        {
            get
            {
                #region Existence check

                #region Code explanation

                /* 
             * Find the Manager_Audio gameObject and set the variable. 
             * If there is no such game object, create one with this script and set variables.
             * If that object does exist but has no Manager_Audio component, create that component on it and set variables.
             */

                #endregion

                #region Existence check code

                if (_gameStateManagerInstance != null) return _gameStateManagerInstance;
                _gameStateManagerInstance = FindObjectOfType<ManagerGameState>();
                if (_gameStateManagerInstance != null) return _gameStateManagerInstance;
                _gameStateManagerInstance = new GameObject("ManagerGameState", typeof(ManagerGameState)).GetComponent<ManagerGameState>();

                #endregion

                #endregion

                return _gameStateManagerInstance;
            }

            set { _gameStateManagerInstance = value; }
        }

        private IGameState _currentState;

        
        public static void DontDestroyMeOnLoad(GameObject thisObject)
        {
            // This protects this, and objects above it (eg Managers gameObject), from being destroyed on load
            // This also means don't need to protect other manager classes?
            
            
            
            Transform parentTransform = thisObject.transform;

            // If this object doesn't have a parent then its the root transform.
            while (parentTransform.parent != null)
            {
                // Keep going up the chain.
                parentTransform = parentTransform.parent;
            }
            
            
            
            DontDestroyOnLoad(parentTransform.gameObject);
        }
        private void Awake()
        {


            _gameStateManagerObject = gameObject;


            if (_gameStateManagerInstance == null)
            {
                _gameStateManagerInstance = this;
                DontDestroyMeOnLoad(_gameStateManagerObject);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

    }
}