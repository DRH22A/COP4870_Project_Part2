﻿#pragma checksum "C:\Users\drhal\source\repos\CSharpSpring23_StudentProject\App.LearningManagement\UWP.LearningManagement\Pages\MainPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "21721C535DABA2A68C100F9A42FBADCC1506DF36BCC409C07C4E000D7BEC2130"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UWP.LearningManagement
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // Pages\MainPage.xaml line 18
                {
                    this.ViewToggle = (global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target);
                    ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)this.ViewToggle).Checked += this.ViewToggleButton_Checked;
                    ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)this.ViewToggle).Unchecked += this.ViewToggleButton_Unchecked;
                }
                break;
            case 3: // Pages\MainPage.xaml line 19
                {
                    this.Login = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.Login).Click += this.Login_Click;
                }
                break;
            case 4: // Pages\MainPage.xaml line 20
                {
                    this.studentInfo = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.studentInfo).Click += this.StudentOptions_Click;
                }
                break;
            case 5: // Pages\MainPage.xaml line 21
                {
                    this.courseInfo = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.courseInfo).Click += this.MaintainCourses_Click;
                }
                break;
            case 6: // Pages\MainPage.xaml line 22
                {
                    global::Windows.UI.Xaml.Controls.Button element6 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)element6).Click += this.Exit_Click;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.19041.685")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

