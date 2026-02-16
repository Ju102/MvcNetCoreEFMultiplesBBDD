using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;

#region STORED_VIEWS_PROCEDURES_MYSQL
//create view v_empleados as
//	select EMP.EMP_NO as ID_EMPLEADO, EMP.APELLIDO, EMP.OFICIO, EMP.SALARIO,
//    DEPT.DEPT_NO as ID_DEPARTAMENTO, DEPT.DNOMBRE as DEPARTAMENTO, DEPT.LOC as LOCALIDAD
//	from DEPT inner join EMP on DEPT.DEPT_NO = EMP.DEPT_NO;

//DELIMITER //
//create procedure sp_all_vempleados()
//begin
//	select * from v_empleados;
//end //
//DELIMITER;
#endregion

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public class RepositoryEmpleadosMySql : IRepositoryEmpleados
    {
        private readonly EmpleadosContext context;

        public RepositoryEmpleadosMySql(EmpleadosContext context)
        {
            this.context = context;
        }

        public async Task<List<Empleado>> GetListaEmpleadosAsync()
        {
            string sql = "call sp_all_vempleados()";
            var consulta = this.context.Empleados.FromSqlRaw(sql);

            List<Empleado> empleados = await consulta.ToListAsync();

            return empleados;
        }

        public async Task<Empleado> GetDetailsEmpleadoAsync(int idEmpleado)
        {
            var consulta =
                from datos in this.context.Empleados
                where datos.IdEmpleado == idEmpleado
                select datos;

            Empleado emp = await consulta.FirstOrDefaultAsync();

            return emp;
        }

        public async Task<int> InsertEmpleadoAsync(
            string apellido,
            string oficio,
            int idDirector,
            int salario,
            int comision,
            string departamento
        )
        {
            throw new NotImplementedException();
        }
    }
}
