using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSideInstruction : Instruction
{
    private void OnEnable()
    {
        InitializeInstruction();
    }

    void InitializeInstruction()
    {
        this._instructionID = 2;
        this._instructionSeen = false;

        this._currentInstructionStep = 0;
        this._instructionSteps = 3;
        this._instructionName = "Side of the Hand";
        this._instructionSeen = false;
        this._howToInstruction = "Place your hand with your palm side facing the camera.";
        this._cardText = "SDK Lite is able to differentiate which side of the hand is being detected.";

        this._stepInstructions = new string[_instructionSteps];

        this._stepInstructions[0] = "Place your hand with your palm side facing the camera.";
        this._stepInstructions[1] = "Great! Now try to place your hand with your hand's back side facing the camera";
        this._stepInstructions[2] = "Legend! You are ready to see more features. Tap on the thumbs up icon";


    }

    private void Update()
    {
        RepondToUserActions();
    }

    HandSide currentHandSide;
    HandSide requestedHandSide;

    int framesNeededOfDetection = 60;
    int currentFramesDetected;

    private void RepondToUserActions()
    {
        if (_shouldRespondToUserInput)
        {
            currentHandSide = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.hand_side;

            if (currentHandSide == requestedHandSide)
            {
                currentFramesDetected++;

                if (currentFramesDetected == framesNeededOfDetection)
                {
                    ProgressWithInstructionStep();

                }

            }
        }

    }

    /// <summary>
    /// Handles the behavior of progressing with the Instruction steps
    /// </summary>
    override public void ProgressWithInstructionStep()
    {
        _currentInstructionStep++;
        Handheld.Vibrate();
        UpdateHandSideNeeded();
        currentFramesDetected = 0;
        if (_currentInstructionStep == _instructionSteps - 1)
        {
            ApplicationManager.Instance.runTimeApplication.ShouldShowHandSide(false);
            ApplicationManager.Instance.howToInstructor.HighlightConfirmation();
            StopResponding();

        }

        ApplicationManager.Instance.howToInstructor.UpdateCurrentInstructionStepOnCanvas(this._stepInstructions[_currentInstructionStep]);


    }

    private void UpdateHandSideNeeded()
    {
        switch (_currentInstructionStep)
        {
            case 0:
                requestedHandSide = HandSide.Palmside;
                break;
            case 1:
                requestedHandSide = HandSide.Backside;
                break;

            default:
                break;
        }
    }

    override public void GuideHowTo()
    {

        ApplicationManager.Instance.runTimeApplication.ShouldShowHandSide(true);
        this._shouldRespondToUserInput = true;
        UpdateHandSideNeeded();

    }

    override public void StopResponding()
    {
        this._shouldRespondToUserInput = false;
        ApplicationManager.Instance.runTimeApplication.ShouldShowHandSide(false);

    }

}
