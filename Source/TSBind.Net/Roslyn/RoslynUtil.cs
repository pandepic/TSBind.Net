using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;

namespace TSBindDotNet
{
    public static class RoslynUtil
    {
        public static List<SourceFileAttribute> GetAttributes(SyntaxList<AttributeListSyntax> attributeLists)
        {
            var attributes = new List<SourceFileAttribute>();

            foreach (var attributeList in attributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                    attributes.Add(new SourceFileAttribute(attribute));
            }

            return attributes;
        }
    }
}
