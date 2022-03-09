using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static GenShin_Launcher_Plus.Core.NativeMethods;

namespace GenShin_Launcher_Plus.Core
{
    /// <summary>
    /// FPS Unlocker
    /// 需要 .NET 进程为 64 位 才能正常使用
    /// <para/>
    /// Credit to @Crskycode Github
    /// </summary>
    public class Unlocker
    {
        /// <summary>
        /// 设置fps位的偏移量
        /// </summary>
        private UIntPtr fpsOffset;

        /// <summary>
        /// 游戏进程
        /// </summary>
        private readonly Process gameProcess;

        /// <summary>
        /// 当前解锁器是否无效
        /// </summary>
        private bool isInvalid = false;

        /// <summary>
        /// 目标FPS,运行动态设置以动态更改帧率
        /// </summary>
        public int TargetFPS
        {
            get;
            set;
        }

        /// <summary>
        /// 目标FPS对应的字节形式
        /// </summary>
        private byte[] TargetFPSBytes
        {
            get => BitConverter.GetBytes(TargetFPS);
        }

        /// <summary>
        /// 构造一个新的 <see cref="Unlocker"/> 对象，
        /// 每个解锁器只能解锁一次原神的进程，
        /// 再次解锁需要重新创建对象
        /// <para/>
        /// 解锁器需要在管理员模式下才能正确的完成解锁操作，
        /// 非管理员模式不能解锁
        /// </summary>
        /// <param name="gameProcess">游戏进程</param>
        /// <param name="targetFPS">目标fps</param>
        public Unlocker(Process gameProcess, int targetFPS)
        {
            if (!Environment.Is64BitProcess)
            {
                throw new InvalidOperationException("无法在32位进程中使用 Unlocker");
            }

            if (targetFPS < 30 || targetFPS > 2000)
            {
                throw new ArgumentOutOfRangeException(nameof(targetFPS));
            }

            TargetFPS = targetFPS;
            this.gameProcess = gameProcess;
        }

        /// <summary>
        /// 启动进程，然后调用<see cref="UnlockAsync(int, int, int)"/>
        /// </summary>
        /// <param name="findModuleMillisecondsDelay">每次查找UnityPlayer的延时,默认100毫秒</param>
        /// <param name="findModuleTimeMillisecondLimit">查找UnityPlayer的最大阈值,默认10000毫秒</param>
        /// <param name="adjustFpsMillisecondsDelay">每次循环调整的间隔时间，默认2000毫秒</param>
        /// <returns>解锁的结果</returns>
        public async Task<UnlockResult> StartProcessAndUnlockAsync(int findModuleMillisecondsDelay = 100, int findModuleTimeMillisecondLimit = 10000, int adjustFpsMillisecondsDelay = 2000)
        {
            bool result = gameProcess.Start();
            if (result)
            {
                return await UnlockAsync(findModuleMillisecondsDelay, findModuleTimeMillisecondLimit, adjustFpsMillisecondsDelay)
                    .ConfigureAwait(false);
            }
            else
            {
                return UnlockResult.ProcessStartFailed;
            }
        }

        /// <summary>
        /// 异步的解锁原神进程的帧数限制，
        /// 只调整了fps，并没有调整垂直同步限制，需要用户手动关闭，
        /// 调用不会阻止，直到遇到错误或原神进程结束前不会返回
        /// <para/>
        /// 应在 async void 方法内使用此方法，以使调用等待不影响UI线程
        /// <para/>
        /// 用法
        /// <code>
        /// Process p = new(){...};
        /// Unlocker unlocker = new(p,144);
        /// p.Start();
        /// var result = await unlocker.UnlockAsync();
        /// </code>
        /// </summary>
        /// <param name="findModuleMillisecondsDelay">每次查找UnityPlayer的延时,默认100毫秒</param>
        /// <param name="findModuleTimeMillisecondsLimit">查找UnityPlayer的最大阈值,默认10000毫秒</param>
        /// <param name="adjustFpsMillisecondsDelay">每次循环调整的间隔时间，默认2000毫秒</param>
        /// <returns>解锁的结果</returns>
        public async Task<UnlockResult> UnlockAsync(int findModuleMillisecondsDelay = 100, int findModuleTimeMillisecondsLimit = 10000, int adjustFpsMillisecondsDelay = 2000)
        {
            if (isInvalid)
            {
                return UnlockResult.UnlockerInvalid;
            }
            if (gameProcess.HasExited)
            {
                return UnlockResult.ProcessHasExited;
            }

            MODULEENTRY32? module;
            module = await FindModuleContinuouslyAsync(findModuleMillisecondsDelay, findModuleTimeMillisecondsLimit)
                .ConfigureAwait(false);

            if (module is null)
            {
                return UnlockResult.ModuleSearchTimeExceed;
            }
            if (gameProcess.HasExited)
            {
                return UnlockResult.ProcessHasExited;
            }

            MODULEENTRY32 unityPlayer = module.Value;
            byte[] image = new byte[unityPlayer.modBaseSize];
            // Read UnityPlayer.dll
            bool readOk = ReadProcessMemory(gameProcess.Handle, unityPlayer.modBaseAddr, image, unityPlayer.modBaseSize, out _);

            if (!readOk)
            {
                return UnlockResult.ReadProcessMemoryFailed;
            }

            // Find FPS offset
            // 7F 0F              jg   0x11
            // 8B 05 ? ? ? ?      mov eax, dword ptr[rip+?]
            uint? adr = SearchPattern(image, new byte[] { 0x7F, 0x0F, 0x8B, 0x05, 0x2A, 0x2A, 0x2A, 0x2A });

            if (adr is null)
            {
                return UnlockResult.NoMatchedPatternFound;
            }

            CalculateFPSOffset(unityPlayer, image, adr.Value);

            while (true)
            {
                if (!gameProcess.HasExited && fpsOffset != UIntPtr.Zero)
                {
                    WriteProcessMemory(gameProcess.Handle, fpsOffset, TargetFPSBytes, sizeof(int), out _);
                }
                else
                {
                    isInvalid = true;
                    fpsOffset = UIntPtr.Zero;
                    return UnlockResult.Ok;
                }
                await Task.Delay(adjustFpsMillisecondsDelay)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 计算FPS偏移量
        /// </summary>
        /// <param name="unityPlayer">UnityPlayer的模块信息</param>
        /// <param name="image">游戏进程镜像</param>
        /// <param name="adr">adr 指令偏移</param>
        private void CalculateFPSOffset(MODULEENTRY32 unityPlayer, byte[] image, uint adr)
        {
            uint rip = adr + 2;
            uint rel = BitConverter.ToUInt32(image, Convert.ToInt32(rip + 2));
            uint ofs = rip + rel + 6;
            fpsOffset = (UIntPtr)((long)unityPlayer.modBaseAddr + ofs);
        }

        /// <summary>
        /// 循环查找UnityPlayer Module
        /// 调用前需要确保 <see cref="gameProcess"/> 不为 null
        /// </summary>
        /// <param name="findModuleMillisecondsDelay">延迟</param>
        /// <param name="findModuleTimeMillisecondsLimit">上限</param>
        /// <returns>模块</returns>
        private async Task<MODULEENTRY32?> FindModuleContinuouslyAsync(int findModuleMillisecondsDelay, int findModuleTimeMillisecondsLimit)
        {
            MODULEENTRY32? module;
            Stopwatch watch = Stopwatch.StartNew();
            TimeSpan timeLimit = TimeSpan.FromMilliseconds(findModuleTimeMillisecondsLimit);

            //gameProcess 实际上可能为 null
            while ((module = FindModule(gameProcess.Id, "UnityPlayer.dll")) is null)
            {
                if (watch.Elapsed > timeLimit)
                {
                    break;
                }
                await Task.Delay(findModuleMillisecondsDelay)
                    .ConfigureAwait(false);
            }
            watch.Stop();
            return module;
        }

        /// <summary>
        /// 特征搜索
        /// </summary>
        /// <param name="source">待搜索的源数据</param>
        /// <param name="pattern">使用 0x2A 作为通配符</param>
        /// <returns>偏移量</returns>
        private uint? SearchPattern(byte[] source, byte[] pattern)
        {
            uint startAddr = 0;
            uint endAddr = startAddr + (uint)source.Length - (uint)pattern.Length;

            while (startAddr < endAddr)
            {
                bool found = true;
                for (uint i = 0; i < pattern.Length; i++)
                {
                    byte code = source[startAddr + i];
                    if (pattern[i] != 0x2A && pattern[i] != code)
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    return startAddr;
                }

                startAddr++;
            }

            return null;
        }

        /// <summary>
        /// 在进程中查找对应名称的模块
        /// 在找到模块或创建快照失败后立即返回
        /// </summary>
        /// <param name="processId">进程id</param>
        /// <param name="moduleName">模块名称</param>
        /// <returns>模块</returns>
        private MODULEENTRY32? FindModule(int processId, string moduleName)
        {
            IntPtr snapshot = CreateToolhelp32Snapshot(SnapshotFlags.Module, (uint)processId);
            //Snapshot cannot be created.
            if (Marshal.GetLastWin32Error() != 0)
            {
                return null;
            }

            MODULEENTRY32 entry = new()
            {
                dwSize = Marshal.SizeOf(typeof(MODULEENTRY32))
            };
            //First module must be exe. Ignoring it.
            for (Module32First(snapshot, ref entry); Module32Next(snapshot, ref entry);)
            {
                if (entry.th32ProcessID == processId && entry.szModule == moduleName)
                {
                    CloseHandle(snapshot);
                    return entry;
                }
            }
            CloseHandle(snapshot);
            return null;
        }
    }
}