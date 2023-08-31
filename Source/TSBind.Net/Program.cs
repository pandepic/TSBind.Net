using System.Linq.Expressions;
using TSBindDotNet;

#if !DEBUG
try
{
#endif
    var programArgs = new ProgramArgs(args);
    var configPath = "tsbinddotnet.config.json";

    if (File.Exists(configPath))
        programArgs.LoadConfigFile(configPath);

    var errors = ProgramArgsValidator.Validate(programArgs);

    foreach (var error in errors)
        Console.WriteLine($"ERROR: {error}");

    if (errors.Count > 0)
    {
        if (programArgs.Config.WaitForKey ?? false)
            Console.ReadKey();

        return;
    }

    try
    {
        var outputBuilder = SourceCodeParser.GenerateTS(
            programArgs.Config.Inputs,
            programArgs.Config.GeneralTemplatePath,
            programArgs.Config.APIControllerTemplatePath,
            programArgs.Config.APIEndpointTemplatePath);

        foreach (var output in programArgs.Config.Outputs)
            File.WriteAllText(output, outputBuilder.ToString());
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR: {ex.Message}");

        if (programArgs.Config.WaitForKey ?? false)
            Console.ReadKey();

        return;
    }

    Console.WriteLine("Done! Press any key to exit.");

    if (programArgs.Config.WaitForKey ?? false)
        Console.ReadKey();

#if !DEBUG
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
#endif