using System.Text.Json;

namespace TSBindDotNet;

public class ProgramArgsConfig
{
    public List<string> Inputs { get; set; } = new();
    public List<string> Outputs { get; set; } = new();
    public string GeneralTemplatePath { get; set; } = "";
    public string APIControllerTemplatePath { get; set; } = "";
    public string APIEndpointTemplatePath { get; set; } = "";
    public bool? WaitForKey { get; set; }
}

public class ProgramArgs
{
    public static string Version = "1.5.0";

    public ProgramArgsConfig Config = new();
    public bool Run = true;

    public ProgramArgs(string[] args)
    {
#if DEBUG
        Config.WaitForKey = true;
#endif

        var currentArgType = ArgType.Unknown;

        foreach (var str in args)
        {
            if (string.IsNullOrEmpty(str))
                continue;

            string arg = str;

            if (arg.StartsWith("--"))
                arg = str.Substring(2);

            if (Enum.TryParse<ArgType>(arg, true, out var newArg))
            {
                currentArgType = newArg;
                continue;
            }

            switch (currentArgType)
            {
                case ArgType.Version:
                case ArgType.V:
                    Run = false;
                    Console.WriteLine($"Version: {Version}");
                    break;

                case ArgType.Input:
                    Config.Inputs.Add(arg);
                    break;

                case ArgType.Output:
                    Config.Outputs.Add(arg);
                    break;

                case ArgType.GeneralTemplate:
                    Config.GeneralTemplatePath = arg;
                    break;

                case ArgType.APIControllerTemplate:
                    Config.APIControllerTemplatePath = arg;
                    break;

                case ArgType.APIEndpointTemplate:
                    Config.APIEndpointTemplatePath = arg;
                    break;

                case ArgType.WaitForKey:
                    Config.WaitForKey = true;
                    break;
            }
        }
    }

    public void LoadConfigFile(string filename)
    {
        Console.WriteLine($"Loading config from: {filename}");

        var configString = File.ReadAllText(filename);
        var config = JsonSerializer.Deserialize<ProgramArgsConfig>(configString);

        if (config != null)
        {
            if (!string.IsNullOrEmpty(config.GeneralTemplatePath))
                Config.GeneralTemplatePath = config.GeneralTemplatePath;
            if (!string.IsNullOrEmpty(config.APIControllerTemplatePath))
                Config.APIControllerTemplatePath = config.APIControllerTemplatePath;
            if (!string.IsNullOrEmpty(config.APIEndpointTemplatePath))
                Config.APIEndpointTemplatePath = config.APIEndpointTemplatePath;

            Config.Inputs.AddRange(config.Inputs);
            Config.Outputs.AddRange(config.Outputs);

            if (config.WaitForKey.HasValue)
                Config.WaitForKey = config.WaitForKey.Value;
        }
    }
}
