using UnityEngine;

[RequireComponent(typeof(PlayerDataReference))]
public class PlayerSetup : MonoBehaviour
{
    [SerializeField] GameObject p1Visual_pf, p2Visual_pf;
    [SerializeField] PlayerFeedbackSetup p1Feedback_pf, p2Feedback_pf;

    public void Init(PlayerDataSO data, Vector3 position)
    {
        GetComponent<PlayerDataReference>().playerData = data;

        transform.position = position;

        GameObject visual;
        PlayerFeedbackSetup feedback;
        if (data.ID == 0) {
            visual = Instantiate(p1Visual_pf, transform);
            feedback = Instantiate(p1Feedback_pf, transform);
            feedback.playerVisual = visual;
            feedback.playerMovement = GetComponent<PlayerMovement>();
        }
        else if (data.ID == 1) {
            visual = Instantiate(p2Visual_pf, transform);
            feedback = Instantiate(p2Feedback_pf, transform);
            feedback.playerVisual = visual;
            feedback.playerMovement = GetComponent<PlayerMovement>();
        }  
    }
}