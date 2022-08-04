using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using UnityEngine.EventSystems;


public class ColorPicker : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public static bool MouseOverColorPicker = false;
    public event Action<Color> OnColorPressed;
    public event Action<Color> OnColorOver;
    public event Action OnColorPressUp;
    public event Action<float> OnMetalValueChange;
    [SerializeField] Text InfoText = null;
    [SerializeField] Slider metalSlider = null;
    [SerializeField] RectTransform rect;
    [SerializeField] Text metalicValueText;
    Texture2D colorTexture;
    [SerializeField] Image img;

    // Start is called before the first frame update
    void Start()
    {
        colorTexture = img.mainTexture as Texture2D;
        metalicValueText.text = metalSlider.value.ToString("F2");
    }

    private void OnEnable()
    {
        metalSlider.onValueChanged.AddListener(OnSliderChange);
    }

    // Update is called once per frame
    void Update()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition))
        {
            Vector2 delta;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out delta);

            float width = rect.rect.width;
            float height = rect.rect.height;
            delta += new Vector2(width * 0.5f, height * 0.5f);
            float x = Mathf.Clamp(delta.x / width, 0f, 1f);
            float y = Mathf.Clamp(delta.y / height, 0f, 1f);
            int texX = Mathf.RoundToInt(x * colorTexture.width);
            int texY = Mathf.RoundToInt(y * colorTexture.height);

            Color color = colorTexture.GetPixel(texX, texY);

            if(color.a != 0)
            {
                if (Input.GetMouseButton(0))
                {
                    OnColorOver?.Invoke(color);
                }
                if (Input.GetMouseButtonUp(1))
                {
                    OnColorPressed?.Invoke(color);
                }
                if (Input.GetMouseButtonUp(0))
                {
                    OnColorPressUp?.Invoke();
                }

                InfoText.text = ColorUtility.ToHtmlStringRGB(color);
            }
        }
    }

    private void OnSliderChange(float value)
    {
        metalicValueText.text = value.ToString("F2");
        OnMetalValueChange?.Invoke(value);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseOverColorPicker = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseOverColorPicker = false;
    }

    private void OnDisable()
    {
        metalSlider.onValueChanged.RemoveAllListeners();
    }
}
