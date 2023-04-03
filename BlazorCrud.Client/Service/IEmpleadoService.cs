using BlazorCrud.Shared;
namespace BlazorCrud.Client.Service
{
    public interface IEmpleadoService
    {
        Task<List<EmpleadoDTO>> Lista();

        Task<EmpleadoDTO> Buscar(int id);

        Task<int> Guardar(EmpleadoDTO empleadoDTO);

        Task<int> Editar(EmpleadoDTO empleadoDTO);

        Task<bool> Eliminar(int id);

    }
}
