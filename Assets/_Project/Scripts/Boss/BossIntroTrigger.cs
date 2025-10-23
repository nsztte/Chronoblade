using UnityEngine;

public class BossIntroTrigger : MonoBehaviour
{
    [SerializeField] BossIntroCutscene bossIntroCutscene;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bossIntroCutscene.StartPlay();
            
            gameObject.SetActive(false);
        }
    }
}
