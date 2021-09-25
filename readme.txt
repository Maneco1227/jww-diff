jww_diff: jw_cadの図面比較機能をコマンドラインから実行する.

* 使い方:

  jww_diff file1.jww file2.jww

jw_winの図面比較機能をコマンドラインから起動し、UIAutomationを使用して実行します。
これにより、TortoiseSVN等リビジョン管理ツールからjw_winの図面比較機能を利用する
ことが可能となります。

* インストール,設定:
   
jww-diff.exeとjww-diff.exe.configを同じフォルダに保存します。
jww-diff.exe.configの中には、jw_winの実行パスを設定して下さい。

* TortoiseSVNの設定:

右クリックでTortoiseSVNの設定画面(Settings)に入ります。
左側のDiff Viewerを選び。右側でAdvanced...をクリックします。
Add...をクリックし、Filename, extension or mime-type: に .jww を設定します。
External Program: には、(jww-diffの実行パス)\jww-diff.exe %base %mine と
設定します。これで準備完了です。
 
* 注意など

本プログラムを使用した結果、どのような損害が生じても作者は一切責任を
負いません。バグや改善のご連絡は歓迎します。改変、再配布はご自由に
どうぞ。

Masanori MORITA (@Maneco1227)
