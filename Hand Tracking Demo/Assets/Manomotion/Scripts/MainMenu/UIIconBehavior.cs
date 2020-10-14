using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ManoMotion.UI.Buttons
{
	public class UIIconBehavior : MonoBehaviour
	{
		public enum IconFunctionality
		{
			UnKnown,
			States,
			ManoClass,
			HandSide,
			Continuous,
			Warnings,
			Depth,
			TriggerPick,
			TriggerDrop,
			TriggerClick,
			TriggerGrab,
			TriggerRelease,
			BoundingBox,
			InnerBoundingBox,
			PalmCenter,
			POI,
			Background,
			LicenseInfo,
			Smoothing,
			Instructions
		}
		public IconFunctionality myIconFunctionality;

		[SerializeField]
		Sprite activeFrame, inactiveFrame;

		public bool isActive;

		private Button thisButton;
		private Image buttonFrame, buttonIcon;

		private Color activeColor;

		void OnEnable()
		{
			activeColor = new Color32(61, 87, 127, 255);
			thisButton = this.GetComponent<Button>();
			buttonFrame = transform.Find("Frame").GetComponent<Image>();
			buttonIcon = transform.Find("Icon").GetComponent<Image>();

		}

		private void Start()
		{
			UpdateIconAndFrame(isActive);
		}

		/// <summary>
		/// Updates the icon according to its state (on/off)
		/// </summary>
		/// <param name="state">Requires the state of the icon</param>
		private void UpdateIconAndFrame(bool state)
		{
			if (!buttonFrame)
			{
				buttonFrame = transform.Find("Frame").GetComponent<Image>();
			}

			if (!buttonIcon)
			{
				buttonIcon = transform.Find("Icon").GetComponent<Image>();
			}

			if (state)
			{
				buttonFrame.sprite = activeFrame;
				buttonIcon.color = activeColor;
			}

			else
			{
				buttonFrame.sprite = inactiveFrame;
				buttonIcon.color = Color.white;
			}
		}

		/// <summary>
		/// Toggles the state of the icon.
		/// </summary>
		public void ToggleActive()
		{
			isActive = !isActive;
			UpdateIconAndFrame(isActive);
		}

		public void SetIsActive(bool state)
		{
			isActive = state;
			//This creates a problem
			UpdateIconAndFrame(state);
		}

		/// <summary>
		/// Sets the icon functionality.
		/// </summary>
		/// <param name="functionality">Functionality.</param>
		public void SetIconFunctionality(IconFunctionality functionality)
		{
			this.myIconFunctionality = functionality;
		}
	}
}