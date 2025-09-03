using UnityEngine;

[RequireComponent(typeof(SaveId))]
public abstract class SaveableBehaviour : MonoBehaviour, ISaveable
{
    private SaveId _id;
    public string SaveId => (_id ??= GetComponent<SaveId>()).Guid;

    public abstract string CaptureStateJson();

    public abstract void RestoreStateJson(string json);
}
