using UnityEngine;
using UnityEngine.UI;

public class ManualModeToggle : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private SierpinskiGasket sierpinskiGasket;
    [SerializeField] private GameObject[] manualModeUIElements;

    private void Awake()
    {
        toggle.onValueChanged.AddListener(delegate { ToggleManualMode(); });
    }

    private void ToggleManualMode()
    {
        sierpinskiGasket.ManualMode = toggle.isOn;
        ToggleUI(toggle.isOn);
    }

    private void ToggleUI(bool toggle)
    {
        foreach (GameObject element in manualModeUIElements)
        {
            element.SetActive(toggle);
        }
    }
}
