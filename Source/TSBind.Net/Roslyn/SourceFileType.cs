using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TSBindDotNet;

public abstract class SourceFileType
{
    public TypeDeclarationSyntax TypeDeclaration;

    public FileInfo SourceFile;
    public string Name;

    public List<SourceFileTypeMethod> Methods = new();
    public List<SourceFileAttribute> Attributes = new();
    public List<SourceFileTypeField> Fields = new();
    public List<SourceFileTypeProperty> Properties = new();

    public SourceFileType(FileInfo sourceFile, TypeDeclarationSyntax declaration)
    {
        SourceFile = sourceFile;
        TypeDeclaration = declaration;
        Name = TypeDeclaration.Identifier.ValueText;

        Attributes = RoslynUtil.GetAttributes(declaration.AttributeLists);

        foreach (var methodDeclaration in TypeDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>())
            Methods.Add(new(methodDeclaration));

        foreach (var fieldDeclaration in TypeDeclaration.DescendantNodes().OfType<FieldDeclarationSyntax>())
        {
            var type = fieldDeclaration.Declaration.Type.ToString();

            foreach (VariableDeclaratorSyntax variable in fieldDeclaration.Declaration.Variables)
            {
                var name = variable.Identifier.ValueText;
                Fields.Add(new(name, type));
            }
        }

        foreach (var propertyDeclaration in TypeDeclaration.DescendantNodes().OfType<PropertyDeclarationSyntax>())
        {
            var name = propertyDeclaration.Identifier.ValueText;
            var type = propertyDeclaration.Type.ToString();
            Properties.Add(new(name, type));
        }
    }

    public override string ToString()
    {
        return Name;
    }
}
