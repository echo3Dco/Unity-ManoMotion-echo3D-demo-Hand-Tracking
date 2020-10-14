using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManoMotion.UI.Buttons;
public class Category : MonoBehaviour
{
    public string categoryName;
    public int categoryOrder;
    public GameObject[] icons;
    public List<UIIconBehavior> iconBehaviors = new List<UIIconBehavior>();
    public GameObject[,] iconArray;

    #region UIValues

    public int maxIconsForRow;
    public float categoryPositionY;
    public int categoryHeight;
    public int numberOfRows;

    private int defaultHeight = 160;
    private int extraHeightForRow = 100;
    private int iconWidth = 60;
    private int iconLeftMargin;
    private int iconRightMargin;
    private int iconSpaceTaken;
    private int categoryWidth;
    private RectTransform rt;

    #endregion

    private void Awake()
    {
        InitializeIconBehaviors();
    }

    void Start()
    {
        InitializeUIValues();
        InitializeIconBehaviors();
        IconMainMenu.OnOrientationChanged += CalculateDimensions;
    }

    /// <summary>
    /// Initializes the values needed for the UI in order to be responsive.
    /// </summary>
    void InitializeUIValues()
    {
        defaultHeight = 160;
        extraHeightForRow = 100;
        iconWidth = 60;
        iconLeftMargin = iconWidth / 3;
        iconRightMargin = iconWidth / 3;
        iconSpaceTaken = iconWidth + iconLeftMargin + iconRightMargin;

        rt = this.GetComponent<RectTransform>();
    }

    /// <summary>
    /// Calculates the dimensions.
    /// </summary>
    public void CalculateDimensions()
    {
        StartCoroutine(Calculate());
    }

    /// <summary>
    /// Calculate the dimensions of each category in order to align the icons.
    /// </summary>
    /// <returns>The calculate.</returns>
    IEnumerator Calculate()
    {
        yield return new WaitForSeconds(0.05f);
        categoryWidth = Mathf.RoundToInt(rt.rect.width);
        numberOfRows = Mathf.CeilToInt((float)iconSpaceTaken * icons.Length / categoryWidth);
        categoryHeight = (Mathf.Max(numberOfRows, 1) * 100) + 50;

        //Fix the size of the category
        rt.sizeDelta = new Vector2(0, categoryHeight);
        maxIconsForRow = categoryWidth / iconSpaceTaken;
        rt = this.GetComponent<RectTransform>();
        categoryPositionY = rt.anchoredPosition.y;

        //Allign the icons inside the space of the category
        StartCoroutine(AlignIcons());
    }

    /// <summary>
    /// Aligns the icons.
    /// </summary>
    /// <returns>The icons.</returns>
    IEnumerator AlignIcons()
    {
        yield return new WaitForSeconds(0.01f);
        iconArray = new GameObject[numberOfRows, maxIconsForRow];

        int totalNumber = numberOfRows * maxIconsForRow;

        for (int index = 0; index < totalNumber; index++)
        {
            int column = index % maxIconsForRow;
            int row = index / maxIconsForRow;
            if (index < icons.Length)
            {
                iconArray[row, column] = icons[index];
            }
            if (iconArray[row, column])
            {
                iconArray[row, column].transform.localPosition = new Vector2(iconLeftMargin + column * iconSpaceTaken, -55 + (-100 * row));
            }
        }
    }

    void InitializeIconBehaviors()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            iconBehaviors.Add(icons[i].GetComponent<UIIconBehavior>());
        }
    }
}
