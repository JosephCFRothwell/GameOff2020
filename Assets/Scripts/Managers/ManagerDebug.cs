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
using UnityEngine;

namespace Rothwell.Managers
{
    public class ManagerDebug : MonoBehaviour
    {
        #region Class Variables

        public bool inEditMode;
        #endregion

        private void Awake()
        {
            inEditMode = false;
        }
                
        public void DebugMessage(string message)
        {
            if (!inEditMode) return;
            Debug.Log($"ManagerDebug.Message(): {message}");

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
