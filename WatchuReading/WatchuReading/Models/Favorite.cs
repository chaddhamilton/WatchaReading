using System;
using Xamarin.Forms;

namespace WatchuReading.Models
{

    public class Favorite : IBusinessEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Book Book { get; set; }
        public bool ShowMenu { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}

