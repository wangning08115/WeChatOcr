using System.Runtime.InteropServices;

namespace WeChatOcr;

public enum MMMojoInfoMethod
{
    kMMNone = 0,
    kMMPush,
    kMMPullReq,
    kMMPullResp,
    kMMShared
}

public enum MMMojoCallbackType
{
    kMMUserData = 0,
    kMMReadPush,
    kMMReadPull,
    kMMReadShared,
    kMMRemoteConnect,
    kMMRemoteDisconnect,
    kMMRemoteProcessLaunched,
    kMMRemoteProcessLaunchFailed,
    kMMRemoteMojoError
}

public enum MMMojoEnvironmentInitParamType
{
    kMMHostProcess = 0,
    kMMLoopStartThread,
    kMMExePath,
    kMMLogPath,
    kMMLogToStderr,
    kMMAddNumMessagepipe,
    kMMSetDisconnectHandlers,
    kMMDisableDefaultPolicy = 1000,
    kMMElevated,
    kMMCompatible
}

public class MmmojoDll
{
    public MmmojoDll()
    {
    }

    // Function Definitions (P/Invoke declarations)
    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void InitializeMMMojo(int argc, IntPtr argv);

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void ShutdownMMMojo();

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr CreateMMMojoEnvironment();

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetMMMojoEnvironmentCallbacks(IntPtr mmmojo_env, int type, IntPtr callback);

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern void SetMMMojoEnvironmentInitParams(IntPtr mmmojo_env, int type, IntPtr param);

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    public static extern void AppendMMSubProcessSwitchNative(IntPtr mmmojo_env, IntPtr switchStringPtr, IntPtr valuePtr);

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void StartMMMojoEnvironment(IntPtr mmmojo_env);

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void StopMMMojoEnvironment(IntPtr mmmojo_env);

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void RemoveMMMojoEnvironment(IntPtr mmmojo_env);

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetMMMojoReadInfoRequest(IntPtr mmmojo_readinfo, ref uint requestDataSize);

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetMMMojoReadInfoAttach(IntPtr mmmojo_readinfo, ref uint attachDataSize);

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void RemoveMMMojoReadInfo(IntPtr mmmojo_readinfo);

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern int GetMMMojoReadInfoMethod(IntPtr mmmojo_readinfo);

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool GetMMMojoReadInfoSync(IntPtr mmmojo_readinfo);

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr CreateMMMojoWriteInfo(int method, int sync, uint requestId);

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetMMMojoWriteInfoRequest(IntPtr mmmojo_writeinfo, uint requestDataSize);

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void RemoveMMMojoWriteInfo(IntPtr mmmojo_writeinfo);

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr GetMMMojoWriteInfoAttach(IntPtr mmmojo_writeinfo, uint attachDataSize);

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetMMMojoWriteInfoMessagePipe(IntPtr mmmojo_writeinfo, int numOfMessagePipe);

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl)]
    public static extern void SetMMMojoWriteInfoResponseSync(IntPtr mmmojo_writeinfo, ref IntPtr mmmojo_readinfo);

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool SendMMMojoWriteInfo(IntPtr mmmojo_env, IntPtr mmmojo_writeinfo);

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool SwapMMMojoWriteInfoCallback(IntPtr mmmojo_writeinfo, IntPtr mmmojo_readinfo);

    [DllImport(Constant.MojoDllName, CallingConvention = CallingConvention.Cdecl)]
    [return: MarshalAs(UnmanagedType.I1)]
    public static extern bool SwapMMMojoWriteInfoMessage(IntPtr mmmojo_writeinfo, IntPtr mmmojo_readinfo);
}
