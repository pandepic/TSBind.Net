using System.Reflection.Metadata;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TSBindDotNet;

public class SourceFileRecord : SourceFileType
{
    public RecordDeclarationSyntax RecordDeclaration;

    public SourceFileRecord(FileInfo sourceFile, RecordDeclarationSyntax declaration)
        : base(sourceFile, declaration)
    {
        RecordDeclaration = declaration;

        if (RecordDeclaration.ParameterList != null)
        {
            foreach (var recordParam in RecordDeclaration.ParameterList.Parameters)
            {
                if (recordParam == null || recordParam.Type == null)
                    continue;

                var name = recordParam.Identifier.ValueText;
                var type = recordParam.Type.ToString();

                Properties.Add(new(name, type));
            }
        }
    }
}
