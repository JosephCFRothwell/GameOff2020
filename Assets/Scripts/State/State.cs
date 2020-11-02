#region Snippet Information and Use

/* Creator Information
 *
 * Script Name: State
 * Author: Joseph CF Rothwell
 * Note: This is the top level State class
*/

/* Steps for use
 *
 * 1) Do...
 */

#endregion

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.UI;
//using System;
//using UnityEngine;


namespace Rothwell.State
{
    public interface IGameState
    {
        IGameState DoState(StateSearch state);
    }
    
}