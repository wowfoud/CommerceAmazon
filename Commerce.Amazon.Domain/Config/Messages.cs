using System;
using System.Collections.Generic;
using System.Text;

namespace Commerce.Amazon.Domain.Config
{
    public class Messages
    {
        public string ErrorConnexion { get { return "No se ha podido establecer la conexión con el servidor, Vuelve a intentarlo en unos minutos"; } }

        public string OrdenRegistrarFallado { get { return "No se puede registar orden producción"; } }
        public string OrdenProcesarFallado { get { return "No se puede procesar orden producción  numero {id}"; } }

        public string OrdenRegistrarConExito { get { return "orden producción registrado con exito en registros pendientes"; } }

        public string OrdenNoValid { get { return "No se puede registar orden producción no valid"; } }
        public string ErrorAuth { get; }= "Une erreur s'est produite lors de l'authentification";
        public string EmailInvalid { get; set; } = "Email invalid";
        public string PasswordInvalid { get; set; } = "Email ou mot de passe incorrecte";
        public string GroupNameRepeted { get; set; } = "Nom groupe deja existe";
        public string GroupIdNotFound { get; set; } = "id group n'existe pas";
    }
}
