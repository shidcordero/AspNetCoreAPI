using System;
using System.Linq;
using System.Threading.Tasks;
using Data.Utilities;
using Domain.Contracts;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Utilities
{
    /// <summary>
    /// Helpers for API Solution only
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Used to get errors from exception
        /// </summary>
        /// <param name="ex">Holds the exceptionn data</param>
        /// <param name="emailService"></param>
        /// <returns>Error Messages</returns>
        public static async Task<string> GetErrors(Exception ex)
        {
            // handle errors here
            var message = Constants.Message.ErrorProcessing;
            if (ex == null) return string.Empty;

            return message;
        }
    }
}