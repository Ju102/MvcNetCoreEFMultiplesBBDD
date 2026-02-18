using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;
using Mysqlx.Crud;
using Oracle.ManagedDataAccess.Client;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Azure.Core.HttpHeader;

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

//create or replace procedure sp_insert_empleado (
//   p_apellido     emp.apellido%type,
//   p_oficio       emp.oficio%type,
//   p_dir          emp.dir%type,
//   p_salario      emp.salario%type,
//   p_comision     emp.comision%type,
//   p_departamento dept.dnombre%type,
//   p_idemple out emp.emp_no%type
//) as
//   v_maxid   emp.emp_no%type;
//v_fecha date := sysdate; --Equivalente a GETDATE()
//   v_numdept emp.dept_no % type;
//begin
//   select(max(emp_no) + 1)
//     into v_maxid
//     from emp;

//select dept_no
//     into v_numdept
//     from dept
//    where dnombre = p_departamento;

//insert into emp values ( v_maxid,
//                         p_apellido,
//                         p_oficio,
//                         p_dir,
//                         v_fecha,
//                         p_salario,
//                         p_comision,
//                         v_numdept );
//commit;

//select emp_no into p_idemple from emp where apellido = p_apellido;
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
            OracleParameter paramDirector = new OracleParameter(":p_dir", idDirector);
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

            return int.Parse(paramEmpleado.Value.ToString());
        }
    }
}
