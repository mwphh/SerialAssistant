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

        }

        private void autoSendDataSettingViewMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void serialCommunicationSettingViewMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void compactViewMenuItem_Click(object sender, RoutedEventArgs e)
        {

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

        }

        private void sendDataButton_Click(object sender, RoutedEventArgs e)
        {
            SendData();
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
