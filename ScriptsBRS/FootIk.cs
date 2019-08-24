using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIk : MonoBehaviour {

    private Animator animator;
    public Transform lookObj;
    public Vector3 rightFootPosWorld;
    public Vector3 leftFootPosWorld;
    public Vector3 leftFootPos;
    public Vector3 rightFootPos;
    public Transform leftFoot;
    public Transform rightFoot;
    public float newPosThreshold = 0.5f;
   
    public bool ikActive = true;
    // Use this for initialization
    void Start()
    {
        Debug.Log("Yes it works");
        animator = GetComponent<Animator>();
        leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
        rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
    }
    private void Update()
    {
        RaycastHit leftRay;
        RaycastHit rightRay;

        leftFootPosWorld = leftFoot.TransformPoint(Vector3.zero);
        rightFootPosWorld = rightFoot.TransformPoint(Vector3.zero);
        if (animator.GetFloat("LeftFoot") < newPosThreshold)
        {
            if (Physics.Raycast(leftFootPosWorld, -Vector3.up, out leftRay, 1f))
            {
                leftFootPos = leftRay.point;
            }
        }
        if (animator.GetFloat("RightFoot") < newPosThreshold)
        {
            if (Physics.Raycast(rightFootPosWorld, -Vector3.up, out rightRay, 1f))
            {
                rightFootPos = rightRay.point;
            }
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {

        if (animator != null)
        {

            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive)
            {

                // Set the look target position, if one has been assigned
                if (lookObj != null)
                {
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(lookObj.position);
                }

                // Set the right hand target position and rotation, if one has been assigned
                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, animator.GetFloat("RightFoot"));
                //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPos);
                // animator.SetIKRotation(AvatarIKGoal.RightHand, rightFootObj.rotation);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, animator.GetFloat("LeftFoot"));
                //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPos);
            }

            
        }
    }
}
