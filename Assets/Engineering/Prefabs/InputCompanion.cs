using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputCompanion : MonoBehaviour
{
    [SerializeField] private InputMaster inputMaster;


    private void Update() {
        inputMaster.SetAll(
            leftPressedOne, rightPressedOne, middlePressedOne, leftHeldOne, rightHeldOne, middleHeldOne,
            leftPressedTwo, rightPressedTwo, middlePressedTwo, leftHeldTwo, rightHeldTwo, middleHeldTwo);


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
}
