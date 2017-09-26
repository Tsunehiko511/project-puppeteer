using UnityEngine;
using System.Collections;

static class Constants{

    /*----------
    *Plan変更設定*
    -----------*/
    public const int F_NODATA               = -1; // 設定ない 　＝　1<< 31
    public const int F_INIT                 = 0; //初期文字
    public const int F_START                = 1;
    // 自分に関わること
    // HP
    public const int F_HP_75_100            = 2;
    public const int F_HP_50_75             = 3;
    public const int F_HP_25_50             = 4;
    public const int F_HP_0_25              = 5;
    // スタミナ
    public const int F_STAMINA_4             = 44;
    public const int F_STAMINA_3             = 45;
    public const int F_STAMINA_3_4           = 6;
    public const int F_STAMINA_2             = 7;
    public const int F_STAMINA_0_1           = 8;
    //Pointポジションに到着
    public const int F_POINT_RED            = 9;
    public const int F_POINT_BLUE           = 10;
    public const int F_POINT_YELLOW         = 11;
    public const int F_POINT_GREEN          = 12;
    // 残りUNIT数に関わること
    public const int F_LEFT_ENEMY_5_6         = 13;
    public const int F_LEFT_ENEMY_3_4         = 14;
    public const int F_LEFT_ENEMY_1_2         = 15;
    public const int F_LEFT_FRIEND_OVER_ENEMY = 16; // >
    public const int F_LEFT_FRIEND_5_6        = 17;
    public const int F_LEFT_FRIEND_3_4        = 18;
    public const int F_LEFT_FRIEND_1_2        = 19;
    public const int F_LEFT_FRIEND_UNDER_ENEMY = 20; // <
    public const int F_LEFT_EQUAL              = 46;

    // キングのHP F_E:敵，F_F：味方
    public const int F_E_KING_HP_75_100        = 42;
    public const int F_E_KING_HP_50_75         = 21;
    public const int F_E_KING_HP_25_50         = 22;
    public const int F_E_KING_HP_0_25          = 23;
    public const int F_F_KING_HP_WIN_E         = 24;
    public const int F_F_KING_HP_75_100        = 43;
    public const int F_F_KING_HP_50_75         = 25;
    public const int F_F_KING_HP_25_50         = 26;
    public const int F_F_KING_HP_0_25          = 27;
    public const int F_F_KING_HP_LOSE_E        = 28;
    public const int F_KING_HP_EQUAL           = 47;
    // 時間に関わること
    public const int F_TIME_150_200             = 41;
    public const int F_TIME_100_150             = 29;
    public const int F_TIME_50_100              = 30;
    public const int F_TIME_0_50                = 31;
    // 3マス以内の味方の数
    public const int F_UNIT_F_COUNT_0           = 32;
    public const int F_UNIT_F_COUNT_1           = 33;
    public const int F_UNIT_F_COUNT_2           = 34;
    public const int F_UNIT_F_COUNT_3           = 35;
    // 3マス以内の敵の数
    public const int F_UNIT_E_COUNT_0           = 36;
    public const int F_UNIT_E_COUNT_1           = 37;
    public const int F_UNIT_E_COUNT_2           = 38;
    public const int F_UNIT_E_COUNT_3           = 39;
    public const int F_UNIT_E_COUNT_4           = 40;

    public const int F_DIE_COUNT_0              = 48;
    public const int F_DIE_COUNT_NOT_0          = 49;

    public const int F_NOT_CHARGE               = 50; // 強化
    public const int F_NOT_GUARD                = 51; // 強化
    public const int F_CHARGE                   = 52; // 強化
    public const int F_GUARD                    = 53; // 強化

    public const int F_DOUBLE_MORE_ROOK         = 54;
    public const int F_DOUBLE_MORE_KNIGHT       = 55;
    public const int F_DOUBLE_MORE_PAWN         = 56;
    public const int F_SINGLE_MORE_BISHOP       = 57;

    /*------------
    *範囲条件*
    ------------*/
    public const int FI_INIT        = 0;
    public const int FI_DIST_ALL    = 1; // 全範囲
    public const int FI_DIST_7      = 2; // 7マス以内
    public const int FI_DIST_4      = 3; // 4マス以内
    public const int FI_DIST_1      = 4; // 1マス以内
    public const int FI_NEAR_DIS    = 5; // 最も近い：これは使わない

    public const int FI_HP_75_100   = 10;
    public const int FI_HP_50_75    = 11;
    public const int FI_HP_25_50    = 12;
    public const int FI_HP_0_25     = 13;
    public const int FI_LOW_HP      = 14;
    public const int FI_STAMINA_3   = 20;
    public const int FI_STAMINA_2   = 21;
    public const int FI_STAMINA_1   = 22;

    public const int FI_NON_CHARGE  = 25; // 未強化
    public const int FI_NON_GUARD   = 26; // 未強化
    public const int FI_NON_SPEEDUP = 27; // 未強化
    public const int FI_CHARGE      = 28; // 強化
    public const int FI_GUARD       = 29; // 強化

    public const int FI_STAMINA_4   = 23;
    public const int FI_DIST_8_OVER      = 30; // 8マス以上
    public const int FI_DIST_5_OVER      = 31; // 5マス以上
    public const int FI_DIST_2_OVER      = 32; // 2マス以上
    public const int FI_HP_0_75      = 35;
    public const int FI_HP_25_100    = 36;

    public const int FI_NOT_KING   = 40;
    public const int FI_NOT_QUEEN  = 41;
    public const int FI_NOT_KNIGHT = 42;
    public const int FI_NOT_BISHOP = 43;
    public const int FI_NOT_PAWN   = 44;
    public const int FI_NOT_ROOK   = 45;

    // 死亡回数
    public const int FI_DIE_COUNT_0  = 46;
    public const int FI_DIE_COUNT_NOT_0  = 47;



    /*------------
    *ターゲット条件*３つ
    ------------*/
    public const int T_INIT     = 0;
    // 敵
    public const int T_E_UNIT   = 1;
    public const int T_E_KING   = 2;
    public const int T_E_QUEEN  = 3;
    public const int T_E_KNIGHT = 4;
    public const int T_E_BISHOP = 5;
    public const int T_E_PAWN   = 12;
    public const int T_E_ROOK   = 13;

    // 味方
    public const int T_F_UNIT   = 6;
    public const int T_F_KING   = 7;
    public const int T_F_QUEEN  = 8;
    public const int T_F_KNIGHT = 9;
    public const int T_F_BISHOP = 10;
    public const int T_F_PAWN   = 14;
    public const int T_F_ROOK   = 15;
    public const int T_MINE     = 11; // 自身

    public const int T_POINT_0          = 20;
    public const int T_POINT_1          = 21;
    public const int T_POINT_2          = 22;
    public const int T_POINT_3          = 23;

    // ユニット上中下
    // 敵
    public const int T_E_UNIT_UP        = 24;
    public const int T_E_UNIT_CENTER    = 25;
    public const int T_E_UNIT_DOWN      = 26;
    // 味方
    public const int T_F_UNIT_UP        = 27;
    public const int T_F_UNIT_CENTER    = 28;
    public const int T_F_UNIT_DOWN      = 29;


    /*-------
    *  行動  *
    --------*/
    public const int A_INIT     = 0; // 待機
    public const int A_WAIT     = 1; // 待機
    public const int A_ATTACK   = 2; // 攻撃
    public const int A_RETREAT  = 3; // 緊急回避
    public const int A_MOVE_TO  = 4; // 近づく
    public const int A_MOVE_FROM  = 5; // 離れる

    public const int A_SKILL    = 10; // 治癒
    public const int A_HEAL     = 11; // 治癒
    public const int A_LONG_ATTACK  = 12; // 退却
    public const int A_CHARGE  = 13; // 攻撃強化
    public const int A_SPEEDUP = 14; // 攻撃強化
    public const int A_GUARD   = 15; // 防御強化
    public const int A_S_LONG_ATTACK = 16;





    /*-----------------
    *以下不要なデータ   *
    -----------------*/
    public const string GAME_NAME                = "Project Puppeteer";
    public const string VERSION                  = "0.079";
    public const int    VERSION_NUMBER           = 5;
    public const string SAVE_UNITS               = "Units";
    public const string SAVE_UNIT_TOURNAMENT     = "UnitsTournament";
    public const string SAVE_STORAGE             = "Connect";
    public const string SAVE_STORAGE_TOURNAMENT  = "AITournament";
    public const string SERVER                   = "Expert";
    public const string SINGLE_SERVER            = "TestClass";
    public const int BLOCK_SIZE                  = 2;
    public const int MAP_SIZE_X                  = 18;
    public const int MAP_SIZE_Z                  = 11;
    public const int NODATA                      = 0;
    public const int P_INIT                      = 1;
}