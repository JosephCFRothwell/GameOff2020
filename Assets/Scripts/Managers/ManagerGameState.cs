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
using UnityEngine;

namespace Rothwell.Managers
{
    public class ManagerGameState : MonoBehaviour
    {
        public string defaultState = "StatePlayPlatformer";
        public string currentStateName = "";

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


    }
}