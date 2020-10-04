using System;

namespace WatchuReading
{
    public class Enums
    {
       public enum ViewAction{
            Default,
            Login,
            New
        }

        public enum ActivitySort
        {
            Title,
            Author,
            AddDate
        }

        public enum BookAction
        {
            Comments,
            WhoHasRead
        }
    }
}
