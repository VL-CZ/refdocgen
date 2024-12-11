using RefDocGen.CodeElements.Abstract.Members;
using RefDocGen.TemplateGenerators.Default.TemplateModels.Members;

namespace RefDocGen.TemplateGenerators.Default.TemplateModelCreators;

internal class ExceptionTMCreator
{
    /// <summary>
    /// Creates a <see cref="ExceptionTM"/> instance based on the provided <see cref="IExceptionData"/> object.
    /// </summary>
    /// <param name="exception">The <see cref="IExceptionData"/> instance representing the exception.</param>
    /// <returns>A <see cref="ExceptionTM"/> instance based on the provided <paramref name="exception"/>.</returns>
    internal static ExceptionTM GetFrom(IExceptionData exception)
    {
        return new ExceptionTM(exception.Name, exception.DocComment.Value);
    }
}
