using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VRMovement : MonoBehaviour
{

    public XRNode inputSource;
    public float speed = 1.5f;
    public LayerMask groundLayer;
    public float additionalHeight = .20f; 

    public float gravity = -9.8f;
    private float fallingSpeed = 0f;
    
    private XROrigin rig;
    private Vector2 inputAccess;
    private CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rig = GetComponent<XROrigin>();   

    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource); 
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAccess);
    }

    private void FixedUpdate()
    {
        followHeadet();

        Quaternion headYaw = Quaternion.Euler(0, rig.Camera.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAccess.x, 0, inputAccess.y);

        characterController.Move(direction * Time.fixedDeltaTime * speed);

        //gravity
        bool isGrounded = CheckGrounded();
        if (isGrounded)
            fallingSpeed = 0;
        else 
            fallingSpeed += Time.fixedDeltaTime * gravity;
            
        
        characterController.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    }

    void followHeadet()
    {   
        characterController.height = rig.CameraInOriginSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.Camera.transform.position);
        characterController.center = new Vector3(capsuleCenter.x, characterController.height/2 + characterController.skinWidth, capsuleCenter.z);
    }
    bool CheckGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(characterController.center);
        float rayLength = characterController.center.y + 0.01f;
        bool hit = Physics.SphereCast(rayStart, characterController.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hit;    
    }
}
