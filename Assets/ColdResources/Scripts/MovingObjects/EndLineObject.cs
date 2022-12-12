using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EndLineObject : MonoBehaviour
{
    // Very ugly code for playtests
    [SerializeField] private PlayersManagerSO _playersManager;
    [SerializeField] private float _endWaitTime = 3.0f;

    private void OnTriggerEnter2D(Collider2D other) {
        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
        if (playerMovement) {
            StartCoroutine("EndProcess");
            foreach (PlayerInput player in _playersManager.players)
            {
                playerMovement = player.GetComponent<PlayerMovement>();
                playerMovement.enabled = false;
            }
        }
    }

    IEnumerator EndProcess() {
        yield return new WaitForSeconds(_endWaitTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
