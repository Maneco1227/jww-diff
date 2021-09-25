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

namespace jww_diff
{
    //-----------------------------------------------------------------------------
    // UIAutomationLib
    //
    // 下記を元に若干改変しています。
    //
    // (C) Kenichiro Hamada (@ken_hamada)
    // https://qiita.com/ken_hamada/items/501b164374667319d270
    //
    //-----------------------------------------------------------------------------
    class UIAutomationLib
    {
        // UI automation系以外に、Win32APIも使いますのでその宣言。 
        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        static extern void SetCursorPos(int X, int Y);
        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        //　マウスイベント.
        //　定義は以下に.
        //  https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-mouse_event
        //
        private const int MOUSEEVENTF_LEFTDOWN = 0x2;
        private const int MOUSEEVENTF_LEFTUP = 0x4;

        // 指定されたプロセスのMainFrameに関するAutomationElementを取得.
        public AutomationElement GetMainFrameElement(Process p)
        {
            return AutomationElement.FromHandle(p.MainWindowHandle);
        }

        // 指定されたAutomationIdのButtonをクリックします.
        // （例外対策はしていませんので注意）
        public void PushButtonById(AutomationElement element, string AutomationId)
        {
            InvokePattern button;
            
            while(FindElementById(element, AutomationId).GetCurrentPattern(InvokePattern.Pattern) == null);
            button = FindElementById(element, AutomationId).GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
            
            button.Invoke();
        }

        // 指定されたAutomationIdのパーツをクリックします.
        // （例外対策はしていませんので注意。clickableじゃないパーツ叩くと多分落ちるw）
        public void ClickElement(AutomationElement element, string AutomationId)
        {
            AutomationElement target;
            
            while(FindElementById(element, AutomationId) == null);
            target = FindElementById(element, AutomationId);
            
            System.Windows.Point p = target.GetClickablePoint();
            SetCursorPos((int)p.X, (int)p.Y);

            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
       
        // 指定されたautomationIdに一致するAutomationElementを取得.
        public AutomationElement FindElementById(AutomationElement rootElement, string automationId)
        {
            return rootElement.FindFirst(
                TreeScope.Element | TreeScope.Descendants,
                new PropertyCondition(AutomationElement.AutomationIdProperty, automationId));
        }
    }
}
