using ReviewApi.Modelos.Customs;

namespace ReviewApi.Service
{
    public interface IAutorizacionService
    {
        Task<AutorizacionResponse> DevolverToker(AutorizacionRequest autorizacion);
    }
}
