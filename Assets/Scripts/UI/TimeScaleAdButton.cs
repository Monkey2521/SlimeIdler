using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScaleAdButton : MonoBehaviour
{
    [SerializeField] private Button _adButton;
    [SerializeField] private Text _timeText;
    [SerializeField] private float _timeScale = 2f;
    [Tooltip("Duration in seconds")]
    [SerializeField] private int _timeScaleDuration = 600;

    private readonly float _defaultTimeScale = 1;
    private int _timer;

    private void OnEnable()
    {
        _timeText.enabled = false;
        Time.timeScale = _defaultTimeScale;
    }

    public void OnClick()
    {
        StopAllCoroutines();

        _timeText.enabled = true;

        Time.timeScale = _timeScale;
        _timer = _timeScaleDuration;

        _adButton.enabled = false;

        StartCoroutine(OnAdWatched());
        StartCoroutine(WaitUpdate());
    }

    private IEnumerator OnAdWatched()
    {
        yield return new WaitForSecondsRealtime(_timeScaleDuration);

        _timeText.enabled = false;
        Time.timeScale = _defaultTimeScale;
        _adButton.enabled = true;
    }

    private IEnumerator WaitUpdate()
    {
        _timeText.text = IntegerFormatter.GetTime(_timer);

        yield return new WaitForSecondsRealtime(1);

        _timer--;

        StartCoroutine(WaitUpdate());
    }
}
