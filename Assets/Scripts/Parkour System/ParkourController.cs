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

        var animState = animator.GetNextAnimatorStateInfo(0);
        if (!animState.IsName(action.AnimName))
            Debug.Log("The parkour animation name is different with data");

        yield return new WaitForSeconds(animState.length);

        playerController.SetControl(true);
        inAction = false;        
    }
}
