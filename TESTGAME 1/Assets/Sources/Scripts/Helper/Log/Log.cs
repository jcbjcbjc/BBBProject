/***
 * 
 *    Title: "SUIFW" UI框架项目
 *           主题： 框架日志系统       
 *    Description: 
 *           功能： 
 *           1：更方便于软件（游戏）开发人员，调试系统程序。
 *           2：记录用户程序的流转。为程序调试、项目上线后记录核心信息使用。
 *    Date: 2017
 *    Version: 0.1版本
 *    Modify Recoder: 
 *    
 *   
 */
using System.Collections;
using System.Collections.Generic;
using System;                                               //C#的核心命名空间
using System.Diagnostics;
using System.IO;                                            //文件读写命名空间
using System.Threading;                                     //多线程命名空间

namespace SUIFW
{
    public static class Log
    {
        /* 核心字段 */
        private static List<string> _LiLogArray;            //Log日志缓存数据
        private static string _LogPath = null;              //Log日志文件路径
        private static State _LogState;                     //Log日志状态（部署模式）
        private static int _LogMaxCapacity;                 //Log日志最大容量
        private static int _LogBufferMaxNumber;             //Log日志缓存最大容量
        /* 日志文件常量定义  */
        //JSON 配置文件“标签常量”
        private const string JSON_CONFIG_LOG_DRIVE_NAME = "LogDriveName";
        private const string JSON_CONFIG_LOG_PATH = "LogPath";
        private const string JSON_CONFIG_LOG_STATE = "LogState";
        private const string JSON_CONFIG_LOG_MAX_CAPACITY = "LogMaxCapacity";
        private const string JSON_CONFIG_LOG_BUFFER_NUMBER = "LogBufferNumber";
        //日志状态常量(部署模式)
        private const string JSON_CONFIG_LOG_STATE_DEVELOP = "Develop";
        private const string JSON_CONFIG_LOG_STATE_SPECIAL = "Speacial";
        private const string JSON_CONFIG_LOG_STATE_DEPLOY = "Deploy";
        private const string JSON_CONFIG_LOG_STATE_STOP = "Stop";
        //日志默认路径
        private static string JSON_CONFIG_LOG_DEFAULT_PATH = "DungeonFighterLog.txt";
        //日志默认最大容量
        private static int LOG_DEFAULT_MAX_CACITY_NUMBER = 2000;
        //日志缓存默认最大容量
        private static int LOG_DEFAULT_MAX_LOG_BUFFER_NUMBER = 1;
        //日志提示信息
        private static string LOG_ImportTIPS = "@Important !!! ";
        private static string LOG_WarningTIPS = "Warning ";

        /*  临时字段定义 */
        private static string strLogState = null;           //日志状态(部署模式)
        private static string strLogMaxCapacity = null;     //日志最大容量  
        private static string strLogBufferNumber = null;    //日志缓存最大容量




        /// <summary>
        /// 静态构造函数
        /// </summary>
        static Log()
        {

            //日志缓存数据
            _LiLogArray = new List<string>();

            //日志文件路径
            IConfigManager configMgr = new ConfigManagerByJson(SysDefine.SYS_PATH_CONFIG_INFO);

            //PC与编辑器环境下的路径，使用配置文件。
#if UNITY_STANDALONE_WIN||UNITY_EDITOR
            string strPCTruePath = configMgr.AppSetting[JSON_CONFIG_LOG_DRIVE_NAME] + ":\\" + configMgr.AppSetting[JSON_CONFIG_LOG_PATH] + ".txt";
            _LogPath = strPCTruePath;
#endif

            //日志状态(部署模式)
            strLogState = configMgr.AppSetting[JSON_CONFIG_LOG_STATE];
            //日志最大容量
            strLogMaxCapacity = configMgr.AppSetting[JSON_CONFIG_LOG_MAX_CAPACITY];
            //日志缓存最大容量
            strLogBufferNumber = configMgr.AppSetting[JSON_CONFIG_LOG_BUFFER_NUMBER];

            //日志文件路径
            if (string.IsNullOrEmpty(_LogPath))
            {
                _LogPath = UnityEngine.Application.persistentDataPath + "//" + JSON_CONFIG_LOG_DEFAULT_PATH;
            }

            //日志状态(部署模式)
            if (!string.IsNullOrEmpty(strLogState))
            {
                switch (strLogState)
                {
                    case JSON_CONFIG_LOG_STATE_DEVELOP:
                        _LogState = State.Develop;
                        break;
                    case JSON_CONFIG_LOG_STATE_SPECIAL:
                        _LogState = State.Speacial;
                        break;
                    case JSON_CONFIG_LOG_STATE_DEPLOY:
                        _LogState = State.Deploy;
                        break;
                    case JSON_CONFIG_LOG_STATE_STOP:
                        _LogState = State.Stop;
                        break;
                    default:
                        _LogState = State.Stop;
                        break;
                }
            }
            else
            {
                _LogState = State.Stop;
            }

            //日志最大容量
            if (!string.IsNullOrEmpty(strLogMaxCapacity))
            {
                _LogMaxCapacity = Convert.ToInt32(strLogMaxCapacity);
            }
            else
            {
                _LogMaxCapacity = LOG_DEFAULT_MAX_CACITY_NUMBER;
            }

            //日志缓存最大容量
            if (!string.IsNullOrEmpty(strLogBufferNumber))
            {
                _LogBufferMaxNumber = Convert.ToInt32(strLogBufferNumber);
            }
            else
            {
                _LogBufferMaxNumber = LOG_DEFAULT_MAX_LOG_BUFFER_NUMBER;
            }
        }//Log_end(构造函数)

        /// <summary>
        /// 写数据到文件中
        /// </summary>
        /// <param name="writeFileDate">写入的调试信息</param>
        /// <param name="level">重要等级级别</param>
        public static void Write(string writeFileDate, Level level)
        {
            //参数检查
            if (_LogState == State.Stop)
            {
                return;
            }

            //如果日志缓存数量超过指定容量，则清空
            if (_LiLogArray.Count >= _LogMaxCapacity)
            {
                _LiLogArray.Clear();                                           //清空缓存中的数据
            }

            if (!string.IsNullOrEmpty(writeFileDate))
            {
                //增加日期与时间
                writeFileDate = _LogState.ToString() + "|" + DateTime.Now.ToShortTimeString() + "|   " + writeFileDate + "\r\n";

                //对于不同的“日志状态”，分特定情形写入文件
                if (level == Level.High)
                {
                    writeFileDate = LOG_ImportTIPS + writeFileDate;
                }
                else if (level == Level.Special)
                {
                    writeFileDate = LOG_WarningTIPS + writeFileDate;
                }
                switch (_LogState)
                {
                    case State.Develop:                                        //开发状态
                        //追加调试信息，写入文件
                        AppendDateToFile(writeFileDate);
                        break;
                    case State.Speacial:                                       //“指定"状态
                        if (level == Level.High || level == Level.Special)
                        {
                            AppendDateToFile(writeFileDate);
                        }
                        break;
                    case State.Deploy:                                         //部署状态
                        if (level == Level.High)
                        {
                            AppendDateToFile(writeFileDate);
                        }
                        break;
                    case State.Stop:                                           //停止输出
                        break;
                    default:
                        break;
                }
            }
        }//Write_end

        public static void Write(string writeFileDate)
        {
            Write(writeFileDate, Level.Low);
        }

        /// <summary>
        /// 追加数据到文件
        /// </summary>
        /// <param name="writeFileDate">调试信息</param>
        private static void AppendDateToFile(string writeFileDate)
        {
            if (!string.IsNullOrEmpty(writeFileDate))
            {
                //调试信息数据追加到缓存集合中
                _LiLogArray.Add(writeFileDate);
            }

            //缓存集合数量超过一定指定数量（"_LogBufferMaxNumber"）,则同步到实体文件中。
            if (_LiLogArray.Count % _LogBufferMaxNumber == 0)
            {
                //同步缓存数据信息到实体文件中。
                SyncLogCatchToFile();
            }
        }

        /// <summary>
        /// 创建文件与写入文件
        /// </summary>
        /// <param name="pathAndName">路径与名称</param>
        /// <param name="info"></param>
        private static void CreateFile(string pathAndName, string info)
        {
            //文件流信息
            StreamWriter sw;
            FileInfo t = new FileInfo(pathAndName);
            if (!t.Exists)
            {
                //如果此文件不存在则创建
                sw = t.CreateText();
            }
            else
            {
                //如果此文件存在则打开
                sw = t.AppendText();
            }
            //以行的形式写入信息
            sw.WriteLine(info);
            //关闭流
            sw.Close();
            //销毁流
            sw.Dispose();
        }

        #region  重要管理方法

        /// <summary>
        /// 查询日志缓存中的内容
        /// </summary>
        /// <returns>
        /// 返回缓存中的查询内容
        /// </returns>
        public static List<string> QueryAllDateFromLogBuffer()
        {
            if (_LiLogArray != null)
            {
                return _LiLogArray;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 查询日志缓存中实际数量个数
        /// </summary>
        /// <returns>
        /// 返回-1,表示查询失败。
        /// </returns>
        public static int QueryLogBufferCount()
        {
            if (_LiLogArray != null)
            {
                return _LiLogArray.Count;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 清除日志缓存中所有数据
        /// </summary>
        public static void ClearLogBufferAllDate()
        {
            if (_LiLogArray != null)
            {
                //数据全部清空
                _LiLogArray.Clear();
            }
        }

        /// <summary>
        /// 同步缓存数据信息到实体文件中
        /// </summary>
        public static void SyncLogCatchToFile()
        {
            if (!string.IsNullOrEmpty(_LogPath))
            {
                foreach (string item in _LiLogArray)
                {
                    CreateFile(_LogPath, item);
                }
                //清除日志缓存中所有数据
                ClearLogBufferAllDate();
            }
        }

        #endregion

        #region  本类的枚举类型
        /// <summary>
        /// 日志状态（部署模式）
        /// </summary>
        public enum State
        {
            Develop,                                                           //开发模式（输出所有日志内容）
            Speacial,                                                          //指定输出模式
            Deploy,                                                            //部署模式（只输出最核心日志信息，例如严重错误信息，用户登陆账号等）
            Stop                                                               //停止输出模式（不输出任何日志信息）
        };

        /// <summary>
        /// 调试信息的等级（表示调试信息本身的重要程度）
        /// </summary>
        public enum Level
        {
            High,
            Special,
            Low
        }
        #endregion
    }//Class_end
}