public class ResourcesPath
{
    private const string CHARACTERS = "Prefabs/Characters";
    private const string EFFECTS    = "Prefabs/Effects";
    private const string ENEMYS     = "Prefabs/Enemys";
    private const string UI         = "Prefabs/UI";

    public static string Characters(string _charName) { return CHARACTERS + "/" + _charName; }
    public static string Effects(string _effectName)  { return EFFECTS + "/" + _effectName; }
    public static string Enemys(string _enemyName)    { return ENEMYS + "/" + _enemyName; }
    public static string Ui(string _uiName)           { return UI + "/" + _uiName; }
}
