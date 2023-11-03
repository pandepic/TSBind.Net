using System.Linq.Expressions;
using TSBindDotNet;

#if !DEBUG
try
{
#endif
    var programArgs = new ProgramArgs(args);

    if (!programArgs.Run)
    {
        if (programArgs.FirstConfig.WaitForKey ?? false)
            Console.ReadKey();

        return;
    }

    var configPath = "tsbinddotnet.json";

    if (File.Exists(configPath))
        programArgs.LoadConfigFile(configPath);

#if DEBUG
    programArgs.FirstConfig.WaitForKey = true;
#endif

    var errors = ProgramArgsValidator.Validate(programArgs);

    foreach (var error in errors)
        Console.WriteLine($"ERROR: {error}");

    if (errors.Count > 0)
    {
        if (programArgs.FirstConfig.WaitForKey ?? false)
            Console.ReadKey();

        return;
    }

    try
    {
        foreach (var config in programArgs.Configs)
        {
            var outputBuilder = SourceCodeParser.GenerateTS(
                config.Inputs,
                config.IncludeTypes,
                config.GeneralTemplatePath,
                config.APIControllerTemplatePath,
                config.APIEndpointTemplatePath);

            foreach (var output in config.Outputs)
            {
                File.WriteAllText(output, outputBuilder.ToString());
                Console.WriteLine($"Successfully wrote output to {output}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR: {ex.Message}");

        if (programArgs.FirstConfig.WaitForKey ?? false)
            Console.ReadKey();

        return;
    }

    Console.WriteLine("Done! Press any key to exit.");

    if (programArgs.FirstConfig.WaitForKey ?? false)
        Console.ReadKey();

#if !DEBUG
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
#endif