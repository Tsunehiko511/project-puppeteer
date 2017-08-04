using UnityEngine;
using System.Collections.Generic;

public class Setting : MonoBehaviour {
  GameMaster Game;
  Player player1;
  Player player2;
  AI_Database ai_db;
  GA_2NE_ORIGIN ga_2ne_origin;
  ROOT7 Root7;
  void Start() {
    ai_db = new AI_Database();
    Game = new GameMaster();
    player1 = new Player();
    player2 = new Player();
    ga_2ne_origin = new GA_2NE_ORIGIN();
    Root7 = new ROOT7();
  }

  // ボタンを押すとこの関数を実行する
  public void PushTestButton() {
    TestPlay("ROOT7");
  }

  // 数々のテストプレイ
  void TestPlay(string _name) {
    switch (_name) {
      case "TEST":// とりあえず対戦させる
                  // player1とplayer2にデータをセットする。SetData(セットするプレイヤ，ユニットの種類の配列，AIの配列);
        player1.SetData(ai_db.GetUnits(ai_db.GetName(7)), ai_db.GetCodes(ai_db.GetName(7)));
        player2.SetData(ai_db.GetUnits(ai_db.GetName(10)), ai_db.GetCodes(ai_db.GetName(10)));
        //Game.Play(赤, 青)：1回処理するのに0.02秒程度かかる
        for (int i = 0; i < 1; i++) {
          Debug.LogFormat("勝者：{0}  詳細(赤,青)：残機({1},{2})，キングHP({3}, {4})", Game.Play(player1, player2), Game.GetResult("RED_LEFT"), Game.GetResult("BLUE_LEFT"), Game.GetResult("RED_KING_HP"), Game.GetResult("BLUE_KING_HP"));
          Debug.LogFormat("勝者：{0}  詳細(赤,青)：残機({1},{2})，キングHP({3}, {4})", Game.Play(player2, player1), Game.GetResult("RED_LEFT"), Game.GetResult("BLUE_LEFT"), Game.GetResult("RED_KING_HP"), Game.GetResult("BLUE_KING_HP"));
        }
        break;
      case "GA_2NE_ORIGIN":// 2neが開発した遺伝的アルゴリズム ボタンを押してから数十秒処理するからご注意を！
        ga_2ne_origin.Play(Game);
        break;
      case "ROOT7"://ROOT7のランダムアルゴリズム　処理時間多くて2分以内
        Root7.Play(Game);      // ここに処理を書いてね
        break;
      case "":// 後の人はここにファイル名を入れてね

        break;
      default:
        break;
    }
  }
}
// 時間計測
// float start_time = Time.time;
// float end_time = Time.time - start_time;
// Debug.Log(end_time);っとすればstart_timeから end_time の時間を計測できる