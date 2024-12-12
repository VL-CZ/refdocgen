using RefDocGen.CodeElements.Abstract.Types.Exception;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

namespace RefDocGen.TemplateGenerators.Default.TemplateModelCreators;

internal class ExceptionTMCreator
{
    /// <summary>
    /// Creates a <see cref="ExceptionTM"/> instance based on the provided <see cref="IExceptionDocumentation"/> object.
    /// </summary>
    /// <param name="exception">The <see cref="IExceptionDocumentation"/> instance representing the exception.</param>
    /// <returns>A <see cref="ExceptionTM"/> instance based on the provided <paramref name="exception"/>.</returns>
    internal static ExceptionTM GetFrom(IExceptionDocumentation exception)
    {
        return new ExceptionTM(exception.TypeName, exception.DocComment.Value);
    }
}
