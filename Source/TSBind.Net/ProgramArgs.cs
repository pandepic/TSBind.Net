using System.Text.Json;

namespace TSBindDotNet;

public class ProgramArgsConfig
{
    public List<string> Inputs { get; set; } = new();
    public List<string> Outputs { get; set; } = new();
    public List<string> IncludeTypes { get; set; } = new();
    public string GeneralTemplatePath { get; set; } = "";
    public string APIControllerTemplatePath { get; set; } = "";
    public string APIEndpointTemplatePath { get; set; } = "";
    public bool? WaitForKey { get; set; }
}

public class ProgramArgs
{
    public static string Version = "1.9.1";

    public ProgramArgsConfig FirstConfig => Configs[0];
    
    public List<ProgramArgsConfig> Configs = new() { new() };
    public bool Run = true;

    public ProgramArgs(string[] args)
    {
#if DEBUG
        FirstConfig.WaitForKey = true;
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

                switch (currentArgType)
                {
                    case ArgType.Version:
                    case ArgType.V:
                        Run = false;
                        Console.WriteLine($"Version: {Version}");
                        return;
                }

                continue;
            }

            switch (currentArgType)
            {
                case ArgType.Input:
                    FirstConfig.Inputs.Add(arg);
                    break;

                case ArgType.Output:
                    FirstConfig.Outputs.Add(arg);
                    break;

                case ArgType.GeneralTemplate:
                    FirstConfig.GeneralTemplatePath = arg;
                    break;

                case ArgType.APIControllerTemplate:
                    FirstConfig.APIControllerTemplatePath = arg;
                    break;

                case ArgType.APIEndpointTemplate:
                    FirstConfig.APIEndpointTemplatePath = arg;
                    break;

                case ArgType.WaitForKey:
                    FirstConfig.WaitForKey = true;
                    break;

                case ArgType.IncludeTypes:
                    FirstConfig.IncludeTypes.Add(arg);
                    break;
            }
        }
    }

    public void LoadConfigFile(string path)
    {
        Console.WriteLine($"Loading config from: {path}");

        var configString = File.ReadAllText(path);
        var configs = JsonSerializer.Deserialize<List<ProgramArgsConfig>>(configString);

        if (configs == null || configs.Count == 0)
            return;

        for (var i = 0; i < configs.Count; i++)
        {
            var config = configs[i];
            
            if (i == 0)
            {
                if (!string.IsNullOrEmpty(config.GeneralTemplatePath))
                    FirstConfig.GeneralTemplatePath = config.GeneralTemplatePath;
                if (!string.IsNullOrEmpty(config.APIControllerTemplatePath))
                    FirstConfig.APIControllerTemplatePath = config.APIControllerTemplatePath;
                if (!string.IsNullOrEmpty(config.APIEndpointTemplatePath))
                    FirstConfig.APIEndpointTemplatePath = config.APIEndpointTemplatePath;

                FirstConfig.Inputs.AddRange(config.Inputs);
                FirstConfig.Outputs.AddRange(config.Outputs);
                FirstConfig.IncludeTypes.AddRange(config.IncludeTypes);

                if (config.WaitForKey.HasValue)
                    FirstConfig.WaitForKey = config.WaitForKey.Value;
            }
            else
            {
                Configs.Add(config);
            }
        }
    }
}
