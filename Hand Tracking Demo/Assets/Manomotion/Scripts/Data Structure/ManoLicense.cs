using System.Runtime.InteropServices;

/// <summary>
/// List of available answers for the status of a ManoMotion answer.
/// </summary>
public enum LicenseAnswer
{
    LICENSE_OK = 30,
    LICENSE_KEY_NOT_FOUND = 31,
    LICENSE_EXPIRED = 32,
    LICENSE_INVALID_PLAN = 33,
    LICENSE_KEY_BLOCKED = 34,
    LICENSE_INVALID_ACCESS_TOKEN = 35,
    LICENSE_ACCESS_DENIED = 36,
    LICENSE_MAX_NUM_DEVICES = 37,
    LICENSE_UNKNOWN_SERVER_REPLY = 38,
    LICENSE_PRODUCT_NOT_FOUND = 39,
    LICENSE_INCORRECT_INPUT_PARAMETER = 40,
    LICENSE_INTERNET_REQUIRED = 41,
    LICENSE_INCORRECT_BUNDLE_ID = 42
};

/// <summary>
/// Contains information regarding the ManoMotion licence currently in use.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ManoLicense
{
    public LicenseAnswer license_status;
    public int machines_left;
    public int days_left;
    public float version;
}
