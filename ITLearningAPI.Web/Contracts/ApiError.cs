using System.Runtime.CompilerServices;

namespace ITLearningAPI.Web.Contracts;

public class ApiError
{
    public string ErrorMessage { get; set; }

    public static ApiError WithParameterException(string arg,
        [CallerArgumentExpression("arg")] string parameterName = "param")
    {
        return new ApiError
        {
            ErrorMessage = $"Invalid parameter! Parameter name {parameterName} - Parameter value {arg}"
        };
    }
}