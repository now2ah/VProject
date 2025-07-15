using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace VProject.UIs
{
    public class MainGuideTextUI : MonoBehaviour
    {
        private float _alpha = 1f;
        private bool _isFading = true;
        private TextMeshProUGUI _guideText;

        private void Awake()
        {
            _guideText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (_isFading)
            {
                _alpha -= Time.deltaTime;
                Color newColor = new Color(_guideText.color.r, _guideText.color.g, _guideText.color.b, _alpha);
                _guideText.color = newColor;

                if (_alpha <= 0f)
                    _isFading = false;
            }
            else
            {
                _alpha += Time.deltaTime;
                Color newColor = new Color(_guideText.color.r, _guideText.color.g, _guideText.color.b, _alpha);
                _guideText.color = newColor;

                if (_alpha >= 1f)
                    _isFading = true;
            }
        }
    }
}

