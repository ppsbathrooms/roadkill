using System;

[Serializable]
public static class PlayerData {
    private static int _eggCount;

    public static int EggCount {
        get => _eggCount;
        set {
            _eggCount = value;
            HUDController.Instance.UpdateEggText();
        }

    }
}
