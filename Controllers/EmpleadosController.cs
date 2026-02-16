using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MvcNetCoreEFMultiplesBBDD.Models;
using MvcNetCoreEFMultiplesBBDD.Repositories;

namespace MvcNetCoreEFMultiplesBBDD.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly IRepositoryEmpleados repo;

        public EmpleadosController(IRepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Empleado> empleados = await this.repo.GetListaEmpleadosAsync();
            return View(empleados);
        }

        public async Task<IActionResult> Details(int idempleado)
        {
            Empleado emp = await this.repo.GetDetailsEmpleadoAsync(idempleado);

            return View(emp);
        }

        public IActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Insert(
            string apellido,
            string oficio,
            int idDirector,
            int salario,
            int comision,
            string departamento
        )
        {
            int idEmpleado = await this.repo.InsertEmpleadoAsync(
                apellido,
                oficio,
                idDirector,
                salario,
                comision,
                departamento
            );
            return RedirectToAction("Details", new { idempleado = idEmpleado });
        }
    }
}
