namespace GenShin_Launcher_Plus.Core
{
    /// <summary>
    /// 解锁结果
    /// </summary>
    public enum UnlockResult
    {
        /// <summary>
        /// 解锁成功，且游戏已经顺利运行完成
        /// </summary>
        Ok,
        /// <summary>
        /// 进程已经退出
        /// </summary>
        ProcessHasExited,
        /// <summary>
        /// 模块搜索超时
        /// </summary>
        ModuleSearchTimeExceed,
        /// <summary>
        /// 读取进程内存失败
        /// </summary>
        ReadProcessMemoryFailed,
        /// <summary>
        /// 进程内存中不存在对应的模式
        /// </summary>
        NoMatchedPatternFound,
        /// <summary>
        /// 此解锁器已经失效
        /// </summary>
        UnlockerInvalid,

        /// <summary>
        /// 进程启动失败
        /// </summary>
        ProcessStartFailed
    }
}