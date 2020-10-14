using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManoMotion.RunTime;
public class DetectionInstruction : Instruction
{

    private void OnEnable()
    {
        InitializeInstruction();
    }

    void InitializeInstruction()
    {
        this._instructionID = 0;
        this._currentInstructionStep = 0;
        this._instructionSteps = 2;
        this._instructionName = "Getting Started";
        this._instructionSeen = false;
        this._howToInstruction = "Place your hand clearly infront of the camera in order to be detected";
        this._cardText = "Hand Placement is really important. if the camera cant see your hand, it wont be able to detect it.";

        this._stepInstructions = new string[_instructionSteps];
        this._stepInstructions[0] = "Place your hand clearly infront of the camera in order to be detected";
        this._stepInstructions[1] = "Awesome! You are ready to see more features. Tap on the thumbs up icon";
    }

    int framesNeeded = 30;
    int framesHandBeingDetected;

    private void Update()
    {
        InstructionBehavior();
    }
    /// <summary>
    /// How the Instruction should behave to user Input
    /// </summary>
    override public void InstructionBehavior()
    {
        if (_shouldRespondToUserInput)
        {
            switch (_currentInstructionStep)
            {
                case 0:

                    if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_class != ManoClass.NO_HAND)
                    {
                        framesHandBeingDetected++;
                        ApplicationManager.Instance.howToInstructor.triggerAnimations.ShouldShowHandOutlineImage(false);
                        if (framesHandBeingDetected == framesNeeded)
                        {
                            ProgressWithInstructionStep();
                        }
                    }
                    else
                    {
                        if (framesHandBeingDetected > 0)
                        {
                            framesHandBeingDetected--;
                        }
                        ApplicationManager.Instance.howToInstructor.triggerAnimations.ShouldShowHandOutlineImage(true);

                    }
                    break;
                default:
                    break;
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
        if (_currentInstructionStep == _instructionSteps - 1)
        {
            ApplicationManager.Instance.howToInstructor.HighlightConfirmation();

        }
        ApplicationManager.Instance.howToInstructor.UpdateCurrentInstructionStepOnCanvas(this._stepInstructions[_currentInstructionStep]);
    }

    /// <summary>
    /// Visualy ProvidesFeedback to the user on what to do.
    /// </summary>
    override public void GuideHowTo()
    {
        this._shouldRespondToUserInput = true;
        framesHandBeingDetected = 0;
        ApplicationManager.Instance.runTimeApplication.ShouldShowBoundingBox(true);
        ApplicationManager.Instance.runTimeApplication.ShouldShowCursor(true);
        ApplicationManager.Instance.howToInstructor.triggerAnimations.ShouldShowHandOutlineImage(true);

    }

    override public void StopResponding()
    {
        this._shouldRespondToUserInput = false;
        ApplicationManager.Instance.runTimeApplication.ShouldShowBoundingBox(false);
        ApplicationManager.Instance.runTimeApplication.ShouldShowCursor(false);
        ApplicationManager.Instance.howToInstructor.triggerAnimations.ShouldShowHandOutlineImage(false);


    }

}
