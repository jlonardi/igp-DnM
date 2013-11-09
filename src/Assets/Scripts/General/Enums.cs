public enum GameState {
	RUNNING,
	PAUSE_MENU,
	MAIN_MENU,
	LOAD_MENU_MAIN,
	LOAD_MENU_PAUSE,
	SAVE_MENU,
	SAVE_DIALOG,
	GAME_OVER,
}

public enum HitType
{
	ENEMY,
	CONCRETE,
	WOOD,
	METAL,
	OLD_METAL,
	GLASS,
	GENERIC
}

public enum DamageType {
	BULLET,
	FIRE,
	EXPLOSION,
	HIT,
}

public enum focusTarget {
	PLAYER,
	TRESAURE
}

public enum EnemyType {
	ORC,
}