using UnityEngine;

public static class Consts
{
    public const string HORIZONTAL = "horizontal";
    public const string VERTICAL = "vertical";
    public const string ATTACK = "attack";
    public const string IS_GROUNDED = "isGrounded";
    public const string SPEED = "speed";
    public const string YSPEED = "ySpeed";

    public const string PLAYER = "Player";
    public const string PLAYER_PROJECTILE = "PlayerProjectile";
    public const string ENEMY = "Enemy";
    public const string ENEMY_EFFECT_AREA = "EnemyEffectArea";
    public const string MOVING_PLATFORM = "MovingPlatform";
    public const string ROTATED_PLATFORM_SPIKE = "RotatedPlatformSpike";
    public const string SIGNPOST = "Signpost";
    public const string COLLECTABLE = "Collectable";

    public const string MAIN_MENU = "MainMenu";
    public const string LEVEL_MAP = "LevelMap";

    public const string FINISH = "Ukoñczono poziom: ";
    public const string LEVEL1_1 = "Level1_1";
    public const string LEVEL1_1_NAME = "Nadleœnictwo Zielony Dzik"; 
    public const string LEVEL1_2 = "Level1_2";
    public const string LEVEL1_2_NAME = "Park Linowy Po³amaniec";
    public const string LEVEL1_3 = "Level1_3";
    public const string LEVEL1_3_NAME = "Tartak Pociupa³a i Synowie";
    public const string LEVEL1_4 = "Level1_4";
    public const string LEVEL1_4_NAME = "Generator Portali";
    public const string LEVEL2_1 = "Level2_1";

    public const string LEVEL2_2 = "Level2_3";

    public const string LEVEL2_3 = "Level2_4";

    public const string LEVEL3_1 = "Level3_1";

    public const string LEVEL3_2 = "Level3_2";

    public const string LEVEL3_3 = "Level3_3";

    public const string LEVEL3_4 = "Level3_4";

    public const string LEVEL4_1 = "Level4_1";

    public const string LEVEL4_2 = "Level4_2";

    public const string LEVEL4_3 = "Level4_3";

    public const string LEVEL5_1 = "Level5_1";


    public const string CHERRY = "Cherry";
    public const string ANANAS = "Ananas";
    public const string KIWI = "Kiwi";
    public const string APPLE = "Apple";
    public const string ORANGE = "Orange";
    public const string MELON = "Melon";
    public const string STRAWBERRY = "Strawberry";
    public const string BANANA = "Banana";
    public const string COIN = "Coin";

    public const string HP_ITEM_SMALL = "HpItemSmall";
    public const string KEY = "Key";

    public const string LOCKED_DOOR_P = "LockedDoorParent";
    public const string LOCKED_DOOR = "LockedDoor";
    public const string OPENED_DOOR = "OpenedDoor";

    public const string NEW_PROFILE_ERROR_ALREADY_USED = "Profil o podanej nazwie ju¿ istnieje";
    public const string NEW_PROFILE_ERROR_EMPTY_NAME = "Nazwa profilu nie mo¿e byæ pusta";

    public static string GetLevelName(string levelName)
    {
        switch (levelName)
        {
            case LEVEL1_1:
                return LEVEL1_1_NAME;
            case LEVEL1_2:
                return LEVEL1_2_NAME;
            case LEVEL1_3:
                return LEVEL1_3_NAME;
            case LEVEL1_4:
                return LEVEL1_4_NAME;
            case LEVEL2_1:
                return LEVEL1_4_NAME;
            case LEVEL2_2:
                return LEVEL1_4_NAME;
            case LEVEL2_3:
                return LEVEL1_4_NAME;
            case LEVEL3_1:
                return LEVEL1_4_NAME;
            case LEVEL3_2:
                return LEVEL1_4_NAME;
            case LEVEL3_3:
                return LEVEL1_4_NAME;
            case LEVEL3_4:
                return LEVEL1_4_NAME;
            case LEVEL4_1:
                return LEVEL1_4_NAME;
            case LEVEL4_2:
                return LEVEL1_4_NAME;
            case LEVEL4_3:
                return LEVEL1_4_NAME;
            case LEVEL5_1:
                return LEVEL1_4_NAME;
            case LEVEL_MAP:
                return LEVEL_MAP;
            default:
                Debug.Log("Nieznana nazwa levelu w Consts.GetLevelIndex");
                return "{UNKNOWN NAME}";
        }
    }
    public static int GetLevelIndex(string levelName)
    {
        switch(levelName)
        {
            case LEVEL_MAP:
                return -1;
            case LEVEL1_1:
                return 0;
            case LEVEL1_2:
                return 1;
            case LEVEL1_3:
                return 2;
            case LEVEL1_4:
                return 3;
            case LEVEL2_1:
                return 4;
            case LEVEL2_2:
                return 5;
            case LEVEL2_3:
                return 6;
            case LEVEL3_1:
                return 7;
            case LEVEL3_2:
                return 8;
            case LEVEL3_3:
                return 9;
            case LEVEL3_4:
                return 10;
            case LEVEL4_1:
                return 11;
            case LEVEL4_2:
                return 12;
            case LEVEL4_3:
                return 13;
            case LEVEL5_1:
                return 14;
            default:
                Debug.Log("Nieznana nazwa levelu w Consts.GetLevelIndex");
                return 0;
        }
    }
    public static int GetFruitIndex(string fruitName)
    {
        switch (fruitName)
        {
            case CHERRY:
                return 0;
            case ANANAS:
                return 1;
            case KIWI:
                return 2;
            case APPLE:
                return 3;
            case ORANGE:
                return 4;
            case MELON:
                return 5;
            case STRAWBERRY:
                return 6;
            case BANANA:
                return 7;
            case COIN:
                return 8;
            default:
                Debug.Log("Nieznana nazwa owocu w Consts.GetFruitIndex");
                return 0;
        }
    }

}
