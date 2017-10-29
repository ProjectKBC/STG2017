/// <summary>
/// レイヤー名を定数で管理するクラス
/// </summary>
public static class LayerName
{
	public const int Default = 0;
	public const int TransparentFX = 1;
	public const int IgnoreRaycast = 2;
	public const int Water = 4;
	public const int UI = 5;
	public const int GameArea = 8;
	public const int Player = 9;
	public const int BulletPlayer = 10;
	public const int Enemy = 11;
	public const int BulletEnemy = 12;
	public const int DefaultMask = 1;
	public const int TransparentFXMask = 2;
	public const int IgnoreRaycastMask = 4;
	public const int WaterMask = 16;
	public const int UIMask = 32;
	public const int GameAreaMask = 256;
	public const int PlayerMask = 512;
	public const int BulletPlayerMask = 1024;
	public const int EnemyMask = 2048;
	public const int BulletEnemyMask = 4096;
}
