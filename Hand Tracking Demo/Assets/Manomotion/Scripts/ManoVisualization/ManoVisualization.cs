using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("ManoMotion/ManoVisualization")]
public class ManoVisualization : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private Transform bounding_box_prefab;

    [SerializeField]
    private bool _show_bounding_box;

    //Not the AR Background but a Plane Mesh filled with the camera info from ManoMotion
    //[SerializeField]
    private bool _show_background;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private GameObject manomotionGenericLayer;

    private Transform[] bounding_box;
    private BoundingBoxUI[] bounding_box_ui;

    private GameObject bounding_box_parent;
    private MeshRenderer _layer_background;

    int handsSupportedByLicence;

    #endregion

    #region Properties

    public bool Show_bounding_box
    {
        get
        {
            return _show_bounding_box;
        }

        set
        {
            _show_bounding_box = value;
        }
    }

    public bool Show_background
    {
        get
        {
            return _show_background;
        }

        set
        {
            _show_background = value;
        }
    }

    #endregion

    #region Initializing Components


    /// <summary>
    /// Creates the meshes needed by the different Manomotion Layers and also parents them to the scene's Main Camera
    /// </summary>
    private void InstantiateManomotionMeshes()
    {
        int backgroundInitialDepth = 1;
        GameObject background = Instantiate(manomotionGenericLayer);
        background.transform.name = "Background";
        background.transform.SetParent(cam.transform);
        _layer_background = background.GetComponent<MeshRenderer>();
        _layer_background.transform.localPosition = new Vector3(0, 0, backgroundInitialDepth);
    }

    /// <summary>
    /// Creates the bounding boxes that surround the hand. 
    /// </summary>
    void CreateBoundingBoxes()
    {
        bounding_box_parent = new GameObject();
        bounding_box_parent.name = "Bounding Box Parent";
        bounding_box_parent.transform.SetParent(cam.transform);
        bounding_box = new Transform[handsSupportedByLicence];
        bounding_box_ui = new BoundingBoxUI[bounding_box.Length];

        for (int i = 0; i < bounding_box.Length; i++)
        {
            bounding_box[i] = Instantiate(bounding_box_prefab);
            bounding_box[i].SetParent(bounding_box_parent.transform);
            bounding_box[i].gameObject.name = "BoundingBox";
            bounding_box_ui[i] = bounding_box[i].GetComponent<BoundingBoxUI>();

        }
    }

    void Start()
    {
        if (!cam)
            cam = Camera.main;

        SetHandsSupportedByLicence();
        InstantiateManomotionMeshes();
        CreateBoundingBoxes();
        ManomotionManager.OnManoMotionFrameProcessed += DisplayInformationAfterManoMotionProcessFrame;
    }

    /// <summary>
    /// Set the maximum number of hands that can be simultaneously detected by Manomotion Manager based on the licence.
    /// This process is based on your Licence privilliges.
    /// </summary>
    void SetHandsSupportedByLicence()
    {
        handsSupportedByLicence = 1;
    }
    #endregion

    /// <summary>
    /// Visualizes information from the ManoMotion Manager after the frame has been processed.
    /// </summary>
    void DisplayInformationAfterManoMotionProcessFrame()
    {
        for (int handIndex = 0; handIndex < handsSupportedByLicence; handIndex++)
        {
            Warning warning = ManomotionManager.Instance.Hand_infos[handIndex].hand_info.warning;
            TrackingInfo trackingInfo = ManomotionManager.Instance.Hand_infos[handIndex].hand_info.tracking_info;
            ShowBoundingBoxInfo(trackingInfo, handIndex, warning);
        }
        if (_layer_background)
        {
            ShowBackground(ManomotionManager.Instance.Visualization_info.rgb_image, _layer_background);
        }
    }

    /// <summary>
    /// Shows the bounding box info.
    /// </summary>
    /// <param name="tracking_info">Requires tracking information of the hand</param>
    /// <param name="index">Requires the int index value that refers to a given hand from the array of hands </param>
    private void ShowBoundingBoxInfo(TrackingInfo tracking_info, int index, Warning warning)
    {
        if (warning != Warning.WARNING_HAND_NOT_FOUND && Show_bounding_box)
        {
            if (!bounding_box_ui[index])
            {
                CreateBoundingBoxes();
            }
            if (bounding_box_ui[index])
            {

                if (!bounding_box_ui[index].gameObject.activeInHierarchy)
                {
                    bounding_box_ui[index].gameObject.SetActive(true);
                }
                bounding_box_ui[index].UpdateInfo(tracking_info.bounding_box);
            }
        }
        else
        {
            if (bounding_box_ui[index] && bounding_box_ui[index].gameObject.activeInHierarchy)
            {
                bounding_box_ui[index].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Projects the texture received from the camera as the background
    /// </summary>
    /// <param name="backgroundTexture">Requires the texture captured from the camera</param>
    /// <param name="backgroundMeshRenderer">Requires the MeshRenderer that the texture will be displayed</param>
    void ShowBackground(Texture2D backgroundTexture, MeshRenderer backgroundMeshRenderer)
    {
        backgroundMeshRenderer.enabled = _show_background;

        if (_show_background)
        {
            ManoUtils.Instance.OrientMeshRenderer(backgroundMeshRenderer);
            backgroundMeshRenderer.material.mainTexture = backgroundTexture;
            ManoUtils.Instance.AjustBorders(backgroundMeshRenderer, ManomotionManager.Instance.Manomotion_Session);
        }
    }

    /// <summary>
    /// Toggles the visibility of the given gameobject
    /// </summary>
    /// <param name="givenObject">Requires a gameObject that will change layers</param>
    private void ToggleObjectVisibility(GameObject givenObject)
    {
        givenObject.SetActive(!givenObject.activeInHierarchy);
    }
}