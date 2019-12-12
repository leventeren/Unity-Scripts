using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
 
public class FollowDragger : MonoBehaviour, IDragHandler, IInitializePotentialDragHandler, IEndDragHandler
{
 
    public float normalizedSliderSpeed = 0.7f;
    public bool easing = false;
    public float smoothing = 0.2f;
 
    private Vector2 m_currentPosition;
    private float currentVelocity;
 
    [SerializeField]private Slider m_slider;
    public Slider Slider
    {
        get
        {
            if (!this.m_slider)
                this.m_slider = this.GetComponent<Slider>();
 
            if (!this.m_slider) return null;
 
            return this.m_slider;
        }
        set { this.m_slider = value; }
    }
 
    private RectTransform m_sliderTransform;
    private RectTransform SliderTransform
    {
        get
        {
            if (!this.Slider)
                return null;
 
            if (!this.m_sliderTransform)
                this.m_sliderTransform = this.Slider.GetComponent<RectTransform>();
 
            return this.m_sliderTransform;
        }
    }
 
    private void Start()
    {
        SetTargetToCurrentValue();
    }
 
 
    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (!this.SliderTransform) return;
 
        Vector2 localPoint;
 
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this.SliderTransform, eventData.position, eventData.pressEventCamera, out localPoint))
            return;
 
        this.m_currentPosition = localPoint;
    }
 
    public void OnDrag(PointerEventData eventData)
    {
        if (!this.SliderTransform) return;
 
        Vector2 localPoint;
 
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this.SliderTransform, eventData.position, eventData.pressEventCamera, out localPoint))
            return;
 
        this.m_currentPosition = localPoint;
    }
 
    public void OnEndDrag(PointerEventData eventData)
    {
        SetTargetToCurrentValue();
    }
 
    private Vector2 NormalizeLocalPosition(Vector2 value)
    {
        if (!this.SliderTransform) return Vector2.zero;
 
        var size = this.SliderTransform.rect.size;
        var pivot = this.SliderTransform.pivot;
 
        value = new Vector2(Mathf.Clamp01(value.x / size.x + pivot.x), Mathf.Clamp01(value.y / size.y + pivot.y));
        return value;
    }
 
 
 
    private void SetTargetToCurrentValue()
    {
        if ((bool)this.SliderTransform)
        {
            m_currentPosition = new Vector2((this.Slider.normalizedValue - this.SliderTransform.pivot.x) * this.SliderTransform.rect.size.x, 0);
        }
    }
 
    private void OnValidate()
    {
        currentVelocity = 0;
    }
 
    private void Update()
    {
        if (!this.Slider) return;
 
        var current = this.Slider.normalizedValue;
        var target = NormalizeLocalPosition(this.m_currentPosition);
 
        if (Mathf.Abs(target.x - current) <= float.Epsilon)
            return;
 
        if (easing)
        {
            this.Slider.normalizedValue = Mathf.SmoothDamp(current, target.x, ref currentVelocity, smoothing, this.normalizedSliderSpeed);
        }
        else
        {
            this.Slider.normalizedValue = Mathf.MoveTowards(current, target.x, Time.deltaTime * this.normalizedSliderSpeed);
        }
 
    }
 
}
