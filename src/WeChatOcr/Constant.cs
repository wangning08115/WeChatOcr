namespace WeChatOcr;

public class Constant
{
    /// <summary>
    ///     内部OCR数据目录
    /// </summary>
    public const string WeChatOcrData = ".\\wco_data";

#if WIN32
    public const string MojoDllName = $"{Constant.WeChatOcrData}\\mmmojo.dll";
#else
    public const string MojoDllName = $"{Constant.WeChatOcrData}\\mmmojo_64.dll";
#endif
}
