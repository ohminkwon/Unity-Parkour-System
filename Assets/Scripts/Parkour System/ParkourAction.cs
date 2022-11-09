using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour System/New parkour action")]
public class ParkourAction : ScriptableObject
{
    [Tooltip("test")]
    [field: SerializeField] public string AnimName { get; private set; }

    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;

    public bool CheckIfpossible(ObstacleHitData hitData, Transform player)
    {
        float height = hitData.heightHit.point.y - player.position.y;

        if (height < minHeight || height > maxHeight)
            return false;

        return true;
    }
}
