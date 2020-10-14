using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEngine.Networking;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : MonoBehaviour
{
    public GameObject echoAR;
    public string APIKey;
    public Text debugText;
    public int maxModels;

    private GameObject toInstantiate;
    private GameObject[] models;
    // private Entry hologramEntry;
    private Vector3 modelPos;
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition;
    private Vector3 initialScale;
    private bool isCurrentVisible = false;
    private int numModelsLoaded = 0;
    private int currentModel = 0;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
        models = new GameObject[maxModels];
        if (maxModels == 0) Debug.Log("Warning: please set maxModels to be >0");
    }

    // Called by OnClick() in SwitchModel.
    // If there are models in models[], make the last model invisible, make the next model (with wraparound) visible,
    // and update the next model with the most recently touched position.
    public void SwitchModel() {
        if (numModelsLoaded > 0) {
            models[currentModel].GetComponent<CustomBehaviour>().SetVisible(false);
            currentModel = (currentModel + 1) % numModelsLoaded;
            models[currentModel].gameObject.transform.position = modelPos;
            models[currentModel].GetComponent<CustomBehaviour>().SetVisible(true);
        }
    }

    // Called by CustomBehavior. Add a model to models[]
    public void AddModel(GameObject obj) {
        if (numModelsLoaded < maxModels) {
            models[numModelsLoaded] = obj;
            numModelsLoaded++;
            // update the newly-added model to be invisible
            models[numModelsLoaded - 1].GetComponent<CustomBehaviour>().SetVisible(false);
        } else {
            if (maxModels == 0) Debug.Log("Warning: could not add new model, models[] is full. Please increase maxModels");
        }
    }

    // Make touchPosition equal the place the user touches on the screen, if any.
    bool TryGetTouchPosition(out Vector2 touchPosition) {
        if (Input.touchCount > 0) {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }

    // If there's at least one model and a plane is being touched, make sure the current model is visible
    // and update its position to be at the point that was touched.
    void Update()
    {
        if (numModelsLoaded == 0) {
            return;
        }

        if(!TryGetTouchPosition(out Vector2 touchPosition)) {
            return;
        }

        if(_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon)) {
            if (!isCurrentVisible) {
                isCurrentVisible = true;
                models[currentModel].GetComponent<CustomBehaviour>().SetVisible(true);
            }
            var hitPose = hits[0].pose;
            modelPos = hitPose.position;
            models[currentModel].transform.position = modelPos;
        }
    }
}
