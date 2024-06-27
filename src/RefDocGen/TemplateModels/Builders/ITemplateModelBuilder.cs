using RefDocGen.Intermed;

namespace RefDocGen.TemplateModels.Builders;

internal interface ITemplateModelBuilder
{
    ClassTemplateModel CreateClassTemplateModel(ClassIntermed classIntermed);
}
