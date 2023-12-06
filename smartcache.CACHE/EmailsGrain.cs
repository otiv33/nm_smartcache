
using Microsoft.VisualStudio.Services.Profile;
using Orleans.GrainDirectory;
using Orleans.Runtime;
using smartcache.CACHE;
using Orleans.BroadcastChannel;
using static System.Runtime.InteropServices.JavaScript.JSType;

public interface IEmailsGrain : IGrainWithStringKey
{
    Task<bool> AddEmail(string email);

    Task<bool> EmailFound(string localPart);

    void WriteToStorage();
}

//[GrainDirectory(GrainDirectoryName = "emailsmartcache")]
public sealed class EmailsGrain : Grain, IEmailsGrain
{
    private readonly IPersistentState<EmailsState> _state;

    public EmailsGrain(
        [PersistentState(stateName: "emails", storageName: "emailsmartcache")]
        IPersistentState<EmailsState> state)
    {
        _state = state;
    }

    public Task<bool> AddEmail(string email)
    {
        if(_state.State.Emails.Find(x => x == email) == null)
        {
            _state.State.Emails.Add(email);
            //_state.State = _state.State;
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<bool> EmailFound(string localPart)
    {
        List<string> emails = _state.State.Emails;
        return Task.FromResult(emails.Find(x => x == localPart) != null);
    }


    public async void WriteToStorage()
    {
        await _state.WriteStateAsync();
    }

}

[GenerateSerializer]
public record class EmailsState
{
    [Id(0)]
    public List<string> Emails { get; set; }
}