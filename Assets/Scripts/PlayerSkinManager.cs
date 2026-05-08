using Unity.Netcode;
using UnityEngine;

// Đổi từ MonoBehaviour sang NetworkBehaviour
public class PlayerSkinManager : NetworkBehaviour
{
    [Header("Nơi chứa Animator của nhân vật")]
    public Animator targetAnimator;

    [Header("Danh sách Bộ Animation (Animator Controller)")]
    public RuntimeAnimatorController[] characterAnimators;

    private NetworkVariable<int> selectedSkinIndex = new NetworkVariable<int>(0,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        selectedSkinIndex.OnValueChanged += (oldValue, newValue) =>
        {
            ApplyAnimator(newValue);
        };
        if (IsOwner)
        {
            int myLocalIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
            selectedSkinIndex.Value = myLocalIndex;

            ApplyAnimator(myLocalIndex);
        }
        else
        {
            ApplyAnimator(selectedSkinIndex.Value);
        }
    }

    private void ApplyAnimator(int index)
    {
        if (targetAnimator != null && index >= 0 && index < characterAnimators.Length)
        {
            targetAnimator.runtimeAnimatorController = characterAnimators[index];
            Debug.Log($"Đã đồng bộ skin ID: {index} cho nhân vật.");
        }
    }
}