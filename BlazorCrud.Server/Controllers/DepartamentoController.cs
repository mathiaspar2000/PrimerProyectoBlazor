using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlazorCrud.Server.Models;
using BlazorCrud.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorCrud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentoController : ControllerBase
    {
        private readonly DbcrudBlazorContext _dbContext;

        public DepartamentoController(DbcrudBlazorContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var responesApi=new ResponseAPI<List<DepartamentoDTO>>();
            var listaDepartamentoDTO=new List<DepartamentoDTO>();

            try
            {
                foreach(var item in await _dbContext.Departamentos.ToListAsync())
                {
                    listaDepartamentoDTO.Add(new DepartamentoDTO
                        {
                            IdDepartamento=item.IdDepartamento,
                            Nombre=item.Nombre,
                        });
                }
                responesApi.EsCorrecto = true;
                responesApi.Valor = listaDepartamentoDTO;
            }catch (Exception ex)
            {
                responesApi.EsCorrecto = false;
                responesApi.Mensaje=ex.Message;
            }

            return Ok(responesApi);
        }
    }
}
