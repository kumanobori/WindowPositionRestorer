# 概要
ウィンドウ位置情報を記憶・復元するツール

# 想定する用途
いわゆる「DisplayPortスリープ問題」の対策用。
ディスプレイ電源OFFする前にsaveして、復帰後にrestoreする。

# 既知の問題点
1. 以下のウィンドウは復元できない
    1. 管理者権限で動いているウィンドウ
    2. その他特殊なウィンドウ？
2. 復元対象に、復元する必要がなさそうなものがエントリされる
3. 単一exeファイルでビルドしたいがうまくいかない
