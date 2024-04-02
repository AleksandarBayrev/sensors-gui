using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace sensors_gui.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
    private string _cpuTemp = "";
    public string CPUTemp => _cpuTemp;

    public MainWindowViewModel()
    {
        Task.Run(async () =>
        {
            while (true)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = "/usr/bin/sensors", UseShellExecute = false, RedirectStandardOutput = true };
                var proc = new Process() { StartInfo = startInfo, };
                var started = proc.Start();
                try
                {
                    if (started)
                    {
                        proc.BeginOutputReadLine();
                        using (var outputStream = proc.StandardOutput)
                        {
                            var data = outputStream.ReadToEnd();
                            if (data != null)
                            {
                                _cpuTemp = data;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] => {ex.Message}");
                }
                await Task.Delay(1000);
            }
        });
    }
#pragma warning restore CA1822 // Mark members as static
}
