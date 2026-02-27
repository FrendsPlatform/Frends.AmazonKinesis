using System;
using System.Collections.Generic;
using Frends.AmazonKinesis.PutRecords.Definitions;

namespace Frends.AmazonKinesis.PutRecords.Helpers;

internal static class ErrorHandler
{
    internal static Result Handle(Exception exception, bool throwOnFailure, string errorMessageOnFailure, int? failedRecordCount = null, List<RecordResult> recordResults = null)
    {
        if (throwOnFailure)
        {
            if (string.IsNullOrEmpty(errorMessageOnFailure))
                throw new Exception(exception.Message, exception);

            throw new Exception(errorMessageOnFailure, exception);
        }

        var errorMessage = !string.IsNullOrEmpty(errorMessageOnFailure)
            ? $"{errorMessageOnFailure}: {exception.Message}"
            : exception.Message;

        return new Result(
            success: false,
            entries: recordResults,
            failedCount: failedRecordCount,
            error: new Error { Message = errorMessage, AdditionalInfo = exception });
    }
}