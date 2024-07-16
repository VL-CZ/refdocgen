using RefDocGen.MemberData;
using RefDocGen.TemplateGenerators.Default.TemplateModels;

namespace RefDocGen.TemplateGenerators.Default;

public class DefaultTemplateGenerator : RazorTemplateGenerator<ClassTemplateModel>
{
    private readonly DefaultTemplateModelBuilder templateModelBuilder = new();

    public DefaultTemplateGenerator(string projectPath, string templatePath, string outputDir) : base(projectPath, templatePath, outputDir)
    {
    }

    protected override IEnumerable<ClassTemplateModel> GetTemplateModels(ClassData[] classes)
    {
        return classes.Select(templateModelBuilder.CreateClassTemplateModel);
    }
}
