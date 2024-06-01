using Justpharm.Web.Models;
using Syncfusion.Blazor.Schedule;
using System.Collections.ObjectModel;

namespace Justpharm.Web.Data
{
    public class Statics
    {
        public static string ImagenDePerfil { get; set; } = "01.png";
        public static List<_MaestroPerfil> MaestroPerfiles { get; set; } = new List<_MaestroPerfil>();
        public static List<_MaestroPerfil> UsuarioExternosTipo { get; set; } = new List<_MaestroPerfil>();
        public static List<_MaestroDietas> ListadoDietas { get; set; } = new List<_MaestroDietas>();
        public static List<_MaestroCuidados> ListadoCuidadosCategoria { get; set; } = new List<_MaestroCuidados>();
        public static List<_MaestroNecesidades> ListadoNecesidades { get; set; } = new List<_MaestroNecesidades>();
        public static string GetRandomColor()
        {
            Random random = new Random();
            string randomColor = String.Format("#{0:X6}", random.Next(0x1000000));
            return randomColor;
        }
        public static DateTime? ExtractEndTime(string input)
        {
            string untilKeyword = "UNTIL=";
            int untilIndex = input.IndexOf(untilKeyword);

            if (untilIndex != -1)
            {
                string untilValue = input.Substring(untilIndex + untilKeyword.Length, 15); // "20240530T235959"
                DateTime endTime;
                if (DateTime.TryParseExact(untilValue, "yyyyMMddTHHmmss", null, System.Globalization.DateTimeStyles.None, out endTime))
                {
                    return endTime;
                }
            }

            return null;
        }
        public static List<Toma> CrearTomasProgramadas(Tratamiento tratamiento, string UsuarioId, string EmailAviso)
        {
            List<Toma> tomas = new List<Toma>();


            DateTime? day = DateTime.Today;

            ObservableCollection<DateTime>? diasProgramacion = new ObservableCollection<DateTime>();

            if (tratamiento.RecurrenceRule == null)
            {
                Toma? toma = new Toma
                {
                    UidToma = Guid.NewGuid(),
                    UidTratamiento = tratamiento.UidTratamiento,
                    UidMedicamento = tratamiento.UidMedicamento,
                    UsuarioId = UsuarioId,
                    Titulo = tratamiento.Titulo,
                    Descripcion = tratamiento.Descripcion,
                    StartTime = tratamiento.StartTime,
                    EndTime = tratamiento.StartTime.AddMinutes(30),
                    Color = tratamiento.Color,
                    Intervalo = tratamiento.Intervalo,
                    UidPaciente = tratamiento.UidPaciente,
                    EmailAviso = EmailAviso,
                    Ingerido = false,
                    Mejora = 0,
                    Estado = 0
                };
                tomas.Add(toma);
                return tomas;
            }

            // Extraer de la recurrencia los eventos de hoy (deberia estar el de "day" si toca)
            diasProgramacion = RecurrenceHelper
                .GetRecurrenceDateTimeCollection(
                    tratamiento.RecurrenceRule,
                    tratamiento.RecurrenceException,
                    1,
                    tratamiento.StartTime,
                    tratamiento.EndTime,
                    day,
                    tratamiento.EndTime,
                    null);

            List<DateTime> fechasProgramacion = diasProgramacion.Select(dt => dt.Date).ToList();

            // Crear las tomas programadas
            DateTime fecha = tratamiento.StartTime;
            DateTime fechaFin = tratamiento.EndTime;
            double? intervalo = tratamiento.Intervalo;
            Guid uidTratamiento = tratamiento.UidTratamiento;
            string? titulo = tratamiento.Titulo;
            string? descripcion = tratamiento.Descripcion;
            string? color = tratamiento.Color;

            while (fecha <= fechaFin)
            {

                if (fechasProgramacion == null || fechasProgramacion.Contains(fecha.Date))
                {
                    Toma? toma = new Toma
                    {
                        UidToma = Guid.NewGuid(),
                        UidTratamiento = uidTratamiento,
                        UidMedicamento = tratamiento.UidMedicamento,
                        UsuarioId = UsuarioId,
                        Titulo = titulo,
                        Descripcion = descripcion,
                        StartTime = fecha,
                        EndTime = fecha.AddHours(intervalo!.Value),
                        Color = color,
                        Intervalo = intervalo,
                        UidPaciente = tratamiento.UidPaciente,
                        EmailAviso = EmailAviso,
                        Ingerido = false,
                        Mejora = 0,
                        Estado = 0

                    };
                    tomas.Add(toma);
                }
                double sumahora = intervalo.GetValueOrDefault();
                fecha = fecha.AddHours(sumahora);
            }
            return tomas;
        }
        private DateTime EndOfDay(DateTime day) => day.Date.AddDays(1).AddTicks(-1);
        private DateTime StartOfDay(DateTime day) => day.Date;

    }
}
