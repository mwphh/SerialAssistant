using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows;

namespace WPFSerialAssistant
{
    public partial class MainWindow : Window
    {
        // 保存面板的显示状态
        private Stack<Visibility> panelVisibilityStack = new Stack<Visibility>(3);

        /// <summary>
        /// 判断是否处于简洁视图模式
        /// </summary>
        /// <returns></returns>
        private bool IsCompactViewMode()
        {
            if (autoSendConfigPanel.Visibility == Visibility.Collapsed && 
                serialCommunicationConfigPanel.Visibility == Visibility.Collapsed &&
                autoSendConfigPanel.Visibility ==  Visibility.Collapsed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 进入简洁视图模式
        /// </summary>
        private void EnterCompactViewMode()
        {
            // 首先需要保持panel的显示状态
            panelVisibilityStack.Push(serialPortConfigPanel.Visibility);
            panelVisibilityStack.Push(autoSendConfigPanel.Visibility);
            panelVisibilityStack.Push(serialCommunicationConfigPanel.Visibility);

            // 进入简洁视图模式
            serialPortConfigPanel.Visibility = Visibility.Collapsed;
            autoSendConfigPanel.Visibility = Visibility.Collapsed;
            serialCommunicationConfigPanel.Visibility = Visibility.Collapsed;

            // 把对应的菜单项取消选中
            serialSettingViewMenuItem.IsChecked = false;
            autoSendDataSettingViewMenuItem.IsChecked = false;
            serialCommunicationSettingViewMenuItem.IsChecked = false;

            // 此时无法视图模式，必须恢复到原先的视图模式才可以
            serialSettingViewMenuItem.IsEnabled = false;
            autoSendDataSettingViewMenuItem.IsEnabled = false;
            serialCommunicationSettingViewMenuItem.IsEnabled = false;

            // 切换至简洁视图模式，菜单项选中
            compactViewMenuItem.IsChecked = true;

            // 
            Information("进入简洁视图模式。");
        }

        /// <summary>
        /// 恢复到原来的视图模式
        /// </summary>
        private void RestoreViewMode()
        {
            // 恢复面板显示状态
            serialCommunicationConfigPanel.Visibility = panelVisibilityStack.Pop();
            autoSendConfigPanel.Visibility = panelVisibilityStack.Pop();
            serialPortConfigPanel.Visibility = panelVisibilityStack.Pop();

            // 恢复菜单选中状态
            if (serialPortConfigPanel.Visibility == Visibility.Visible)
            {
                serialSettingViewMenuItem.IsChecked = true;
            }

            if (autoSendConfigPanel.Visibility == Visibility.Visible)
            {
                autoSendDataSettingViewMenuItem.IsChecked = true;
            }

            if (serialCommunicationConfigPanel.Visibility == Visibility.Visible)
            {
                serialCommunicationSettingViewMenuItem.IsChecked = true;
            }

            serialSettingViewMenuItem.IsEnabled = true;
            autoSendDataSettingViewMenuItem.IsEnabled = true;
            serialCommunicationSettingViewMenuItem.IsEnabled = true;

            compactViewMenuItem.IsChecked = false;

            // 
            Information("恢复原来的视图模式。");
        }
    }
}
