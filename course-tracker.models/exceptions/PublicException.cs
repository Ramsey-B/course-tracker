using System;

namespace course_tracker.models.exceptions
{
    public class PublicException : Exception
    {
        public PublicException(string msg) : base(msg)
        {

        }
    }
}
