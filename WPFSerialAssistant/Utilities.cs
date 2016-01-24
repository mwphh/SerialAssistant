using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace WPFSerialAssistant
{
    public static class Utilities
    {
        public static string BytesToText(List<byte> bytesBuffer, ReceiveMode mode, Encoding encoding)
        {
            string result = "";

            if (mode == ReceiveMode.Character)
            {
                return encoding.GetString(bytesBuffer.ToArray<byte>());
            }

            foreach (var item in bytesBuffer)
            {
                switch (mode)
                {
                    case ReceiveMode.Hex:
                        result += Convert.ToString(item, 16).ToUpper() + " ";
                        break;
                    case ReceiveMode.Decimal:
                        result += Convert.ToString(item, 10) + " ";
                        break;
                    case ReceiveMode.Octal:
                        result += Convert.ToString(item, 8) + " ";
                        break;
                    case ReceiveMode.Binary:
                        result += Convert.ToString(item, 2) + " ";
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        public static string ToSpecifiedText(string text, SendMode mode, Encoding encoding)
        {
            string result = "";
            switch (mode)
            {
                case SendMode.Character:
                    text = text.Trim();

                    // 转换成字节
                    List<byte> src = new List<byte>();

                    string[] grp = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var item in grp)
                    {
                        src.Add(Convert.ToByte(item, 16));
                    }

                    // 转换成字符串
                    result = encoding.GetString(src.ToArray<byte>());
                    break;
                    
                case SendMode.Hex:
                    
                    byte[] byteStr = encoding.GetBytes(text.ToCharArray());

                    foreach (var item in byteStr)
                    {
                        result += Convert.ToString(item, 16).ToUpper() + " ";
                    }
                    break;
                default:
                    break;
            }

            return result.Trim();
        }

    }
}
