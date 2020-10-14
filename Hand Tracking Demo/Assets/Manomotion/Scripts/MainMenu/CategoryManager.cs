using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ManoMotion.UI.Buttons;
public class CategoryManager : MonoBehaviour
{
	private static CategoryManager instance;
	public static CategoryManager Instance
	{
		get
		{
			return instance;
		}

		set
		{
			instance = value;
		}
	}

	public List<UIIconBehavior> uIIconBehaviors = new List<UIIconBehavior>();

	[SerializeField]
	private GameObject[] categories;

	private Vector2 categoryPosition;
	private Category previousCategory;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	void Start()
	{
		PositionCategories();
		IconMainMenu.OnOrientationChanged += PositionCategories;
	}

	public void SetupMenu(List<UIIconBehavior.IconFunctionality> listOfFeatures)
	{
		GetAllIconBehaviors();

		for (int i = 0; i < uIIconBehaviors.Count; i++)
		{
			uIIconBehaviors[i].SetIsActive(false);
		}

		for (int j = 0; j < uIIconBehaviors.Count; j++)
		{
			for (int i = 0; i < listOfFeatures.Count; i++)
			{

				UIIconBehavior currentIcon = uIIconBehaviors[j];
				UIIconBehavior.IconFunctionality currentFunctionality = listOfFeatures[i];
				if (currentIcon.myIconFunctionality == currentFunctionality)
				{
					currentIcon.SetIsActive(true);
				}
			}
		}
	}

	/// <summary>
	/// Positions the categories.
	/// </summary>
	void PositionCategories()
	{
		StartCoroutine(PositionCategoriesAfter(0.1f));
	}

	/// <summary>
	/// Positions the categories after a delay.
	/// </summary>
	/// <returns>The categories after.</returns>
	/// <param name="time">Requires a float value of delay.</param>
	IEnumerator PositionCategoriesAfter(float time)
	{
		yield return new WaitForSeconds(time);
		for (int i = 1; i < categories.Length; i++)
		{
			if (categories[i - 1].GetComponent<Category>())
			{
				previousCategory = categories[i - 1].GetComponent<Category>();
				categoryPosition = categories[i].GetComponent<RectTransform>().anchoredPosition;
				categories[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (-previousCategory.categoryHeight + categories[i - 1].GetComponent<RectTransform>().anchoredPosition.y));
			}
		}
	}

	/// <summary>
	/// Retrieves all the Icon Behaviors from their Categories
	/// </summary>
	void GetAllIconBehaviors()
	{
		for (int i = 0; i < categories.Length; i++)
		{
			Category currentCategory = categories[i].GetComponent<Category>();

			for (int j = 0; j < currentCategory.iconBehaviors.Count; j++)
			{
				UIIconBehavior currentBehavior = currentCategory.iconBehaviors[j];
				uIIconBehaviors.Add(currentBehavior);
			}
		}
	}
}
