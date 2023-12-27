using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCartHandler : MonoBehaviour
{
    public List<Prop> obtainedGroceryItems = new List<Prop>();
    List<Collider> propCols = new List<Collider>();
    public Transform fill = null;
    

    //Fill dimensions
    Vector3 topLeft = Vector3.zero;
    Vector3 topRight = Vector3.zero;
    Vector3 bottomLeft = Vector3.zero;
    Vector3 bottomRight = Vector3.zero;
    Vector3 lowCenter = Vector3.zero;
    Vector3 highCenter = Vector3.zero;


    void LateUpdate()
    {
        UpdateFillBoundaries();
        ClampGroceries();
        
        //DEBUG_DisplayLines();        
    }

    void UpdateFillBoundaries()
    {
        Vector3 halfExtents = new Vector3(fill.localScale.x / 2, fill.localScale.y / 2, fill.localScale.z / 2);

        //The local corner positions
        //Top and bottom are seen as "further" and "closer" to the player respectively. (as if you're looking from above)
        topLeft = new Vector3(fill.localPosition.x - halfExtents.x, fill.localPosition.y + halfExtents.y, fill.localPosition.z + halfExtents.z);
        topRight = new Vector3(fill.localPosition.x + halfExtents.x, topLeft.y, topLeft.z);
        bottomLeft = new Vector3(topLeft.x, topLeft.y, fill.localPosition.z - halfExtents.z);
        bottomRight = new Vector3(topRight.x, topLeft.y, bottomLeft.z);

        //High and low positions (Y-axis)
        lowCenter = new Vector3(fill.localPosition.x, fill.localPosition.y - halfExtents.y, fill.localPosition.z);
        highCenter = new Vector3(lowCenter.x, fill.localPosition.y + halfExtents.y, lowCenter.z);
        
        //Remove offset
        topLeft -= fill.localPosition;
        topRight -= fill.localPosition;
        bottomLeft -= fill.localPosition;
        bottomRight -= fill.localPosition;

        lowCenter -= fill.localPosition;
        highCenter -= fill.localPosition;
    }


    public float bounciness = 0.4f;
    /// <summary>
    /// Use the fill's boundaries to clamp all groceries inside it.
    /// </summary>
    void ClampGroceries()
    {
        foreach(Prop p in obtainedGroceryItems)
        {
            if(p == null)
            {
                obtainedGroceryItems.Remove(p);
                continue;
            }
            Transform t = p.transform;
            Rigidbody rb = p.rb;
            Collider col = p.col;

            //CLAMP GROCERY
            //Convert the ball's position to be relative to the Cart
            Vector3 ballPos = fill.InverseTransformPoint(t.position); 
            Vector3 ballsizeHalf = t.localScale / 2;        //Get a variable that represents the balls size in half
            
            float halfSizeX = col.bounds.size.x;
            float halfSizeY = col.bounds.size.y/2;
            float halfSizeZ = col.bounds.size.z;

            //Prep the clamp variables, keeping in mind that the origin's potential offset in position and scale.
            Vector2 clampX = new Vector2(topLeft.x / fill.localScale.x + halfSizeX, topRight.x / fill.localScale.x - halfSizeX);
            Vector2 clampY = new Vector2(lowCenter.y / fill.localScale.x + halfSizeY, highCenter.y / fill.localScale.y - halfSizeY);
            Vector2 clampZ = new Vector2(bottomLeft.z / fill.localScale.z + halfSizeZ, topLeft.z / fill.localScale.z - halfSizeZ);



            #region velocityClamp attempt
            //Vector3 velo = fill.InverseTransformDirection(rb.velocity);

            ////Clamp and bounce on all axis

            ////X AXIS
            //if (ballPos.x < clampX.x)
            //{
            //    velo.x = -velo.x * bounciness;
            //    ballPos.x = clampX.x;
            //}
            //else if (ballPos.x > clampX.y)
            //{
            //    velo.x = -velo.x * bounciness;
            //    ballPos.x = clampX.y;
            //}

            ////Y AXIS
            //if (ballPos.y < clampY.x)
            //{
            //    velo.y = -velo.y * bounciness;
            //    ballPos.y = clampY.x;
            //}
            //else if (ballPos.y > clampY.y)
            //{
            //    velo.y = -velo.y * bounciness;
            //    ballPos.y = clampY.y;
            //}

            ////Z AXIS
            //if (ballPos.z < clampZ.x)
            //{
            //    velo.z = -velo.z * bounciness;
            //    ballPos.z = clampZ.x;
            //}
            //else if (ballPos.z > clampZ.y)
            //{
            //    velo.z = -velo.z * bounciness;
            //    ballPos.z = clampZ.y;
            //}

            //rb.velocity = fill.TransformDirection(velo);
            #endregion

            //Apply the clamping
            ballPos.x = Mathf.Clamp(ballPos.x, clampX.x, clampX.y);
            ballPos.y = Mathf.Clamp(ballPos.y, clampY.x, clampY.y);
            ballPos.z = Mathf.Clamp(ballPos.z, clampZ.x, clampZ.y);

            //Now that the relative position is clamped, revert it back to world in order to apply it.
            t.position = fill.TransformPoint(ballPos);
        }       
    }


    void DEBUG_DisplayLines()
    {
        //Apply the dimensions to the right position to test it in world-space        
        Vector3 dif = fill.position;
        Vector3 w_topLeft = fill.TransformDirection(topLeft) + dif;
        Vector3 w_topRight = fill.TransformDirection(topRight) + dif;
        Vector3 w_bottomLeft = fill.TransformDirection(bottomLeft) + dif;
        Vector3 w_bottomRight = fill.TransformDirection(bottomRight) + dif;

        Vector3 w_lowCenter = fill.TransformDirection(lowCenter) + dif;
        Vector3 w_highCenter = fill.TransformDirection(highCenter) + dif;

        //Display them using Debug.Drawline. Note: Gizmos should be turned on for this to be visible. They liens might appear inside the model.
        Debug.DrawLine(w_topLeft, w_topRight, Color.magenta);
        Debug.DrawLine(w_bottomLeft, w_bottomRight, Color.magenta);
        Debug.DrawLine(w_topLeft, w_bottomLeft, Color.magenta);
        Debug.DrawLine(w_topRight, w_bottomRight, Color.magenta);

        Debug.DrawLine(w_lowCenter, w_highCenter, Color.blue);
    }
}