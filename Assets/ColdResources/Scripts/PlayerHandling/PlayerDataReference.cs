using UnityEngine;

public class PlayerDataReference : MonoBehaviour
{
    public PlayerDataSO playerData;

    public int ID => playerData?.ID ?? -1;
}
