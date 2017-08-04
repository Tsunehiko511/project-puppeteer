using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ROOT7 : MonoBehaviour {

  //世代変数
  int generation = 0;
  int generation_max = 100; // こいつを小さくすると処理の時間が短くなるかも

  Player player = new Player();   //こいつを鍛える（？）
  Player enemy = new Player();    //こいつに勝てる個体を見つける

  bool evaluations = true; // 評価（今回はとにかくランダムなので評価は勝つ[false]か負ける[true]かのみ！）

  AI_Database ai_db = new AI_Database();
  System.Random cRandom = new System.Random();

  public ROOT7() {
    // 登場人物はあらかじめ生成しておく
    player.SetData(ai_db.GetUnits(""), ai_db.GetCodes(""));

    // Plan0のPlan内設定をランダムに設定する
    Initial_Setting();
  }

  // 初期設定
  void Initial_Setting() {
    // 子供をランダム設定
    SetInitRandom(player);

    // 評価する時に対戦する相手の設定
    enemy.SetData(ai_db.GetUnits("第7回(fred)"), ai_db.GetCodes("第7回(fred)"));
  }

  // Plan0のPlan内設定をランダム生成・Plan変更設定はリセット
  void SetInitRandom(Player _player) {
    // 今回はキング，クイーン，ナイト，ビショップで固定
    _player.Units = new int[4] { 0, 1, 2, 3 };

    // 4体のPlan0のデータを初期設定する i=ユニットのid, j=Plan0の行のid
    for (int i = 0; i < 4; i++) {
      // Plan0のPlan変更設定を初期にする
      for (int j = 0; j < 5; j++) {
        _player.plans[i].GetPlan(0).transitions.SetRow(j, new int[3] { 0, 0, 0 }); // Plan変更設定を初期にする=Plan変更しない
        _player.plans[i].GetPlan(0).elements.SetRow(j, GetPlanElementCode()); // Plan内設定にランダムな行を入れる
      }
    }
  }

  // AIのデータベースからPlan内設定の行をランダムに取得する。
  int[] GetPlanElementCode() {
    List<int[]> tmp_list = ai_db.GetPlanSettingCodeRowList();
    int rndm = cRandom.Next(tmp_list.Count);
    return tmp_list[rndm];
  }

  public void Play(GameMaster _gamemaster) {

    // 勝つまで立ち上がれ！
    while (evaluations) {

      //世代交代
      generation++;
      SetInitRandom(player);

      // 対戦に勝利すればRED，負ければBLUEを取得する
      string tmp_win = _gamemaster.Play(player, enemy);

      // 勝ったね！やった！
      if (tmp_win == "RED") {
        evaluations = false;
      }

      if (generation >= generation_max) { // フリーズするので一定数を超えると処理を終える
        Debug.Log("むりだようかてないよう");
        break;
      }
    }

    if (!evaluations) {
      //output
      Debug.Log(generation + "体目");
      Debug.LogFormat("勝者：{0}  詳細(赤,青)：残機({1},{2})，キングHP({3}, {4})", _gamemaster.Play(player, enemy), _gamemaster.GetResult("RED_LEFT"), _gamemaster.GetResult("BLUE_LEFT"), _gamemaster.GetResult("RED_KING_HP"), _gamemaster.GetResult("BLUE_KING_HP"));
      Debug.Log(player.plans[0].SaveToString() + "\n"
        + player.plans[1].SaveToString() + "\n"
        + player.plans[2].SaveToString() + "\n"
        + player.plans[3].SaveToString());
    }

    Debug.Log("終了");
  }
}
