using UnityEngine;
using UnityEngine.UI;

public class AudioTabController : MonoBehaviour, IOptionsTab
{
    [SerializeField] private Toggle muteToggle;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        muteToggle.onValueChanged.AddListener(OnMuteChanged);
        masterSlider.onValueChanged.AddListener(AudioManager.Instance.SetMaster);
        bgmSlider.onValueChanged.AddListener(AudioManager.Instance.SetBGM);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFX);
    }

    private void Start()
    {
        // PlayerPrefs 값을 반영하여 UI 초기화
        muteToggle.isOn = AudioManager.Instance.IsMasterMuted();
        masterSlider.value = AudioManager.Instance.GetMaster();
        bgmSlider.value = AudioManager.Instance.GetBGM();
        sfxSlider.value = AudioManager.Instance.GetSFX();
    }

    private void OnMuteChanged(bool isMuted)
    {
        AudioManager.Instance.SetMasterMuted(isMuted);
    }

    public void OnOpen(OptionOpenMode from)
    {
        gameObject.SetActive(true);

        // 열릴 때 UI 상태 동기화
        muteToggle.isOn = AudioManager.Instance.IsMasterMuted();
        masterSlider.value = AudioManager.Instance.GetMaster();
        bgmSlider.value = AudioManager.Instance.GetBGM();
        sfxSlider.value = AudioManager.Instance.GetSFX();
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }
}
