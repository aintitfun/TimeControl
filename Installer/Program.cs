using System;
using WixSharp;
using WixSharp.CommonTasks;

class BuildInstaller
{
    static void Main()
    {
        // Rutas relativas a tu solución
        var blazorPath = @"..\MiBlazorApp\bin\Release\net8.0\publish\*.*";
        var backendPath = @"..\MiBackend\bin\Release\net8.0\MiBackend.dll";
        var scriptsPath = @"..\Scripts\*.ps1";

        var project = new ManagedProject("Mi Aplicación Blazor",
            new Dir(@"%ProgramFiles%\MiApp",
                new Files(blazorPath),
                new File(backendPath),

                new Dir("Scripts",
                    new Files(scriptsPath)),

                new Dir("Data",
                    new DirFiles(@"..\Config\*.*"))
            ),

            // Ejecutar script PowerShell después de instalar
            new ElevatedManagedAction(CustomActions.ConfigurarPermisos,
                Return.check,
                When.After,
                Step.InstallFinalize,
                Condition.NOT_Installed),

            // Ejecutar configuración PostgreSQL
            new ElevatedManagedAction(CustomActions.ConfigurarBD,
                Return.check,
                When.After,
                Step.InstallFinalize,
                Condition.NOT_Installed)
        );

        project.GUID = new Guid("12345678-1234-1234-1234-123456789012");
        project.Version = new Version("1.0.0");
        project.ControlPanelInfo.Manufacturer = "Tu Empresa";
        project.ControlPanelInfo.ProductIcon = @"..\MiBlazorApp\wwwroot\favicon.ico";

        // Crear acceso directo en el escritorio
        //project.AddShortcut(new FileShortcut("Mi App", "[ProgramFiles]\\MiApp\\MiBlazorApp.exe")
        //{
        //    WorkingDirectory = "[INSTALLDIR]",
        //    Location = @"%Desktop%"
        //});

        // Requiere privilegios de administrador
        project.InstallScope = InstallScope.perMachine;

        // Generar el MSI
        project.BuildMsi();
    }
}