using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TSBindDotNet;

public class SourceFileAttribute
{
    public string Name;
    public List<SourceFileAttributeArgument> Arguments = new();

    public SourceFileAttribute(AttributeSyntax attribute)
    {
        Name = attribute.Name.ToString();

        if (attribute.ArgumentList != null && attribute.ArgumentList.Arguments.Count > 0)
        {
            var argumentIndex = 0;

            foreach (var argument in attribute.ArgumentList.Arguments)
            {
                Arguments.Add(new(argumentIndex++, argument.NameEquals?.Name.Identifier.Text ?? null, argument.Expression.ToString()));
            }
        }
    }

    public override string ToString()
    {
        return Name;
    }
}

public class SourceFileAttributeArgument
{
    public int Index;
    public string? Name;
    public string Value;

    public SourceFileAttributeArgument(int index, string? name, string value)
    {
        Index = index;
        Name = name;
        Value = value;
    }

    public override string ToString()
    {
        return Name ?? $"Positional {Index}";
    }
}
