using VortexPings.Models;

namespace VortexPings.Ping
{
    public interface IPinger
    {
        bool IsPinging { get; }

        void StartPing(Node node);
        void StopPing(Node node);
    }
}