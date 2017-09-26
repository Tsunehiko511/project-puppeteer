//using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dictionarys {
  public Dictionary<string, int> Condition_Dic = new Dictionary<string, int>(){
      {"設定なし", Constants.F_NODATA},
      {"条件", Constants.F_INIT},
      {"初期条件", Constants.F_START},
      {"自身HP\n75-100%", Constants.F_HP_75_100},
      {"自身HP\n50-75%", Constants.F_HP_50_75}, // 100%までを追加
      {"自身HP\n25-50%", Constants.F_HP_25_50},
      {"自身HP\n0-25%", Constants.F_HP_0_25},
      {"スタミナ\n3-4", Constants.F_STAMINA_3_4},
      {"スタミナ\n2", Constants.F_STAMINA_2},
      {"スタミナ\n0-1", Constants.F_STAMINA_0_1},
      {"敵残機\n5-6", Constants.F_LEFT_ENEMY_5_6}, //TODO
      {"敵残機\n3-4", Constants.F_LEFT_ENEMY_3_4},
      {"敵残機\n1-2", Constants.F_LEFT_ENEMY_1_2},
      {"残機\n敵<味方", Constants.F_LEFT_FRIEND_OVER_ENEMY},
      {"味方残機\n5-6", Constants.F_LEFT_FRIEND_5_6},
      {"味方残機\n3-4", Constants.F_LEFT_FRIEND_3_4},
      {"味方残機\n1-2", Constants.F_LEFT_FRIEND_1_2},
      {"残機\n敵>味方", Constants.F_LEFT_FRIEND_UNDER_ENEMY},
      {"残り時間\n100-150", Constants.F_TIME_100_150},
      {"残り時間\n50-100", Constants.F_TIME_50_100},
      {"残り時間\n0-50", Constants.F_TIME_0_50},
      {"敵キングHP\n50-75%", Constants.F_E_KING_HP_50_75},
      {"敵キングHP\n25-50%", Constants.F_E_KING_HP_25_50},
      {"敵キングHP\n0-25%", Constants.F_E_KING_HP_0_25},
      {"キングHP\n敵<味方", Constants.F_F_KING_HP_WIN_E},
      {"キングHP\n50-75%", Constants.F_F_KING_HP_50_75},
      {"キングHP\n25-50%", Constants.F_F_KING_HP_25_50},
      {"キングHP\n0-25%", Constants.F_F_KING_HP_0_25},
      {"キングHP\n敵>味方", Constants.F_F_KING_HP_LOSE_E},
      {"ポイント赤\nに到着", Constants.F_POINT_RED},
      {"ポイント青\nに到着", Constants.F_POINT_BLUE},
      {"ポイント黄\nに到着", Constants.F_POINT_YELLOW},
      {"ポイント緑\nに到着", Constants.F_POINT_GREEN},
      {"近くの味方\n0", Constants.F_UNIT_F_COUNT_0},
      {"近くの味方\n1", Constants.F_UNIT_F_COUNT_1},
      {"近くの味方\n2", Constants.F_UNIT_F_COUNT_2},
      {"近くの味方\n3", Constants.F_UNIT_F_COUNT_3},
      {"近くの敵\n0", Constants.F_UNIT_E_COUNT_0},
      {"近くの敵\n1", Constants.F_UNIT_E_COUNT_1},
      {"近くの敵\n2", Constants.F_UNIT_E_COUNT_2},
      {"近くの敵\n3", Constants.F_UNIT_E_COUNT_3},
      {"近くの敵\n4", Constants.F_UNIT_E_COUNT_4},

      {"残り時間\n150-200"   , Constants.F_TIME_150_200},
      {"敵キングHP\n75-100%" , Constants.F_E_KING_HP_75_100},
      {"キングHP\n75-100%" , Constants.F_F_KING_HP_75_100},
      {"スタミナ\n4"         , Constants.F_STAMINA_4},
      {"スタミナ\n3"         , Constants.F_STAMINA_3},
      {"残機\n敵=味方"   , Constants.F_LEFT_EQUAL},
      {"キングHP\n敵=味方"    , Constants.F_KING_HP_EQUAL},
      {"死亡回数\n0"    , Constants.F_DIE_COUNT_0},
      {"死亡回数\n0以外"    , Constants.F_DIE_COUNT_NOT_0},
      {"攻撃強化=0", Constants.F_NOT_CHARGE},
      {"防御強化=0", Constants.F_NOT_GUARD},
      {"攻撃強化≠0", Constants.F_CHARGE},
      {"防御強化≠0", Constants.F_GUARD},
      {"敵ルーク数≥2", Constants.F_DOUBLE_MORE_ROOK},
      {"敵ナイト数≥2", Constants.F_DOUBLE_MORE_KNIGHT},
      {"敵ポーン数≥2", Constants.F_DOUBLE_MORE_PAWN},
      {"敵ビショップ数≥1", Constants.F_SINGLE_MORE_BISHOP},

  };



  // 辞書
  public Dictionary<int, int[]> Chara_Dic = new Dictionary<int, int[]>(){
    {0, new int[2]{0,0}},
    {1, new int[2]{1,0}},//クイーン1
    {2, new int[2]{2,0}},//ナイト1
    {3, new int[2]{3,0}},//ビショップ1
    {4, new int[2]{3,1}},//ビショップ2
    {5, new int[2]{2,1}},//ナイト2
    {6, new int[2]{3,2}},//ビショップ3
    {7, new int[2]{2,2}},//ナイト3
    {8, new int[2]{1,1}},//クイーン2
    {9, new int[2]{1,2}},//クイーン3
    {10, new int[2]{4,0}},// ポーン1
    {11, new int[2]{4,1}},// ポーン2
    {12, new int[2]{4,2}},// ポーン3
    {13, new int[2]{5,0}},// ルーク1
    {14, new int[2]{5,1}},// ルーク2
    {15, new int[2]{5,2}},// ルーク3
  };


  public Dictionary<string, int> NextPlan_Dic = new Dictionary<string, int>(){
      {"変更先", 0},
      {"初期Plan", 1},
      {"Plan1", 2},
      {"Plan2", 3},
      {"Plan3", 4},
      {"Plan4", 5},
      {"Plan5", 6}
  };

      
    
    public Dictionary<string, int> Filter_Dic = new Dictionary<string, int>(){
      {"条件",      Constants.FI_INIT},
      {"1マス以内",   Constants.FI_DIST_1},
      {"4マス以内",   Constants.FI_DIST_4},
      {"7マス以内",  Constants.FI_DIST_7},
      {"全範囲",     Constants.FI_DIST_ALL},
      {"HP 0-25%",      Constants.FI_HP_0_25},
      {"HP 25-50%",       Constants.FI_HP_25_50},
      {"HP 50-75%",       Constants.FI_HP_50_75},
      {"HP 75-100%",      Constants.FI_HP_75_100},//TODO
      {"最小HP",    Constants.FI_LOW_HP},
      {"最小距離",        Constants.FI_NEAR_DIS},
      {"スタミナ=3", Constants.FI_STAMINA_3},
      {"スタミナ=2", Constants.FI_STAMINA_2},
      {"スタミナ=1", Constants.FI_STAMINA_1},
      {"攻撃強化=0", Constants.FI_NON_CHARGE},
      {"防御強化=0", Constants.FI_NON_GUARD},
      {"速度強化=0", Constants.FI_NON_SPEEDUP},
      // 追加
      {"スタミナ=4", Constants.FI_STAMINA_4},
      {"8マス以上", Constants.FI_DIST_8_OVER},
      {"5マス以上", Constants.FI_DIST_5_OVER},
      {"2マス以上", Constants.FI_DIST_2_OVER},
      {"HP 0-75%", Constants.FI_HP_0_75},
      {"HP 25-100%", Constants.FI_HP_25_100},

      {"キング以外", Constants.FI_NOT_KING},
      {"クイーン以外", Constants.FI_NOT_QUEEN},
      {"ナイト以外", Constants.FI_NOT_KNIGHT},
      {"ビショップ以外", Constants.FI_NOT_BISHOP},
      {"ポーン以外", Constants.FI_NOT_PAWN},  
      {"ルーク以外", Constants.FI_NOT_ROOK},

      {"死亡回数=0", Constants.FI_DIE_COUNT_0},  
      {"死亡回数≠0", Constants.FI_DIE_COUNT_NOT_0},

      {"攻撃強化≠0", Constants.FI_CHARGE},
      {"防御強化≠0", Constants.FI_GUARD},

    };

  public Dictionary<string, int> Target_Dic = new Dictionary<string, int>(){
      {"ユニット",        Constants.T_INIT},
      {"敵",             Constants.T_E_UNIT},
      {"敵キング",        Constants.T_E_KING},
      {"敵クイーン",       Constants.T_E_QUEEN},
      {"敵ナイト",        Constants.T_E_KNIGHT},
      {"敵ビショップ",    Constants.T_E_BISHOP},
      {"敵ポーン",      Constants.T_E_PAWN},
      {"敵ルーク",      Constants.T_E_ROOK},
      {"仲間",            Constants.T_F_UNIT},
      {"キング",       Constants.T_F_KING},
      {"クイーン",       Constants.T_F_QUEEN},
      {"ナイト",        Constants.T_F_KNIGHT},
      {"ビショップ",     Constants.T_F_BISHOP},
      {"ポーン",         Constants.T_F_PAWN},
      {"ルーク",         Constants.T_F_ROOK},
      {"自分",          Constants.T_MINE},
      {"ポイント赤", Constants.T_POINT_0},
      {"ポイント青", Constants.T_POINT_1},
      {"ポイント黄", Constants.T_POINT_2},
      {"ポイント緑", Constants.T_POINT_3},
      {"敵 上", Constants.T_E_UNIT_UP},
      {"敵 中", Constants.T_E_UNIT_CENTER},
      {"敵 下", Constants.T_E_UNIT_DOWN},
      {"味方 上", Constants.T_F_UNIT_UP},
      {"味方 中", Constants.T_F_UNIT_CENTER},
      {"味方 下", Constants.T_F_UNIT_DOWN},
  };

  public Dictionary<string, int> Action_Dic = new Dictionary<string, int>(){
      {"行動", Constants.A_INIT},
      {"進撃", Constants.A_ATTACK},
      {"待機", Constants.A_WAIT},
      {"緊急避難", Constants.A_RETREAT},
      {"近づく", Constants.A_MOVE_TO},
      {"離れる", Constants.A_MOVE_FROM},

      {"固有スキル", Constants.A_SKILL},
      {"治癒", Constants.A_HEAL},
      {"遠距離攻撃", Constants.A_LONG_ATTACK},
      {"攻撃強化", Constants.A_CHARGE},
      {"速度強化", Constants.A_SPEEDUP},
      {"防御強化", Constants.A_GUARD},
      {"超距離攻撃", Constants.A_S_LONG_ATTACK},
  };

  public int GetInt(Dictionary<string, int> _dic, string _string){
      return _dic[_string];
  }
  public string GetString(Dictionary<string, int> _dic, int _int){
    foreach (KeyValuePair<string, int> pair in _dic) {
      if(pair.Value == _int){
        return pair.Key;
      }
    }
    return "エラー";
  }
}