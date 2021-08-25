using System.Timers;
using Microsoft.AspNetCore.Components;

namespace AwesomeBlazorBrowser.Components
{
    public partial class AppBar : IDisposable
    {
        private string Keywords = "";

        private readonly System.Timers.Timer DebounceTimer = new System.Timers.Timer(interval: 500) { AutoReset = false };

        [Parameter]
        public bool EnableSerachBox { get; set; }

        [Parameter]
        public EventCallback<string> OnChangeKeywords { get; set; }

        protected override void OnInitialized()
        {
            this.DebounceTimer.Elapsed += DebounceTimer_Elapsed;
        }

        private void OnInputKeywords(ChangeEventArgs args)
        {
            this.Keywords = args.Value.ToString();
            this.DebounceTimer.Stop();
            this.DebounceTimer.Start();
        }

        private void DebounceTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.OnChangeKeywords.InvokeAsync(this.Keywords);
        }

        public void Dispose()
        {
            this.DebounceTimer.Elapsed -= DebounceTimer_Elapsed;
            this.DebounceTimer.Dispose();
        }
    }
}
