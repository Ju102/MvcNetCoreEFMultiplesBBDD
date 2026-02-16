using System.Data;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;
using Oracle.ManagedDataAccess.Client;

#region STORED_VIEWS_PROCEDURES_ORACLE
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

//create or replace procedure SP_ALL_VEMPLEADOS
//(p_cursor_empleados out SYS_REFCURSOR)
//as
//begin
//   open p_cursor_empleados for
//	   select * from V_EMPLEADOS;
//end;
#endregion

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public class RepositoryEmpleadosOracle : IRepositoryEmpleados
    {
        private readonly EmpleadosContext context;

        public RepositoryEmpleadosOracle(EmpleadosContext context)
        {
            this.context = context;
        }

        public async Task<List<Empleado>> GetListaEmpleadosAsync()
        {
            string sql = "begin";
            sql += " SP_ALL_VEMPLEADOS (:p_cursor_empleados); ";
            sql += "end;";

            OracleParameter paramCursor = new OracleParameter();
            paramCursor.ParameterName = "p_cursor_empleados";
            paramCursor.Value = null;
            paramCursor.Direction = ParameterDirection.Output;

            // Indicamos el tipo de Oracle
            paramCursor.OracleDbType = OracleDbType.RefCursor;

            var consulta = this.context.Empleados.FromSqlRaw(sql, paramCursor);

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
            string sql = "begin";
            sql +=
                " SP_INSERT_EMPLEADO (:p_apellido, :p_oficio, :p_dir, :p_salario, :p_comision, :p_departamento, :p_idemple ); ";
            sql += "end;";
            OracleParameter paramApellido = new OracleParameter(":p_apellido", apellido);
            OracleParameter paramOficio = new OracleParameter(":p_oficio", oficio);
            OracleParameter paramDirector = new OracleParameter(":p_dir", apellido);
            OracleParameter paramSalario = new OracleParameter(":p_salario", salario);
            OracleParameter paramComision = new OracleParameter(":p_comision", comision);
            OracleParameter paramDepartamento = new OracleParameter(
                ":p_departamento",
                departamento
            );
            OracleParameter paramEmpleado = new OracleParameter(":p_idemple", OracleDbType.Int32);
            paramEmpleado.Direction = ParameterDirection.Output;

            var consulta = await this.context.Database.ExecuteSqlRawAsync(
                sql,
                paramApellido,
                paramOficio,
                paramDirector,
                paramSalario,
                paramComision,
                paramDepartamento,
                paramEmpleado
            );

            return (int)paramEmpleado.Value;
        }
    }
}
