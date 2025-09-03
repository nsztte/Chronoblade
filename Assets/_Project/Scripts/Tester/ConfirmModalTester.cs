using UnityEngine;

public class ConfirmModalTester : MonoBehaviour
{
     private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) // 테스트용 단축키
        {
            UIManager.Instance.ShowConfirm(
                "테스트 모달",
                "정말로 삭제하시겠습니까?",
                () => Debug.Log("✅ 확인 누름"),
                () => Debug.Log("❌ 취소 누름")
            );
        }
    }
}
