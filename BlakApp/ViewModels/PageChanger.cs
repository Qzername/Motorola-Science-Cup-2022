using Avalonia.Media;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlakApp.ViewModels
{
    /// <summary>
    /// Contains tools for making element with changing viewmodels or pages
    /// </summary>
    public abstract class PageChanger : ViewModelBase
    {
        ViewModelBase _currentPage;
        public ViewModelBase CurrentPage
        {
            get => _currentPage;
            protected set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        ObservableCollection<Page> _pages;
        /// <summary>
        /// Array that contains every page, modify list if you want to add/modify/delete page
        /// </summary>
        protected ObservableCollection<Page> Pages { get => _pages; set => this.RaiseAndSetIfChanged(ref _pages, value); }

        public PageChanger()
        {
            Pages = new ObservableCollection<Page>();
        }

        /// <summary>
        /// Change current page
        /// </summary>
        /// <param name="pageOBJ"></param>
        public void ChangePage(object pageOBJ)
        {
            ChangePage((Page)pageOBJ);
        }

        /// <summary>
        /// Change current page
        /// </summary>
        public void ChangePage(Page page)
        {
            int pageID = Pages.IndexOf(page);
            ChangePage(pageID);
        }

        /// <summary>
        /// Change current page
        /// </summary>
        public void ChangePage(int pageID)
        {
            //turn off all pages that are not the chosen one
            for (int i = 0; i < Pages.Count; i++)
            {
                if (i == pageID)
                    continue;

                var copy = Pages[i];
                copy.Color = Page.OffColor;
                Pages[i] = copy;
            }

            //turining one that one page
            var turnOnOne = Pages[pageID];
            turnOnOne.Color = Page.OnColor;
            Pages[pageID] = turnOnOne;

            CurrentPage = Pages[pageID].Content;
        }
    }

    public struct Page
    {
        public static SolidColorBrush OnColor = new SolidColorBrush(new Color(255, 175, 175, 175));
        public static SolidColorBrush OffColor = new SolidColorBrush(new Color(255, 255, 255, 255));

        public SolidColorBrush Color { get; set; }
        public ViewModelBase Content { get; }
        public string Name { get; }

        public Page(string Name, ViewModelBase Content)
        {
            this.Name = Name;
            this.Color = OffColor;
            this.Content = Content;
        }
    }
}
