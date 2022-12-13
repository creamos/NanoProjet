using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EndLineObject : MonoBehaviour
{
    // Very ugly code - and that's it
    [SerializeField] private float _endWaitTime = 3.0f;

    private void OnTriggerEnter2D(Collider2D other) {
        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
        if (playerMovement) {
            StartCoroutine("EndProcess");
        }
    }

    IEnumerator EndProcess() {
        GameObject target_vcam = GameObject.FindGameObjectWithTag("GameplayCamera");
        target_vcam.SetActive(false);
        
        yield return new WaitForSeconds(_endWaitTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
