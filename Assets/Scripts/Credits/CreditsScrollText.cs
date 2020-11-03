#region Snippet Information and Use
/* Creator Information
 *
 * Script Name: CreditsScrollText
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

namespace Rothwell.Credits
{
    public class CreditsScrollText : MonoBehaviour
    {
        private Rigidbody2D _rb;
        #region Class Variables
    
        #endregion

        void Start()
        {
            _rb = gameObject.GetComponent<Rigidbody2D>();
        
        
            var velocity = _rb.velocity;
            velocity = new Vector2(velocity.x, velocity.y +1);
            _rb.velocity = velocity;
        }




    }
}
