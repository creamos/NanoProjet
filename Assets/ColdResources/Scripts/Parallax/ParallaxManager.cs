using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    [SerializeField] PlayersManagerSO _playersManager;
    GameManager _gameManager;
    ParallaxStarter[] _parallaxes;
    private void Awake ()
    {
        _parallaxes = FindObjectsOfType<ParallaxStarter>(true)
            .OrderBy(parallax => parallax.startTime).ToArray();

        _gameManager = FindObjectOfType<GameManager>();
    }
    private void Start ()
    {
        _gameManager.GameStarted.AddListener(OnGameStarted);
    }

    private void OnGameStarted ()
    {
        Debug.Log("Parallax Handling Started");
        StartCoroutine(ParallaxLoadRoutine());
    }

    private IEnumerator ParallaxLoadRoutine ()
    {
        for (int i = 0; i < _parallaxes.Length; i++) {
            yield return new WaitUntil(() => _parallaxes[i].startTime <= _gameManager.GameTime);
            StartParallax(_parallaxes[i]);
        }
    }

    private void StartParallax (ParallaxStarter parallax)
    {
        parallax.transform.position = Vector3.up * (_playersManager._players
            .Select(player => player.transform.position.y)
            .OrderBy(position => position).First()
            + parallax.startOffset);

        parallax.gameObject.SetActive(true);
    }
}
