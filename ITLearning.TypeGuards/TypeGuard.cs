using System.Runtime.CompilerServices;

namespace ITLearning.TypeGuards;

public static class TypeGuard
{
    public static T ThrowIfNull<T>(T arg, [CallerArgumentExpression("arg")] string parameterName = "param")
    {
        if (arg == null)
        {
            throw new ArgumentNullException($"{parameterName} is null");
        }

        return arg;
    }

    public static string ThrowIfStringIsNullOrWhitespace(string arg,
        [CallerArgumentExpression("arg")] string parameterName = "param")
    {
        if (!string.IsNullOrWhiteSpace(arg))
        {
            return arg;
        }

        throw new ArgumentException($"{parameterName} is null or whitespace string !");
    }

    public static int ThrowIfZeroOrNegative(int arg,
        [CallerArgumentExpression("arg")] string parameterName = "param")
    {
        if (arg > 0)
        {
            return arg;
        }

        throw new ArgumentException($"{parameterName} is zero or negative !");
    }
}