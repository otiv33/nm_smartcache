
using Orleans.Runtime;
using Orleans.Timers;

namespace smartcache.API.Models
{
    public interface IEmailsGrain : IGrainWithStringKey
    {
        Task<bool> AddEmail(string email);

        Task<bool> EmailFound(string localPart);

    }

    public sealed class EmailsGrain : Grain, IEmailsGrain, IRemindable
    {
        private readonly IPersistentState<EmailsState> _state;

        public EmailsGrain(
            [PersistentState(stateName: "emails", storageName: "nomniotest")]
            IPersistentState<EmailsState> state)
        {
            _state = state;
            this.RegisterOrUpdateReminder("save-to-blob-storage", TimeSpan.FromMinutes(0), TimeSpan.FromMinutes(1));
        }

        public override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            if (_state.State.Emails == null)
            {
                _state.State.Emails = new List<string>();
            }
            return base.OnActivateAsync(cancellationToken);
        }

        public Task<bool> AddEmail(string email)
        {
            if (_state.State.Emails.Find(x => x == email) == null)
            {
                _state.State.Emails.Add(email);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> EmailFound(string localPart)
        {
            List<string> emails = _state.State.Emails;
            return Task.FromResult(emails.Find(x => x == localPart) != null);
        }

        // Save grain state to persistent blob storage every 5 minutes
        public async Task ReceiveReminder(string reminderName, TickStatus status)
        {
            await _state.WriteStateAsync();
            return;
        }
    }

    [GenerateSerializer]
    public record class EmailsState
    {
        [Id(0)]
        public List<string> Emails { get; set; }
    }
}