using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Configuration;
using System.IO;

namespace jww_diff
{
    class Program
    {
        //
        // jww_diff: jw_cadの図面比較機能をコマンドラインから実行する.
        //
        // * 使い方:
        //
        //   jww_diff file1.jww file2.jww
        //
        // jw_winの図面比較機能をコマンドラインから起動し、UIAutomationを使用して実行します。
        // これにより、TortoiseSVN等リビジョン管理ツールからjw_winの図面比較機能を利用する
        // ことが可能となります。
        //
        // * インストール,設定:
        //   
        // jww-diff.exeとjww-diff.exe.configを同じフォルダに保存します。
        // jww-diff.exe.configの中には、jw_winの実行パスを設定して下さい。
        //
        // * TortoiseSVNの設定:
        //
        // 右クリックでTortoiseSVNの設定画面(Settings)に入ります。
        // 左側のDiff Viewerを選び。右側でAdvanced...をクリックします。
        // Add...をクリックし、Filename, extension or mime-type: に .jww を設定します。
        // External Program: には、(jww-diffの実行パス)\jww-diff.exe %base %mine と
        // 設定します。これで準備完了です。
        // 
        // * 注意など
        //
        //  本プログラムを使用した結果、どのような損害が生じても作者は一切責任を
        // 負いません。バグや改善のご連絡は歓迎します。改変、再配布はご自由に
        // どうぞ。
        //
        // Masanori MORITA (@Maneco1227)
        //
        static void Main(string[] args)
        {
            // UIAutomationLibのインスタンスを作成.
            UIAutomationLib ui = new UIAutomationLib();

            // jw_winのパスを取得.
            string sJwPath = ConfigurationManager.AppSettings["jw_win-path"];
            string sFile1 = args[0];

            // jw_winを起動. 1個目のファイルを比較元として開く.
            Process pJww = Process.Start(sJwPath, sFile1);
            pJww.WaitForInputIdle(); // 起動してアイドル状態になるまで待機. 

            // jw_winのAutomationElementを取得.
            AutomationElement aeJww = ui.GetMainFrameElement(pJww);

            // jw_winにフォーカス設定.
            aeJww.SetFocus();

            ////////////////
            // jw_win操作.
            ////////////////

            // [図面比較]のファイル選択を開く.
            SendKeys.SendWait("%f"); // ALT + F
            SendKeys.SendWait("f");  // F
            SendKeys.SendWait("p");  // P ... ファイル比較(P)            

            ui.PushButtonById(aeJww, "1069"); // [図面比較]ボタンをクリック.

            // フォルダ選択ツリーの初期化.
            SendKeys.SendWait("{HOME}");     // Home
            SendKeys.SendWait("{SUBTRACT}"); // テンキーの"-"
            SendKeys.SendWait("{ADD}");      // テンキーの"+"

            // フォルダ選択ツリー降下.
            string sFile2 = args[1];
            string sFile2_folder = Path.GetDirectoryName(sFile2); // ドライブ名込みフォルダ名.
            string sFile2_file = Path.GetFileName(sFile2);        // ファイル名.

            foreach(char c in sFile2_folder) {
                if(c == '\\') {
                    SendKeys.SendWait("{ADD}"); // フォルダ名末尾('\')毎に下層フォルダを開く.
                } else {
                    string s1 = c.ToString();

                    SendKeys.SendWait(s1);      // フォルダ名を入力していく.
                }
            }

            // ファイル選択.
            ui.ClickElement(aeJww, "1882");  // 右側ファイル選択ボックスをクリック.
            SendKeys.SendWait("{HOME}");     // Home
            SendKeys.SendWait(sFile2_file);  // ファイル名を入力.

            // 比較実行.
            ui.PushButtonById(aeJww, "1064"); // [比較実行]ボタンをクリック.
        }
    }
}
