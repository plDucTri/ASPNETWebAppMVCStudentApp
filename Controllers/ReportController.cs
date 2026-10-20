using Microsoft.Reporting.WebForms;
using ASPNETWebAppMVCStudentApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace ASPNETWebAppMVCStudentApp.Controllers
{
    public class ReportController : Controller
    {
        private readonly SchoolDBEntities _context;

        public ReportController()
        {
            _context = new SchoolDBEntities();
        }

        public ActionResult Index()
        {
            var model = new CourseViewModel
            {
                Courses = new SelectList(_context.Courses, "CourseID", "Title")
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Generate(int selectedCourseID, string action)
        {
            var reportPath = Path.Combine(
                HostingEnvironment.MapPath("~/Reports/"),
                "StudentReport.rdlc");

            var localReport = new LocalReport { ReportPath = reportPath };

            var students = _context.Enrollments
                .Where(e => e.CourseID == selectedCourseID)
                .Select(e => new
                {
                    e.Student.StudentID,
                    e.Student.FirstName,
                    e.Student.LastName,
                    e.Grade
                })
                .ToList();

            localReport.DataSources.Add(new ReportDataSource("StudentDataSet", students));

            var reportBytes = localReport.Render("PDF");

            if (action == "preview")
            {
                Response.AppendHeader("Content-Disposition", "inline; filename=StudentReport.pdf");
                return File(reportBytes, "application/pdf");
            }
            else
            {
                return File(reportBytes, "application/pdf", "StudentReport.pdf");
            }
        }
    }
}