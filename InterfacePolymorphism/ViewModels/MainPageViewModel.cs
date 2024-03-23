using CommunityToolkit.Maui.Views;
using Lession1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lession1.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public MainPageViewModel()
        {
            CreateBlog = new Command(CreateBlogAction);
            this.Blogs = new ObservableCollection<Blog>();
            CreateBlog.Execute("TextBlog");
            CreateBlog.Execute("PhotoBlog");
        }


        private async void CreateBlogAction(object obj)
        {
            var type = obj as string;
            if (type == "TextBlog")
            {
                var blog = new Blog()
                {
                    NoteId = Guid.NewGuid(),
                    Title = type + " Blog",
                    Type = type,
                    Content = type + " Blog Test, There are so many little details that a software developer must take care of before publishing an application. One of the most time-consuming is the task of adding icons to your toolbars, buttons, menus, headers, footers and so on.",
                    State = BlogState.PreView,
                    IsHidden = false,

                };
                this.Blogs.Add(blog);

            }
            else if (type == "PhotoBlog")
            {
                var blog = new Blog()
                {
                    NoteId = Guid.NewGuid(),
                    Title = type + " Blog",
                    Type = type,
                    Content = type + " Blog Test",
                    Images = new List<string>() { "p1.png", "p2.png", "p3.png", "p4.png" },
                    State = BlogState.PreView,
                    IsHidden = false,
                };
                this.Blogs.Add(blog);
            }
        }




        private ObservableCollection<Blog> _blogs;

        public ObservableCollection<Blog> Blogs
        {
            get { return _blogs; }
            set
            {
                _blogs = value;
                NotifyPropertyChanged(nameof(Blogs));
            }
        }


        public Command CreateBlog { get; set; }





        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }



}
