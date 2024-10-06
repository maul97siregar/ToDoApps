using Microsoft.JSInterop;

public class AuthService
{
    private readonly IJSRuntime _jsRuntime;

    public AuthService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> IsLoggedInAsync()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
        Console.WriteLine($"Auth token: {token}");  // Logging for debugging
        return !string.IsNullOrEmpty(token);
    }

    public async Task SetTokenAsync(string token)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
    }

    public async Task ClearTokenAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
    }
}
