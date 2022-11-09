using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour System/New parkour action")]
public class ParkourAction : ScriptableObject
{
    [SerializeField] private string animName;
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;

    [SerializeField] private bool canRotateToObstacle;

    [Header("Target Matching Settings")]
    [SerializeField] private bool enableTargetMatching = true;
    [SerializeField] private AvatarTarget matchBodyPart;
    [SerializeField] private float matchStartTime;
    [SerializeField] private float matchTargetTime;

    public Quaternion TargetRotation { get; private set; }
    public Vector3 MatchPos { get; private set; }

    public bool CheckIfpossible(ObstacleHitData hitData, Transform player)
    {
        float height = hitData.heightHit.point.y - player.position.y;

        if (height < minHeight || height > maxHeight)
            return false;

        // Set action animation direction change from origin to obstacle using a normal vector of the 3d object obstacle 
        if (CanRotateToObstacle)
            TargetRotation = Quaternion.LookRotation(-hitData.forwardHit.normal);

        if (enableTargetMatching)
            MatchPos = hitData.heightHit.point;

        return true;
    }

    // properties
    public string AnimName => animName;
    public bool CanRotateToObstacle => canRotateToObstacle;
    public bool EnableTargetMatching => enableTargetMatching;
    public AvatarTarget MatchBodyPart => matchBodyPart;
    public float MatchStartTime => matchStartTime;
    public float MatchTargetTime => matchTargetTime;
}
