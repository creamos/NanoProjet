using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PlayerFeedbackSetup : MonoBehaviour
{
    public GameObject playerVisual;
    public PlayerMovement playerMovement;
    
    [Header("Feedbacks")]
    [SerializeField] private MMF_Player _collisionFeedback;
    [SerializeField] private MMF_Player _boostFeedback;
    [SerializeField] private MMF_Player _grazeFeedback;
    
    // Start is called before the first frame update
    private void Start()
    {
        playerMovement.BoostEvent.AddListener(_boostFeedback.PlayFeedbacks);
        playerMovement.CollisionEvent.AddListener(_collisionFeedback.PlayFeedbacks);
        playerMovement.GrazeEvent.AddListener(_grazeFeedback.PlayFeedbacks);
        
        MMF_Scale scale_feedback = _collisionFeedback.GetFeedbackOfType<MMF_Scale>();
        scale_feedback.AnimateScaleTarget = playerVisual.transform;

        MMF_SquashAndStretch sns_feedback = _boostFeedback.GetFeedbackOfType<MMF_SquashAndStretch>();
        sns_feedback.SquashAndStretchTarget = playerVisual.transform;
    }
}