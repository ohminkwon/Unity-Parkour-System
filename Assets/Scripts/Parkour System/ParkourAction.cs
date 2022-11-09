using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour System/New parkour action")]
public class ParkourAction : ScriptableObject
{
    [field: SerializeField] public string AnimName { get; private set; }
    [field: SerializeField] public bool CanRotateToObstacle { get; private set; }

    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;

    public Quaternion TargetRotation { get; set; }

    public bool CheckIfpossible(ObstacleHitData hitData, Transform player)
    {
        float height = hitData.heightHit.point.y - player.position.y;

        if (height < minHeight || height > maxHeight)
            return false;

        // Set action animation direction change from origin to obstacle using a normal vector of the 3d object obstacle 
        if (CanRotateToObstacle)
            TargetRotation = Quaternion.LookRotation(-hitData.forwardHit.normal);


        return true;
    }
}
