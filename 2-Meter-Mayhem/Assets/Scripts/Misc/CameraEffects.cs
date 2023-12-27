using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    [Header("Screenshake")]
    [SerializeField, Tooltip("The Cinemachine component, attached to the virtual camera.")]
    CinemachineVirtualCamera vcam = null;
    CinemachineBasicMultiChannelPerlin perlin = null;   //The specific perlin module from the cinemachine vcam.

    [SerializeField, Tooltip("This noise profile will be set when screenshake is active.")]
    NoiseSettings noiseProfile = null;

    [Header("Camera Orientation")]
    [SerializeField]Transform camT = null;  //Main camera's transform
    [SerializeField]Transform forwardCamera = null;

    [Header("Component References")]
    [SerializeField] ExposureMeter exposureMeter = null;

    void Awake()
    {
        perlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();        
    }
    void Start()
    {
        DeactivateNoise();  //Game shouldn't have noise active for no reason.
        forwardCamera.eulerAngles = new Vector3(0, camT.eulerAngles.y, 0);
    }


    private void FixedUpdate()
    {
        if(exposureMeter.state == ExposureMeter.states.hurting)
        {
            ActivateNoise();
        }else
        {
            DeactivateNoise();
        }
    }






    /// <summary>
    /// Activates noise by setting the noise profile to the assigned one in the Inspector.
    /// </summary>
    void ActivateNoise()
    {
        perlin.m_NoiseProfile = noiseProfile;
    }
    /// <summary>
    /// Sets the noise profile to null, effectively deactivating noise for the virtual camera.
    /// </summary>
    void DeactivateNoise()
    {
        perlin.m_NoiseProfile = null;
    }
}