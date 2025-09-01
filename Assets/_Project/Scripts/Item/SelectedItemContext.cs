using System;

public static class SelectedItemContext
{
    public static ItemData SelectedItem { get; private set; }
    public static event Action<ItemData> OnSelectedItemChanged;

    public static void Set(ItemData item)
    {
        if (SelectedItem == item) return;
        SelectedItem = item;
        OnSelectedItemChanged?.Invoke(item);
    }

    public static void Clear() => Set(null);
}
