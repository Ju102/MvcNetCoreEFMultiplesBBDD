using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;

#region STORED_VIEWS_PROCEDURES_SQLSERVER
//create view V_EMPLEADOS
//as
//	select EMP.EMP_NO as ID_EMPLEADO, EMP.APELLIDO, EMP.OFICIO, EMP.SALARIO,
//         DEPT.DEPT_NO as ID_DEPARTAMENTO, DEPT.DNOMBRE as DEPARTAMENTO, DEPT.LOC as LOCALIDAD
//	    from DEPT inner join EMP on DEPT.DEPT_NO = EMP.DEPT_NO
//go

//create procedure SP_ALL_VEMPLEADOS
//as
//	select * from V_EMPLEADOS
//go

//create procedure SP_INSERT_EMPLEADO
//(@apellido nvarchar(50), @oficio nvarchar(50), @dir int, @salario int, @comision int, @departamento nvarchar(50), @idemple int out)
//as
//	declare @maxid int
//	declare @fecha datetime = GETDATE() -- Fecha de Alta
//	declare @numdept int

//	select @maxid = (MAX(EMP_NO) + 1) from EMP -- Nº Empleado
//	select @numdept = DEPT_NO from DEPT where DNOMBRE = @departamento -- NºDept

//	insert into EMP values (@maxid, @apellido, @oficio, @dir, @fecha, @salario, @comision, @numdept)

//	select @idemple = EMP_NO from EMP where APELLIDO = @apellido
//go
#endregion

namespace MvcNetCoreEFMultiplesBBDD.Repositories
{
    public class RepositoryEmpleadosSqlServer : IRepositoryEmpleados
    {
        private readonly EmpleadosContext context;

        public RepositoryEmpleadosSqlServer(EmpleadosContext context)
        {
            this.context = context;
        }

        public async Task<List<Empleado>> GetListaEmpleadosAsync()
        {
            // var consulta = from datos in this.context.Empleados select datos;

            string sql = "SP_ALL_VEMPLEADOS";
            var consulta = this.context.Empleados.FromSqlRaw(sql);

            List<Empleado> empleados = await consulta.ToListAsync();

            return empleados;
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

        public async Task<int> InsertEmpleadoAsync(
            string apellido,
            string oficio,
            int idDirector,
            int salario,
            int comision,
            string departamento
        )
        {
            var sql =
                "SP_INSERT_EMPLEADO @apellido, @oficio, @dir, @salario, @comision, @departamento, @idemple out";
            SqlParameter paramApellido = new SqlParameter("@apellido", apellido);
            SqlParameter paramOficio = new SqlParameter("@oficio", oficio);
            SqlParameter paramDirector = new SqlParameter("@dir", idDirector);
            SqlParameter paramSalario = new SqlParameter("@salario", salario);
            SqlParameter paramComision = new SqlParameter("@comision", comision);
            SqlParameter paramDepartamento = new SqlParameter("@departamento", departamento);

            SqlParameter paramIdEmpleado = new SqlParameter("@idemple", SqlDbType.Int);
            paramIdEmpleado.Direction = ParameterDirection.Output;

            var consulta = await this.context.Database.ExecuteSqlRawAsync(
                sql,
                paramApellido,
                paramOficio,
                paramDirector,
                paramSalario,
                paramComision,
                paramDepartamento,
                paramIdEmpleado
            );

            return (int)paramIdEmpleado.Value;
        }
    }
}
