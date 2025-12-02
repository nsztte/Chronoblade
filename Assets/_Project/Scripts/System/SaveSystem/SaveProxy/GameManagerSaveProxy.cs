using System;
using UnityEngine;

public class GameManagerSaveProxy : SaveableBehaviour
{
    [Serializable]
    private class Data
    {
        public string state;
    }

    public override string CaptureStateJson()
    {
        var gm = GameManager.Instance;
        var d = new Data();

        if (gm.CurrentGameState is PuzzleState)
            d.state = "Puzzle";
        else if (gm.CurrentGameState is CombatState)
            d.state = "Combat";
        else
            d.state = "Exploration";

        return JsonUtility.ToJson(d);
    }

    public override void RestoreStateJson(string json)
    {
        if (string.IsNullOrEmpty(json)) return;
        var d = JsonUtility.FromJson<Data>(json);

        var gm = GameManager.Instance;
        if (gm == null) return;

        gm.SetLoadedGameState(d.state);
    }
}
