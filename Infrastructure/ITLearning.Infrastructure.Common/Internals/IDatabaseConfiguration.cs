namespace ITLearning.Infrastructure.DataAccess.Common.Internals;

internal interface IDatabaseConfiguration
{
    public string ConnectionString { get; }
}