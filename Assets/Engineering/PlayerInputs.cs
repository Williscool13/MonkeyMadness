public struct PlayerInputs
{
    public int id;
    public bool leftPressed;
    public bool rightPressed;
    public bool middlePressed;
    public bool leftHeld;
    public bool rightHeld;
    public bool middleHeld;
    public PlayerInputs(int id, bool leftPressed, bool rightPressed, bool middlePressed, bool leftHeld, bool rightHeld, bool middleHeld) {
        this.id = id;
        this.leftPressed = leftPressed;
        this.rightPressed = rightPressed;
        this.middlePressed = middlePressed;
        this.leftHeld = leftHeld;
        this.rightHeld = rightHeld;
        this.middleHeld = middleHeld;
    }

    public void SetInputs(bool leftPressed, bool rightPressed, bool middlePressed, bool leftHeld, bool rightHeld, bool middleHeld) {
        this.leftPressed = leftPressed;
        this.rightPressed = rightPressed;
        this.middlePressed = middlePressed;
        this.leftHeld = leftHeld;
        this.rightHeld = rightHeld;
        this.middleHeld = middleHeld;
    }

    public void ResetInputs() {
        leftPressed = false;
        rightPressed = false;
        middlePressed = false;
        leftHeld = false;
        rightHeld = false;
        middleHeld = false;
    }
}