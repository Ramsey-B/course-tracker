using System;
using course_tracker.Models;

namespace course_tracker.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public Term Item { get; set; }
        public ItemDetailViewModel(Term item = null)
        {
            Title = item?.Title;
            Item = item;
        }
    }
}
