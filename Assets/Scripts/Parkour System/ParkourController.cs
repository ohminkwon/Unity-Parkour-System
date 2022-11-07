using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourController : MonoBehaviour
{
    [field: SerializeField] public InputReader InputReader { get; private set; }

    private EnvironmentScanner environmentScanner;
    private PlayerController playerController;
    private Animator animator;

    private bool inAction;

    private void Awake()
    {
        environmentScanner = GetComponent<EnvironmentScanner>();
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (InputReader.IsJumping && !inAction)
        {
            var hitData = environmentScanner.ObstacleCheck();
            if (hitData.forwardHitFound)
            {
                StartCoroutine(DoParkourAction());
            }
        }        
    }

    IEnumerator DoParkourAction()
    {
        inAction = true;
        playerController.SetControl(false);

        animator.CrossFade("StepUp", 0.2f);
        yield return null; // wait for the end of a frame

        var animState = animator.GetNextAnimatorStateInfo(0);

        yield return new WaitForSeconds(animState.length);

        playerController.SetControl(true);
        inAction = false;        
    }
}
