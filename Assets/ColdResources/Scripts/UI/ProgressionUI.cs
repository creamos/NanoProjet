using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionUI : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    private float sliderValue = 0.0f;

    [Header("Visual")]
    [SerializeField] private RectTransform _point;
    [SerializeField] private RectTransform _progressBar;

    // Update is called once per frame
    private void Update()
    {
        if (_gameManager && _gameManager.isGameRunning) {
            sliderValue = _gameManager.GameTime / _gameManager.MaxTime;
        }

        Vector2 point_pos = Vector2.zero;
        point_pos.y = (0.5f - sliderValue) * (_progressBar.rect.height - _progressBar.rect.width);
        _point.localPosition = point_pos;
    }
}
