using RefDocGen.MemberData;

namespace RefDocGen.TemplateGenerators;

public interface ITemplateGenerator
{
    void GenerateTemplates(ClassData[] classes);
}
