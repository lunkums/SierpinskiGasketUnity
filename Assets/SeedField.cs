using UnityEngine;

public class SeedField : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField textField;
    [SerializeField] private TMPro.TMP_Text seedDisplay;
    [SerializeField] private SierpinskiGasket sierpinskiGasket;

    private void Awake()
    {
        textField.onSubmit.AddListener(delegate { Submit(); });
    }

    private void Submit()
    {
        sierpinskiGasket.Seed = int.Parse(textField.text);
        textField.text = "";
        seedDisplay.text = string.Format("{0} [Seed]", sierpinskiGasket.Seed);
    }
}
