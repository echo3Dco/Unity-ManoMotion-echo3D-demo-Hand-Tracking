using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GestureInstruction : Instruction
{


    public List<TriggerGestureToPerformInstruction> triggerGesturesToPerformInstructions = new List<TriggerGestureToPerformInstruction>();
    int ammountOfTriggersPerformed;
    int triggersNeeded = 3;
    int numberOfCompleteSetOfTriggersPerformed = 0;
    ManoGestureTrigger requestedTrigger;

    //The delay is not the issue
    float delay = 0.5f;

    private void OnEnable()
    {
        InitializeInstruction();
    }

    private void PopulateTriggerList()
    {
        if (triggerGesturesToPerformInstructions.Count <= 0)
        {
            string howToPerformClick = "Perform a Click by closing and opening your thumb and index together. Click 3 times.";
            TriggerGestureToPerformInstruction ClickGestureToPerform = new TriggerGestureToPerformInstruction(new List<ManoGestureTrigger> { ManoGestureTrigger.CLICK }, new List<string> { howToPerformClick });
            triggerGesturesToPerformInstructions.Add(ClickGestureToPerform);

            string howToPerformGrab = "Perform a Grab by closing all your fingers together into a fist.";
            string howToPerformRelease = "Now perform a Release by fully opening your fingers.";
            TriggerGestureToPerformInstruction GrabReleaseGestureToPerform = new TriggerGestureToPerformInstruction(new List<ManoGestureTrigger> { ManoGestureTrigger.GRAB_GESTURE, ManoGestureTrigger.RELEASE_GESTURE }, new List<string> { howToPerformGrab, howToPerformRelease });
            GrabReleaseGestureToPerform.howToPerformTriggerDescription.Add(howToPerformGrab);
            triggerGesturesToPerformInstructions.Add(GrabReleaseGestureToPerform);


            string howToPerformPick = "Perform a Pick by just closing your thumb and index fingers together";
            string howToPerformDrop = "Now perform a Drop by opening your thumb and index fingers";
            TriggerGestureToPerformInstruction PickDropGestureToPerform = new TriggerGestureToPerformInstruction(new List<ManoGestureTrigger> { ManoGestureTrigger.PICK, ManoGestureTrigger.DROP }, new List<string> { howToPerformPick, howToPerformDrop });
            triggerGesturesToPerformInstructions.Add(PickDropGestureToPerform);

        }

    }

    void InitializeInstruction()
    {
        PopulateTriggerList();

        this._instructionID = 1;
        this._instructionSeen = false;
        //This is the main index
        this._currentInstructionStep = 0;
        this._instructionSteps = 4;
        this._instructionName = "Performing Gestures";
        this._howToInstruction = triggerGesturesToPerformInstructions[_currentInstructionStep].howToPerformTriggerDescription[ammountOfTriggersPerformed];
        this._cardText = "Now that your hand is well detected you can try and perform some of our trigger gestures";

        this._stepInstructions = new string[0];

        ammountOfTriggersPerformed = 0;
        numberOfCompleteSetOfTriggersPerformed = 0;

    }

    private void Update()
    {
        RepondToUserActions();
    }






    //This is how the system responds to user input
    private void RepondToUserActions()
    {
        //This never stops because it never reaches the Stop responding
        if (_shouldRespondToUserInput)
        {
            ManoGestureTrigger detectedTrigger = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_gesture_trigger;

            if (detectedTrigger == requestedTrigger && detectedTrigger != ManoGestureTrigger.NO_GESTURE)
            {
                HandleCorrectUserGestureInput();
            }
        }

    }

    private void HandleCorrectUserGestureInput()
    {
        //I get an argument out of range exception here

        //ArgumentOutOfRangeException: Argument is out of range.
        //     Parameter name: index
        //       at System.Collections.Generic.List`1[T].get_Item(Int32 index)[0x00000] in < filename unknown >:0
        //   at GestureInstruction.HandleCorrectUserGestureInput()[0x00000] in < filename unknown >:0

        //The max is the length of the current trigger list
        int totalTriggersForGesture = triggerGesturesToPerformInstructions[_currentInstructionStep].triggerGestures.Count - 1;
        string currentInstructionToDisplay = "";

        //There are more gestures within the trigger
        if (ammountOfTriggersPerformed < totalTriggersForGesture && !isChanging)
        {
            ammountOfTriggersPerformed++;
            currentInstructionToDisplay = triggerGesturesToPerformInstructions[_currentInstructionStep].howToPerformTriggerDescription[ammountOfTriggersPerformed];
            UpdateRequestedTrigger();

        }
        else
        {
            //I have performed all the triggers required for the gesture example Grab AND Release ( that is consider 1 out of 3)
            if (!isChanging)
            {
                ammountOfTriggersPerformed = 0;
                numberOfCompleteSetOfTriggersPerformed++;
                UpdateRequestedTrigger();

                currentInstructionToDisplay = triggerGesturesToPerformInstructions[_currentInstructionStep].howToPerformTriggerDescription[ammountOfTriggersPerformed];
                ApplicationManager.Instance.howToInstructor.triggerAnimations.HighlightImagesUpToStep(numberOfCompleteSetOfTriggersPerformed);

                Debug.LogFormat("I have completed {0} complete Sets ", numberOfCompleteSetOfTriggersPerformed);

            }


        }
        //This means that I have performed 3xtimes Grab/Release

        if (numberOfCompleteSetOfTriggersPerformed == triggersNeeded && !isChanging)
        {

            ProgressWithInstructionStep();
        }
        //I get the exception sometimes
        ApplicationManager.Instance.howToInstructor.UpdateCurrentInstructionStepOnCanvas(currentInstructionToDisplay);




    }



    /// <summary>
    /// Handles the behavior of progressing with the Instruction steps
    /// </summary>
    override public void ProgressWithInstructionStep()
    {
        if (!isChanging)
        {
            StartCoroutine(ProceedToNextTriggerAfterTime(delay));
        }
    }

    bool isChanging;
    IEnumerator ProceedToNextTriggerAfterTime(float time)
    {
        isChanging = true;
        yield return new WaitForSeconds(time);
        _currentInstructionStep++;
        numberOfCompleteSetOfTriggersPerformed = 0;

        Handheld.Vibrate();
        ApplicationManager.Instance.howToInstructor.triggerAnimations.HighlightImagesUpToStep(numberOfCompleteSetOfTriggersPerformed);
        UpdateRequestedTrigger();
        UpdateTriggerGestureDescription();

        isChanging = false;
    }

    /// <summary>
    /// Updates the bottom text description of how to perform the requested trigger gesture. If the triggers are done then it displays the end message
    /// </summary>
    private void UpdateTriggerGestureDescription()
    {
        string currentDescription = "";
        if (_currentInstructionStep < _instructionSteps - 1)
        {
            currentDescription = triggerGesturesToPerformInstructions[_currentInstructionStep].howToPerformTriggerDescription[ammountOfTriggersPerformed];

        }
        else if (_currentInstructionStep == _instructionSteps - 1)
        {
            Debug.Log("Inside 1");
            ApplicationManager.Instance.howToInstructor.HighlightConfirmation();
            Debug.Log("Inside 2");

            StopResponding();
            Debug.Log("Inside 3");

            Handheld.Vibrate();
            string endMessage = "Great! You are ready to see more features. Tap on the thumbs up icon.";
            currentDescription = endMessage;
            Debug.Log("Inside 4");

        }
        ApplicationManager.Instance.howToInstructor.UpdateCurrentInstructionStepOnCanvas(currentDescription);
    }

    //[SerializeField]
    //Text currentRequested;

    //[SerializeField]
    //Text clickEnabled;

    //[SerializeField]
    //Text grabEnabled;

    //[SerializeField]
    //Text releaseEnabled;

    //[SerializeField]
    //Text pickEnabled;

    //[SerializeField]
    //Text dropEnabled;

    //[SerializeField]
    //Text respondingText;

    private void UpdateRequestedTrigger()
    {
        //The issue that I am facing is that when I update the trigger It changes so fast that I am not able to show the trigger
        try
        {

            if (_currentInstructionStep < _instructionSteps - 1)
            {

                requestedTrigger = triggerGesturesToPerformInstructions[_currentInstructionStep].triggerGestures[ammountOfTriggersPerformed];
                StartCoroutine(DisableTriggersAfterDelay(delay, requestedTrigger));

                switch (requestedTrigger)
                {
                    case ManoGestureTrigger.CLICK:
                        ApplicationManager.Instance.howToInstructor.ShouldShowTriggerAnimationVisuals(true);
                        ApplicationManager.Instance.runTimeApplication.ShouldShowClick(true);
                        ApplicationManager.Instance.howToInstructor.triggerAnimations.ShowHowToClick();
                        break;
                    case ManoGestureTrigger.GRAB_GESTURE:
                        ApplicationManager.Instance.howToInstructor.ShouldShowTriggerAnimationVisuals(true);
                        ApplicationManager.Instance.runTimeApplication.ShouldShowGrab(true);
                        ApplicationManager.Instance.howToInstructor.triggerAnimations.ShowHowToGrab();
                        break;
                    case ManoGestureTrigger.RELEASE_GESTURE:
                        ApplicationManager.Instance.howToInstructor.ShouldShowTriggerAnimationVisuals(true);
                        ApplicationManager.Instance.runTimeApplication.ShouldShowRelease(true);
                        ApplicationManager.Instance.howToInstructor.triggerAnimations.ShowHowToRelease();
                        break;
                    case ManoGestureTrigger.PICK:
                        ApplicationManager.Instance.howToInstructor.ShouldShowTriggerAnimationVisuals(true);
                        ApplicationManager.Instance.runTimeApplication.ShouldShowPick(true);
                        ApplicationManager.Instance.howToInstructor.triggerAnimations.ShowHowToPick();
                        break;
                    case ManoGestureTrigger.DROP:
                        ApplicationManager.Instance.howToInstructor.ShouldShowTriggerAnimationVisuals(true);
                        ApplicationManager.Instance.runTimeApplication.ShouldShowDrop(true);
                        ApplicationManager.Instance.howToInstructor.triggerAnimations.ShowHowToDrop();
                        break;
                    default:
                        break;
                }
            }

        }
        catch (System.Exception ex)
        {
            Debug.Log("I cant assign trigger");
        }




    }

    IEnumerator DisableTriggersAfterDelay(float time, ManoGestureTrigger trigger)
    {
        yield return new WaitForSeconds(time);
        ApplicationManager.Instance.runTimeApplication.DisableAllTrigersExcept(trigger);
    }

    override public void GuideHowTo()
    {
        this._shouldRespondToUserInput = true;
        ApplicationManager.Instance.howToInstructor.triggerAnimations.ShouldDisplayImageSteps(_shouldRespondToUserInput);
        ApplicationManager.Instance.howToInstructor.triggerAnimations.ShouldShowAnimationImage(_shouldRespondToUserInput);
        ApplicationManager.Instance.howToInstructor.triggerAnimations.HighlightImagesUpToStep(numberOfCompleteSetOfTriggersPerformed);

        InitializeInstruction();
        UpdateRequestedTrigger();

    }

    override public void StopResponding()
    {
        this._shouldRespondToUserInput = false;
        ApplicationManager.Instance.howToInstructor.triggerAnimations.ShouldDisplayImageSteps(_shouldRespondToUserInput);
        ApplicationManager.Instance.howToInstructor.triggerAnimations.ShouldShowAnimationImage(_shouldRespondToUserInput);
        ApplicationManager.Instance.runTimeApplication.DisableAllTriggers();

    }




}
/// <summary>
/// Trigger gesture to perform instruction.
/// </summary>
public class TriggerGestureToPerformInstruction
{
    public List<ManoGestureTrigger> triggerGestures;
    public List<string> howToPerformTriggerDescription;

    public TriggerGestureToPerformInstruction(List<ManoGestureTrigger> triggers, List<string> instructions)
    {
        this.triggerGestures = triggers;
        this.howToPerformTriggerDescription = instructions;
    }
}
