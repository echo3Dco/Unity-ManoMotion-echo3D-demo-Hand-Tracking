using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartApplicationInstruction : Instruction
{

    private void OnEnable()
    {
        InitializeInstruction();
    }

    void InitializeInstruction()
    {
        this._instructionID = 3;
        this._instructionSeen = false;

        this._currentInstructionStep = 0;
        this._instructionSteps = 1;
        this._instructionName = "Ready to go!";
        this._instructionSeen = false;
        this._cardText = "You have now seen some of the core features of the SDK Lite. But there are definately more. Feel free to further explore them";


    }

    override public void GuideHowTo()
    {
        ApplicationManager.Instance.howToInstructor.ConfirmInstructionSeen();

    }
}
