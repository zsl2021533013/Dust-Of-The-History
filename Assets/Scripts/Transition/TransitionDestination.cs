using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DestinationTag 
{
    Scene_1_Portal_1,
    Scene_1_Portal_2,
    Scene_1_Portal_3,
    Scene_1_Portal_4,
    Scene_2_Portal_1,
    Scene_2_Portal_2,
    Scene_3_Portal_1,
}


public class TransitionDestination : MonoBehaviour
{

    [Header("Transition Detination Point")]
    public DestinationTag destinationTag;
}
