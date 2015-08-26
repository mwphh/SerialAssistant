using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFSerialAssistant
{
    public partial class MainWindow : Window
    {
        #region Event handler for menu items
        private void saveSerialDataMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void saveConfigMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void loadConfigMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void serialSettingViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            bool state = serialSettingViewMenuItem.IsChecked;

            if (state == false)
            {
                serialPortConfigPanel.Visibility = Visibility.Visible;
            }
            else
            {
                serialPortConfigPanel.Visibility = Visibility.Collapsed;
            }

            serialSettingViewMenuItem.IsChecked = !state;
            compactViewMenuItem.IsChecked = false;
        }

        private void autoSendDataSettingViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            bool state = autoSendDataSettingViewMenuItem.IsChecked;

            if (state == false)
            {
                autoSendConfigPanel.Visibility = Visibility.Visible;
            }
            else
            {
                autoSendConfigPanel.Visibility = Visibility.Collapsed;
            }

            autoSendDataSettingViewMenuItem.IsChecked = !state;
            compactViewMenuItem.IsChecked = false;
        }

        private void serialCommunicationSettingViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            bool state = serialCommunicationSettingViewMenuItem.IsChecked;

            if (state == false)
            {
                serialCommunicationConfigPanel.Visibility = Visibility.Visible;
            }
            else
            {
                serialCommunicationConfigPanel.Visibility = Visibility.Collapsed;
            }

            serialCommunicationSettingViewMenuItem.IsChecked = !state;
            compactViewMenuItem.IsChecked = false;
        }

        private void compactViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            bool state = compactViewMenuItem.IsChecked;

            if (state == false)
            {
                serialSettingViewMenuItem.IsChecked = false;
                autoSendDataSettingViewMenuItem.IsChecked = false;
                serialCommunicationSettingViewMenuItem.IsChecked = false;

                serialCommunicationConfigPanel.Visibility = Visibility.Collapsed;
                autoSendConfigPanel.Visibility = Visibility.Collapsed;
                serialPortConfigPanel.Visibility = Visibility.Collapsed;
            }

            compactViewMenuItem.IsChecked = true;
        }

        private void aboutMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void helpMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region Event handler for buttons and so on.
        private void openClosePortButton_Click(object sender, RoutedEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                if (ClosePort())
                {
                    openClosePortButton.Content = "打开";
                }
            }
            else
            {
                if (OpenPort())
                {
                    openClosePortButton.Content = "关闭";
                }
            }
        }

        private void findPortButton_Click(object sender, RoutedEventArgs e)
        {
            FindPorts();
        }

        private void autoSendEnableCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (autoSendEnableCheckBox.IsChecked == true)
            {
                Information(string.Format("使能串口自动发送功能，发送间隔：{0} {1}。", autoSendIntervalTextBox.Text, timeUnitComboBox.Text.Trim()));
            }
            else
            {
                Information("禁用串口自动发送功能。");
                StopAutoSendDataTimer();
                progressBar.Visibility = Visibility.Collapsed;
            }
        }

        private void sendDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (autoSendEnableCheckBox.IsChecked == true)
            {
                AutoSendData();
            }
            else
            {
                SendData();
            }
        }

        private void recvCharacterRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void recvHexRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void showRecvDataDisableButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void saveRecvDataButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void clearRecvDataBoxButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void sendCharacterRadioButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void sendHexRadioButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void manualInputRadioButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void loadFileRadioButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void clearSendDataTextBox_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region Event handler for timers
        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            UpdateTimeDate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoSendDataTimer_Tick(object sender, EventArgs e)
        {
            bool ret = false;
            ret = SendData();

            if (ret == false)
            {
                StopAutoSendDataTimer();
            }
        }
        #endregion

        #region EventHandler for serialPort
        private void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            System.IO.Ports.SerialPort sp = sender as System.IO.Ports.SerialPort;

            if (sp != null)
            {
                string textBuffer = sp.ReadExisting();

                this.Dispatcher.Invoke(new Action(() =>
                {
                    RecvDataBoxAppend(textBuffer);
                }));
            }
        }
        #endregion
    }
}
