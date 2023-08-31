using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TSBindDotNet;

public class SourceFileClass
{
    public FileInfo SourceFile;
    public string Name;
    public ClassDeclarationSyntax ClassDeclaration;

    public List<SourceFileClassMethod> ClassMethods = new();
    public List<SourceFileAttribute> Attributes = new();
    public List<SourceFileClassField> Fields = new();
    public List<SourceFileClassProperty> Properties = new();

    public SourceFileClass(FileInfo sourceFile, ClassDeclarationSyntax declaration)
    {
        SourceFile = sourceFile;
        ClassDeclaration = declaration;
        Name = ClassDeclaration.Identifier.ValueText;

        Attributes = RoslynUtil.GetAttributes(declaration.AttributeLists);

        foreach (var methodDeclaration in ClassDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>())
            ClassMethods.Add(new(methodDeclaration));

        foreach (var fieldDeclaration in ClassDeclaration.DescendantNodes().OfType<FieldDeclarationSyntax>())
        {
            var type = fieldDeclaration.Declaration.Type.ToString();

            foreach (VariableDeclaratorSyntax variable in fieldDeclaration.Declaration.Variables)
            {
                var name = variable.Identifier.ValueText;
                Fields.Add(new(name, type));
            }
        }

        foreach (var propertyDeclaration in ClassDeclaration.DescendantNodes().OfType<PropertyDeclarationSyntax>())
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
