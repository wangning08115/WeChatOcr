using NHotkey.Wpf;
using NHotkey;
using ScreenGrab;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Input;
using ScreenGrab.Extensions;
using Wpf.Ui.Tray.Controls;
using System.Windows.Media.Imaging;
using System.Text;

namespace WeChatOcr.Sample;


public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Capture()
    {
        Clean();

        ScreenGrabber.OnCaptured = Captured;
        ScreenGrabber.Capture(AuxiliaryCb.IsChecked ?? false);
    }

    private void Captured(Bitmap bitmap)
    {
        Img.Source = bitmap.ToImageSource();
        if (WindowState != WindowState.Normal)
            WindowState = WindowState.Normal;
        if (!IsVisible)
            Show();
        Activate();

        var tcs = new TaskCompletionSource<string>();
        try
        {
            var bytes = ImageUtilities.ConvertBitmap2Bytes(bitmap, ImageFormat.Png);
            using var ocr = new ImageOcr(WeChatPathTb.Text.Trim());
            ocr.Run(bytes, (path, result) =>
            {
                if (result == null) return;
                var list = result?.OcrResult?.SingleResult;
                if (list == null)
                    tcs.SetResult("WeChatOCR get result is null");


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

                tcs.SetResult(sb.ToString());
            });

            var timeoutTask = Task.Delay(10000);
            var completedTask = Task.WhenAny(tcs.Task, timeoutTask);

            if (completedTask == timeoutTask)
            {
                throw new TimeoutException("WeChatOCR operation timed out.");
            }
            // 提取content的值
            var finalResult = tcs.Task;

            ResultTb.Text = finalResult.Result;
        }
        catch (Exception ex)
        {
            ResultTb.Text = ex.Message;
        }
    }

    private void Capture(object? sender, HotkeyEventArgs e)
    {
        Capture();
    }

    private void Capture_Click(object sender, RoutedEventArgs e)
    {
        Capture();
    }

    private void Clean_Click(object sender, RoutedEventArgs e)
    {
        Clean();
    }

    private void Clean()
    {
        Img.Source?.Freeze();
        Img.Source = null;
        GC.Collect();
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        HotkeyManager.Current.AddOrReplace("Capture", Key.A, ModifierKeys.Windows | ModifierKeys.Shift, Capture);
    }

    private void MainWindow_OnUnloaded(object sender, RoutedEventArgs e)
    {
        HotkeyManager.Current.Remove("Capture");
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }

    private void NotifyIcon_OnLeftClick(NotifyIcon sender, RoutedEventArgs e)
    {
        Show();
        Activate();
    }

    private void MenuItem_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}