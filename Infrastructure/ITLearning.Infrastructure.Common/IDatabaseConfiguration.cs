namespace ITLearning.Infrastructure.DataAccess.Common;

internal interface IDatabaseConfiguration
{
    public string ConnectionString { get; }
}