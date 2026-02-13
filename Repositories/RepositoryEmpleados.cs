using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;

#region STORED_PROCEDURES_ORACLE
//create or replace view V_EMPLEADOS as
//   select emp.emp_no as ID_EMPLEADO,
//          emp.apellido,
//          emp.oficio,
//          emp.salario,
//          dept.dept_no as ID_DEPARTAMENTO,
//          dept.dnombre as DEPARTAMENTO,
//          dept.loc as LOCALIDAD
//     from dept
//    inner join emp
//   on dept.dept_no = emp.dept_no;
#endregion

#region STORED_PROCEDURES_SQLSERVER
//create view V_EMPLEADOS
//as
//		select EMP.EMP_NO as ID_EMPLEADO, EMP.APELLIDO, EMP.OFICIO, EMP.SALARIO,
//               DEPT.DEPT_NO as ID_DEPARTAMENTO, DEPT.DNOMBRE as DEPARTAMENTO, DEPT.LOC as LOCALIDAD
//			from DEPT inner join EMP on DEPT.DEPT_NO = EMP.DEPT_NO
//go
#endregion

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public class RepositoryEmpleados
    {
        private readonly EmpleadosContext context;

        public RepositoryEmpleados(EmpleadosContext context)
        {
            this.context = context;
        }

        public async Task<List<Empleado>> GetListaEmpleadosAsync()
        {
            var consulta = from datos in this.context.Empleados select datos;

            return await consulta.ToListAsync();
        }

        public async Task<Empleado> GetDetailsEmpleadoAsync(int idEmpleado)
        {
            // Empleado emp = await this.context.Empleados.FindAsync(idEmpleado);

            var consulta =
                from datos in this.context.Empleados
                where datos.IdEmpleado == idEmpleado
                select datos;

            Empleado emp = await consulta.FirstOrDefaultAsync();

            return emp;
        }
    }
}
