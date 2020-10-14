using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BoundingBoxUI : MonoBehaviour
{

    [SerializeField]
    TextMesh top_left, width, height;
    public LineRenderer bound_line_renderer;
    private ManoUtils mano_utils;
    float textDepthModifier = 4;
    float textAdjustment = 0.01f;

    float backgroundDepth = 8;
    private void Start()
    {
        mano_utils = ManoUtils.Instance;
        bound_line_renderer.positionCount = 4;
    }

    float normalizedTopLeftX;
    float normalizedTopLeftY;
    float normalizedBBWidth;
    float normalizedHeight;

    Vector3 normalizedTopLeft;
    Vector3 normalizedTopRight;
    Vector3 normalizedBotRight;
    Vector3 normalizedBotLeft;
    Vector3 normalizedTextHeightPosition;
    Vector3 normalizedTextWidth;

    /// <summary>
    /// Positions the parts of the BoundingBox Points and updates the 3D Text information
    /// </summary>
    /// <param name="bounding_box"></param>
    public void UpdateInfo(BoundingBox bounding_box)
    {

        if (!mano_utils)
            mano_utils = ManoUtils.Instance;
        if (!bound_line_renderer)
        {
            Debug.Log("bound_line_renderer: null");
            return;

        }
        normalizedTopLeftX = bounding_box.top_left.x;
        normalizedTopLeftY = bounding_box.top_left.y;
        normalizedBBWidth = bounding_box.width;
        normalizedHeight = bounding_box.height;

        normalizedTopLeft = new Vector3(normalizedTopLeftX, normalizedTopLeftY);
        normalizedTopRight = new Vector3(normalizedTopLeftX + normalizedBBWidth, normalizedTopLeftY);
        normalizedBotRight = new Vector3(normalizedTopLeftX + normalizedBBWidth, normalizedTopLeftY - normalizedHeight);
        normalizedBotLeft = new Vector3(normalizedTopLeftX, normalizedTopLeftY - normalizedHeight);

        bound_line_renderer.SetPosition(0, ManoUtils.Instance.CalculateNewPosition(normalizedTopLeft, backgroundDepth));
        bound_line_renderer.SetPosition(1, ManoUtils.Instance.CalculateNewPosition(normalizedTopRight, backgroundDepth));
        bound_line_renderer.SetPosition(2, ManoUtils.Instance.CalculateNewPosition(normalizedBotRight, backgroundDepth));
        bound_line_renderer.SetPosition(3, ManoUtils.Instance.CalculateNewPosition(normalizedBotLeft, backgroundDepth));


        normalizedTopLeft.y += textAdjustment * 3;
        top_left.gameObject.transform.position = ManoUtils.Instance.CalculateNewPosition(normalizedTopLeft, backgroundDepth / textDepthModifier);
        top_left.text = "Top Left: " + "X: " + normalizedTopLeftX.ToString("F2") + " Y: " + normalizedTopLeftY.ToString("F2");

        normalizedTextHeightPosition = new Vector3(normalizedTopLeftX + normalizedBBWidth + textAdjustment, (normalizedTopLeftY - normalizedHeight / 2f));
        height.transform.position = ManoUtils.Instance.CalculateNewPosition(normalizedTextHeightPosition, backgroundDepth / textDepthModifier);
        height.GetComponent<TextMesh>().text = "Height: " + normalizedHeight.ToString("F2");

        normalizedTextWidth = new Vector3(normalizedTopLeftX + normalizedBBWidth / 2f, (normalizedTopLeftY - normalizedHeight) - textAdjustment);
        width.transform.position = ManoUtils.Instance.CalculateNewPosition(normalizedTextWidth, backgroundDepth / textDepthModifier);
        width.GetComponent<TextMesh>().text = "Width: " + normalizedBBWidth.ToString("F2");



    }

}




