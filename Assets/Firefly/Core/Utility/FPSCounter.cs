using System;
using UnityEngine;
using TMPro;

namespace UnityStandardAssets.Utility
{
    [RequireComponent(typeof (TextMeshProUGUI))]
    public class FPSCounter : MonoBehaviour
    {
        const float FPSMeasurePeriod = 0.5f;
        private int _fpsAccumulator = 0;
        private float _fpsNextPeriod = 0;
        private int _currentFps;
        const string Display = "{0} FPS";
        private TextMeshProUGUI _textMeshPro;


        private void Start()
        {
            _fpsNextPeriod = Time.realtimeSinceStartup + FPSMeasurePeriod;
            _textMeshPro = GetComponent<TextMeshProUGUI>();
        }


        private void Update()
        {
            // measure average frames per second
            _fpsAccumulator++;
            if (Time.realtimeSinceStartup > _fpsNextPeriod)
            {
                _currentFps = (int) (_fpsAccumulator/FPSMeasurePeriod);
                _fpsAccumulator = 0;
                _fpsNextPeriod += FPSMeasurePeriod;
                _textMeshPro.text = string.Format(Display, _currentFps);
            }
        }
    }
}
