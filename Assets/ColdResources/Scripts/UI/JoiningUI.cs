using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoiningUI : MonoBehaviour
{
    [SerializeField] private PlayersManagerSO _playersManagerSO;
    private GameStarter _gameStarter;

    [SerializeField] private RectTransform[] _imagesOrigins = new RectTransform[2]; 
    [SerializeField] private Image[] _joinImages = new Image[2];
    [SerializeField] private Image[] _startImages = new Image[2];
    [SerializeField] private Image[] _readyImages = new Image[2];
    
    private void Awake ()
    {
        _gameStarter = FindObjectOfType<GameStarter>();
        if (_gameStarter == null) gameObject.SetActive(false);
    }

    private void Start() {
        for (int i = 0; i < _playersManagerSO.players.Length; i++)
        {
            float offset = _playersManagerSO.startPositionsOffset * (i - 0.5f) * 80f;
            Vector2 pos = _imagesOrigins[i].position;
            pos.x += offset;
            _imagesOrigins[i].position = pos;

            /*
             
            var cam = GameObject.FindGameObjectWithTag("BackgroundCamera").GetComponent<Camera>();
            var pos = cam.ScreenToWorldPoint(_imagesOrigins[i].position);

            float offset = (i==0?-1:1) * _playersManagerSO.startPositionsOffset;
            
            pos.x += offset;


            _imagesOrigins[i].position = cam.WorldToScreenPoint(pos);
             
             */
        }
    }

    // Update is called once per frame
    private void Update()
    {
        for (int i = 0; i < _playersManagerSO.players.Length; i++)
        {
            if (_playersManagerSO.players[i] != null) {
                _joinImages[i].enabled = false;
                if (_gameStarter.playersReady[i]) {
                    _startImages[i].enabled = false;
                    _readyImages[i].enabled = true;
                } else {
                    
                    _startImages[i].enabled = true;
                    _readyImages[i].enabled = false;
                }
            }
            else {
                _joinImages[i].enabled = true;
                _startImages[i].enabled = false;
                _readyImages[i].enabled = false;
            }
        }
    }
}
