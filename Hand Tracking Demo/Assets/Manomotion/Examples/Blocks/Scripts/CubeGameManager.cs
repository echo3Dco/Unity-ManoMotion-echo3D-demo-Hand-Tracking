using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CubeGameManager : MonoBehaviour
{

	private static CubeGameManager _instance;

	public static CubeGameManager Instance
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

	private void Awake()
	{
		if (!_instance)
		{
			_instance = this;
			ManomotionManager.OnManoMotionFrameProcessed += HandleManoMotionFrameProcessed;
		}
		else
		{
			Debug.LogError("More than 1 CubeManagers in the scene");
			Destroy(this.gameObject);
		}
	}
	public string interactableTag = "ExampleBlock";

	private void Start()
	{
		instructions.SetActive(!gameHasStarted);
		cursorRectTransform = cursor.GetComponent<RectTransform>();
		totalPoints = 0;
		streak = 0;
	}

	int streak;
	public bool gameHasStarted;
	int totalPoints;
	public ManoGestureTrigger interactionTrigger = ManoGestureTrigger.CLICK;
	public ManoClass movingManoclass = ManoClass.PINCH_GESTURE;

	public GameObject cursor;

	RectTransform cursorRectTransform;

	[SerializeField]
	GameObject instructions;

	[SerializeField]
	Text scoreKeeper;

	[SerializeField]
	AudioSource fireSound;

	/// <summary>
	/// Awards the points.
	/// </summary>
	/// <param name="points">Requires new points to .</param>
	public void AwardPoints(int points)
	{
		if (totalPoints + points >= 0)
		{
			totalPoints += points;
		}
		else
		{
			totalPoints = 0;
		}
		scoreKeeper.text = "Score: " + totalPoints;
	}

	/// <summary>
	/// Handles the information from the processed frame in order to use the gesture information and tracking information in moving the cursor and firing at the blocks.
	/// </summary>
	void HandleManoMotionFrameProcessed()
	{
		GestureInfo gesture = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;
		TrackingInfo trackingInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info;
		Warning warning = ManomotionManager.Instance.Hand_infos[0].hand_info.warning;

		MoveCursorAt(gesture, trackingInfo, warning);
		FireAt(gesture);
	}

	/// <summary>
	/// Moves the cursor according to the gesture information in the center of the detected bounding box.
	/// The cursor will disapear if there is no hand detected -> Warning Hand not found.
	/// </summary>
	/// <param name="gestureInfo">Gesture info.</param>
	/// <param name="trackingInfo">Tracking info.</param>
	/// <param name="warning">Warning.</param>
	void MoveCursorAt(GestureInfo gestureInfo, TrackingInfo trackingInfo, Warning warning)
	{
		if (warning != Warning.WARNING_HAND_NOT_FOUND && gestureInfo.mano_class == movingManoclass)
		{
			if (!cursor.activeInHierarchy)
			{
				cursor.SetActive(true);
			}
			cursorRectTransform.position = Camera.main.ViewportToScreenPoint(trackingInfo.palm_center);
		}
		else
		{
			if (cursor.activeInHierarchy)
			{
				cursor.SetActive(false);
			}
		}
	}

	/// <summary>
	/// Fires a raycast from the position of the cursor forward seeking to hit an example block.
	/// The fire will only happen with the user performes the interaction trigger.
	/// </summary>
	/// <param name="gestureInfo">Gesture info.</param>
	/// <param name="trackingInfo">Tracking info.</param>
	void FireAt(GestureInfo gestureInfo)
	{
		if (gestureInfo.mano_gesture_trigger == interactionTrigger)
		{
			fireSound.Play();
			if (!gameHasStarted)
			{
				gameHasStarted = true;
				instructions.SetActive(!gameHasStarted);
				scoreKeeper.enabled = gameHasStarted;
			}

			Ray ray = Camera.main.ScreenPointToRay(cursorRectTransform.position);
            RaycastHit hit;

            Debug.Log("Ray is fired");

			if (Physics.Raycast(ray.origin, ray.direction, out hit))
			{
                Debug.Log("Ray has hit: " + hit.transform.name);

                if (hit.transform.tag == interactableTag)
				{
					hit.transform.GetComponent<CubeSpawn>().AwardPoints();
					Handheld.Vibrate();
				}
			}
		}
	}
}
