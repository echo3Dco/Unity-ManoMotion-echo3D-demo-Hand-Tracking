using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExampleSpooky : MonoBehaviour
{
	[SerializeField]
	float fadeSpeed = 5;

	Image spookeyImageHolder;

	RectTransform spookyRectTransform;

	[SerializeField]
	Sprite openHandSprite;
	[SerializeField]
	Sprite closedHandSprite;

	// Use this for initialization
	void Start()
	{
		spookeyImageHolder = this.GetComponent<Image>();
		spookeyImageHolder.preserveAspect = true;
		spookyRectTransform = this.GetComponent<RectTransform>();
		ManomotionManager.OnManoMotionFrameProcessed += HandleManoMotionFrameUpdated;
	}


	/// <summary>
	/// Handles the information from the processed frame in order to use the gesture and tracking information to illustrate Spooky.
	/// </summary>
	void HandleManoMotionFrameUpdated()
	{
		GestureInfo gesture = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;
		TrackingInfo tracking = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info;
		Warning warning = ManomotionManager.Instance.Hand_infos[0].hand_info.warning;

		AssignSpookeyFace(gesture, warning);
		MoveAndScaleSpookey(tracking, warning);
		HighlightSpookeyImage(warning);
	}

	/// <summary>
	/// Places the visualization of the ghost into the center of the bounding box and updates its position based on the detection.
	///  The scale of the image will depend based on the width or height (depends which one is bigger) of the bounding box. The Image is set to preserve aspect in order to prevent any disform
	/// </summary>
	/// <param name="trackingInfo">Tracking info.</param>
	/// <param name="warning">Warning.</param>
	void MoveAndScaleSpookey(TrackingInfo trackingInfo, Warning warning)
	{
		if (warning != Warning.WARNING_HAND_NOT_FOUND)
		{
			spookyRectTransform.position = Camera.main.ViewportToScreenPoint(trackingInfo.palm_center);
			float width = Screen.width * trackingInfo.bounding_box.width;
			float height = Screen.height * trackingInfo.bounding_box.height;

			float size = Mathf.Min(width, height);
			spookyRectTransform.sizeDelta = new Vector2(size, size);
		}
	}

	/// <summary>
	/// Based on the continuous gesture performed (Open hand or Closed Hand) the ghost will change its appearance
	/// </summary>
	/// <param name="gesture">Gesture.</param>
	/// <param name="warning">Warning.</param>
	void AssignSpookeyFace(GestureInfo gesture, Warning warning)
	{
		if (warning != Warning.WARNING_HAND_NOT_FOUND)
		{
			switch (gesture.mano_gesture_continuous)
			{
				case ManoGestureContinuous.OPEN_HAND_GESTURE:
					spookeyImageHolder.sprite = openHandSprite;
					break;
				case ManoGestureContinuous.CLOSED_HAND_GESTURE:
					spookeyImageHolder.sprite = closedHandSprite;
					break;
				default:
					break;
			}
		}
	}

	/// <summary>
	/// If there is no hand detected Spooky will become transparent and fade out, while if the hand is being detected the image of the ghost will turn back on visible.
	/// </summary>
	/// <param name="warning">Warning.</param>
	void HighlightSpookeyImage(Warning warning)
	{
		if (warning == Warning.WARNING_HAND_NOT_FOUND)
		{
			FadeOut(spookeyImageHolder);
		}
		else
		{
			FadeIn(spookeyImageHolder);
		}
	}

	/// <summary>
	///Gradually decreases the alpha value of the image to create the effect of fading out
	/// </summary>
	/// <param name="image">Image.</param>
	void FadeOut(Image image)
	{
		Color currentColor = image.color;
		if (currentColor.a > 0)
		{
			currentColor.a -= Time.deltaTime * fadeSpeed;
		}
		image.color = currentColor;
	}

	/// <summary>
	/// Gradually increases the alpha value of the image to create the effect of fading in
	/// </summary>
	/// <param name="image">Image.</param>
	void FadeIn(Image image)
	{
		Color currentColor = image.color;
		if (currentColor.a < 1)
		{
			currentColor.a += Time.deltaTime * fadeSpeed;
		}
		image.color = currentColor;
	}
}
