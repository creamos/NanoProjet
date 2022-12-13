using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    [SerializeField] PlayersManagerSO _playersManager;
    GameManager _gameManager;
    ParallaxStarter[] _parallaxesWithStart, _parallaxesWithEnd;
    private void Awake ()
    {
        var parallaxes = FindObjectsOfType<ParallaxStarter>(true)
            .OrderBy(parallax => parallax.startTime).ToArray();

        _parallaxesWithStart = parallaxes.Where(parallax => parallax.asStartTime)
            .OrderBy(parallax => parallax.startTime).ToArray();
        _parallaxesWithEnd = parallaxes.Where(parallax => parallax.asEndTime)
            .OrderBy(parallax => parallax.asEndTime).ToArray();

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
        StartCoroutine(ParallaxUnloadRoutine());
    }

    private IEnumerator ParallaxLoadRoutine ()
    {
        for (int i = 0; i < _parallaxesWithStart.Length; i++) {
            yield return new WaitUntil(() => _parallaxesWithStart[i].startTime <= _gameManager.GameTime);
            StartParallax(_parallaxesWithStart[i]);
        }
    }

    private IEnumerator ParallaxUnloadRoutine ()
    {
        for (int i = 0; i < _parallaxesWithEnd.Length; i++) {
            yield return new WaitUntil(() => _parallaxesWithEnd[i].endTime <= _gameManager.GameTime);
            EndParallax(_parallaxesWithEnd[i]);
        }
    }

    private void StartParallax (ParallaxStarter parallax)
    {
        if (parallax) {

            parallax.transform.position = Vector3.up * (Camera.main.transform.position.y
                + parallax.startOffset);

            parallax.gameObject.SetActive(true);
        }
    }


    private void EndParallax (ParallaxStarter parallax)
    {
        if (parallax) {

            foreach (var p in parallax.GetComponentsInChildren<ParallaxPaner>()) {
                p.isArriving = false;
                p.isLeaving = true;
            }
        }
    }
}
