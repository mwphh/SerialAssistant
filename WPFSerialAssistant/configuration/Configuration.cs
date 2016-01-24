using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace WPFSerialAssistant
{
    /// <summary>
    /// 配置类
    /// </summary>
    class Configuration
    {
        #region 私有字段
        /// <summary>
        /// 默认的配置文件存储路径
        /// </summary>
        private static readonly string defaultConfPath = @"Config\config.conf";

        /// <summary>
        /// 默认配置文件的存储编码格式
        /// </summary>
        private static readonly Encoding defaultConfEncoding = Encoding.Default;

        /// <summary>
        /// 存储配置信息的字典
        /// </summary>
        private Dictionary<string, object> configuration = null;
        #endregion

        private Dictionary<string, object> ConfigInfo
        {
            get
            {
                return configuration;
            }
        }

        #region 方法
        /// <summary>
        /// 构造函数
        /// </summary>
        public Configuration()
        {
            if (configuration == null)
            {
                configuration = new Dictionary<string, object>();
                
            }
        }

        /// <summary>
        /// 私有构造函数
        /// </summary>
        /// <param name="confDic"></param>
        private Configuration(Dictionary<string, object> confDic)
        {
            if (confDic == null)
            {
                this.configuration = new Dictionary<string, object>();
            }
            else
            {
                this.configuration = confDic;
            }
        }

        /// <summary>
        /// 添加配置键值对
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void Add(string key, object value)
        {
            configuration.Add(key, value);
        }

        /// <summary>
        /// 移除配置信息键值对
        /// </summary>
        /// <param name="key">键</param>
        public void Remove(string key)
        {
            configuration.Remove(key);
        }

        /// <summary>
        /// 清除所有的配置信息键值对
        /// </summary>
        public void Clear()
        {
            configuration.Clear();
        }

        /// <summary>
        /// 获取配置信息值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public object Get(string key)
        {
            return configuration[key];
        }

        /// <summary>
        /// 获取string类型的配置信息
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>string类型的值</returns>
        public string GetString(string key)
        {
            return Get(key).ToString();
        }

        /// <summary>
        /// 获取int类型的配置信息
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>int类型的值</returns>
        public int GetInt(string key)
        {
            return int.Parse(Get(key).ToString());
        }

        /// <summary>
        /// 获取bool类型的配置信息
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>bool类型的值</returns>
        public bool GetBool(string key)
        {
            return bool.Parse(Get(key).ToString());
        }

        /// <summary>
        /// 获取double类型的配置信息
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>double类型的值</returns>
        public double GetDouble(string key)
        {
            return double.Parse(Get(key).ToString());
        }

        /// <summary>
        /// 获取decimal类型的配置信息
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>decimal类型的值</returns>
        public decimal GetDecimal(string key)
        {
            return decimal.Parse(Get(key).ToString());
        }
        #endregion

        #region 静态方法
        /// <summary>
        /// 存储配置信息到默认的路径中
        /// </summary>
        /// <param name="conf"></param>
        public static void Save(Configuration conf)
        {
            Save(conf, defaultConfPath);
        }

        /// <summary>
        /// 存储配置信息到自定义路径
        /// </summary>
        /// <param name="conf"></param>
        /// <param name="path">存储配置信息的路径</param>
        public static void Save(Configuration conf, string path)
        {
            // 构建配置信息的json字符串
            string confStr = Newtonsoft.Json.JsonConvert.SerializeObject(conf.ConfigInfo);

            // 如果指定的配置路径不存在，则新建一个
            if (System.IO.File.Exists(path) == false)
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
            }

            // 保存配置信息
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(path, false, defaultConfEncoding))
            {
                try
                {
                    sw.Write(confStr);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 从默认存储路径读取配置信息
        /// </summary>
        /// <returns>配置信息</returns>
        public static Configuration Read()
        {
            return Read(defaultConfPath);
        }

        /// <summary>
        /// 从自定义存储路径读取配置信息
        /// </summary>
        /// <param name="path">存储配置信息路径</param>
        /// <returns>配置信息</returns>
        public static Configuration Read(string path)
        {
            string confStr = "";
            Configuration conf = null;

            if (System.IO.File.Exists(path) == false)
            {
                return null;
            }

            // 读取配置信息
            using (System.IO.StreamReader sr = new System.IO.StreamReader(path, defaultConfEncoding))
            {
                try
                {
                    confStr = sr.ReadToEnd();
                    // 解析得到配置信息
                    Dictionary<string, object> confInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(confStr);
                    conf = new Configuration(confInfo);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return conf;
        }
        #endregion
    }
}
