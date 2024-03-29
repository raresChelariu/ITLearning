﻿using Microsoft.Data.SqlClient;

namespace ITLearning.Infrastructure.DataAccess.Mssql;

internal static class DataReaderExtensions
{
    public static T GetFromColumn<T>(this SqlDataReader reader, string columnName)
    {
        if (reader != null && reader[columnName] != DBNull.Value)
        {
            return (T)reader[columnName];
        }

        return default;
    }
}