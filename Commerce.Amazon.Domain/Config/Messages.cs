using System;
using System.Collections.Generic;
using System.Text;

namespace Commerce.Amazon.Domain.Config
{
    public static class Messages
    {
        public static string ErrorConnexion { get { return "No se ha podido establecer la conexión con el servidor, Vuelve a intentarlo en unos minutos"; } }

        public static string OrdenRegistrarFallado { get { return "No se puede registar orden producción"; } }
        public static string OrdenProcesarFallado { get { return "No se puede procesar orden producción  numero {id}"; } }

        public static string OrdenRegistrarConExito { get { return "orden producción registrado con exito en registros pendientes"; } }

        public static string OrdenNoValid { get { return "No se puede registar orden producción no valid"; } }
        public static string ErrorAuth { get; }= "Une erreur s'est produite lors de l'authentification";
    }
}
