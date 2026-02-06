using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace FacialRecognition.Common
{
    public static class FacialRecognitionDALConstants
    {
        private static readonly string _connectionString;
        public const int CommandTimeOut = 0;

 
        static FacialRecognitionDALConstants()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            configurationBuilder.AddJsonFile(path, optional: false, reloadOnChange: true);
            IConfiguration root = configurationBuilder.Build();
            _connectionString = root.GetConnectionString("DefaultConnection");
        }

        public static string GetConnectionString()
        {
            return _connectionString;
        }

        public static class ErrorMessages
        {
            public const string FriendlyErrorMessage =
                "Internal server error. Please contact admin.";
            public const string DBErrorOccured =
                "An error occurred while interacting with database. Please contact admin.";
            public const string GeneralNetworkError =
                "A network related error occurred. Please contact admin.";
        }

        public static string GetSqlExecutionStatus(int executionOutput)
        {
            return executionOutput switch
            {
                0 => "Cannot save record. Transaction failed.",
                1 => "Data saved successfully.",
                2 => "Cannot save duplicate record.",
                3 => "Data removed successfully.",
                4 => "Data updated successfully.",
                -1 => "Cannot save/update record. Either duplicate record exists or transaction failed.",
                -2 => "Data saved successfully.",
                _ => "Unknown operation status."
            };
        }
    }
}
