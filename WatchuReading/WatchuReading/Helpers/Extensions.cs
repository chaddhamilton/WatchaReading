using System;

namespace WatchuReading
{
    public static class Extensions
    {
        public static string ToFriendlyName(this String str)
        {
            return $"{str.Split(' ')[0]}'s";
        }

        public static string ToLineItemName(this String str)
        {
            var n = str.Split(' ');
            return $"{n[0]} {n[1].Substring(0,1)}.";
        }

        public static string Swap(this String str)
        {
            if(str.Contains("Bookshelf")){
              return  str.Replace("Bookshelf", "Favorites");    
            }
       
            return str.Replace("Favorites", "Bookshelf");
        }

    }
}
