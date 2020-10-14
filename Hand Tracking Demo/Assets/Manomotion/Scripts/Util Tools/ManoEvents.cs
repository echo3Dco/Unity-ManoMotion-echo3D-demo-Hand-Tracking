using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[AddComponentMenu("ManoMotion/ManoEvents")]
public class ManoEvents : MonoBehaviour
{
	#region Singleton
	protected static ManoEvents _instance;
	public static ManoEvents Instance
	{
		get
		{
			return _instance;
		}

		set
		{
			_instance = value;
		}
	}
	#endregion

	[SerializeField]
	Animator statusAnimator;

	private string debugMessage = "";

	private void Awake()
	{
		if (!_instance)
		{
			_instance = this;
		}
		else
		{
			Destroy(this.gameObject);
			Debug.Log("More than 1 Mano events instances in scene");
		}
	}

	void Update()
	{
		if (!statusAnimator)
		{
			GameObject.Find("statusAnimator").GetComponent<Animator>();
			Debug.LogError("The application needs the ManoMotion canvas in order to display status messages through the animator");
			return;
		}

		HandleManomotionMessages();
	}

	/// <summary>
	/// Interprets the message from the Manomotion Manager class and assigns a string message to be displayed.
	/// </summary>
	void HandleManomotionMessages()
	{
		switch (ManomotionManager.Instance.ManoLicense.license_status)
		{
			case LicenseAnswer.LICENSE_OK:
				break;
			case LicenseAnswer.LICENSE_KEY_NOT_FOUND:
				DisplayLogMessage("Licence key not found");
				break;
			case LicenseAnswer.LICENSE_EXPIRED:
				DisplayLogMessage("Licence expired");
				break;
			case LicenseAnswer.LICENSE_INVALID_PLAN:
				DisplayLogMessage("Invalid plan");
				break;
			case LicenseAnswer.LICENSE_KEY_BLOCKED:
				DisplayLogMessage("Licence key blocked");
				break;
			case LicenseAnswer.LICENSE_INVALID_ACCESS_TOKEN:
				DisplayLogMessage("Invalid access token");
				break;
			case LicenseAnswer.LICENSE_ACCESS_DENIED:
				DisplayLogMessage("Licence access denied");
				break;
			case LicenseAnswer.LICENSE_MAX_NUM_DEVICES:
				DisplayLogMessage("Licence out of credits\t");
				break;
			case LicenseAnswer.LICENSE_UNKNOWN_SERVER_REPLY:
				DisplayLogMessage("Unknown Server Reply");
				break;
			case LicenseAnswer.LICENSE_PRODUCT_NOT_FOUND:
				DisplayLogMessage("Licence product");
				break;
			case LicenseAnswer.LICENSE_INCORRECT_INPUT_PARAMETER:
				DisplayLogMessage("Incorect Licence");
				break;
			case LicenseAnswer.LICENSE_INTERNET_REQUIRED:
				DisplayLogMessage("Internet Required");
				break;
			case LicenseAnswer.LICENSE_INCORRECT_BUNDLE_ID:
				DisplayLogMessage("Incorect Bundle ID");
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// Displays Log messages from the Manomotion Flags 
	/// </summary>
	/// <param name="message">Requires the string message to be displayed</param>
	void DisplayLogMessage(string message)
	{
		if (!statusAnimator)
		{
			statusAnimator = Resources.FindObjectsOfTypeAll<Animator>()[0];
		}
		statusAnimator.Play("SlideIn");
		statusAnimator.GetComponentInChildren<Text>().text = message;
	}
}
