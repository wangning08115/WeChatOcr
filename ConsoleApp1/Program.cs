using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatOcr;
using WeChatOcr.Sample;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            string str = Test();
            sw.Stop();
            Console.WriteLine(str);
            Console.WriteLine($"{sw.ElapsedMilliseconds}");
            Console.ReadLine();
        }

        public static string Test()
        {

            var _tcs = new TaskCompletionSource<string>();
            try
            {
                var bitmap = (Bitmap)Image.FromFile(@$"{AppDomain.CurrentDomain.BaseDirectory}ct图片pdf_01(1).jpg");
                var bytes = ImageUtilities.ConvertBitmap2Bytes(bitmap, ImageFormat.Jpeg);

                using var ocr = new ImageOcr("");
                ocr.Run(bytes, (path, result) =>
                {
                    try
                    {
                        if (result == null) return;
                        var list = result?.OcrResult?.SingleResult;
                        if (list == null)
                        {
                            //避免重复set
                            _tcs.SetResult("WeChatOCR get result is null");
                            return;
                        }


                        var sb = new StringBuilder();
                        for (var i = 0; i < list?.Count; i++)
                        {
                            if (list[i] is not { } item || string.IsNullOrEmpty(item.SingleStrUtf8))
                                continue;

                            sb.AppendLine(item.SingleStrUtf8);
                        }

                        try
                        {
                            if (System.IO.File.Exists(path))
                                System.IO.File.Delete(path);
                        }
                        catch
                        {
                            // ignore
                        }

                        _tcs.SetResult(sb.ToString());
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                    }
                });

                var timeoutTask = Task.Delay(3000);
                var completedTask = Task.WhenAny(_tcs.Task, timeoutTask);

                if (completedTask == timeoutTask)
                {
                    _tcs.SetCanceled();
                    throw new TimeoutException("WeChatOCR operation timed out.");
                }
                // 提取content的值
                var finalResult = _tcs.Task;

                var restult = finalResult.Result;
                return restult;
            }
            catch (Exception ex)
            {

            }
            return "";
        }
    }
}




