namespace TSBindDotNet;

public static class ProgramArgsValidator
{
    public static List<string> Validate(ProgramArgs settings)
    {
        var errors = new List<string>();

        if (settings.Config.Inputs.Count == 0)
            errors.Add("You must provide at least one input with --Input");
        if (settings.Config.Outputs.Count == 0)
            errors.Add("You must provide at least one output with --Output");

        if (string.IsNullOrEmpty(settings.Config.APIControllerTemplatePath))
            errors.Add("You must provide a path to an API controller template file with --APIControllerTemplate");
        if (string.IsNullOrEmpty(settings.Config.APIControllerTemplatePath))
            errors.Add("You must provide a path to an API endpoint template file with --APIEndpointTemplate");

        foreach (var input in settings.Config.Inputs)
        {
            if (!Directory.Exists(input))
                errors.Add($"Can't find input path at {input}");
        }

        foreach (var output in settings.Config.Outputs)
        {
            var outputFile = new FileInfo(output);

            if (outputFile.Directory == null || !outputFile.Directory.Exists)
                errors.Add($"Can't find output path at {output}");
        }

        if (!string.IsNullOrEmpty(settings.Config.GeneralTemplatePath) && !File.Exists(settings.Config.GeneralTemplatePath))
            errors.Add($"Can't find general template path at {settings.Config.GeneralTemplatePath}");
        if (!string.IsNullOrEmpty(settings.Config.APIControllerTemplatePath) && !File.Exists(settings.Config.APIControllerTemplatePath))
            errors.Add($"Can't find API controller template path at {settings.Config.APIControllerTemplatePath}");
        if (!string.IsNullOrEmpty(settings.Config.APIEndpointTemplatePath) && !File.Exists(settings.Config.APIEndpointTemplatePath))
            errors.Add($"Can't find API endpoint template path at {settings.Config.APIEndpointTemplatePath}");

        return errors;
    }
}
