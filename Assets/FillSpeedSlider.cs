using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FillSpeedSlider : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] private Slider slider;
    [SerializeField] private SierpinskiGasket sierpinskiGasket;

    public void OnPointerUp(PointerEventData eventData)
    {
        sierpinskiGasket.FillPercentage = slider.value;
    }
}
