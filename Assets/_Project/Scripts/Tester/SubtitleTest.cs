using UnityEngine;

public class SubtitleTest : MonoBehaviour
{
    private void Start()
    {
        var type = SubtitleMode.Click;
        UIManager.Instance.SubtitleUI.Enqueue("안녕하세요", type);
        UIManager.Instance.SubtitleUI.Enqueue("대사테스트 중입니다", type);
        UIManager.Instance.SubtitleUI.Enqueue("대사가 끝났습니다~", type);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            UIManager.Instance.SubtitleUI.Open();
            UIManager.Instance.SubtitleUI.Play();
        }
    }
}
