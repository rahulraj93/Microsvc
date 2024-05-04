namespace Microsvc.Web.Services.IServices
{
    public interface ITokenProvider
    {
        void ClearToken();
        string? GetToken();
        void SetToken(string token);
    }
}
