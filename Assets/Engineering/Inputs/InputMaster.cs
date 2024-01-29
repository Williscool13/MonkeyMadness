using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMaster : MonoBehaviour
{
    public static InputMaster Instance { get; private set; }
    private List<IInputReceiver> inputReceivers;


    private void Awake() {
        if (Instance != null) {
            Destroy(this);
            return;
        }
        Instance = this;
        inputReceivers = new List<IInputReceiver>();
    }

    private void OnDestroy() {
        if (Instance == this) {
            Instance = null;
        }
    }

    bool cleftPressedOne = false;
    bool crightPressedOne = false;
    bool cmiddlePressedOne = false;

    bool cleftHeldOne = false;
    bool crightHeldOne = false;
    bool cmiddleHeldOne = false;

    bool crightPressedTwo = false;
    bool cleftPressedTwo = false;
    bool cmiddlePressedTwo = false;

    bool crightHeldTwo = false;
    bool cleftHeldTwo = false;
    bool cmiddleHeldTwo = false;

    bool cleftPressedThree = false;
    bool crightPressedThree = false;
    bool cmiddlePressedThree = false;

    bool cleftHeldThree = false;
    bool crightHeldThree = false;
    bool cmiddleHeldThree = false;

    bool cleftPressedFour = false;
    bool crightPressedFour = false;
    bool cmiddlePressedFour = false;

    bool cleftHeldFour = false;
    bool crightHeldFour = false;
    bool cmiddleHeldFour = false;


    public void SetAll(
        bool cLeftPressedOne, bool cRightPressedOne, bool cMiddlePressedOne, bool cLeftHeldOne, bool cRightHeldOne, bool cMiddleHeldOne,
        bool cLeftPressedTwo, bool cRightPressedTwo, bool cMiddlePressedTwo, bool cLeftHeldTwo, bool cRightHeldTwo, bool cMiddleHeldTwo
        ) {

        cleftPressedOne = cLeftPressedOne;
        crightPressedOne = cRightPressedOne;
        cmiddlePressedOne = cMiddlePressedOne;
        cleftHeldOne = cLeftHeldOne;
        crightHeldOne = cRightHeldOne;
        cmiddleHeldOne = cMiddleHeldOne;

        crightPressedTwo = cRightPressedTwo;
        cleftPressedTwo = cLeftPressedTwo;
        cmiddlePressedTwo = cMiddlePressedTwo;
        crightHeldTwo = cRightHeldTwo;
        cleftHeldTwo = cLeftHeldTwo;
        cmiddleHeldTwo = cMiddleHeldTwo;
    }

    void Update()
    {
        for (int i = 0; i < inputReceivers.Count; i++) {
            inputReceivers[i].SetPlayerInputs(0, 
                leftPressedOne || cleftPressedOne, rightPressedOne || crightPressedOne, middlePressedOne || cmiddlePressedOne, 
                leftHeldOne || cleftHeldOne, rightHeldOne || crightHeldOne, middleHeldOne || cmiddleHeldOne);
            inputReceivers[i].SetPlayerInputs(1, 
                leftPressedTwo || cleftPressedTwo, rightPressedTwo || crightPressedTwo, middlePressedTwo || cmiddlePressedTwo, 
                leftHeldTwo || cleftHeldTwo, rightHeldTwo || crightHeldTwo, middleHeldTwo || cmiddleHeldTwo);
            inputReceivers[i].SetPlayerInputs(2, leftPressedThree, rightPressedThree, middlePressedThree, leftHeldThree, rightHeldThree, middleHeldThree);
            inputReceivers[i].SetPlayerInputs(3, leftPressedFour, rightPressedFour, middlePressedFour, leftHeldFour, rightHeldFour, middleHeldFour);
        }

        leftPressedOne = false;
        rightPressedOne = false;
        middlePressedOne = false;

        leftPressedTwo = false;
        rightPressedTwo = false;
        middlePressedTwo = false;

        leftPressedThree = false;
        rightPressedThree = false;
        middlePressedThree = false;

        leftPressedFour = false;
        rightPressedFour = false;
        middlePressedFour = false;
    }


    bool leftPressedOne = false;
    bool rightPressedOne = false;
    bool middlePressedOne = false;

    bool leftHeldOne = false;
    bool rightHeldOne = false;
    bool middleHeldOne = false;

    bool rightPressedTwo = false;
    bool leftPressedTwo = false;
    bool middlePressedTwo = false;

    bool rightHeldTwo = false;
    bool leftHeldTwo = false;
    bool middleHeldTwo = false;

    bool leftPressedThree = false;
    bool rightPressedThree = false;
    bool middlePressedThree = false;

    bool leftHeldThree = false;
    bool rightHeldThree = false;
    bool middleHeldThree = false;

    bool leftPressedFour = false;
    bool rightPressedFour = false;
    bool middlePressedFour = false;

    bool leftHeldFour = false;
    bool rightHeldFour = false;
    bool middleHeldFour = false;

    public void OnLeftOne(InputValue value) {
        if (value.isPressed) {
            leftPressedOne = true;
        }

        if (value.Get<float>() > 0.5f) {
            leftHeldOne = true;
        }
        else {
            leftHeldOne = false;
        }
    }
    public void OnRightOne(InputValue value) {
        if (value.isPressed) {
            rightPressedOne = true;
        }

        if (value.Get<float>() > 0.5f) {
            rightHeldOne = true;
        }
        else {
            rightHeldOne = false;
        }
    }
    public void OnMiddleOne(InputValue value) {
        if (value.isPressed) {
            middlePressedOne = true;
        }

        if (value.Get<float>() > 0.5f) {
            middleHeldOne = true;
        }
        else {
            middleHeldOne = false;
        }
    }
    
    public void OnLeftTwo(InputValue value) {
        if (value.isPressed) {
            leftPressedTwo = true;
        }

        if (value.Get<float>() > 0.5f) {
            leftHeldTwo = true;
        }
        else {
            leftHeldTwo = false;
        }
    }
    public void OnRightTwo(InputValue value) {
        if (value.isPressed) {
            rightPressedTwo = true;
        }

        if (value.Get<float>() > 0.5f) {
            rightHeldTwo = true;
        }
        else {
            rightHeldTwo = false;
        }
    }
    public void OnMiddleTwo(InputValue value) {
        if (value.isPressed) {
            middlePressedTwo = true;
        }

        if (value.Get<float>() > 0.5f) {
            middleHeldTwo = true;
        }
        else {
            middleHeldTwo = false;
        }
    }

    public void OnLeftThree(InputValue value) {
        if (value.isPressed) {
            leftPressedThree = true;
        }

        if (value.Get<float>() > 0.5f) {
            leftHeldThree = true;
        }
        else {
            leftHeldThree = false;
        }
    }
    public void OnRightThree(InputValue value) {
        if (value.isPressed) {
            rightPressedThree = true;
        }

        if (value.Get<float>() > 0.5f) {
            rightHeldThree = true;
        }
        else {
            rightHeldThree = false;
        }
    }
    public void OnMiddleThree(InputValue value) {
        if (value.isPressed) {
            middlePressedThree = true;
        }

        if (value.Get<float>() > 0.5f) {
            middleHeldThree = true;
        }
        else {
            middleHeldThree = false;
        }
    }

    public void OnLeftFour(InputValue value) {
        if (value.isPressed) {
            leftPressedFour = true;
        }

        if (value.Get<float>() > 0.5f) {
            leftHeldFour = true;
        }
        else {
            leftHeldFour = false;
        }
    }
    public void OnRightFour(InputValue value) {
        if (value.isPressed) {
            rightPressedFour = true;
        }

        if (value.Get<float>() > 0.5f) {
            rightHeldFour = true;
        }
        else {
            rightHeldFour = false;
        }
    }
    public void OnMiddleFour(InputValue value) {
        if (value.isPressed) {
            middlePressedFour = true;
        }

        if (value.Get<float>() > 0.5f) {
            middleHeldFour = true;
        }
        else {
            middleHeldFour = false;
        }
    }

    public void RegisterInputReceiver(IInputReceiver receiver) {
        inputReceivers.Add(receiver);
    }

    public void UnregisterInputReceiver(IInputReceiver receiver) {
        if (inputReceivers.Contains(receiver)) {
            inputReceivers.Remove(receiver);
        }
    }
}