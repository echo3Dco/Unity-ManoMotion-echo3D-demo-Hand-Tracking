
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TriggerGizmo : MonoBehaviour
{
    public float fadeSpeed = 20f;
    private float currentAlphaValue = 1f;
    public bool canExpand;
    public Color clickColor, pickColor, dropColor, grabColor, releaseColor, tapColor;

    private Text triggerLabelText;
    private Vector3 increaseScaleFactor;
    private Vector3 originalScale = Vector3.one * 0.5f;

    void OnEnable()
    {
        triggerLabelText = GetComponent<Text>();
        increaseScaleFactor = Vector3.one * 0.01f;
        this.transform.localScale = originalScale;
    }

    void FixedUpdate()
    {
        FadeAndExpand();
    }

    private void FadeAndExpand()
    {
        if (canExpand)
        {
            currentAlphaValue = Mathf.Lerp(currentAlphaValue, 0f, fadeSpeed * Time.deltaTime);
            Color CurrentColor = triggerLabelText.color;
            triggerLabelText.color = new Color(CurrentColor.r, CurrentColor.g, CurrentColor.b, currentAlphaValue);
            transform.localScale += increaseScaleFactor;

            if (currentAlphaValue < 0.05f)
            {
                canExpand = false;

            }
        }
        else
        {
            currentAlphaValue = 1;
            triggerLabelText.color = Color.white;
            this.gameObject.SetActive(false);
        }
    }

    public virtual void InitializeTriggerGizmo(ManoGestureTrigger triggerGesture)
    {
        this.transform.localScale = originalScale;
        canExpand = true;
        if (!triggerLabelText)
        {
            triggerLabelText = GetComponent<Text>();
        }

        switch (triggerGesture)
        {
            case ManoGestureTrigger.CLICK:

                triggerLabelText.text = "Click";
                triggerLabelText.color = clickColor;
                break;
            case ManoGestureTrigger.DROP:
                triggerLabelText.text = "Drop";
                triggerLabelText.color = dropColor;
                break;
            case ManoGestureTrigger.PICK:
                triggerLabelText.text = "Pick";
                triggerLabelText.color = pickColor;
                break;
            case ManoGestureTrigger.GRAB_GESTURE:
                triggerLabelText.text = "Grab";
                triggerLabelText.color = grabColor;
                break;
            case ManoGestureTrigger.RELEASE_GESTURE:
                triggerLabelText.text = "Release";
                triggerLabelText.color = releaseColor;
                break;
        }
    }
}
