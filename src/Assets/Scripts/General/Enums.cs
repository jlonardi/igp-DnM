public enum GameState {
	RUNNING,
	STORY,
	DIFFICULTY,
	HIGHSCORE,
	HIGHSCORE_DIALOG,
	PAUSE_MENU,
	MAIN_MENU,
	LOAD_MENU_MAIN,
	LOAD_MENU_PAUSE,
	SAVE_MENU,
	SAVE_DIALOG,
	SAVE_SCREENSHOT,
	GAME_OVER,
}

public enum PickupState {
	TREASURE,
	SCAR_L,
	ARMOR,
	MINIGUN,
	GRENADE_BOX,
	NONE
}

public enum LevelState {
	LOADING_NEW,
	LOADING_SAVE,
	LOADING_NEXT,
	LOADED,
	LOADING_HIGHSCORE
}

public enum DifficultySetting {
	EASY = 0,
	NORMAL = 1,
	HARD = 2,
	EPIC = 3
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
	DRAGONJAWS,
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
	LIZARD,
	WEREWOLF,
	DRAGON
}