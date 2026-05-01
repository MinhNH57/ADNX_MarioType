using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{
    [Header("Nơi chứa Animator của nhân vật")]
    public Animator targetAnimator;

    [Header("Danh sách Bộ Animation (Animator Controller)")]
    public RuntimeAnimatorController[] characterAnimators;

    private void Start()
    {
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);

        if (targetAnimator != null && selectedIndex < characterAnimators.Length)
        {
            targetAnimator.runtimeAnimatorController = characterAnimators[selectedIndex];
        }
        else
        {
            Debug.LogWarning("Lỗi: Bạn chưa gán Target Animator hoặc mảng Animators bị thiếu!");
        }
    }
}