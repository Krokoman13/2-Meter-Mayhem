using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb = null;    
    [SerializeField] Transform forwardCam = null;
    [SerializeField] Animator anim = null;
    [Space(10)]

    [Header("Player Movement Variables")]
    [SerializeField, Tooltip("The speed at which the player picks up momentum when attempting to move.")]
    float acceleration = 1f;
    [SerializeField, Tooltip("The max speed the player's momentum will be clamped at.")]
    float maxSpeed = 13f;
    [SerializeField, Tooltip("The speed at which the player will lose momentum when not giving any movement input.")]
    float decceleration = 3f;

    [Header("Rotate Variables")]
    [SerializeField, Tooltip("When CarMovement == false, this will determine the player's rotation speed.")]
    float rotateSpeed = 80f;
    [SerializeField, Tooltip("When CarMovement = true, this will determine the player's rotation speed.")]
    float carRotateSpeed = 40f;    


    void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void Start()
    {
        anim = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Animator>();
    }

    public void DoMove(Vector2 input)
    {
        Vector3 velo = forwardCam.InverseTransformDirection(rb.velocity);

        //Make sure the player has a speed cap.
        Vector2 clampedVelo = Vector2.ClampMagnitude(new Vector2(velo.x, velo.z), maxSpeed);
        velo = new Vector3(clampedVelo.x, velo.y, clampedVelo.y);

        //Horizontal
        if (input.x != 0)        
            velo.x += input.x * acceleration * Time.fixedDeltaTime;        
        else
            velo.x = Mathf.Lerp(velo.x, 0, decceleration * Time.fixedDeltaTime);

        //Vertical
        if (input.y != 0)
            velo.z += input.y * acceleration * Time.fixedDeltaTime;
        else
            velo.z = Mathf.Lerp(velo.z, 0, decceleration * Time.fixedDeltaTime);


        #region classic movement
        //if (input != Vector2.zero)
        //{
        //    //velo.z += acceleration * Time.fixedDeltaTime;
        //    Vector3 veloInput = new Vector3(input.x, 0, input.y);
        //    velo += veloInput * acceleration * Time.fixedDeltaTime;
        //}
        //else
        //{
        //    velo = Vector3.Lerp(velo, Vector3.zero, decceleration * Time.fixedDeltaTime);
        //}
        #endregion

        rb.velocity = forwardCam.TransformDirection(velo);
        anim.SetBool("Walking", input.magnitude > 0.1f);
    }



    
    public void DoRotate(Vector2 input)
    {        
        //If there's no input, no need to rotate
        if (input == Vector2.zero)
            return;

        Vector3 lookatPos = transform.position + new Vector3(input.x, 0, input.y);

        Vector3 targetDir = lookatPos - transform.position;
        float singleStep = rotateSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDir, singleStep, 0.0f);        

        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    public void CarMovement(Vector2 input)
    {        
        //Rotation
        Vector3 euler = new Vector3(0, input.x * carRotateSpeed, 0);
        transform.Rotate(euler);

        //Set the velocity variable be relevant to the player, instead of the world. This makes it so Z will be the player's forward.
        Vector3 velo = transform.InverseTransformDirection(rb.velocity);


        //Make sure the player has a speed cap.
        Vector2 clampedVelo = Vector2.ClampMagnitude(new Vector2(velo.x, velo.z), maxSpeed);
        velo = new Vector3(clampedVelo.x, velo.y, clampedVelo.y);

        //Moving
        if (input.y > 0)
            velo.z += acceleration * Time.fixedDeltaTime;
        else if (input.y < 0 && velo.z > 0)
            velo.z -= acceleration * Time.fixedDeltaTime;

        //Not moving
        else
            velo = Vector3.Lerp(velo, Vector3.zero, decceleration * Time.fixedDeltaTime);

        anim.SetBool("Walking", input.magnitude > 0.1f);
        //Revert the velocity variable back to world-space so it'll be properly applied
        rb.velocity = transform.TransformDirection(velo);
    }
}