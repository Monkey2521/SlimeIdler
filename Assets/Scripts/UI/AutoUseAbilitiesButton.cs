using UnityEngine;
using UnityEngine.UI;

public class AutoUseAbilitiesButton : MonoBehaviour
{
    [SerializeField] private Text _buttonText;

    private ButtonStates _currentState = ButtonStates.Off;

    public void OnClick()
    {
        if (_currentState.Equals(ButtonStates.Off))
        {
            _currentState = ButtonStates.On;
            _buttonText.text = "AUTO on";
        }
        else
        {
            _currentState = ButtonStates.Off;
            _buttonText.text = "AUTO off";
        }
    }

    private enum ButtonStates 
    {
        Off,
        On
    }
}
