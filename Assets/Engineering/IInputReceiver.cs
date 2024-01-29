using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputReceiver
{
    public void SetPlayerInputs(int playerId, bool left, bool right, bool middle, bool leftHeld, bool rightHeld, bool middleHeld);
}