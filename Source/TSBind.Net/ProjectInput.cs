using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TSBindDotNet;

public class ProjectInput
{
    public DirectoryInfo BaseDirectory;
    public FileInfo ProjectFile;

    public List<FileInfo> SourceFiles = new();

    public List<SourceFileType> SourceTypes = new();
    public List<SourceFileEnum> SourceEnums = new();

    public ProjectInput(string path)
    {
        BaseDirectory = new DirectoryInfo(path);

        var projectFile = BaseDirectory.GetFiles().Where(f => f.Name.EndsWith(".csproj", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        if (projectFile == null)
            throw new Exception();

        ProjectFile = projectFile;
        SourceFiles.AddRange(BaseDirectory.GetFiles("*.cs", searchOption: SearchOption.AllDirectories));

        Parse();
    }

    public void Parse()
    {
        SourceTypes.Clear();
        SourceEnums.Clear();

        foreach (var file in SourceFiles)
        {
            var code = File.ReadAllText(file.FullName);

            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetCompilationUnitRoot();

            foreach (var classDeclaration in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
                SourceTypes.Add(new SourceFileClass(file, classDeclaration));

            foreach (var recordDeclaration in root.DescendantNodes().OfType<RecordDeclarationSyntax>())
                SourceTypes.Add(new SourceFileRecord(file, recordDeclaration));

            foreach (var enumDeclaration in root.DescendantNodes().OfType<EnumDeclarationSyntax>())
                SourceEnums.Add(new(file, enumDeclaration));
        }
    }
}
