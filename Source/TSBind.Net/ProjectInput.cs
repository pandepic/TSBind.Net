using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TSBindDotNet;

public class ProjectInput
{
    public DirectoryInfo BaseDirectory;
    public FileInfo ProjectFile;

    public List<FileInfo> SourceFiles = new();

    public List<SourceFileClass> SourceClasses = new();
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
        SourceClasses.Clear();
        SourceEnums.Clear();

        foreach (var file in SourceFiles)
        {
            var code = File.ReadAllText(file.FullName);

            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetCompilationUnitRoot();

            foreach (var classDeclaration in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
                SourceClasses.Add(new(file, classDeclaration));

            foreach (var enumDeclaration in root.DescendantNodes().OfType<EnumDeclarationSyntax>())
                SourceEnums.Add(new(file, enumDeclaration));
        }
    }
}
