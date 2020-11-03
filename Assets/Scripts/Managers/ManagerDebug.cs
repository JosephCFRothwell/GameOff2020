#region Snippet Information and Use
/* Creator Information
    *
    * Script Name: ManagerDebug
    * Author: Joseph CF Rothwell
*/

/* Steps for use
    *
    * 1) Do...
    */
#endregion

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.UI;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rothwell.Managers
{
    public class ManagerDebug : MonoBehaviour
    {
        #region Class Variables

        public bool inEditMode;
        #endregion

        
        private static GameObject _debugManagerObject;
        private static ManagerDebug _debugManagerInstance;

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        [SuppressMessage("ReSharper", "HeapView.ObjectAllocation")]
        [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeNullComparison")]
        public static ManagerDebug DMI
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

                if (_debugManagerInstance != null) return _debugManagerInstance;
                _debugManagerInstance = FindObjectOfType<ManagerDebug>();
                if (_debugManagerInstance != null) return _debugManagerInstance;
                _debugManagerInstance = new GameObject("ManagerDebug", typeof(ManagerDebug)).GetComponent<ManagerDebug>();

                #endregion

                #endregion

                return _debugManagerInstance;
            }
        }

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


            _debugManagerObject = gameObject;


            if (_debugManagerInstance == null)
            {
                _debugManagerInstance = this;
                DontDestroyMeOnLoad(_debugManagerObject);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        private void Update()
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            DebugKeyPress();
        }
        
        
        public void DebugMessage(string message)
        {
            if (!inEditMode) return;
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            Debug.Log($"ManagerDebug.Message(): {message}");

        }

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
        private void DebugKeyPress()
        {
            if (!inEditMode) return;
            if (Input.GetKeyUp(KeyCode.PageUp))
            {
                ManagerIO.IOMI.IO_ReadWriteConfigFile("audio", true);
            }
            if (Input.GetKeyUp(KeyCode.PageDown))
            {
                ManagerIO.IOMI.IO_ReadWriteConfigFile("audio");
            }


       
        }

        
        
        
        
        public void DebugLoadAsyncScene(string sceneName)
        {
            //This really needs some error handling for the sceneName string

            StartCoroutine(DebugLoadAsyncScene_Co(sceneName));

        }
        
        public IEnumerator DebugLoadAsyncScene_Co(string sceneName)
        {
            //This really needs some error handling for the sceneName string

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
        
        
        

        //Example region zone
        #region Example region
        #region Code Explanation
        /*
                    * Explanation of the code
                    */
        #endregion
        #region Example Code
        // Put code here
        #endregion
        #endregion
    }
}
