using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using Testgridexport.Data;

namespace Testgridexport.Controllers
{
    public partial class ExportTestController : ExportController
    {
        private readonly TestContext context;
        private readonly TestService service;

        public ExportTestController(TestContext context, TestService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/Test/emplyeetables/csv")]
        [HttpGet("/export/Test/emplyeetables/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEmplyeeTablesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetEmplyeeTables(), Request.Query), fileName);
        }

        [HttpGet("/export/Test/emplyeetables/excel")]
        [HttpGet("/export/Test/emplyeetables/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEmplyeeTablesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetEmplyeeTables(), Request.Query), fileName);
        }
    }
}
