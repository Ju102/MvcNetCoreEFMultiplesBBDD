using MvcNetCoreEFMultiplesBBDD.Models;

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public interface IRepositoryEmpleados
    {
        Task<List<Empleado>> GetListaEmpleadosAsync();
        Task<Empleado> GetDetailsEmpleadoAsync(int idEmpleado);
        Task<int> InsertEmpleadoAsync(
            string apellido,
            string oficio,
            int idDirector,
            int salario,
            int comision,
            string departamento
        );
    }
}
