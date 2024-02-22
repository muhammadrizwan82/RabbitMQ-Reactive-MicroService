namespace ReactiveMicroService.OrderService.API.Service
{
    public class TokenBlacklistService
    {
        private readonly HashSet<string> _blacklistedTokens;

        public TokenBlacklistService()
        {
            _blacklistedTokens = new HashSet<string>();
        }

        public void AddToBlacklist(string token)
        {
            _blacklistedTokens.Add(token);
        }

        public bool IsTokenBlacklisted(string token)
        {
            return _blacklistedTokens.Contains(token);
        }
    }
}
