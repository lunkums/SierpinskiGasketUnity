using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MaxPointsSlider : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] private Slider slider;
    [SerializeField] private SierpinskiGasket sierpinskiGasket;

    public void OnPointerUp(PointerEventData eventData)
    {
        sierpinskiGasket.MaxPoints = Mathf.CeilToInt(slider.value);
    }
}
