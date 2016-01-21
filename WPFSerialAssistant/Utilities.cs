using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSerialAssistant
{
    public static class Utilities
    {
        public static string BytesToText(List<byte> bytesBuffer, ReceiveMode mode)
        {
            string result = "";

            foreach (var item in bytesBuffer)
            {
                switch (mode)
                {
                    case ReceiveMode.Character:
                        result += (char)item;
                        break;
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

        public static string ToSpecifiedText(string text, SendMode mode)
        {
            string result = "";
            switch (mode)
            {
                case SendMode.Character:
                    text = text.Trim();

                    if (text == "")
                    {
                        return "";
                    }

                    try
                    {
                        string[] grp = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var item in grp)
                        {
                            result += (char)Convert.ToInt32(item, 16);
                        }
                    }
                    catch { }
                    
                    break;
                case SendMode.Hex:
                    foreach (var item in text)
                    {
                        result += Convert.ToString(item, 16).ToUpper() + " ";
                    }
                    break;
                default:
                    break;
            }

            return result;
        }

    }
}
