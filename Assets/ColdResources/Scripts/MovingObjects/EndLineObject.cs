using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using MoreMountains.Feedbacks;

public class EndLineObject : MonoBehaviour
{
    // Very ugly code - and that's it
    [SerializeField] private float _endWaitTime = 3.0f;

    private Coroutine _endRoutine;

    private void OnTriggerEnter2D(Collider2D other) {
        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
        if (playerMovement && _endRoutine == null) {
            _endRoutine = StartCoroutine(EndProcess());
        }
    }

    IEnumerator EndProcess() {
        MMF_Player end_feedback = GetComponent<MMF_Player>();
        if (end_feedback) end_feedback.PlayFeedbacks();

        GameObject target_vcam = GameObject.FindGameObjectWithTag("GameplayCamera");
        target_vcam.SetActive(false);
        
        yield return new WaitForSeconds(_endWaitTime);

        _endRoutine = null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
