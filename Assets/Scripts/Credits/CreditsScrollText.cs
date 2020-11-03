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

public class CreditsScrollText : MonoBehaviour
{
    private Rigidbody2D rb;
    #region Class Variables
    
    #endregion

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        
        
        var velocity = rb.velocity;
        velocity = new Vector2(velocity.x, velocity.y +1);
        rb.velocity = velocity;
    }

    void Update()
    {
        
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
