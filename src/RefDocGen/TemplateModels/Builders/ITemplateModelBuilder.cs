using RefDocGen.MemberData;

namespace RefDocGen.TemplateModels.Builders;

internal interface ITemplateModelBuilder
{
    ClassTemplateModel CreateClassTemplateModel(ClassData classIntermed);
}
