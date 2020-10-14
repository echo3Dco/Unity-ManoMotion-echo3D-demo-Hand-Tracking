using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public abstract class ManomotionBase : MonoBehaviour
{
    [Tooltip("Insert the key gotten from the webpage here https://www.manomotion.com/my-account/licenses/")]
    [SerializeField]
    protected string _licenseKey;
    protected abstract void Init(string serial_key);
    protected abstract void SetResolutionValues(int width, int height);
    protected abstract void SetUnityConditions();
    protected abstract void CalculateFPSAndProcessingTime();
    protected abstract void ProcessManomotion();
    protected abstract void UpdateTexturesWithNewInfo();
    protected abstract void InstantiateSession();
}
