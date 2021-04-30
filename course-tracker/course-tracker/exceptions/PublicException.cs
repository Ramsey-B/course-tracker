using System;

namespace course_tracker.Models.exceptions
{
    public class PublicException : Exception
    {
        public PublicException(string msg) : base(msg)
        {

        }
    }
}
