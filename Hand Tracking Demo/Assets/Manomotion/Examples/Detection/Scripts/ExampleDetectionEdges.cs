using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExampleDetectionEdges : MonoBehaviour
{

	[SerializeField]
	Image rightEdge;
	[SerializeField]
	Image leftEdge;
	[SerializeField]
	Image topEdge;


	private void Start()
	{
		ManomotionManager.OnManoMotionFrameProcessed += HandleManoMotionFrameUpdated;
	}

	/// <summary>
	/// Handles the information from the processed frame in order to use the warning information to highlight the user position approaching to the edges.
	/// </summary>
	void HandleManoMotionFrameUpdated()
	{
		Warning warning = ManomotionManager.Instance.Hand_infos[0].hand_info.warning;

		HighlightEdgeWarning(warning);
	}

	/// <summary>
	/// Visually illustrated the users hand approaching the edges of the screen
	/// </summary>
	/// <param name="warning"></param>
	void HighlightEdgeWarning(Warning warning)
	{
		switch (warning)
		{

			case Warning.WARNING_APPROACHING_LEFT_EDGE:
				FadeIn(leftEdge);
				FadeOut(rightEdge);
				FadeOut(topEdge);
				break;

			case Warning.WARNING_APPROACHING_RIGHT_EDGE:
				FadeIn(rightEdge);
				FadeOut(leftEdge);
				FadeOut(topEdge);
				break;
			case Warning.WARNING_APPROACHING_UPPER_EDGE:
				FadeIn(topEdge);
				FadeOut(leftEdge);
				FadeOut(rightEdge);
				break;


			default:
				FadeOut(leftEdge);
				FadeOut(rightEdge);
				FadeOut(topEdge);
				break;
		}



	}

	public float fadeSpeed = 5f;
	float maxOpacity = 1;
	float minOpacity = 0;
	/// <summary>
	///Gradually decreases the alpha value of the image to create the effect of fading out
	/// </summary>
	/// <param name="image">Image.</param>
	void FadeOut(Image image)
	{
		Color currentColor = image.color;
		if (currentColor.a > minOpacity)
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
		if (currentColor.a < maxOpacity)
		{
			currentColor.a += Time.deltaTime * fadeSpeed;
		}
		image.color = currentColor;
	}
}
