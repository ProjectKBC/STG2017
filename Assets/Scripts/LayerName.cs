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
	public const int Player = 8;
	public const int BulletPlayer = 9;
	public const int Enemy = 10;
	public const int BulletEnemy = 11;
	public const int DefaultMask = 1;
	public const int TransparentFXMask = 2;
	public const int IgnoreRaycastMask = 4;
	public const int WaterMask = 16;
	public const int UIMask = 32;
	public const int PlayerMask = 256;
	public const int BulletPlayerMask = 512;
	public const int EnemyMask = 1024;
	public const int BulletEnemyMask = 2048;
}
