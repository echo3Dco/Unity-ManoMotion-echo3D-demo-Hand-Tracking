using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToAnimations : MonoBehaviour
{


    [SerializeField]
    GameObject gestureAnimationsObject;
    [SerializeField]
    Animator gestureAnimator;


    [SerializeField]
    Image howToImage;

    [SerializeField]
    Image[] steps;

    [SerializeField]
    Image checkMark;

    public Color inactiveStepColor;
    public Color activeStepColor;

    [SerializeField]
    Image handOutline;



    public void ShouldShowCheckMark(bool condition)
    {
        checkMark.enabled = condition;
    }


    public void HighlightImagesUpToStep(int currentStep)
    {
        if (currentStep > steps.Length)
        {
            currentStep = steps.Length;
        }
        ShouldShowCheckMark(currentStep == steps.Length);


        for (int i = 0; i < steps.Length; i++)
        {
            if (i < currentStep)
            {
                steps[i].color = activeStepColor;
            }
            else
            {
                steps[i].color = inactiveStepColor;

            }
        }
    }

    public void ShouldDisplayImageSteps(bool condition)
    {
        for (int i = 0; i < steps.Length; i++)
        {
            steps[i].enabled = condition;
        }
    }

    public void ShouldShowAnimationImage(bool state)
    {
        howToImage.enabled = state;
    }

    void InitializeAnimator()
    {
        if (!gestureAnimationsObject)
        {
            try
            {
                string name = "HowToAnimations";
                gestureAnimationsObject = GameObject.Find(name);

            }
            catch (System.Exception ex)
            {
                Debug.Log(ex);
            }
        }

        if (!gestureAnimator)
        {
            gestureAnimator = gestureAnimationsObject.GetComponent<Animator>();
        }
    }

    public void ShouldShowHandOutlineImage(bool condition)
    {
        handOutline.enabled = condition;
    }

    string pickAnimationName = "PickAnimation";
    string dropAnimationName = "DropAnimation";
    string clickAnimationName = "ClickAnimation";
    string grabAnimationName = "GrabAnimation";
    string releaseAnimationName = "ReleaseAnimation";


    public void ShowHowToPick()
    {
        gestureAnimator.Play(pickAnimationName);
    }
    public void ShowHowToDrop()
    {
        gestureAnimator.Play(dropAnimationName);

    }
    public void ShowHowToClick()
    {
        gestureAnimator.Play(clickAnimationName);

    }
    public void ShowHowToGrab()
    {
        gestureAnimator.Play(grabAnimationName);

    }
    public void ShowHowToRelease()
    {
        gestureAnimator.Play(releaseAnimationName);

    }


}
