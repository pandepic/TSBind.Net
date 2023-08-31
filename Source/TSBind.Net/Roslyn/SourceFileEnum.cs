using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TSBindDotNet;

public class SourceFileEnum
{
    public FileInfo SourceFile;
    public string Name;
    public string TypeName;
    public EnumDeclarationSyntax EnumDeclaration;

    public List<SourceFileEnumMember> Members = new();

    public SourceFileEnum(FileInfo sourceFile, EnumDeclarationSyntax declaration)
    {
        SourceFile = sourceFile;
        EnumDeclaration = declaration;
        Name = EnumDeclaration.Identifier.ValueText;

        TypeName = "int";

        if (EnumDeclaration.BaseList != null)
        {
            var type = EnumDeclaration.BaseList.Types.FirstOrDefault();

            if (type != null)
                TypeName = type.Type.ToString();
        }

        foreach (var member in EnumDeclaration.Members)
        {
            var autoValue = 0;

            if (Members.Count > 0)
                autoValue = int.Parse(Members.Last().Value) + 1;

            var name = member.Identifier.ValueText;
            var value = autoValue.ToString();

            if (member.EqualsValue != null)
                value = member.EqualsValue.Value.ToString();

            Members.Add(new(name, value));
        }
    }

    public override string ToString()
    {
        return Name;
    }
}
