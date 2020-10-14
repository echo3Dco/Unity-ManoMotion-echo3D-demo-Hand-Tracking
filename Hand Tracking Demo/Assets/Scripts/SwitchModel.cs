using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SwitchModel : MonoBehaviour
{
    public PlaceOnPlane placeOnPlane;
    public Text debugText;
    void Start() {
		ManomotionManager.OnManoMotionFrameProcessed += HandleManoMotionFrameUpdated;
	}

    public void OnClick() {
        placeOnPlane.SwitchModel();
    }

    bool WithinBounds(Vector3 point) {
        return point.y > gameObject.transform.position.y;

        // I tried for a long time to make this work with the exact borders of the button rather than just checking for
        // "y greater than the bottom edge." If you need this functionality, please feel free to use some of the discarded 
        // code snippets below as a starting point.

        // Vector3 buttonPos = Camera.main.ViewportToScreenPoint(gameObject.transform.position);
        // Vector3[] v = new Vector3[4];
        // GetComponent<RectTransform>().GetWorldCorners(v);
        // Vector3 bl = Camera.main.WorldToScreenPoint(v[0]);
        // Vector3 tr = Camera.main.WorldToScreenPoint(v[2]);
        
        // return bl.x < point.x && bl.y < point.y && point.x < tr.x && point.y < tr.y;
        // return buttonPos.x < point.x && buttonPos.y < point.y
        //        && point.x < buttonPos.x + buttonRect.width/2 && point.y < buttonPos.y + buttonRect.height/2;
    }


	/// <summary>
	/// Handles the information from the processed frame in order to use the gesture and tracking information
    //  to check if the button is selected.
	/// </summary>
	void HandleManoMotionFrameUpdated()
	{
		GestureInfo gesture = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;
		TrackingInfo tracking = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info;
		Warning warning = ManomotionManager.Instance.Hand_infos[0].hand_info.warning;
        
        if (warning != Warning.WARNING_HAND_NOT_FOUND && gesture.mano_gesture_trigger == ManoGestureTrigger.CLICK) {
            // poi = point of interest in ManoMotion, in this case the location that you click
            Vector3 poi = Camera.main.ViewportToScreenPoint(tracking.poi);
            if (WithinBounds(poi))
                OnClick();


            // I tried for a long time to make this work with the exact borders of the button rather than just checking for
            // "y greater than the bottom edge." If you need this functionality, please feel free to use some of the discarded 
            // code snippets below as a starting point.

            // RectTransform rectTransform = GetComponent<RectTransform>();

            // bool rectContains = RectTransformUtility.RectangleContainsScreenPoint(rectTransform, poi, Camera.main);
            // Vector2 localPoint;
            // RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, poi, Camera.main, out localPoint);

            // Vector3 buttonPos = Camera.main.ViewportToScreenPoint(gameObject.transform.position);
            // Rect buttonRect = GetComponent<RectTransform>().rect;

            // Vector3[] v = new Vector3[4];
            // GetComponent<RectTransform>()
            // Vector3 bl = Camera.main.WorldToScreenPoint(v[0]);
            // Vector3 tr = Camera.main.WorldToScreenPoint(v[2]);
            
            // return bl.x < point.x && bl.y < point.y && point.x < tr.x && point.y < tr.y;

            // debugText.text = "" + poi + "\n" + WithinBounds(poi) + "\n" +
            //     bl.x + " " + bl.y + "\n" + tr.x + " " + tr.y;
            // placeOnPlane.setVisible(!WithinBounds(poi));

            // debugText.text = "" + poi + "\n" + WithinBounds(new Vector2(poi.x, poi.y)) + "\n" + 
            //     buttonPos.x + " " + buttonPos.y + "\n" + (buttonPos.x + buttonRect.width/2) + " " + (buttonPos.y + buttonRect.height/2);
            // placeOnPlane.setVisible(!WithinBounds(new Vector2(poi.x, poi.y)));
        }
	}
}
