using System.Data;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreEFMultiplesBBDD.Data;
using MvcNetCoreEFMultiplesBBDD.Models;
using MySql.Data.MySqlClient;
using Mysqlx.Cursor;
using Org.BouncyCastle.Utilities.Zlib;

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

//DELIMITER //

//CREATE PROCEDURE SP_INSERT_EMPLEADO (
//    IN p_apellido VARCHAR(50),
//    IN p_oficio VARCHAR(50),
//    IN p_dir INT,
//    IN p_salario INT,
//    IN p_comision INT,
//    IN p_departamento VARCHAR(50)
//)
//BEGIN
//    DECLARE v_maxid INT;
//DECLARE v_fecha DATETIME DEFAULT NOW(); --Fecha de Alta
//    DECLARE v_numdept INT;

//--Obtener el siguiente ID
//    SELECT (MAX(EMP_NO) + 1) INTO v_maxid FROM EMP;

//--Obtener el número de departamento
//    SELECT DEPT_NO INTO v_numdept
//    FROM DEPT
//    WHERE DNOMBRE = p_departamento;

//--Insertar el registro
//    INSERT INTO EMP
//    VALUES (v_maxid, p_apellido, p_oficio, p_dir, v_fecha, p_salario, p_comision, v_numdept);

//END //

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
            string sql =
                "call sp_insert_empleado(@p_apellido, @p_oficio, @p_dir, @p_salario, @p_comision, @p_departamento, @)";
            MySqlParameter paramApellido = new MySqlParameter("@p_apellido", apellido);
            MySqlParameter paramOficio = new MySqlParameter("@p_oficio", oficio);
            MySqlParameter paramIdDir = new MySqlParameter("@p_dir", idDirector);
            MySqlParameter paramSalario = new MySqlParameter("@p_salario", salario);
            MySqlParameter paramComision = new MySqlParameter("@p_comision", comision);
            MySqlParameter paramDepart = new MySqlParameter("@p_departamento", departamento);
            MySqlParameter paramIdEmpleado = new MySqlParameter("@p_idemple", MySqlDbType.Int32);
            paramIdEmpleado.Direction = ParameterDirection.Output;

            var consulta = await this.context.Database.ExecuteSqlRawAsync(
                sql,
                paramApellido,
                paramOficio,
                paramIdDir,
                paramSalario,
                paramComision,
                paramDepart
            );
        }
    }
}
