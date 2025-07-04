﻿using FMOD;


namespace Kintsugi.Audio
{
    internal class FMODException : Exception
    {
        public FMODException(RESULT result) : base(GenerateMessageFromResult(result))
        {
        }

        public FMODException(RESULT? result, Exception? innerException) : base(GenerateMessageFromResult(result), innerException)
        {
        }

        private static string? GenerateMessageFromResult(RESULT? result)
        {
            if (result == null) return "Fmod error! Result is null, something went horribly wrong internally.";
            return "Fmod error: " + Error.String(result.Value);
        }
    }
}
