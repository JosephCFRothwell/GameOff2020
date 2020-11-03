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
using Rothwell.Managers;
using UnityEngine;

namespace Rothwell.Credits
{
 public class CreditsEndMech : MonoBehaviour
 {

  private void OnCollisionEnter2D()
  {
   ManagerGameState.GSMI.creditsHaveFinished = true;
  }


 }
}
