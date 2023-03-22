using System;

namespace festivalServices
{

    public class FestivalException : Exception
    {
        public FestivalException() : base()
        {
        }

        public FestivalException(String msg) : base(msg)
        {
        }

        public FestivalException(String msg, Exception ex) : base(msg, ex)
        {
        }
    }
}