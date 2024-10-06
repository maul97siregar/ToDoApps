using Microsoft.JSInterop;

public class AuthService
{
    private readonly IJSRuntime _jsRuntime;

    public AuthService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    // Mengecek apakah user sedang login
    public async Task<bool> IsLoggedInAsync()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");
         await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userId");

        return !string.IsNullOrEmpty(token);
    }

    // Menyimpan token dan userId ke localStorage
    public async Task SetTokenAsync(string token, string userId)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", token);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userId", userId);
    }

    // Menghapus token dan userId dari localStorage
    public async Task ClearTokenAsync()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "userId");
    }

    // Mengambil userId dari localStorage
    public async Task<string> GetUserIdAsync()
    {
        return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userId");
    }

}
