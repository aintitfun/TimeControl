using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Deployment.WindowsInstaller;
using WixSharp;
using Npgsql;

public class CustomActions
{
    [CustomAction]
    public static ActionResult ConfigurarPermisos(Session session)
    {
        try
        {
            session.Log("Ejecutando script de permisos...");

            string installDir = session.Property("INSTALLDIR");
            string scriptPath = Path.Combine(installDir, "Scripts", "ConfigurarPermisos.ps1");

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\" -InstallPath \"{installDir}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            session.Log($"Script output: {output}");

            if (process.ExitCode != 0)
            {
                session.Log($"Script error: {error}");
                return ActionResult.Failure;
            }

            return ActionResult.Success;
        }
        catch (Exception ex)
        {
            session.Log($"Error: {ex.Message}");
            return ActionResult.Failure;
        }
    }

    [CustomAction]
    public static ActionResult ConfigurarBD(Session session)
    {
        try
        {
            session.Log("Configurando PostgreSQL...");

            // Aquí tu lógica de configuración de BD
            string connString = "Host=localhost;Username=postgres;Password=postgres";

            var conn = new NpgsqlConnection(connString);
            conn.Open();

            string createDb = "CREATE DATABASE IF NOT EXISTS miapp_db";
            var cmd = new Npgsql.NpgsqlCommand(createDb, conn);
            cmd.ExecuteNonQuery();

            session.Log("Base de datos configurada correctamente");
            return ActionResult.Success;
        }
        catch (Exception ex)
        {
            session.Log($"Error configurando BD: {ex.Message}");
            // No fallar la instalación si la BD ya existe
            return ActionResult.Success;
        }
    }
}