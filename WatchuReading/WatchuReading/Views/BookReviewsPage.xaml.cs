using System;
using System.Collections.Generic;
using WatchuReading.ViewModels;
using Xamarin.Forms;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using static WatchuReading.Enums;
using WhatchaReading.Models;

namespace WatchuReading.Views
{

    public partial class BookReviewsPage : PopupPage
    {
        BookReviewsViewModel vm;
        private BookAction ba;
        private Book _book;
        public BookReviewsPage(BookAction action, Book book)
        {
            InitializeComponent();
            BindingContext = vm = new BookReviewsViewModel(book);
            ba = action;
            _book = book;

        }
        private async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //Load the modal with requested list
            //reviews
            //Who has read/is reading
            if(ba==BookAction.Comments)
            { 
                vm.GetReviewsCommand.Execute(null);
            }
            else
            {
                //show past/current readers
            }

        }

        protected override Task OnAppearingAnimationEnd()
        {
            return Content.FadeTo(0.5);
        }

        protected override Task OnDisappearingAnimationBegin()
        {
            return Content.FadeTo(1);
        }
    }
}
