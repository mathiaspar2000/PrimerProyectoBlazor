using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlazorCrud.Server.Models;
using BlazorCrud.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorCrud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly DbcrudBlazorContext _dbContext;

        public EmpleadoController(DbcrudBlazorContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var responesApi = new ResponseAPI<List<EmpleadoDTO>>();
            var listaEmpleadoDTO = new List<EmpleadoDTO>();

            try
            {
                foreach (var item in await _dbContext.Empleados.Include(d=>d.IdDepartamentoNavigation).ToListAsync())
                {
                    listaEmpleadoDTO.Add(new EmpleadoDTO
                    {
                        IdEmpleado=item.IdEmpleado,
                        NombreCompleto=item.NombreCompleto,
                        IdDepartamento=item.IdDepartamento,
                        Sueldo=item.Sueldo,
                        FechaContrato=item.FechaContrato,
                        Departamento = new DepartamentoDTO
                        {
                            IdDepartamento = item.IdDepartamentoNavigation.IdDepartamento,
                            Nombre=item.IdDepartamentoNavigation.Nombre
                        }
                    });
                }
                responesApi.EsCorrecto = true;
                responesApi.Valor = listaEmpleadoDTO;
            }
            catch (Exception ex)
            {
                responesApi.EsCorrecto = false;
                responesApi.Mensaje = ex.Message;
            }

            return Ok(responesApi);
        }

        [HttpGet]
        [Route("Buscar/{id}")]
        public async Task<IActionResult> Buscar(int id)
        {
            var responesApi = new ResponseAPI<EmpleadoDTO>();
            var EmpleadoDTO = new EmpleadoDTO();

            try
            {
                var dbEmpleado = await _dbContext.Empleados.FirstOrDefaultAsync(x => x.IdEmpleado == id);
                if (dbEmpleado != null)
                {
                    EmpleadoDTO.IdEmpleado = dbEmpleado.IdEmpleado;
                    EmpleadoDTO.NombreCompleto = dbEmpleado.NombreCompleto;
                    EmpleadoDTO.IdDepartamento = dbEmpleado.IdDepartamento;
                    EmpleadoDTO.Sueldo = dbEmpleado.Sueldo;
                    EmpleadoDTO.FechaContrato = dbEmpleado.FechaContrato;

                    responesApi.EsCorrecto = true;
                    responesApi.Valor = EmpleadoDTO;
                }
                else

                {
                    responesApi.EsCorrecto = false;
                    responesApi.Mensaje = "No encontrado";
                }
     
               
            }
            catch (Exception ex)
            {
                responesApi.EsCorrecto = false;
                responesApi.Mensaje = ex.Message;
            }

            return Ok(responesApi);
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar(EmpleadoDTO empleado)
        {
            var responesApi = new ResponseAPI<int>();

            try
            {
                var dbEmpleado = new Empleado
                {
                    NombreCompleto = empleado.NombreCompleto,
                    IdDepartamento = empleado.IdDepartamento,
                    Sueldo = empleado.Sueldo,
                    FechaContrato = empleado.FechaContrato,
                };
                _dbContext.Empleados.Add(dbEmpleado);
                await _dbContext.SaveChangesAsync();

                if (dbEmpleado.IdEmpleado != 0)
                {
                    responesApi.EsCorrecto = true;
                    responesApi.Valor = dbEmpleado.IdEmpleado;
                }
                else
                {
                    responesApi.EsCorrecto = false;
                    responesApi.Mensaje = "No guardado";
                }

            }
            catch (Exception ex)
            {
                responesApi.EsCorrecto = false;
                responesApi.Mensaje = ex.Message;
            }

            return Ok(responesApi);
        }

        [HttpPut]
        [Route("Editar/{id}")]
        public async Task<IActionResult> Editar(EmpleadoDTO empleado, int id)
        {
            var responesApi = new ResponseAPI<int>();

            try
            {
                var dbEmpleado = await _dbContext.Empleados.FirstOrDefaultAsync(e => e.IdEmpleado == id);

                

                if (dbEmpleado != null)
                {
                    dbEmpleado.NombreCompleto=empleado.NombreCompleto;
                    dbEmpleado.IdDepartamento=empleado.IdDepartamento;
                    dbEmpleado.Sueldo = empleado.Sueldo;    
                    dbEmpleado.FechaContrato= empleado.FechaContrato;   
                    
                    _dbContext.Empleados.Update(dbEmpleado);
                    await _dbContext.SaveChangesAsync();

                    responesApi.EsCorrecto = true;
                    responesApi.Valor = dbEmpleado.IdEmpleado;
                }
                else
                {
                    responesApi.EsCorrecto = false;
                    responesApi.Mensaje = "Empleado no encontrado";
                }

            }
            catch (Exception ex)
            {
                responesApi.EsCorrecto = false;
                responesApi.Mensaje = ex.Message;
            }

            return Ok(responesApi);
        }

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar( int id)
        {
            var responesApi = new ResponseAPI<int>();

            try
            {
                var dbEmpleado = await _dbContext.Empleados.FirstOrDefaultAsync(e => e.IdEmpleado == id);



                if (dbEmpleado != null)
                {
                    

                    _dbContext.Empleados.Remove(dbEmpleado);
                    await _dbContext.SaveChangesAsync();

                    responesApi.EsCorrecto = true;
                }
                else
                {
                    responesApi.EsCorrecto = false;
                    responesApi.Mensaje = "Empleado no encontrado";
                }

            }
            catch (Exception ex)
            {
                responesApi.EsCorrecto = false;
                responesApi.Mensaje = ex.Message;
            }

            return Ok(responesApi);
        }
    }
}
