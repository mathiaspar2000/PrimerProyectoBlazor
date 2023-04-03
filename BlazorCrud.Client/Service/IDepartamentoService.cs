using BlazorCrud.Shared;

namespace BlazorCrud.Client.Service
{
    public interface IDepartamentoService
    {
        Task<List<DepartamentoDTO>> Lista();
    }
}
