using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TSBindDotNet;

public class SourceFileClass : SourceFileType
{
    public ClassDeclarationSyntax ClassDeclaration;

    public SourceFileClass(FileInfo sourceFile, ClassDeclarationSyntax declaration)
        : base(sourceFile, declaration)
    {
        ClassDeclaration = declaration;
    }
}
