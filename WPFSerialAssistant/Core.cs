using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WPFSerialAssistant
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 核心初始化
        /// </summary>
        private void InitCore()
        {
            // 加载配置信息
            LoadConfig();

            // 其他模块初始化
            InitClockTimer();
            InitAutoSendDataTimer();
            InitSerialPort();

            // 查找可以使用的端口号
            FindPorts();
        }

        #region 状态栏
        /// <summary>
        /// 更新时间信息
        /// </summary>
        private void UpdateTimeDate()
        {
            string timeDateString = "";
            DateTime now = DateTime.Now;
            timeDateString = string.Format("{0}年{1}月{2}日 {3}:{4}:{5}", 
                now.Year, 
                now.Month.ToString("00"), 
                now.Day.ToString("00"), 
                now.Hour.ToString("00"), 
                now.Minute.ToString("00"), 
                now.Second.ToString("00"));

            timeDateTextBlock.Text = timeDateString;
        }

        /// <summary>
        /// 警告信息提示（一直提示）
        /// </summary>
        /// <param name="message">提示信息</param>
        private void Alert(string message)
        {
            // #FF68217A
            statusBar.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x21, 0x2A));
            statusInfoTextBlock.Text = message;
        }

        /// <summary>
        /// 普通状态信息提示
        /// </summary>
        /// <param name="message">提示信息</param>
        private void Information(string message)
        {
            if (serialPort.IsOpen)
            {
                // #FFCA5100
                statusBar.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xCA, 0x51, 0x00));
            }
            else
            {
                // #FF007ACC
                statusBar.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x7A, 0xCC));
            }
            statusInfoTextBlock.Text = message;
        }

        #endregion

        private void RecvDataBoxAppend(string textData)
        {
            if (showRecvDataCheckBox.IsChecked == true)
            {
                this.recvDataRichTextBox.AppendText(textData);
                this.recvDataRichTextBox.ScrollToEnd();
            }
        }

        private bool SendData()
        {
            string textToSend = sendDataTextBox.Text;
            if (string.IsNullOrEmpty(textToSend))
            {
                Alert("要发送的内容不能为空！");
                return false;
            }

            if (autoSendEnableCheckBox.IsChecked == true)
            {
                return SerialPortWrite(textToSend, false);
            }
            else
            {
                return SerialPortWrite(textToSend);
            }
        }

        private void AutoSendData()
        {
            bool ret = SendData();

            if (ret == false)
            {
                return;
            }

            // 启动自动发送定时器
            StartAutoSendDataTimer(GetAutoSendDataInterval());

            // 提示处于自动发送状态
            progressBar.Visibility = Visibility.Visible;
            Information("串口自动发送数据中...");
        }

        private int GetAutoSendDataInterval()
        {
            int interval = 1000;

            if (int.TryParse(autoSendIntervalTextBox.Text.Trim(), out interval) == true)
            {
                string select = timeUnitComboBox.Text.Trim();

                switch (select)
                {
                    case "毫秒":
                        break;
                    case "秒钟":
                        interval *= 1000;
                        break;
                    case "分钟":
                        interval = interval * 60 * 1000;
                        break;
                    default:
                        break;
                }
            }

            return interval;
        }

        #region 配置信息
        // 
        // 目前保存的配置信息如下：
        // 1. 波特率
        // 2. 奇偶校验位
        // 3. 数据位
        // 4. 停止位
        // 5. 字节编码
        // 6. 发送区文本内容
        // 7. 自动发送时间间隔
        // 8. 窗口状态：最大化|高度+宽度
        // 9. 面板显示状态
        // 10. 接收数据模式
        // 11. 是否显示接收数据
        // 12. 发送数据模式
        // 13. 发送追加内容
        //

        /// <summary>
        /// 保存配置信息
        /// </summary>
        private void SaveConfig()
        {
            // 配置对象实例
            Configuration config = new Configuration();

            // 保存波特率
            AddBaudRate(config);

            // 保存奇偶校验位
            config.Add("parity", parityComboBox.SelectedIndex);

            // 保存数据位
            config.Add("dataBits", dataBitsComboBox.SelectedIndex);

            // 保存停止位
            config.Add("stopBits", stopBitsComboBox.SelectedIndex);

            // 字节编码
            config.Add("encoding", encodingComboBox.SelectedIndex);

            // 保存发送区文本内容
            config.Add("sendDataTextBoxText", sendDataTextBox.Text);

            // 自动发送时间间隔
            config.Add("autoSendDataInterval", autoSendIntervalTextBox.Text);
            config.Add("timeUnit", timeUnitComboBox.SelectedIndex);

            // 窗口状态信息
            config.Add("maxmized", this.WindowState == WindowState.Maximized);  
            config.Add("windowWidth", this.Width);
            config.Add("windowHeight", this.Height);
            config.Add("windowLeft", this.Left);
            config.Add("windowTop", this.Top);

            // 面板显示状态
            config.Add("serialPortConfigPanelVisible", serialPortConfigPanel.Visibility == Visibility.Visible);
            config.Add("autoSendConfigPanelVisible", autoSendConfigPanel.Visibility == Visibility.Visible);
            config.Add("serialCommunicationConfigPanelVisible", serialCommunicationConfigPanel.Visibility == Visibility.Visible);

            // 保存接收模式
            config.Add("receiveMode", receiveMode);
            config.Add("showReceiveData", showReceiveData);

            // 保存发送模式
            config.Add("sendMode", sendMode);

            // 保存发送追加
            config.Add("appendContent", appendContent);


            // 保存配置信息到磁盘中
            Configuration.Save(config, @"Config\default.conf");
        }

        /// <summary>
        /// 将波特率列表添加进去
        /// </summary>
        /// <param name="conf"></param>
        private void AddBaudRate(Configuration conf)
        {
            conf.Add("baudRate", baudRateComboBox.Text);
        }

        /// <summary>
        /// 加载配置信息
        /// </summary>
        private bool LoadConfig()
        {
            Configuration config = Configuration.Read(@"Config\default.conf");

            if (config == null)
            {
                return false;
            }

            // 获取波特率
            string baudRateStr = config.GetString("baudRate");
            baudRateComboBox.Text = baudRateStr;

            // 获取奇偶校验位
            int parityIndex = config.GetInt("parity");
            parityComboBox.SelectedIndex = parityIndex;

            // 获取数据位
            int dataBitsIndex = config.GetInt("dataBits");
            dataBitsComboBox.SelectedIndex = dataBitsIndex;

            // 获取停止位
            int stopBitsIndex = config.GetInt("stopBits");
            stopBitsComboBox.SelectedIndex = stopBitsIndex;

            // 获取编码
            int encodingIndex = config.GetInt("encoding");
            encodingComboBox.SelectedIndex = encodingIndex;

            // 获取发送区内容
            string sendDataText = config.GetString("sendDataTextBoxText");
            sendDataTextBox.Text = sendDataText;

            // 获取自动发送数据时间间隔
            string interval = config.GetString("autoSendDataInterval");
            int timeUnitIndex = config.GetInt("timeUnit");
            autoSendIntervalTextBox.Text = interval;
            timeUnitComboBox.SelectedIndex = timeUnitIndex;

            // 窗口状态
            if (config.GetBool("maxmized"))
            {
                this.WindowState = WindowState.Maximized;
            }
            double width = config.GetDouble("windowWidth");
            double height = config.GetDouble("windowHeight");
            double top = config.GetDouble("windowTop");
            double left = config.GetDouble("windowLeft");
            this.Width = width;
            this.Height = height;
            this.Top = top;
            this.Left = left;

            // 面板显示状态
            if (config.GetBool("serialPortConfigPanelVisible"))
            {
                serialSettingViewMenuItem.IsChecked = true;
                serialPortConfigPanel.Visibility = Visibility.Visible;
            }
            else
            {
                serialSettingViewMenuItem.IsChecked = false;
                serialPortConfigPanel.Visibility = Visibility.Collapsed;
            }

            if (config.GetBool("autoSendConfigPanelVisible"))
            {
                autoSendDataSettingViewMenuItem.IsChecked = true;
                autoSendConfigPanel.Visibility = Visibility.Visible;
            }
            else
            {
                autoSendDataSettingViewMenuItem.IsChecked = false;
                autoSendConfigPanel.Visibility = Visibility.Collapsed;
            }

            if (config.GetBool("serialCommunicationConfigPanelVisible"))
            {
                serialCommunicationSettingViewMenuItem.IsChecked = true;
                serialCommunicationConfigPanel.Visibility = Visibility.Visible;
            }
            else
            {
                serialCommunicationSettingViewMenuItem.IsChecked = false;
                serialCommunicationConfigPanel.Visibility = Visibility.Collapsed;
            }

            // 加载接收模式
            receiveMode = (ReceiveMode)config.GetInt("receiveMode");

            switch (receiveMode)
            {
                case ReceiveMode.Character:
                    recvCharacterRadioButton.IsChecked = true;
                    break;
                case ReceiveMode.Hex:
                    recvHexRadioButton.IsChecked = true;
                    break;
                case ReceiveMode.Decimal:
                    recvDecRadioButton.IsChecked = true;
                    break;
                case ReceiveMode.Octal:
                    recvOctRadioButton.IsChecked = true;
                    break;
                case ReceiveMode.Binary:
                    recvBinRadioButton.IsChecked = true;
                    break;
                default:
                    break;
            }

            showReceiveData = config.GetBool("showReceiveData");
            showRecvDataCheckBox.IsChecked = showReceiveData;

            // 加载发送模式
            sendMode = (SendMode)config.GetInt("sendMode");

            switch (sendMode)
            {
                case SendMode.Character:
                    sendCharacterRadioButton.IsChecked = true;
                    break;
                case SendMode.Hex:
                    sendHexRadioButton.IsChecked = true;
                    break;
                default:
                    break;
            }

            //加载追加内容
           appendContent = config.GetString("appendContent");

            switch (appendContent)
            {
                case "":
                    appendNoneRadioButton.IsChecked = true;
                    break;
                case "\r":
                    appendReturnRadioButton.IsChecked = true;
                    break;
                case "\n":
                    appednNewLineRadioButton.IsChecked = true;
                    break;
                case "\r\n":
                    appendReturnNewLineRadioButton.IsChecked = true;
                    break;
                default:
                    break;
            }
            return true;
        }
        #endregion
    }
}
