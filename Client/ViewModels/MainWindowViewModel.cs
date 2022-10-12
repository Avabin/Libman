using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive;
using ReactiveUI;
using Server.GrainInterfaces.Book;

namespace Client.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ReactiveCommand<string, BookDto?> GetBookCommand { get; }

        public MainWindowViewModel()
        {
            GetBookCommand = ReactiveCommand.CreateFromTask<string, BookDto?>(async isbn =>
            {
                HttpResponseMessage response;

                using (var client = new HttpClient() { BaseAddress = new Uri("https://localhost:7258") })
                {
                    response = await client.GetAsync($"/BookStore/{isbn}");
                }

                var maybeBook = await response.Content.ReadFromJsonAsync<BookDto>();
                
                return maybeBook;
            });
        }
    }
}