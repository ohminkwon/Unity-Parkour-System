using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
    [SerializeField] List<ParkourAction> parkourActions;

    private EnvironmentScanner environmentScanner;
    private PlayerController playerController;
    private Animator animator;

    private bool inAction = false;

    private void Awake()
    {   
        environmentScanner = GetComponent<EnvironmentScanner>();
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();        
    }

    private void Update()
    {        
        if (playerController.InputReader.IsJumping && !inAction)
        {
            var hitData = environmentScanner.ObstacleCheck();
            if (hitData.forwardHitFound)
            {
                foreach (var action in parkourActions)
                {
                    if(action.CheckIfpossible(hitData, transform))
                    {
                        StartCoroutine(DoParkourAction(action));
                        break;
                    }
                }                
            }
        }        
    }

    IEnumerator DoParkourAction(ParkourAction action)
    {
        inAction = true;
        playerController.SetControl(false);

        animator.CrossFade(action.AnimName, 0.2f);
        yield return null; // wait for the end of a frame

        // Check animation name is same with action data
        var animState = animator.GetNextAnimatorStateInfo(0);
        if (!animState.IsName(action.AnimName))
            Debug.Log("The parkour animation name is different with data");      

        float timer = 0f;
        while (timer <= animState.length)
        {
            timer += Time.deltaTime;

            // Rotate player towards obstacle while action animation is playing
            if (action.CanRotateToObstacle)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, action.TargetRotation, playerController.RotationSpeed * Time.deltaTime);

            // Target Matching
            if (action.EnableTargetMatching)
                MatchTarget(action);

            yield return null;
        }

        playerController.SetControl(true);
        inAction = false;        
    }

    private void MatchTarget(ParkourAction action)
    {
        if (animator.isMatchingTarget)
            return;
        
        animator.MatchTarget(action.MatchPos, transform.rotation, action.MatchBodyPart, 
            new MatchTargetWeightMask(new Vector3(0, 1, 0), 0), action.MatchStartTime, action.MatchTargetTime);
    }
}
