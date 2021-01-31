using UnityEngine;

/// <summary>
/// Resizes a UI element with a RectTransform to respect the safe areas of the current device.
/// This is particularly useful on an iPhone X, where we have to avoid the notch and the screen
/// corners.
/// 
/// The easiest way to use it is to create a root Canvas object, attach this to a "SafeAreaContainer"
/// child of that canvas, and then to lay out other UI elements within the SafeAreaContainer, which
/// will adjust size appropriately for the current device.
/// </summary>
public class IOS_Safezone : MonoBehaviour
{

    private Rect lastSafeArea;
    private RectTransform parentRectTransform;

    [SerializeField]
    private bool isIphone;

    [SerializeField]
    private bool isStage;

    private void Start()
    {
        parentRectTransform = this.GetComponentInParent<RectTransform>();
    }

    private void Update()
    {
        if (isIphone)
        {
            if (!isStage)
            {
                if (lastSafeArea != Screen.safeArea)
                {
                    ApplySafeArea();
                }
            }

        }

    }

    public bool getSystemType()
    {
        return isIphone;
    }

    private void ApplySafeArea()
    {
        Rect safeAreaRect = Screen.safeArea;

        float scaleRatio = parentRectTransform.rect.width / Screen.width;

        var left = safeAreaRect.xMin * scaleRatio;
        var right = -(Screen.width - safeAreaRect.xMax) * scaleRatio;
        var top = -safeAreaRect.yMin * scaleRatio;
        var bottom = (Screen.height - safeAreaRect.yMax) * scaleRatio;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.offsetMin = new Vector2(left, bottom);
        rectTransform.offsetMax = new Vector2(right, top);

        lastSafeArea = Screen.safeArea;

        //Debug.Log("Safe area is: " + lastSafeArea);
    }
}