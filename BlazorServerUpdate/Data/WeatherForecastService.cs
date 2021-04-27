using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerUpdate.Data
{
    public class WeatherForecastService
    {
        /// <summary>
        /// Event to notify frontend about data change
        /// </summary>
        public event Action OnDataChange;
        /// <summary>
        /// Method to call event
        /// </summary>
        private void NotifyDataChanged() => OnDataChange?.Invoke();
        private static Timer DataFetcher { get; set; }
        private static int CheckInterval { get; set; } = 100;

        //Default Blazor server stuff
        public static Random Rng { get; set; } = new Random();

        private static List<string> Summaries = new List<string>
        {
            "Freezing"
        };

        /// <summary>
        /// Initialise timer
        /// </summary>
        public WeatherForecastService()
        {
            if (DataFetcher == null)
            {
                DataFetcher = new Timer(new TimerCallback(FetchData), null, CheckInterval, Timeout.Infinite);
            }
        }

        /// <summary>
        /// Fetch data from somewhere
        /// </summary>
        /// <param name="sender"></param>
        private void FetchData(object sender)
        {
            try
            {

                if (1 == 1) // Would be custom logic to determine if something has been updated.. 
                {
                    Summaries.Add(Guid.NewGuid().ToString("n").Substring(0, 8));
                    NotifyDataChanged(); // Trigger event -> 'data has changed'
                }

                DataFetcher.Change(CheckInterval, Timeout.Infinite); // Trigger timer again
            }
            catch (Exception ex)
            {
                DataFetcher.Change(CheckInterval, Timeout.Infinite);  // Trigger timer again
            }
        }

        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = Rng.Next(-20, 55),
                Summary = Summaries[Rng.Next(Summaries.Count - 1)]
            }).ToArray());
        }
    }
}
