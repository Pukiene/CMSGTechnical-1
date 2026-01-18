using System.Text.Json;
using Microsoft.JSInterop;
namespace CMSGTechnical.Code;

public sealed class BasketStorage : IAsyncDisposable
{
    private const string Key = "cmsg_basket_lines_v1";

    private readonly IJSRuntime _js;
    private readonly DotNetObjectReference<BasketStorage> _objRef;

    public event Action? OnExternalChange;

    public BasketStorage(IJSRuntime js)
    {
        _js = js;
        _objRef = DotNetObjectReference.Create(this);
    }

    public async Task InitCrossTabListenerAsync()
    {
        await _js.InvokeVoidAsync("basketStorage.init", _objRef, Key);
    }

    public async Task SaveAsync(IEnumerable<BasketLinePersist> lines)
    {
        var json = JsonSerializer.Serialize(lines);
        await _js.InvokeVoidAsync("basketStorage.save", Key, json);
    }

    public async Task<List<BasketLinePersist>> LoadAsync()
    {
        var json = await _js.InvokeAsync<string?>("basketStorage.load", Key);
        if (string.IsNullOrWhiteSpace(json)) return new List<BasketLinePersist>();

        try
        {
            return JsonSerializer.Deserialize<List<BasketLinePersist>>(json) ?? new List<BasketLinePersist>();
        }
        catch
        {
            return new List<BasketLinePersist>();
        }
    }

    [JSInvokable]
    public void NotifyExternalChange()
    {
        OnExternalChange?.Invoke();
    }

    public ValueTask DisposeAsync()
    {
        _objRef.Dispose();
        return ValueTask.CompletedTask;
    }
}