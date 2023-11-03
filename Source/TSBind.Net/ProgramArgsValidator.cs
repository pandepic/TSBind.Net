namespace TSBindDotNet;

public static class ProgramArgsValidator
{
    public static List<string> Validate(ProgramArgs settings)
    {
        var errors = new List<string>();

        foreach (var config in settings.Configs)
        {
            if (config.Inputs.Count == 0)
                errors.Add("You must provide at least one input with --Input");
            if (config.Outputs.Count == 0)
                errors.Add("You must provide at least one output with --Output");

            if (string.IsNullOrEmpty(config.APIControllerTemplatePath))
                errors.Add("You must provide a path to an API controller template file with --APIControllerTemplate");
            if (string.IsNullOrEmpty(config.APIControllerTemplatePath))
                errors.Add("You must provide a path to an API endpoint template file with --APIEndpointTemplate");

            foreach (var input in config.Inputs)
            {
                if (!Directory.Exists(input))
                    errors.Add($"Can't find input path at {input}");
            }

            foreach (var output in config.Outputs)
            {
                var outputFile = new FileInfo(output);

                if (outputFile.Directory == null || !outputFile.Directory.Exists)
                    errors.Add($"Can't find output path at {output}");
            }

            if (!string.IsNullOrEmpty(config.GeneralTemplatePath) &&
                !File.Exists(config.GeneralTemplatePath))
                errors.Add($"Can't find general template path at {config.GeneralTemplatePath}");
            if (!string.IsNullOrEmpty(config.APIControllerTemplatePath) &&
                !File.Exists(config.APIControllerTemplatePath))
                errors.Add($"Can't find API controller template path at {config.APIControllerTemplatePath}");
            if (!string.IsNullOrEmpty(config.APIEndpointTemplatePath) &&
                !File.Exists(config.APIEndpointTemplatePath))
                errors.Add($"Can't find API endpoint template path at {config.APIEndpointTemplatePath}");
        }

        return errors;
    }
}
