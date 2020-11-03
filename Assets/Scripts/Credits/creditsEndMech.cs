#region Snippet Information and Use
/* Creator Information
 *
 * Script Name: creditsEndMech
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
using System;
using UnityEngine;
using Rothwell.State;
using Rothwell.Managers;

public class creditsEndMech : MonoBehaviour
{
    #region Class Variables
    
    #endregion

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
     ManagerGameState.GSMI.creditsHaveFinished = true;
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
