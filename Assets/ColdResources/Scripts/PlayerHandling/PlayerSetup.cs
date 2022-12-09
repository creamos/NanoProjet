using UnityEngine;

[RequireComponent(typeof(PlayerDataReference))]
public class PlayerSetup : MonoBehaviour
{
    [SerializeField] GameObject p1Visual_pf, p2Visual_pf;
    public void Init(PlayerDataSO data)
    {
        GetComponent<PlayerDataReference>().playerData = data;

        if (data.ID == 0)
            Instantiate(p1Visual_pf, transform);
        else if (data.ID == 1)
            Instantiate(p2Visual_pf, transform);
    }
}
