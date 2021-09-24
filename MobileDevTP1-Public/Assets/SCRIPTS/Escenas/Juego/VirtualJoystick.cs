using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] string player = "1";
    [SerializeField] RectTransform stick = null;
    [SerializeField] Image background = null;

    public float limit = 250f;

    private void Start()
    {
        background.color = Color.grey;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = ConvertToLocal(eventData);
        if (pos.magnitude > limit)
            pos = pos.normalized * limit;
        stick.anchoredPosition = pos;

        float x = pos.x / limit;

        SetHorizontal(x);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        background.color = Color.red;
        stick.anchoredPosition = ConvertToLocal(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        background.color = Color.grey;
        stick.anchoredPosition = Vector2.zero;
        SetHorizontal(0);
    }

    private void OnDisable()
    {
        SetHorizontal(0);
    }

    // -------------------------------

    Vector2 ConvertToLocal(PointerEventData eventData)
    {
        Vector2 newPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform,
            eventData.position,
            eventData.enterEventCamera,
            out newPos);
        return newPos;
    }

    void SetHorizontal(float val)
    {
        InputManager.Instance.GetInput(player).SetHorizontal(val);
    }
}