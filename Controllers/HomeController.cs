using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Culturest.Models;
using System.IO;
using System.Text;
using System.Xml;
using System.Linq;
using Newtonsoft.Json;

namespace Culturest.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Exportar()
    {
        string path = "App_Data/expedientes.txt";
        var expedientes = System.IO.File.ReadAllText(path);
        return View((object)expedientes);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public IActionResult AgregarExpediente([FromBody] int expediente)
    {
        string path = "App_Data/expedientes.txt";

        if (System.IO.File.Exists(path))
        {
            System.IO.File.AppendAllText(path, expediente.ToString() + Environment.NewLine);
        }
        else
        {
            System.IO.File.WriteAllText(path, expediente.ToString() + Environment.NewLine);
        }

        return Ok();
    }

    public IActionResult ExportarCSV()
    {
        string path = "App_Data/expedientes.txt";
        var expedientes = System.IO.File.ReadAllLines(path);

        var expedientesCSV = new StringBuilder();
        foreach (var expediente in expedientes)
        {
            expedientesCSV.AppendLine($"{expediente},a{expediente}@unison.mx");
        }

        return File(Encoding.UTF8.GetBytes(expedientesCSV.ToString()), "text/csv", "expedientes.csv");
    }

    public IActionResult ExportarSQL()
    {
        string path = "App_Data/expedientes.txt";
        var expedientes = System.IO.File.ReadAllLines(path);

        var expedientesSQL = new StringBuilder();
        expedientesSQL.AppendLine("CREATE DATABASE IF NOT EXISTS evento;");
        expedientesSQL.AppendLine("USE evento;");
        expedientesSQL.AppendLine("CREATE TABLE IF NOT EXISTS asistentes(expediente INT NOT NULL, correo VARCHAR(255) NOT NULL)");
        foreach (var expediente in expedientes)
        {
            expedientesSQL.AppendLine($"INSERT INTO asistentes (expediente, correo) VALUES ({expediente}, 'a{expediente}@unison.mx');");
        }

        return File(Encoding.UTF8.GetBytes(expedientesSQL.ToString()), "text/plain", "expedientes.sql");
    }

    public IActionResult ExportarJSON()
    {
        string path = "App_Data/expedientes.txt";
        var expedientes = System.IO.File.ReadAllLines(path);

        var asistentesJSON = new List<object>();
        foreach (var expediente in expedientes)
        {
            var asistente = new {Expediente = expediente, Correo = $"a{expediente}@unison.mx"};
            asistentesJSON.Add(asistente);
        }

        var expedientesJSON = JsonConvert.SerializeObject(asistentesJSON);

        return File(Encoding.UTF8.GetBytes(expedientesJSON), "application/json", "expedientes.json");
    }

    public IActionResult ExportarXML()
    {
        string path = "App_Data/expedientes.txt";
        var expedientes = System.IO.File.ReadAllLines(path);

        var expedientesXML = new StringBuilder();
        using (var writer = XmlWriter.Create(expedientesXML))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("Asistentes");
            foreach (var expediente in expedientes)
            {
                writer.WriteStartElement("Asistente");
                writer.WriteElementString("Expediente", expediente);
                writer.WriteElementString("Correo", $"a{expediente}@unison.mx");
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
        }

        return File(Encoding.UTF8.GetBytes(expedientesXML.ToString()), "application/xml", "expedientes.xml");
    }
}
