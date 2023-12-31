using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TSBindDotNet;

public class SourceFileTypeMethod
{
    public string Name;
    public string ReturnTypeName;
    public MethodDeclarationSyntax MethodDeclaration;

    public List<SourceFileAttribute> Attributes = new();
    public List<string> ParameterTypes = new();

    public SourceFileTypeMethod(MethodDeclarationSyntax declaration)
    {
        MethodDeclaration = declaration;
        Name = MethodDeclaration.Identifier.ValueText;
        ReturnTypeName = MethodDeclaration.ReturnType.ToString();

        Attributes = RoslynUtil.GetAttributes(declaration.AttributeLists);

        foreach (var parameter in MethodDeclaration.ParameterList.Parameters)
        {
            if (parameter == null || parameter.Type == null)
                continue;

            ParameterTypes.AddIfNotContains(parameter.Type.ToString());
        }
    }

    public override string ToString()
    {
        return Name;
    }
}
